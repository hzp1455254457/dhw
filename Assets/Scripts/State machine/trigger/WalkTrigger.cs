using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class WalkTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {

            return !fSM.IsIdle&&!fSM.IsWalked;
        }

        public override void lnit()
        {
            triggerID = FSMTriggerID.Walk;
        }
    }
    }