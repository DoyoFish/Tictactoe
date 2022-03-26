using Doyo.UnityFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Doyo.Assets
{
    public static class AssetTools
    {
        private static readonly Dictionary<string, AssetBundle> AssetBundleWrappers =
        new Dictionary<string, AssetBundle>();

        public static IEnumerator Initialize()
        {
            var items = StreamingAssetsManifest.Instance.ManifestItems;
            var count = items.Length;
            for (int i = 0; i < count; ++i)
            {
                var item = items[i];
                var name = item.Path;
                CoroutineMng.StartCoroutine(AssetBundleHelper.Load(name, OnAssetBundleLoaded));
            }
            while (AssetBundleWrappers.Count != count)
            {
                yield return null;
            }
            Debug.Log("Load all assets complete");
        }

        private static void OnAssetBundleLoaded(string name, AssetBundle abWrapper)
        {
            if (abWrapper == null)
            {
                Debug.LogErrorFormat("{0} 加载失败", name);
                return;
            }
            AssetBundleWrappers[name] = abWrapper;
        }

        public static T LoadAsset<T>(string abName, string assetName) where T : Object
        {
            var assetBundle = LoadAssetBundle(abName);
            if (assetBundle == null)
            {
                return null;
            }
            return assetBundle.LoadAsset<T>(assetName);
        }

        public static AssetBundle LoadAssetBundle(string abName)
        {
            if (!AssetBundleWrappers.ContainsKey(abName))
            {
                return null;
            }
            return AssetBundleWrappers[abName];
        }
    }
}
