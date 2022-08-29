using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///攻击状态
///</summary>
///
namespace AI.FSM
{
    public class WalkState : FSMState
    {
        bool isArrived = false;
        public override void Action(BaseFSM baseFSM)
        {
            if (GuideManager.Instance.isFirstGame)
            {
                if (isArrived) return;
                if (baseFSM.GetDistance() <= 0.1F)
                {
                    isArrived = true;
                    baseFSM.StopMove();
                    GuideManager.Instance.GuideEvent();
                }
            }
        }

        public override void lnit()
        {
            stateId = FSMStateID.Walk;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
           
              base.EnterState(baseFSM);
            int value = baseFSM.peopleControl.bornType == BornType.Left ? 1 : 0;
            baseFSM.peopleControl.Step++;
            Vector2 vec;
            if (baseFSM.peopleControl.bornType == BornType.Left)
            {
                vec = new Vector2(PeopleManager.Instance.wayPoint[value], PeopleManager.Instance.GetYPos());
            }
            else
            {
                vec = new Vector2(PeopleManager.Instance.wayPoint[value], PeopleManager.Instance.GetYPos());
            }
            if (GuideManager.Instance.isFirstGame)
            {
               vec= Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0, 0));
                vec = new Vector2(vec.x, -6);
                baseFSM.MoveToTarget(vec, 1,1);
            }
            else
            {
                baseFSM.MoveToTarget(vec, 1, 1);
            }
           
            baseFSM.IsCanDestroy = true;
        }
        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
            //baseFSM.IsWalked = true;
        }
    }
}