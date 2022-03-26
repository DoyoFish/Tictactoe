using Doyo.Common;
using System;
using UIBinding;

public class LoginPanel : UIPanelBase
{
    public override UICommon UIPanelName { get { return UICommon.LoginPanel; } }

    public override PanelType PanelType => PanelType.MainPanel;

    public string UserName
    {
        get { return _propertyUserName.GetValue(); }
        set { _propertyUserName.SetValue(value); }
    }
    private readonly Property<string> _propertyUserName = new Property<string>();

    public string UserSign
    {
        get { return _propertyUserSign.GetValue(); }
        set { _propertyUserSign.SetValue(value); }
    }
    private readonly Property<string> _propertyUserSign = new Property<string>();

    public bool HasLastGame
    {
        get { return _propertyHasLastGame.GetValue(); }
        set { _propertyHasLastGame.SetValue(value); }
    }
    private Property<bool> _propertyHasLastGame = new Property<bool>();

    public override void OnInit(object caller)
    {
        AudioMng.Instance.PlayBgm(MusicEnum.Main01);
    }

    public override void OnShow(object caller)
    {
        ShowPage();
    }

    public override void OnRefresh(object caller)
    {
        ShowPage();
    }

    private void ShowPage()
    {
        HasLastGame = ProfileMng.Instance.GameDataProfile.HasLastGame;
        UserSign = ProfileMng.Instance.UserSign;
        UserSign = !string.IsNullOrEmpty(UserSign) || UserSign == "请随便写两句签名" ? UserSign : "请随便写两句签名";

        UserName = ProfileMng.Instance.UserName;
        UserName = !string.IsNullOrEmpty(UserName) || UserName == "请取个名字" ? UserName : "请取个名字";
    }

    public void OnTapStartGame()
    {
        var msg = new Message();
        msg["callback"] = new Message(new Action(EnterGame));
        UIPanelMng.Instance.OpenPanel(this, UICommon.OptionsPanel, msg, false);
    }

    public void OnTapLoadGame()
    {
        GameMng.Instance.LoadGameInfo();
        EnterGame();
    }

    private void EnterGame()
    {
        UIPanelMng.Instance.OpenPanel(this, UICommon.ChessBoardPanel, null, false);
        GameMng.Instance.GameStart();
        UIPanelMng.Instance.ClosePanel(this, UIPanelName, false, true);
    }

    public void OnTapSettings()
    {
        var msg = new Message();
        msg["lastPanel"] = (int)this.UIPanelName;
        UIPanelMng.Instance.OpenPanel(this, UICommon.SettingsPanel, msg, false);
    }

    public void OnTapModifyUserName()
    {
        var msg = new Message();
        msg["key"] = "UserName";
        UIPanelMng.Instance.OpenPanel(this, UICommon.UserInputPanel, msg, false);
    }

    public void OnTapModifyUserSign()
    {
        var msg = new Message();
        msg["key"] = "UserSign";
        UIPanelMng.Instance.OpenPanel(this, UICommon.UserInputPanel, msg, false);
    }

    public override void OnClose(object caller)
    {
    }

}
