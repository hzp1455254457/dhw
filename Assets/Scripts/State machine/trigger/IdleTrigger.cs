using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class IdleTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {

            return fSM.IsIdle;
        }

        public override void lnit()
        {
            triggerID = FSMTriggerID.Idle;
        }
    }
    }