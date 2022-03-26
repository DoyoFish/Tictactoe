using Doyo.Assets;
using UnityEngine;

public static class Loader
{
    public static GameObject LoadPrefab(string prefabName)
    {
#if UNITY_EDITOR
        return FakeAssetTools.Load<GameObject>("prefab.ab", prefabName);
#else
        return AssetTools.LoadAsset<GameObject>("prefab.ab", prefabName);
#endif    
    }

    public static GameObject LoadPanel(string prefabName)
    {
#if UNITY_EDITOR
        return FakeAssetTools.Load<GameObject>("prefab.ab", prefabName, "Panels");
#else
        return AssetTools.LoadAsset<GameObject>("prefab.ab", prefabName);
#endif    
    }

    public static Sprite LoadSprite(string spriteName)
    {
#if UNITY_EDITOR
        return FakeAssetTools.Load<Sprite>("sprite.ab", spriteName);
#else
        return AssetTools.LoadAsset<Sprite>("sprite.ab", spriteName);
#endif 
    }

    public static Texture LoadTexture(string texName)
    {
        return AssetTools.LoadAsset<Texture>("texture.ab", texName);
    }

    public static TextAsset LoadTextAsset(string txtName)
    {
        return AssetTools.LoadAsset<TextAsset>("text.ab", txtName);
    }

    public static AudioClip LoadAudioClip(string audioClipName)
    {
#if UNITY_EDITOR
        return FakeAssetTools.Load<AudioClip>("audio.ab", audioClipName);
#else
        return AssetTools.LoadAsset<AudioClip>("audio.ab", audioClipName);
#endif
    }

    public static ScriptableObject LoadScriptableObject(string scriptableObjectName)
    {
        return AssetTools.LoadAsset<ScriptableObject>("scriptable.ab", scriptableObjectName);
    }
}
