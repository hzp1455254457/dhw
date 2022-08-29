using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///攻击状态
///</summary>
///
namespace AI.FSM
{
    public class IdleState : FSMState
    {
        float times;
        public override void Action(BaseFSM baseFSM)
        {
            //if (baseFSM.IsIdle)
            //{
            //    times += Time.deltaTime;
            //    if (times >= 3)
            //    {
            //        times = 0;
            //        baseFSM.IsIdle = false;
            //    }
            //}
        }

        public override void lnit()
        {
            stateId = FSMStateID.Idle;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
            times = 0;
            base.EnterState(baseFSM);
            baseFSM.StopMove();
        }
        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
            //baseFSM.IsWalked = true;
        }
    }
}