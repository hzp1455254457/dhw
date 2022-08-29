using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///攻击状态
///</summary>
///
namespace AI.FSM
{
    public class BuyState : FSMState
    {
        bool isBuyed = false;
        public override void Action(BaseFSM baseFSM)
        {
            if (baseFSM.targetTransform != null)
            {

                if (baseFSM.GetDistance() <= 0.2f)
                {
                    baseFSM.peopleControl.SetScale();
                    if (!isBuyed)
                    {
                        isBuyed = true;
                        baseFSM.peopleUI.HideUI();
                        baseFSM.StopMove();
                        baseFSM.peopleUI.ShowQiPao1(null,()=> { baseFSM.IsBuyed = true;
                            if (MainUI.Instance.IsRedShow)
                            {
                                if (!GuideManager.Instance.isFirstGame)
                                {
                                    if (AndroidHelper.isShowTabled && AndroidHelper.Instance.TableIsLoaded())
                                    {
                                        var go = GameObjectPool.Instance.CreateObject("红包动画", ResourceManager.Instance.GetProGo("redAnim"), baseFSM.peopleControl.GetPeopleTf().position, Quaternion.identity);
                                        go.GetComponent<RedAnimControl>().Anim();

                                    }
                                }
                                else
                                {
                                    var go = GameObjectPool.Instance.CreateObject("红包动画", ResourceManager.Instance.GetProGo("redAnim"), baseFSM.peopleControl.GetPeopleTf().position, Quaternion.identity);
                                    go.GetComponent<RedAnimControl>().Anim(() =>
                                    {
                                        if (GuideManager.Instance.isFirstGame)
                                        {
                                            go.layer = 8;
                                            GuideManager.Instance.CreactTips1(new Vector3(go.transform.position.x, go.transform.position.y, 0));

                                            GuideManager.Instance.GuideEvent();
                                            baseFSM.StopMove();

                                        }
                                    });
                                }
                            }
                        });
                    }
                }
            }
        }

        public override void lnit()
        {
            stateId = FSMStateID.Buy;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
            base.EnterState(baseFSM);
            Vector3 vector3 = new Vector3(baseFSM.targetTransform.position.x + 0.7f, baseFSM.targetTransform.position.y - 0.5f, 0);
            baseFSM.MoveToTarget(vector3, 1, 1);
        }
        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
            //baseFSM.IsWalked = true;
        }
    }
}