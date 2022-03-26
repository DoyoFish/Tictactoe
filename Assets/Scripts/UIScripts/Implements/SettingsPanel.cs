using System.Collections;
using System.Collections.Generic;
using UIBinding;
using UnityEngine;

public class SettingsPanel : UIPanelBase
{
    public override UICommon UIPanelName => UICommon.SettingsPanel;

    public override PanelType PanelType => PanelType.MainPanel;

    public int BgmVolume
    {
        get { return _propertyBgmVolume.GetValue(); }
        set { _propertyBgmVolume.SetValue(value); }
    }
    private readonly Property<int> _propertyBgmVolume = new Property<int>();

    public int EffectVolume
    {
        get { return _propertyEffectVolume.GetValue(); }
        set { _propertyEffectVolume.SetValue(value); }
    }
    private readonly Property<int> _propertyEffectVolume = new Property<int>();

    public bool QuitGameVisible
    {
        get { return _propertyQuitGameVisible.GetValue(); }
        set { _propertyQuitGameVisible.SetValue(value); }
    }
    private readonly Property<bool> _propertyQuitGameVisible = new Property<bool>();

    private UICommon _lastPanel;

    public override void OnInit(object caller)
    {
    }

    public override void OnShow(object caller)
    {
        BgmVolume = ProfileMng.Instance.SettingsProfile.BgmVolume;
        EffectVolume = ProfileMng.Instance.SettingsProfile.EffectVolumn;
        QuitGameVisible = GameMng.Instance.Gaming;
        _lastPanel = (UICommon)(int)Msg["lastPanel"];
    }

    public void OnTapAddBgm()
    {
        var volume = BgmVolume;
        volume = Mathf.Clamp(++volume, 0, 10);
        BgmVolume = volume;
    }

    public void OnTapSubBgm()
    {
        var volume = BgmVolume;
        volume = Mathf.Clamp(--volume, 0, 10);
        BgmVolume = volume;
    }

    public void OnTapAddEff()
    {
        var volume = EffectVolume;
        volume = Mathf.Clamp(++volume, 0, 10);
        EffectVolume = volume;
    }

    public void OnTapSubEff()
    {
        var volume = EffectVolume;
        volume = Mathf.Clamp(--volume, 0, 10);
        EffectVolume = volume;
    }

    public void OnTapClean()
    {
        UIPanelMng.Instance.OpenMsgBox(this, "Warnning", "即将清除缓存", () =>
        {
            PlayerPrefs.DeleteAll();
        });
    }

    public void OnTapQuitGame()
    {
        UIPanelMng.Instance.OpenMsgBox(this, "Warning", "是否保存进度", () => QuitGame(true), () => QuitGame(false));
    }


    private void QuitGame(bool save)
    {
        if (save)
        {
            ProfileMng.Instance.GameDataProfile.SaveProgrss();
        }
        else
        {
            ProfileMng.Instance.GameDataProfile.Clear();
        }
        MsgMng.Dispatch(MsgTypeEnum.OnQuitGame);
        GameMng.Instance.GameEnded();
    }

    public void OnTapClose()
    {
        UIPanelMng.Instance.ClosePanel(this, this.UIPanelName, false, true);
    }

    public override void OnClose(object caller)
    {
        ProfileMng.Instance.SettingsProfile.BgmVolume = BgmVolume;
        ProfileMng.Instance.SettingsProfile.EffectVolumn = EffectVolume;

        AudioMng.Instance.BgmVolumn = BgmVolume;
        AudioMng.Instance.EffectVolumn = EffectVolume;

        var panel = UIPanelMng.Instance.GetPanelByName(_lastPanel);
        if (panel)
        {
            panel.OnRefresh(this);
        }
    }

    public override void OnEscapeCommand(object caller, EscapeCommandEventArgs e)
    {
        OnTapClose();
    }
}
