using Doyo.Assets;
using System.Collections;
using System.Collections.Generic;
using UIBinding;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    
    IEnumerator Start()
    {
        yield return StartCoroutine(AssetsSyncMng.AndroidAssetSync());
        yield return StartCoroutine(AssetTools.Initialize());
        UGUIImageBinding.SpriteLoader = Loader.LoadSprite;
        UGUIOnClickBinding.MusicEffect = (music) => AudioMng.Instance.PlayEffect(music);
        AudioMng.Instance.BgmVolumn = ProfileMng.Instance.SettingsProfile.BgmVolume;
        AudioMng.Instance.EffectVolumn = ProfileMng.Instance.SettingsProfile.EffectVolumn;
        UIPanelMng.Instance.OpenPanel(this, UICommon.LoginPanel);    
    }

   
}
