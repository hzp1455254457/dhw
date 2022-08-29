using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class ExitTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {
           
            return fSM.IsBuyed && MainUI.Instance.shouyinCount < MainUI.Instance.shouyinMax && PeopleManager.Instance.GoTargetpeopleControls.Count< MainUI.Instance.shouyinMax - MainUI.Instance.shouyinCount && PeopleManager.Instance.paiduiPeopleControls.Count<=0;
        }

        public override void lnit()
        {
            triggerID = FSMTriggerID.Exit;
        }
    }
    }