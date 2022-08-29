using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
///<summary>
///状态抽象类
///</summary>
///
namespace AI.FSM
{
    abstract public class FSMState 
    {  //状态行为
        abstract public void Action(BaseFSM baseFSM);
        //添加条件
        public void AddTrigger(FSMTriggerID fSMTriggerlD, FSMStateID fSMStatelD)
        {
            if (map.ContainsKey(fSMTriggerlD))
                {
                map[fSMTriggerlD] = fSMStatelD;
            }
            else
            { map.Add(fSMTriggerlD,fSMStatelD);
                AddTriggerObject(fSMTriggerlD);
            }
        }
        //添加条件对象
        private void AddTriggerObject(FSMTriggerID fSMTriggerlD) 
        {
            Type type = Type.GetType("AI.FSM." + fSMTriggerlD + "Trigger");
        
            if (type != null)
            {
                
                triggers.Add( Activator.CreateInstance(type)as FSMTrigger);
            }
        }
        //进入状态
        virtual public void EnterState(BaseFSM baseFSM) { }
        //离开状态
     virtual  public void ExitState(BaseFSM baseFSM) { }

        public FSMState() {
            lnit();
        }
        //查找映射
        public FSMStateID GetOutputState(FSMTriggerID fSMTriggerlD)
        {
            if (map.ContainsKey(fSMTriggerlD))
            {
                return map[fSMTriggerlD];
            }
            return FSMStateID.None;
        }
        //初始化
        abstract public void lnit();
        //条件检测

      virtual  public void Reason(BaseFSM baseFSM)//检查条件，大部分相同，所以用虚方法
        {
            for (int i = 0; i < triggers.Count; i++)
            {
               if(triggers[i].HandleTrigger(baseFSM))
                {
                    //变化行为状态
                    baseFSM.ChangActiveState(triggers[i].triggerID); 
                    return;
                }
            }
        }
        //删除条件
        public void RemoveTrigger(FSMTriggerID triggerID) 
        {
            if (map.ContainsKey(triggerID))
            {
                map.Remove(triggerID);
                RemoveTriggerObject(triggerID);
            }
   
        }
        //删除条件对象
       private void RemoveTriggerObject(FSMTriggerID fSMTriggerlD) 
        {
            triggers.RemoveAll(t => t.triggerID == fSMTriggerlD);
        }
        //状态编号
        public FSMStateID stateId;
        //条件列表
        private List<FSMTrigger> triggers = new List<FSMTrigger>();
        //转换映射表
        private Dictionary<FSMTriggerID, FSMStateID> map = new Dictionary<FSMTriggerID, FSMStateID>();


    }    
}