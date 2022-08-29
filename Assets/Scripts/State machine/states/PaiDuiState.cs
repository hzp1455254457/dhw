using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///排队状态
///</summary>
///
namespace AI.FSM
{
    public class PaiDuiState : FSMState
    {
        bool isArrived = false;
        public override void Action(BaseFSM baseFSM)
        {
           if (!isArrived)
            {if (baseFSM.GetDistance() <= 0.01f)
                {
                    baseFSM.StopMove(true);
                    baseFSM.peopleControl.SetScale(false) ;
                    isArrived = true;
                    baseFSM.peopleUI.ShowBiaoQing();
                }
            }
        
        }
        
        public override void lnit()
        {
            stateId = FSMStateID.PaiDui;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
            base.EnterState(baseFSM);
            PeopleManager.Instance.AddPaiDuiP(baseFSM);
           // baseFSM.MoveToTarget(PosManager.Instance.GetKonXianPos().vector2, 1, 1);
            baseFSM.peopleUI.HideUI();
            baseFSM.IsPaidui = true;
            isArrived = false;
            //baseFSM.peopleControl.SetScale(false);
        }
        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
            PeopleManager.Instance.RemovePaiDuiPeople(baseFSM);
            //baseFSM.IsWalked = true;
        }
    }
}