using Doyo.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoSingleton<GameMng>
{
    public static readonly int MaxTurns = 9;
    public static readonly int MaxPiecesCount = 9;

    public ChessBoard ChessBoard { get; set; }

    public int PlayerTurns { get; set; }
    public int AITurns { get; set; }

    public bool Gaming { get; private set; }

    protected override void Construct()
    {
        ChessBoard = new ChessBoard();
        ChessBoard.PlayerChess = ChessPiecesType.None;
    }

    public void SetGameInfo(ChessPiecesType playerChess, ChessPlayerType whoFirst)
    {
        ChessBoard.PlayerChess = playerChess;
        ChessBoard.CurrentChessPlayer = whoFirst;
        MsgMng.Dispatch(MsgTypeEnum.OnTurnRefresh);
    }

    public void LoadGameInfo()
    {
        PlayerTurns = ProfileMng.Instance.GameDataProfile.PlayerTurns;
        AITurns = ProfileMng.Instance.GameDataProfile.AITurns;
        ChessBoard.Copy(ProfileMng.Instance.GameDataProfile.ChessBoard);
    }

    public void GameStart()
    {
        Gaming = true;
        if (ChessBoard.CurrentChessPlayer == ChessPlayerType.AI)
        {
            DelayDoAISelect();
        }
    }

    public void LayChesss(int position)
    {
        var currentPiece = GetCurrentPiece();
        ChessBoard.Data[position] = currentPiece;
        SwitchChessPlayer();
    }

    public ChessPiecesType GetCurrentPiece()
    {
        var currentChessPlayer = ChessBoard.CurrentChessPlayer;
        if (currentChessPlayer == ChessPlayerType.AI)
        {
            return ChessBoard.PlayerChess == ChessPiecesType.X ? ChessPiecesType.O : ChessPiecesType.X;
        }
        else
        {
            return ChessBoard.PlayerChess == ChessPiecesType.X ? ChessPiecesType.X : ChessPiecesType.O;
        }
    }

    public void SwitchChessPlayer()
    {
        if (ChessBoard.CurrentChessPlayer == ChessPlayerType.Player)
        {
            PlayerTurns++;
        }
        else
        {
            AITurns++;
        }
        ChessBoard.Turn++;
        var gameResult = GameResultCalculator.GetGameResult(ChessBoard.Data, GetCurrentPiece(), ChessBoard.PlayerChess);
        if (gameResult != GameResult.None)
        {
            MsgMng.Dispatch(MsgTypeEnum.OnGameResult, new Message((int)gameResult));            
            ProfileMng.Instance.GameDataProfile.Clear();
            return;
        }
        if (ChessBoard.Turn >= MaxTurns)
        {
            // 平局
            MsgMng.Dispatch(MsgTypeEnum.OnGameResult, new Message((int)GameResult.DrawGame));
            ProfileMng.Instance.GameDataProfile.Clear();
            return;
        }
        ChessBoard.CurrentChessPlayer = ChessBoard.CurrentChessPlayer == ChessPlayerType.Player ?
            ChessPlayerType.AI : ChessPlayerType.Player;
        MsgMng.Dispatch(MsgTypeEnum.OnTurnRefresh);
        if (ChessBoard.CurrentChessPlayer == ChessPlayerType.AI)
        {
            DelayDoAISelect();
        }
    }

    private void DelayDoAISelect()
    {
        Invoke("DoAISelect", 1);
    }

    private void DoAISelect()
    {
        var position = AIMng.Instance.GetAILay();
        if (position == -1)
        {
            return;
        }
        AITurns++;
        MsgMng.Dispatch(MsgTypeEnum.OnAISelectBoardBox, new Message(position));
    }

    public void GameEnded()
    {
        Gaming = false;
        ChessBoard.Clear();
        AITurns = 0;
        PlayerTurns = 0;
    }


    protected override void Release()
    {
    }
}
