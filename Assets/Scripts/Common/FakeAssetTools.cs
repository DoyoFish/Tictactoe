#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

public static class FakeAssetTools
{
    public static T Load<T>(string asset, string name) where T : Object
    {
        var paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(asset, name);
        return AssetDatabase.LoadAssetAtPath<T>(paths.FirstOrDefault());
    }
}

#endif