using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CustomScriptGenerator
{
    private static readonly string TemplateRootPath = "Assets/Editor/CSharpScriptTemplates/";

    private static readonly string InterfaceTemplatePath = "InterfaceTemplate.txt"; 
    private static readonly string UIPanelInstanceTemplatePath = "UIPanelTemplate.txt";

    [MenuItem("Assets/Create/C# Interface", false, 1)]
    public static void GeneraterInterfaceInstance()
    {
        GenerateCustomScript(InterfaceTemplatePath);
        Debug.Log("Generate interface done");
    }

    [MenuItem("Assets/Create/C# UIPanelScripts", false, 1)]
    public static void GenerateUIPanelInstance()
    {
        GenerateCustomScript(UIPanelInstanceTemplatePath);
        Debug.Log("Generate uipanel instance done");
    }

    private static void GenerateCustomScript(string custormTemp)
    {
        var selections = Selection.objects;
        if (selections == null || selections.Length == 0)
        {
            Debug.LogError("没有选择任何文件夹");
            return;
        }
        // 默认选择第一个目标的文件夹
        var selection = selections[0];
        var path = selection.GetAssetInFolderPath();
        // 检查是否为文件夹
        if (!Directory.Exists(path))
        {
            path = Path.GetDirectoryName(path);
        }
        string filename = "New" + custormTemp.Replace("Template.txt", string.Empty);
        string fullPath = $"{path}/{filename}.cs";
        int index = 0;
        while (File.Exists(fullPath))
        {
            fullPath = string.Format("{0}{1}", fullPath, index++);
        }

        string template = Path.GetFullPath(TemplateRootPath + custormTemp);
        try
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(template, filename + ".cs");
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("Create Failed... \n\r {0}", ex);
        }
    }
}