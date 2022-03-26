using Doyo.Assets;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Doyo.AssetsEditor
{
    public static class AssetBundleEditor
    {
        [MenuItem("ScriptableObjects/ViewStreamingAssetsManifest")]
        private static void CreateStreamingAssetsManifest()
        {
            var config = StreamingAssetsManifest.Instance;
            if (!config)
            {
                config = ScriptableObject.CreateInstance<StreamingAssetsManifest>();
                AssetDatabase.CreateAsset(config, "Assets/Resources/StreamingAssetsManifest.asset");
            }
            ProjectWindowUtil.ShowCreatedAsset(config);
        }

        [MenuItem("Build/Statistic Streaming Assets")]
        private static void StatisticStreamingAssets()
        {
            var config = StreamingAssetsManifest.Instance;
            if (!config)
            {
                config = ScriptableObject.CreateInstance<StreamingAssetsManifest>();
                AssetDatabase.CreateAsset(config, "Assets/Resources/StreamingAssetsManifest.asset");
                Debug.Log("StreamingAssetsManifest不存在, 重新创建了");
            }
            var folder = Application.streamingAssetsPath;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var directory = new DirectoryInfo(folder);
            var files = directory.GetFiles("*.ab", SearchOption.AllDirectories);
            var list = new List<StreamingAssetsManifestItem>(files.Length);
            for (int i = 0, max = files.Length; i < max; ++i)
            {
                var file = files[i];
                StreamingAssetsManifestItem item = new StreamingAssetsManifestItem();
                item.Paths = GetPaths(file.FullName);
                list.Add(item);
            }

            config.ManifestItems = list.ToArray();
        }

        private static string[] GetPaths(string path)
        {
            path = path.Replace("\\", "/");
            path = path.Replace(Application.streamingAssetsPath + "/", string.Empty);
            var arr = path.Split('/');
            return arr;
        }

        [MenuItem("Build/Build AssetBundle")]
        private static void BundleAssetBundle()
        {
            var output = Application.streamingAssetsPath;
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            BuildPipeline.BuildAssetBundles(output, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}
