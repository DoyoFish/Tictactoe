using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Doyo.Assets
{
    public static class AssetBundleHelper
    {

        private static readonly string RelativePath = "file://" + Application.persistentDataPath;

        /// <summary>
        /// 从persitantDataPath加载资源
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="complete"></param>
        /// <returns></returns>
        public static IEnumerator Load(string asset, Action<string, AssetBundle> complete)
        {
            var path = RelativePath + "/" + asset;
            Debug.LogFormat("path:{0}", path);
            var request = UnityWebRequestAssetBundle.GetAssetBundle(path);
            yield return request.SendWebRequest();
            if (request.error == null)
            {
                var ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
                if (complete != null)
                {
                    Debug.LogFormat("Load {0} success", asset);
                    complete.Invoke(ab.name, ab);
                }
            }
            else
            {
                Debug.LogErrorFormat("Load {0} failed, error: {1}", asset, request.error);
                if (complete != null)
                {
                    complete.Invoke(asset, null);
                }
            }
            request.Dispose();
        }
    }
}
