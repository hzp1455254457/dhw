using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.FSM
{
    public class PaiDuiTrigger : FSMTrigger
    {
        public override bool HandleTrigger(BaseFSM fSM)
        {
            return (MainUI.Instance.shouyinCount>= MainUI.Instance.shouyinMax && fSM.IsBuyed)||(fSM.IsBuyed&& PeopleManager.Instance.GoTargetpeopleControls.Count >= MainUI.Instance.shouyinMax - MainUI.Instance.shouyinCount);
        }

        public override void lnit()
        {
            triggerID = FSMTriggerID.PaiDui;
        }
    }
    }