using UIBinding;
using UnityEngine;
using UnityEngine.UI;

public class UserInputPanel : UIPanelBase
{
    public override UICommon UIPanelName => UICommon.UserInputPanel;

    public override PanelType PanelType => PanelType.TipsPanel;

    public override UIStackLevel StackLevel => UIStackLevel.Higher;

    public InputField UserInput;

    private string _prefsKey;

    public override void OnInit(object caller)
    {

    }

    public override void OnShow(object caller)
    {
        _prefsKey = (string)Msg["key"];
    }

    public void OnTapConfirm()
    {
        if (string.IsNullOrEmpty(_prefsKey))
        {
            return;
        }
        PlayerPrefs.SetString(_prefsKey, UserInput.text);
        OnTapClose();
    }

    public void OnTapClose()
    {
        UIPanelMng.Instance.ClosePanel(this, this.UIPanelName, false, true);
    }

    public override void OnClose(object caller)
    {
        var panel = UIPanelMng.Instance.GetPanelByName<LoginPanel>();
        if (panel)
        {
            panel.OnRefresh(this);
        }
    }
}