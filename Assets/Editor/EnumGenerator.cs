using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

public class EnumGenerator : EditorWindow
{
    private string _sourceJsonFile;
    private string _saveFolder;
    private string _columnName = "Name";
    private string _csFilename;

    [MenuItem("Tools/EnumEditor")]
    private static void OpenWindow()
    {
        EnumGenerator window = GetWindow<EnumGenerator>("EnumEditor");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(50);
        GenerateButton("选择原始Json文件", () => _sourceJsonFile = EditorGUILayout.TextField(_sourceJsonFile),
            "选择文件", OnSelectFile);

        GenerateButton("选择枚举文件存储路径", () => _saveFolder = EditorGUILayout.TextField(_saveFolder),
            "选择文件夹", () => _saveFolder = EditorUtility.OpenFolderPanel("选择枚举文件存储路径",
            Application.dataPath + @"\Scripts\", "Config"));

        GUILayout.Label("指定的列名");
        _columnName = EditorGUILayout.TextField(_columnName);
        GUILayout.Space(10);

        GUILayout.Label("填写枚举名称");
        _csFilename = EditorGUILayout.TextField(_csFilename);

        if (GUILayout.Button("开始转换", GUILayout.Width(100)))
        {
            Translate();
        }
    }

    private void OnSelectFile()
    {
        _sourceJsonFile = EditorUtility.OpenFilePanel("选择Json文件", Application.dataPath + "Jsons/", "txt");
        _csFilename = Path.GetFileNameWithoutExtension(_sourceJsonFile) + "Type";
    }

    private void GenerateButton(string label, Action textAction, string buttonName, Action onClick)
    {
        GUILayout.Label(label);
        textAction?.Invoke();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(buttonName, GUILayout.Width(100)))
        {
            onClick?.Invoke();
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private void Translate()
    {
        if (string.IsNullOrEmpty(_sourceJsonFile) || string.IsNullOrEmpty(_saveFolder) || string.IsNullOrEmpty(_csFilename))
        {
            Debug.LogError("文件路径，保存路径，或枚举名为空");
            return;
        }
        if (!File.Exists(_sourceJsonFile))
        {
            Debug.LogError("Json文件不存在");
            return;
        }
        if (!Directory.Exists(_saveFolder))
        {
            Directory.CreateDirectory(_saveFolder);
        }
        using (FileStream file = File.Open(_sourceJsonFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string content = reader.ReadToEnd();
                JArray jArray = JArray.Parse(content);
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("public enum {0}", _csFilename);
                builder.AppendLine();
                builder.AppendLine("{");
                int id;
                for (int i = 0; i < jArray.Count; ++i)
                {
                    var line = jArray[i];
                    if (!int.TryParse(line["Id"].ToString(), out id))
                    {
                        continue;
                    }
                    string enumName = line[_columnName].ToString();
                    if (string.IsNullOrEmpty(enumName))
                    {
                        continue;
                    }
                    builder.AppendFormat("   {0} = {1}{2}", line[_columnName].ToString(), line["Id"].ToString(), i != jArray.Count - 1 ? "," : "");
                    builder.AppendLine();
                }

                builder.AppendLine("}");
                string enumContent = builder.ToString();
                File.WriteAllText(_saveFolder + "/" + _csFilename + ".cs", enumContent);
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("Done");

    }
}
