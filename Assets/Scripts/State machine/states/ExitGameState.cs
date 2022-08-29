using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///攻击状态
///</summary>
///
namespace AI.FSM
{
    public class ExitGameState : FSMState
    {
        bool isArrived = false;
        public override void Action(BaseFSM baseFSM)
        {

            if (!isArrived)
            {
                if (Vector3.Distance(baseFSM.peopleControl.GetPeopleTf().position, PosManager.Instance.exitTransForm.position) <= 0.1f)
                {
                    isArrived = true;
                    if (MainUI.Instance.shouyinCount< MainUI.Instance.shouyinMax)
                    {
                        MainUI.Instance.shouyinCount++;
                        baseFSM.IsPaidui = false;
                        ConfigData.DataManager.Instance.gameUserDataConfig.achivePeopleCount++;
                        UnityActionManager.Instance.DispatchEvent<int>("Refreshkeliu", ConfigData.DataManager.Instance.gameUserDataConfig.achivePeopleCount);
                        if (GuideManager.Instance.isFirstGame)
                        {
                            GuideManager.Instance.GuideEvent();
                            MainUI.Instance.shouyinCount = MainUI.Instance.shouyinMax;
                        }
                    }
                    //else
                    //{
                    //    baseFSM.IsPaidui = true;
                    //    PeopleManager.Instance.AddPaiDuiP(baseFSM);

                    //}
                }
            
            }
        }
        
        public override void lnit()
        {
            stateId = FSMStateID.ExitGame;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
            base.EnterState(baseFSM);
            PeopleManager.Instance.AddGoTargetPeople(baseFSM);
            baseFSM.MoveToTarget(PosManager.Instance.exitTransForm.position, 1, 1);
            baseFSM.peopleUI.HideUI();
            baseFSM.IsPaidui = true;
        }
        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
            PeopleManager.Instance.RemoveGoTargetPeople(baseFSM);
            MainUI.Instance.shouYinGold += UnityEngine.Random.Range(10, 50);
            AudioManager.Instance.PlaySound("顾客离开");
            AndroidHelper.Instance.UploadDataEvent("custom_leave");
            //baseFSM.IsWalked = true;
        }
    }
}