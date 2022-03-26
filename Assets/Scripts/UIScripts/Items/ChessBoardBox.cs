using Doyo.Common;
using System.Collections;
using System.Collections.Generic;
using UIBinding;
using UnityEngine;
using UnityEngine.UI;

public class ChessBoardBox : ItemContext
{

    public string ChessBg
    {
        get { return _propertyChessBg.GetValue(); }
        set { _propertyChessBg.SetValue(value); }
    }
    private readonly Property<string> _propertyChessBg = new Property<string>();

    public int Position { get; set; }

    public void OnClick()
    {
        if (GameMng.Instance.ChessBoard.Turn >= 9)
        {
            return;
        }
        if (GameMng.Instance.ChessBoard.CurrentChessPlayer != ChessPlayerType.Player)
        {
            return;
        }
        MsgMng.Dispatch(MsgTypeEnum.OnTapChessBoardBox, this);
    }
}
