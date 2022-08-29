using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///条件类
///</summary>
///
namespace AI.FSM
{
    abstract public class FSMTrigger
    {
        public FSMTrigger() { lnit(); }
        public FSMTriggerID triggerID;
        //条件编号
       
        abstract public void lnit();
        // 初始化
        abstract public bool HandleTrigger(BaseFSM fSM);
        //bool检测条件是否达成
    }
}