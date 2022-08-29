using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class MoveScreenTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {
            // return fSM.GetDistance() <= 0.1;
            //if (fSM.GetDistance() <= 0.1)
            //{

            //    return true;
            //}
            //else return false;
          
            return !fSM.IsPaidui;
        }
       
        public override void lnit()
        {
            triggerID = FSMTriggerID.MoveScreen;
        }
    }
    }