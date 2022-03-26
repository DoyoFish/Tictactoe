using Doyo.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UIBinding;
using UnityEngine;
using UnityEngine.UI;

public class ChessBoardPanel : UIPanelBase
{
    public override UICommon UIPanelName => UICommon.ChessBoardPanel;

    public override PanelType PanelType => PanelType.MainPanel;

    public ChessBoardBox Template;
    public Transform ChessBoardBg;

    private readonly ChessBoardBox[] _chessBoardBoxes = new ChessBoardBox[9];

    public string CurrentPlayer
    {
        get { return _propertyCurrentPlayer.GetValue(); }
        set { _propertyCurrentPlayer.SetValue(value); }
    }
    private readonly Property<string> _propertyCurrentPlayer = new Property<string>();

    public int TotalTurn
    {
        get { return _propertyTotalTurn.GetValue(); }
        set { _propertyTotalTurn.SetValue(value); }
    }
    private readonly Property<int> _propertyTotalTurn = new Property<int>();

    public ChessPlayerInfo PlayerInfo;
    public ChessPlayerInfo AIInfo;

    public override void OnInit(object caller)
    {
        InitBoardBoxes();
    }

    private void InitBoardBoxes()
    {
        _chessBoardBoxes[0] = Template;
        Template.ChessBg = "Chess_Empty";
        Template.Position = 0;
        for (int i = 1; i < 9; i++)
        {
            var item = Instantiate(Template);
            item.transform.SetParent(ChessBoardBg, false);
            item.ChessBg = "Chess_Empty";
            item.Position = i;
            _chessBoardBoxes[i] = item;
        }
    }

    public override void OnShow(object caller)
    {
        LoadPlayer();
        LoadAI();
        TotalTurn = GameMng.Instance.ChessBoard.Turn;
        CurrentPlayer = GameMng.Instance.ChessBoard.CurrentChessPlayer == ChessPlayerType.AI ? "AI": ProfileMng.Instance.UserName;
        MsgMng.AddListener(MsgTypeEnum.OnTapChessBoardBox, OnPlayerSelectChessBoardBox);
        MsgMng.AddListener(MsgTypeEnum.OnAISelectBoardBox, OnAISelectChessBoardBox);
        MsgMng.AddListener(MsgTypeEnum.OnTurnRefresh, OnTurnRefresh);
        MsgMng.AddListener(MsgTypeEnum.OnGameResult, OnGameResult);
        MsgMng.AddListener(MsgTypeEnum.OnQuitGame, OnQuitGame);
    }

    private void LoadPlayer()
    {
        PlayerInfo.Alias = ProfileMng.Instance.UserName;
        if (string.IsNullOrEmpty(PlayerInfo.Alias))
        {
            PlayerInfo.Alias = "玩家没有名字";
        }
        PlayerInfo.Sign = ProfileMng.Instance.UserSign;
        if (string.IsNullOrEmpty(PlayerInfo.Sign))
        {
            PlayerInfo.Sign = "玩家不想说话";
        }
        PlayerInfo.ChessType = GetChessType(GameMng.Instance.ChessBoard.PlayerChess);
        PlayerInfo.Turn = GameMng.Instance.PlayerTurns;
        PlayerInfo.HeadImage = ProfileMng.Instance.UserAvatar;
    }

    private void LoadAI()
    {
        AIInfo.Alias = "AI";
        AIInfo.Sign = "我这就放到你";
        AIInfo.ChessType = GetChessType(GameMng.Instance.ChessBoard.PlayerChess == ChessPiecesType.O ? ChessPiecesType.X : ChessPiecesType.O);
        AIInfo.Turn = GameMng.Instance.AITurns;
        AIInfo.HeadImage = "avatar_ai";
    }

    private string GetChessType(ChessPiecesType chessType)
    {
        switch (chessType)
        {
            case ChessPiecesType.O:
                return "Chess_O";
            case ChessPiecesType.X:
                return "Chess_X";
            default:
                return "Chess_Empty";
        }
    }

    public override void OnClose(object caller)
    {
        MsgMng.RemoveListener(MsgTypeEnum.OnTapChessBoardBox, OnPlayerSelectChessBoardBox);
        MsgMng.RemoveListener(MsgTypeEnum.OnAISelectBoardBox, OnAISelectChessBoardBox);
        MsgMng.RemoveListener(MsgTypeEnum.OnTurnRefresh, OnTurnRefresh);
        MsgMng.RemoveListener(MsgTypeEnum.OnGameResult, OnGameResult);
        MsgMng.RemoveListener(MsgTypeEnum.OnQuitGame, OnQuitGame);
        ClearBox();
    }

    private void ClearBox()
    {
        for (int i = 0; i < GameMng.MaxPiecesCount; ++i)
        {
            _chessBoardBoxes[i].ChessBg = "Chess_Empty";
        }
    }

    private void OnPlayerSelectChessBoardBox(Message msg)
    {
        var box = msg.As<ChessBoardBox>();
        PlayerInfo.Turn++;
        DrawChess(box);
    }

    private void OnAISelectChessBoardBox(Message msg)
    {
        var position = (int)msg;
        AIInfo.Turn++;
        var box = _chessBoardBoxes[position];
        DrawChess(box);
    }

    private void OnTurnRefresh(Message msg)
    {
        TotalTurn = GameMng.Instance.ChessBoard.Turn;
        CurrentPlayer = GameMng.Instance.ChessBoard.CurrentChessPlayer == ChessPlayerType.AI ? "AI" : ProfileMng.Instance.UserName;
    }

    private void OnGameResult(Message msg)
    {
        var gameResult = (GameResult)(int)msg;
        if (gameResult != GameResult.None)
        {
            var resultMsg = new Message();
            resultMsg["result"] = (int)msg;
            UIPanelMng.Instance.OpenPanel(this, UICommon.GameOverPanel, resultMsg, false);
        }
    }

    private void OnQuitGame(Message msg)
    {
        UIPanelMng.Instance.ClosePanel(this, this.UIPanelName, false, true);
        UIPanelMng.Instance.OpenPanel(this, UICommon.LoginPanel, null, false);
    }

    private void DrawChess(ChessBoardBox box)
    {
        var currentPiece = GameMng.Instance.GetCurrentPiece();
        LayChess(box, currentPiece);
        GameMng.Instance.LayChesss(box.Position);
    }

    private void LayChess(ChessBoardBox box, ChessPiecesType currenPiece)
    {
        box.ChessBg = currenPiece == ChessPiecesType.O ? "Chess_O" : "Chess_x";
    }

    public void OnTapSettings()
    {
        var msg = new Message();
        msg["lastPanel"] = (int)this.UIPanelName;
        UIPanelMng.Instance.OpenPanel(this, UICommon.SettingsPanel, msg, false);
    }
}