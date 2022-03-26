using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class EditorHelper
{
    public static string GetAssetInFolderPath(this Object unityObject)
    {
        string relativePath = Application.dataPath.Replace("Assets", string.Empty);
        string objectPath = AssetDatabase.GetAssetPath(unityObject);
        string folderPath = Path.GetFullPath(relativePath + objectPath).Replace("\\", "/");
        return folderPath;
    }

    public static void OpenFirstOrDiretory(string path)
    {
        var fullpath = Path.GetFullPath(path);
        if (!Directory.Exists(fullpath))
        {
            Debug.LogError($"UI主要目录{fullpath}不存在");
            return;
        }
        var file = new DirectoryInfo(fullpath).GetFiles().Where(f => !f.FullName.EndsWith(".meta")).FirstOrDefault();
        if (file != null)
        {
            SelectOrderAssetByPath(FullPathToAssetPath(file.FullName));
        }
        else
        {
            SelectOrderAssetByPath(path);
        }
    }

    public static string FullPathToAssetPath(string fullPath)
    {
        string relativePath = "Assets/" + fullPath.Replace("\\", "/").Replace(Application.dataPath, string.Empty);
        return relativePath;
    }

    /// <summary>
    /// 通过项目内路径选择指定的资源
    /// 路径连接符应为"/"而不是"\\"
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool SelectOrderAssetByPath(string path)
    {
        if (path.Contains("\\"))
        {
            path = path.Replace("\\", "/");
        }
        var assetObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        if (!assetObj)
        {
            Debug.Log($"{path} 不是Object路径");
            return false;
        }
        ProjectWindowUtil.ShowCreatedAsset(assetObj);
        return true;
    }

}
