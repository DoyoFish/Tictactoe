using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameResult
{
    None,
    DrawGame,
    PlayerWin,
    PlayerLose,
}

public static class GameResultCalculator
{
    public static GameResult GetGameResult(ChessBoardData data, ChessPiecesType currentChess, ChessPiecesType playerChess)
    {
        var currentChesses = new List<int>();
        foreach (var pair in data.GetAllBoxes())
        {
            if (pair.Value == currentChess)
            {
                currentChesses.Add(pair.Key);
            }
        }

        // 计算横行
        int[] horizental = new int[3];
        int[] vertical = new int[3];

        bool matchCond = false;

        for (int i = 0, max = currentChesses.Count; i < max; ++i)
        {
            int h = currentChesses[i] / 3;
            int v = currentChesses[i] % 3;
            horizental[h]++;
            vertical[v]++;
            if (horizental[h] == 3 || vertical[v] == 3)
            {
                matchCond = true;
            }
        }

        if (!matchCond)
        {
            // 计算2个对角线
            // 0, 4, 8 or 2, 4, 6
            Func<int, bool> simplify = (a) =>
            {
                return currentChesses.Contains(a);
            };
            if ((simplify(0) && simplify(4) && simplify(8)) ||
                (simplify(2) && simplify(4) && simplify(6)))
            {
                matchCond = true;
            }
        }

        if (!matchCond)
        {
            return GameResult.None;
        }
        return playerChess == currentChess ? GameResult.PlayerWin : GameResult.PlayerLose;
    }


}