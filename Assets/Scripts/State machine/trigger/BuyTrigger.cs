using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class BuyTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {

            return fSM.IsHavePro;
        }

        public override void lnit()
        {
            triggerID = FSMTriggerID.Buy;
        }
    }
    }