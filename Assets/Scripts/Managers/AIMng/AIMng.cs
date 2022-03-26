using Doyo.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AILevel
{
    Easy, // 随机落子
    Normal, // 尽可能尝试赢和堵
}

public class AIMng : Singleton<AIMng>
{
    public AILevel AILevel { get; set; }

    public AIMng()
    {
        AILevel = AILevel.Easy;
    }

    public int GetAILay()
    {
        if (AILevel == AILevel.Easy)
        {
            var emptyBoxes = GameMng.Instance.ChessBoard.Data.GetEmptyBoxes();
            if (emptyBoxes.Count == 0)
            {
                return -1;
            }
            return emptyBoxes.GetRandom();
        }
        return CalculateLay();
    }

    private int CalculateLay()
    {
        var allBoxes = GameMng.Instance.ChessBoard.Data.GetAllBoxes();
        var selfChess = GameMng.Instance.ChessBoard.PlayerChess == ChessPiecesType.O ? ChessPiecesType.X : ChessPiecesType.O;
        var enermyChesses = new List<int>();
        var selfChesses = new List<int>();
        var emptyBoxes = new List<int>();

        // 计算自己行列
        int[] selfHorizental = new int[3];
        int[] selfVertical = new int[3];

        // 计算敌人行列
        int[] enermyHorizontal = new int[3];
        int[] enermyVertical = new int[3];

        int h;
        int v;

        for (int i = 0; i < GameMng.MaxPiecesCount; ++i)
        {
            h = i / 3;
            v = i % 3;
            if (!allBoxes.ContainsKey(i) || allBoxes[i] == ChessPiecesType.None)
            {
                emptyBoxes.Add(i);
            }
            else if (allBoxes.ContainsKey(i) && allBoxes[i] == selfChess)
            {
                selfChesses.Add(i);
                selfHorizental[h]++;
                selfVertical[v]++;
            }
            else if (allBoxes.ContainsKey(i) && allBoxes[i] == GameMng.Instance.ChessBoard.PlayerChess)
            {
                enermyChesses.Add(i);
                enermyHorizontal[h]++;
                enermyVertical[v]++;
            }
        }

        if (emptyBoxes.Count == 0)
        {
            return -1;
        }

        // 如果正中间可以下
        if (emptyBoxes.Contains(4))
        {
            return 4;
        }

        // 3是行数或者列数
        int result = -1;
        for (int i = 0; i < 3; ++i)
        {
            if (selfHorizental[i] == 2)
            {
                for (int emptyIndex = 0, emptyCount = emptyBoxes.Count; emptyIndex < emptyCount; ++emptyIndex)
                {
                    if (emptyBoxes[emptyIndex] / 3 == i)
                    {
                        result = emptyBoxes[emptyIndex];
                        break;
                    }
                }
            }
            if (selfVertical[i] == 2)
            {
                for (int emptyIndex = 0, emptyCount = emptyBoxes.Count; emptyIndex < emptyCount; ++emptyIndex)
                {
                    if (emptyBoxes[emptyIndex] % 3 == i)
                    {
                        result = emptyBoxes[emptyIndex];
                        break;
                    }
                }
            }
        }
        // 对角线再试一次
        Func<int, bool> selfSimplify = (a) =>
        {
            return selfChesses.Contains(a);
        };

        Func<int, bool> enermySimplify = (b) =>
        {
            return enermyChesses.Contains(b);
        };

        int[] leftOblique = new int[] { 0, 4, 8 };
        int[] rightOblique = new int[] { 2, 4, 6 };
        List<int> targets = new List<int>(3);
        if (result == -1)
        {
            for (int i = 0, max = leftOblique.Length; i < max; ++i)
            {
                if (!selfSimplify(leftOblique[i]))
                {
                    targets.Add(leftOblique[i]);
                }
            }
            if (targets.Count == 1 && emptyBoxes.Contains(targets[0]))
            {
                return targets[0];
            }

            targets.Clear();
            for (int i = 0, max = rightOblique.Length; i < max; ++i)
            {
                if (!selfSimplify(rightOblique[i]))
                {
                    targets.Add(rightOblique[i]);
                }
            }
            if (targets.Count == 1 && emptyBoxes.Contains(targets[0]))
            {
                return targets[0];
            }

            if (result == -1)
            {
                // 那就堵别人的
                for (int i = 0; i < 3; ++i)
                {
                    if (enermyHorizontal[i] == 2)
                    {
                        for (int emptyIndex = 0, emptyCount = emptyBoxes.Count; emptyIndex < emptyCount; ++emptyIndex)
                        {
                            if (emptyBoxes[emptyIndex] / 3 == i)
                            {
                                result = emptyBoxes[emptyIndex];
                                break;
                            }
                        }
                    }
                    if (enermyVertical[i] == 2)
                    {
                        for (int emptyIndex = 0, emptyCount = emptyBoxes.Count; emptyIndex < emptyCount; ++emptyIndex)
                        {
                            if (emptyBoxes[emptyIndex] % 3 == i)
                            {
                                result = emptyBoxes[emptyIndex];
                                break;
                            }
                        }
                    }
                }

                if (result == -1)
                {
                    targets.Clear();
                    // 对角线堵敌人
                    for (int i = 0, max = leftOblique.Length; i < max; ++i)
                    {
                        if (!enermySimplify(leftOblique[i]))
                        {
                            targets.Add(leftOblique[i]);
                        }
                    }
                    if (targets.Count == 1 && emptyBoxes.Contains(targets[0]))
                    {
                        return targets[0];
                    }

                    targets.Clear();
                    for (int i = 0, max = rightOblique.Length; i < max; ++i)
                    {
                        if (!enermySimplify(rightOblique[i]))
                        {
                            targets.Add(rightOblique[i]);
                        }
                    }
                    if (targets.Count == 1 && emptyBoxes.Contains(targets[0]))
                    {
                        return targets[0];
                    }
                }
            }

            if (result != -1)
            {
                return result;
            }

            Debug.Log(emptyBoxes.Connect("|"));
            return emptyBoxes.GetRandom();
        }
        return result;
    }
}
