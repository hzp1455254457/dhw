using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class DesdroyTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {
            return fSM.GetDistance()<=0.01&&fSM.IsCanDestroy&&!GuideManager.Instance.isFirstGame;
        }

        public override void lnit()
        {
            triggerID = FSMTriggerID.Desdroy;
        }
    }
    }