using System;
using System.Collections;
using System.Collections.Generic;
using UIBinding;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : UIPanelBase
{
    public override UICommon UIPanelName => UICommon.GameOverPanel;

    public override PanelType PanelType => PanelType.TipsPanel;

    public override UIStackLevel StackLevel => UIStackLevel.Higher;

    public string Result
    {
        get { return _propertyResult.GetValue(); }
        set { _propertyResult.SetValue(value); }
    }
    private readonly Property<string> _propertyResult = new Property<string>();

    public override void OnInit(object caller)
    {

    }

    public override void OnShow(object caller)
    {
        var gameResult = (GameResult)(int)Msg["result"];
        switch (gameResult)
        {
            case GameResult.DrawGame:
                Result = "平局";
                break;
            case GameResult.PlayerLose:
                Result = "胜败乃兵家常事, 请下次努力";
                break;
            case GameResult.PlayerWin:
                Result = "你赢了";
                break;

        }
    }

    public override void OnClose(object caller)
    {
        if (UIPanelMng.Instance.IsPanelOpened(UICommon.ChessBoardPanel))
        {
            UIPanelMng.Instance.ClosePanel(this, UICommon.ChessBoardPanel, false, true);
        }
        GameMng.Instance.GameEnded();
    }

    public void OnTapClose()
    {
        UIPanelMng.Instance.OpenPanel(this, UICommon.LoginPanel, null, false);
        UIPanelMng.Instance.ClosePanel(this, this.UIPanelName, false, true);
    }

}