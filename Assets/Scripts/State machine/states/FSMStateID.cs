using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///状态id
///</summary>
///
namespace AI.FSM
{
 public   enum FSMStateID
    {
        None, //无
        Idle,   //待机
        Dead,  // 死亡
      Desdroy,
        Default,//默认
      
        Walk,
        EnterGame,
        Buy,
        Roam,
        ExitGame,
        MoveScreen,
            PaiDui,
    }
}