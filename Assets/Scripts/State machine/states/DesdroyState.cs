using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///攻击状态
///</summary>
///
namespace AI.FSM
{
    public class DesdroyState : FSMState
    {
       
        public override void Action(BaseFSM baseFSM)
        {
            if (baseFSM.targetTransform != null)
            {
              
               
            }
        }

        public override void lnit()
        {
            stateId = FSMStateID.Desdroy;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
            base.EnterState(baseFSM);
            PeopleManager.Instance.RemovePeople(baseFSM.peopleControl);
           
        }
        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
            //baseFSM.IsWalked = true;
        }
    }
}