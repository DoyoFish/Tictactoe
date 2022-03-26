using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : UIPanelBase
{
    public override UICommon UIPanelName => UICommon.OptionsPanel;

    public override PanelType PanelType => PanelType.TipsPanel;

    public Toggle FirstOptionall;
    public Toggle HardOptional;

    private Action _callback;

    private bool _playerFirst = true;
    private bool _easyMode = true;
    private ChessPiecesType _playerChess = ChessPiecesType.O;

    public override void OnInit(object caller)
    {
        FirstOptionall.onValueChanged.AddListener(OnFirstOptionalChanged);
        HardOptional.onValueChanged.AddListener(OnHardOptionalChanged);
    }

    public override void OnShow(object caller)
    {
        _callback = Msg["callback"].As<Action>();
    }

    private void OnFirstOptionalChanged(bool value)
    {
        _playerFirst = value;
    }

    private void OnHardOptionalChanged(bool value)
    {
        _easyMode = value;
    }

    public void OnTapEnter()
    {
        GameMng.Instance.SetGameInfo(_playerChess, _playerFirst ? ChessPlayerType.Player : ChessPlayerType.AI);
        AIMng.Instance.AILevel = _easyMode ? AILevel.Easy : AILevel.Normal;
        _callback?.Invoke();
        UIPanelMng.Instance.ClosePanel(this, this.UIPanelName, false, true);
        UIPanelMng.Instance.ClosePanel(this, UICommon.LoginPanel, false, true);
    }

    public void OnTapClose()
    {
        UIPanelMng.Instance.ClosePanel(this, this.UIPanelName, false, true);
    }

    public override void OnClose(object caller)
    {

    }

}