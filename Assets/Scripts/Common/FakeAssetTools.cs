#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class FakeAssetTools
{
    public static T Load<T>(string asset, string name, string parent = null) where T : Object
    {
        string url;
        switch (asset)
        {
            case "prefab.ab":
                return AssetDatabase.LoadAssetAtPath<T>($"Assets/Prefabs/{(parent != null ? parent + "/" : string.Empty)}{name}.prefab");
            case "sprite.ab":
                url = $"Assets/_Textures/Images/{(parent != null ? parent + "/" : string.Empty)}{name}.png";
                var tex = AssetDatabase.LoadAssetAtPath<T>(url);
                return tex;
            case "audio.ab":
                url = $"Assets/Audios/{(parent != null ? parent + "/" : string.Empty)}{name}.mp3";
                var mp3 = AssetDatabase.LoadAssetAtPath<T>(url);
                return mp3;
            default:
                return null;
        }
    }
}

#endif