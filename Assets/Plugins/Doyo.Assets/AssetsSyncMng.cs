using Doyo.UnityFramework;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Doyo.Assets
{
    public static class AssetsSyncMng
    {
        public static string[] FastFollowPaths { get; private set; }
        public static string[] OnDemandPaths { get; private set; }

        private static int AssetPackPathIndex
        {
            get
            {
                // 定义规则, 从FastFollow开始
                if (_fastFollowPathIndex < FastFollowPaths.Length)
                {
                    return _fastFollowPathIndex;
                }
                return _onDemandPathIndex;
            }
            set
            {
                // 设为0, 重置
                if (value == 0)
                {
                    _fastFollowPathIndex = _onDemandPathIndex = 0;
                    return;
                }

                // 使之等于Length以返回空值
                if (_fastFollowPathIndex <= FastFollowPaths.Length)
                {
                    _fastFollowPathIndex = value;
                }
                else
                {
                    _onDemandPathIndex = value;
                }
            }
        }

        private static string AssetPackPath
        {
            get
            {
                var path = GetFastFollowPath();
                return !string.IsNullOrEmpty(path) ? path : GetOnDemandPath();
            }
        }

        private static bool _tryFindInAssetPack = false;
        private static int _fastFollowPathIndex = 0;
        private static int _onDemandPathIndex = 0;

        public static IEnumerator AndroidAssetSync()
        {
            var items = StreamingAssetsManifest.Instance.ManifestItems;

            for (int i = 0, max = items.Length; i < max; ++i)
            {
                var item = items[i];
                yield return AndroidAssetSyncInternal(item.Path).StartCoroutine();
            }
        }

        private static IEnumerator AndroidAssetSyncInternal(string asset, string parent = null)
        {
            parent = !string.IsNullOrEmpty(parent) ? parent : Application.streamingAssetsPath;
            var url = PathUtils.Combine(parent, asset);
            if (!url.Contains("://"))
            {
                url = "file://" + url;
            }
            Debug.LogFormat("need sync, url: {0}", url);
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.error == null)
            {
                request.OnSuccess(asset);
            }
            else
            {
                yield return request.OnFailed(asset).StartCoroutine();
            }
        }

        private static void OnSuccess(this UnityWebRequest request, string asset)
        {
            if (_tryFindInAssetPack)
            {
                _tryFindInAssetPack = false;
                AssetPackPathIndex = 0;
            }

            var data = request.downloadHandler.data;
            request.Dispose();
            var destPath = Path.Combine(Application.persistentDataPath, asset);
            if (!Directory.Exists(Path.GetDirectoryName(destPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
            }
            File.WriteAllBytes(destPath, data);
            Debug.LogFormat("sync {0} success", asset);
        }

        private static IEnumerator OnFailed(this UnityWebRequest request, string asset)
        {
            var error = request.error;
            request.Dispose();
            Debug.LogErrorFormat("sync {0} failed, because of {1}", asset, error);
            if (_tryFindInAssetPack)
            {
                AssetPackPathIndex++;
            }
            _tryFindInAssetPack = true;
            if (string.IsNullOrEmpty(AssetPackPath))
            {
                AssetPackPathIndex = 0;
                yield break;
            }

            yield return AndroidAssetSyncInternal(asset, AssetPackPath).StartCoroutine();
        }

        private static string GetFastFollowPath()
        {
            if (_fastFollowPathIndex < FastFollowPaths.Length)
            {
                return FastFollowPaths[_fastFollowPathIndex];
            }
            return string.Empty;
        }

        private static string GetOnDemandPath()
        {
            if (_onDemandPathIndex < OnDemandPaths.Length)
            {
                return OnDemandPaths[_onDemandPathIndex];
            }
            return string.Empty;
        }
    }
}
