using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息注册管理，最后两位表示参数个数，倒数三、四位表示监听器序号，倒数第五位开始表示监听器大类
/// </summary>
public enum MsgTypeEnum
{
    None = 0,
    //底层之间广播  10|00|00开始
    OnTapChessBoardBox = 100001, // 玩家选择了棋盘
    OnAISelectBoardBox = 100002, // AI选择了棋盘
    OnTurnRefresh      = 100003, // 新的回合
    OnQuitGame        = 101001,  // 退出游戏
    OnGameResult       = 101002, // 游戏结果 
}
