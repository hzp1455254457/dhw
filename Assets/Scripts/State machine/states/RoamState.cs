using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///攻击状态
///</summary>
///
namespace AI.FSM
{
    public class RoamState : FSMState
    {
        float times = 0;
        float TargetTime = 6;
       bool IsAchived = false;
        public bool isAchiveRoam = false;
        public override void Action(BaseFSM baseFSM)
        {
            if (IsAchived) return;
            if (baseFSM.GetDistance() < 1f)
            {
                Vector3 vector3 = GetRandomVector();
                baseFSM.MoveToTarget(vector3, 1, 1);
            }
            times += Time.deltaTime;
            if (times >= TargetTime)
            {
                IsAchived = true;
                baseFSM.StopMove();

                if (baseFSM.IsHavaItem())
                {

                    isAchiveRoam = true;
                    int id = baseFSM.Tanwei.GetShopInfo().shop_id;
                    var goods = ConfigData.Config.Instance.GoodsList.FindAll(s => s.shop_id == id);
                    if (goods == null || goods.Count <= 0)
                    {
                        Debug.LogError("没有查询到对应商店商品");
                        baseFSM.peopleUI.ShowQiPao(ResourceManager.Instance.GetSprite(""), () => baseFSM.IsHavePro = true, "配置表没对应商店商品");
                    }
                    else
                    {
                        int index = Random.Range(0, goods.Count);
                        ConfigData.Goods good = goods[index];
                        if (GuideManager.Instance.isFirstGame)
                        {
                            good = goods.Find(s => s.item_id == 10);
                        }
                       // Debug.LogError("good:" + good.item_id);
                        baseFSM.peopleUI.ShowQiPao(ResourceManager.Instance.GetSprite(good.item_pic), () => baseFSM.IsHavePro = true, good.item_name);
                        //Debug.LogError("good1:"+good.item_pic);

                    }
                }
                else
                {
                    baseFSM.SetStates(FSMStateID.Roam);
                    int id = baseFSM.Tanwei.GetShopInfo().shop_id;
                    var goods = ConfigData.Config.Instance.GoodsList.FindAll(s => s.shop_id == id);
                    if (goods == null || goods.Count <= 0)
                    {
                        Debug.LogError("没有查询到对应商店商品");
                        baseFSM.peopleUI.ShowQiPao(ResourceManager.Instance.GetSprite(""), () => baseFSM.IsHavePro = true, "配置表没对应商店商品");
                    }
                    else
                    {
                        int index = Random.Range(0, goods.Count);
                        var good = goods[index];
                        baseFSM.peopleUI.ShowQiPao(ResourceManager.Instance.GetSprite(good.item_pic), () => baseFSM.IsHavePro = false, good.item_name);
                    }

                }

            }
        }
        
        public override void lnit()
        {
            stateId = FSMStateID.Roam;
        }
        public override void EnterState(BaseFSM baseFSM)
        {
            base.EnterState(baseFSM);
            times = 0;
            baseFSM.peopleControl. SetQuXian(Pathfinding.SimpleSmoothModifier.SmoothType.OffsetSimple);
            TargetTime = Random.Range(4f, 7f);
            Vector3 vector3 = GetRandomVector();
            baseFSM.MoveToTarget(vector3, 1, 1);
            IsAchived = false;
            isAchiveRoam = false;
            if (GuideManager.Instance.isFirstGame)
            {
                TargetTime = 1f;
            }
        }

        private  Vector3 GetRandomVector()
        {
            //int index = Random.Range(0, 2);
            float offsetX = Random.Range(PeopleManager.Instance.wayPoint[0], PeopleManager.Instance.wayPoint[1]);
            float offsetY = Random.Range(-2.6f, 3f);
           
            float x = offsetX;
            float y = offsetY;
            Vector3 vector3 = new Vector3(x, y, 0);
            return vector3;
        }

        public override void ExitState(BaseFSM baseFSM)
        {
            base.ExitState(baseFSM);
            //baseFSM.IsWalked = true;
        }
    }
}