using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///攻击状态
///</summary>
///
namespace AI.FSM
{
    public class EnterGameState : FSMState
    {
       
        public override void Action(BaseFSM baseFSM)
        {
            if (baseFSM.targetTransform != null)
            {
              
               
            }
        }
        
        public override void lnit()
        {
            stateId = FSMStateID.EnterGame;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
            base.EnterState(baseFSM);
            baseFSM.targetTransform = PosManager.Instance.enterTransForm;
            baseFSM.MoveToTarget(baseFSM.targetTransform.position, 1, 1);
           // baseFSM.peopleUI.HideUI();
            baseFSM.IsCanDestroy = false;
        }
        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
            AndroidHelper.Instance.UploadDataEvent("custom_enter_streest");
            //baseFSM.IsWalked = true;
        }
    }
}