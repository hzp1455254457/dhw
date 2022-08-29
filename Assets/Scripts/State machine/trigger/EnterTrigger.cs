using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class EnterTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {
            return fSM.IsWalked;
        }

        public override void lnit()
        {
            triggerID = FSMTriggerID.Enter;
        }
    }
    }