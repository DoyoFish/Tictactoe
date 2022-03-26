using UnityEditor;

public static class MainDirectoriesEditor
{

    [MenuItem("Tools/主要目录/UI预制体")]
    public static void OpenUIPrefabsFolder()
    {
        EditorHelper.OpenFirstOrDiretory(EditorMisc.UIPrefabPath);
    }

    [MenuItem("Tools/主要目录/UI脚本")]
    public static void OpenUIScriptsFolder()
    {
        EditorHelper.OpenFirstOrDiretory(EditorMisc.UIScriptPath);
    }

    [MenuItem("Tools/主要目录/Configs")]
    public static void OpenConfigsFolder()
    {
        EditorHelper.OpenFirstOrDiretory(EditorMisc.ConfigsFolder);
    }

    [MenuItem("Tools/主要目录/ScriptableConfigs")]
    public static void OpenScriptableConfigsFolder()
    {
        EditorHelper.OpenFirstOrDiretory(EditorMisc.ScriptableConfigFolder);
    }

    [MenuItem("Tools/主要目录/脚本根路径")]
    public static void OpenScriptsFolder()
    {
        EditorHelper.SelectOrderAssetByPath(EditorMisc.ScriptRoot);
    }

    [MenuItem("Tools/主要目录/Managers脚本")]
    public static void OpenManagersFolder()
    {
        EditorHelper.OpenFirstOrDiretory("Assets/Scripts/Managers");
    }

}
