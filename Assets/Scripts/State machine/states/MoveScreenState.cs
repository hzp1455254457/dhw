using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///攻击状态
///</summary>
///
namespace AI.FSM
{
    public class MoveScreenState : FSMState
    {
       
        public override void Action(BaseFSM baseFSM)
        {
          
        }
        
        public override void lnit()
        {
            stateId = FSMStateID.MoveScreen;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
            base.EnterState(baseFSM);
            baseFSM.peopleControl.SetSpeed(2);
            Vector2  vec = new Vector2(PeopleManager.Instance.wayPoint[1], -5.2f);
            baseFSM.MoveToTarget(vec, 1, 1);

            baseFSM.DestroyCollider();
            baseFSM.Delay(1f, () => { baseFSM.IsCanDestroy = true; baseFSM.peopleControl.SetQuXian(Pathfinding.SimpleSmoothModifier.SmoothType.Simple); });
           
            //baseFSM.peopleUI.HideUI();
        }
        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
        
            //baseFSM.IsWalked = true;
        }
    }
}