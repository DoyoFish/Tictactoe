using System;
using UnityEngine;

namespace Doyo.Assets
{
    public class StreamingAssetsManifest : ScriptableObject


    {
        public StreamingAssetsManifestItem[] ManifestItems;
        
        public static StreamingAssetsManifest Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.Load<StreamingAssetsManifest>("StreamingAssetsManifest");
                }
                return _instance;
            }
        }
        private static StreamingAssetsManifest _instance;
    }

    [Serializable]
    public class StreamingAssetsManifestItem
    {
        public string[] Paths;

        public bool SyncOnce;

        public bool Synced { get; set; }

        public string Path
        {
            get
            {
                var path = PathUtils.Combine(Paths);
                if (!path.EndsWith(".ab"))
                {
                    path += ".ab";
                }
                return path;
            }
        }
    }
}
