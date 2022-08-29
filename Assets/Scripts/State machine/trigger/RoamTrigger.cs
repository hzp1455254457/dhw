using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class RoamTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {

            if (fSM.GetDistance() <= 0.01)
            {
              
                return true;
            }
            else return false;
        }

        public override void lnit()
        {
            triggerID = FSMTriggerID.Roam;
        }
    }
    }