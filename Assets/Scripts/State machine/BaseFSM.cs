using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using System;

using System.Reflection;
using Spine.Unity;
///<summary>
///条件抽象类
///</summary>
///

namespace AI.FSM
{
    
    public class BaseFSM : MonoBehaviour
    {
       
        #region//1.0
        //切换状态
        public void ChangActiveState(FSMTriggerID fSMTriggerlD)
        {  if (currentState == null) return;
          var nextStateID = currentState. GetOutputState(fSMTriggerlD);
            if(nextStateID ==FSMStateID.None)
            {
                return;
            }
            FSMState nextState = null;
            if (nextStateID == FSMStateID.Default)
            {
                nextState = defaultState;
            }
            else
            {
                nextState = states.Find(s => s.stateId == nextStateID);
              
            }
            currentState.ExitState(this);
            currentState = nextState;
            currentStateld = nextStateID;
            currentState.EnterState(this);
        }
       
        //为状态类或条件类提供的行为
      
        public FSMStateID currentStateld;
        //当前状态
        private FSMState currentState;
        public FSMState CurrentState { get { return currentState; } }
        //默认状态
        private FSMState defaultState;
        public FSMStateID defaultStateld=FSMStateID.Walk;
        //状态管理
       private List<FSMState> states=new List<FSMState>();
        private SkeletonAnimation skeletonAnimation;
        private Animator animator;
        public PeopleUI peopleUI;
       
        
        //为状态类或条件类提供的数据

        //状态机管理

        //为状态类或条件类提供的行为
        #endregion

        #region//2.0
      public  string aiConfigFile = "BaseAI.txt";  //ai配置文件
        //public AnimationParams animParams;
        //public CharacterStatus chState;

        private void Awake()
        {

            ConfigFSM();

        }
        private void Start()
        {
        
           
            InitCompent();
            InitDefaultState();
        }
        void InitCompent()
        {
            skeletonAnimation = GetComponentInChildren<SkeletonAnimation>(false);
            animator= GetComponentInChildren<Animator>();
            peopleControl =GetComponentInParent<PeopleControl>();
           // peopleUI = GetComponentInChildren<PeopleUI>();
        }
      

        private void OnEnable()
        {//执行时机和执行效率频率考虑
            

            //InvokeRepeating("ResetTarget", 0, 0.2F);
        }
        public void PlayAnimation(string animPara)
        { /*chAnim.PlayAnimation(animPara); */}
        //调用配置文件 确定条件到状态的转换
        private void ConfigFSM()
        {
            #region //硬编码
            WalkState WalkState = new WalkState();
     
            
            EnterGameState enterGameState = new EnterGameState();
            DesdroyState desdroyState = new DesdroyState();
          
            BuyState buyState = new BuyState();
            IdleState idleState = new IdleState();
            RoamState roamState = new RoamState();
            ExitGameState exitGameState = new ExitGameState();
            MoveScreenState moveScreenState = new MoveScreenState();
            PaiDuiState paiDuiState = new PaiDuiState();
            idleState.AddTrigger(FSMTriggerID.Enter, FSMStateID.EnterGame);
            WalkState.AddTrigger(FSMTriggerID.Desdroy, FSMStateID.Desdroy);
            //WalkState.AddTrigger(FSMTriggerID.Walk, FSMStateID.Walk);
            WalkState.AddTrigger(FSMTriggerID.Idle, FSMStateID.Idle);
            enterGameState.AddTrigger(FSMTriggerID.Roam, FSMStateID.Roam);
            roamState.AddTrigger(FSMTriggerID.Buy, FSMStateID.Buy);
            idleState.AddTrigger(FSMTriggerID.Walk, FSMStateID.Walk);
            buyState.AddTrigger(FSMTriggerID.Exit, FSMStateID.ExitGame);
           //paiDuiState.AddTrigger(FSMTriggerID.Exit, FSMStateID.ExitGame);
            buyState.AddTrigger(FSMTriggerID.PaiDui, FSMStateID.PaiDui);
           exitGameState.AddTrigger(FSMTriggerID.MoveScreen, FSMStateID.MoveScreen);
           // exitGameState.AddTrigger(FSMTriggerID.PaiDui, FSMStateID.PaiDui);
            moveScreenState.AddTrigger(FSMTriggerID.Desdroy, FSMStateID.Desdroy);
            states.Add(WalkState);
            states.Add(enterGameState);
            states.Add(buyState);
            states.Add(desdroyState);
            states.Add(idleState);
            states.Add(roamState);
            states.Add(exitGameState);
            states.Add(moveScreenState);
            states.Add(paiDuiState);
            #endregion
            #region//配置文件配置
            //var list = "";
            //foreach (var stateName in list.Keys)
            //{

            //    Type type = Type.GetType("AI.FSM."+stateName+"State");
            //    var stateObj = Activator.CreateInstance(type) as FSMState;
            //    foreach (var triggerID in list[stateName].Keys)
            //    {
            //        var trigger = (FSMTriggerID)Enum.Parse(typeof(FSMTriggerID), triggerID);

            //        var state = (FSMStateID)Enum.Parse(typeof(FSMStateID), list[stateName][triggerID]);

            //        stateObj.AddTrigger(trigger, state);
            //    }
            //    states.Add(stateObj);
            //}
            #endregion

        }
        int clickCount;
      
        bool IsTime = false;
        public void Delay(float time,UnityEngine.Events.UnityAction unityAction)
        {
            StartCoroutine(Global.Delay(1f, unityAction));
        }
        private void OnMouseDown()
        {
            if (ClickManager.IsPointerOverUIObject()) return;

            if (MainUI.Instance.ChuanDan > 0)
            {
                if (!IsWalked)
                {
                    if (clickCount < peopleControl.peopleData.custom_need_click)
                    {
                        clickCount++;
                        AudioManager.Instance.PlaySound("点击顾客发传单时");
                        AndroidHelper.Instance.UploadDataEvent("click_custom_cd_success");
                        float value = (float)clickCount / (float)peopleControl.peopleData.custom_need_click;
                        //Debug.LogError("点击:" + clickCount + "点击：" + peopleControl.peopleData.custom_need_click);
                        if(!GuideManager.Instance.isFirstGame)
                        peopleUI.SetUI(value, () => IsIdle = false, () => { IsWalked = true;
                            AudioManager.Instance.PlaySound("顾客进度条满了");
                        });
                        else
                        {
                            peopleUI.SetUI(value, null ,() => {
                                IsWalked = true;

                                AudioManager.Instance.PlaySound("顾客进度条满了");
                            });
                        }
                        peopleControl.Animtion();
                        IsIdle = true;
                        MainUI.Instance.ChuanDan--;
                    }
                 
                }
            }
            else
            {
                if (!IsWalked)
                {
                    if(MainUI.Instance.IsRedShow)
                    PanelMgr.Instance.OpenPanel<ChuanDanPanel>("ChuanDanPanel", MainUI.Instance.isGetReded);
                }
            }
            
          
        }

        public void DestroyCollider()
        {
            Destroy(GetComponent<Collider2D>());
        }

        private void InitDefaultState()
        {//用默认状态编号查找默认状态
            defaultState = states.Find(s => s.stateId == defaultStateld);
            currentState = defaultState;
            currentStateld = defaultStateld;
            defaultState.EnterState(this);
        }
        public void SetStates(FSMStateID fSMStateID)
        {
            if (currentState == null) return;
            var nextStateID = fSMStateID;
            if (nextStateID == FSMStateID.None)
            {
                return;
            }
            FSMState nextState = null;
            if (nextStateID == FSMStateID.Default)
            {
                nextState = defaultState;
            }
            else
            {
                nextState = states.Find(s => s.stateId == nextStateID);

            }
            currentState.ExitState(this);
            currentState = nextState;
            currentStateld = nextStateID;
            currentState.EnterState(this);
           

        }
        private void Update()
        {
            currentState.Reason(this);
            currentState.Action(this);
           
          
        }
        #endregion
        #region//3.0
        public Transform targetTransform;
       

        public float moveSpeed=2;
        public float sightDistance = 20;
              public string[] targetTags;
        //private NavMeshAgent navAgent;

        //private void ResetTarget()
        //{
        //    List<GameObject> list = new List<GameObject>();
        //    for (int i = 0; i < targetTags.Length; i++)
        //    {
        //        var target = GameObject.FindGameObjectsWithTag(targetTags[i]);//场景中要有标签设置
        //        if (target == null && target.Length == 0) continue;
        //        { list.AddRange(target);  }
        //    }
        //    if (list == null || list.Count == 0) return;
        //    //找出活着的，在攻击范围内的对象
        //    var enemys = list.FindAll(g => g.GetComponent<CharacterStatus>().HP > 0 && Vector3.Distance(g.transform.position, transform.position) < sightDistance);

        //    if (enemys == null || enemys.Count == 0) return ;
        //    //找出单攻或者群攻范围内的敌人数组

        //    targetTransform = ArrayHelper.Min(enemys.ToArray(), e => Vector3.Distance(e.transform.position, transform.position)).transform;

        //}
        public PeopleControl peopleControl;
        public ConfigData.Tanwei Tanwei;
        public bool IsHavaItem()
        {
            if (ConfigData.DataManager.Instance.gameUserDataConfig.onLineTime / 60 <= 15)
            {
                Tanwei = PosManager.Instance.GetRandomTanwei(PosManager.Instance.FindJieSuoedTanWei());
                if (GuideManager.Instance.isFirstGame)
                {
                    Tanwei = PosManager.Instance.GetTanWeis().Find(s => s.shop_id == 3);
                }
                if (Tanwei != null)
                { targetTransform = Tanwei.transform; }
                return true;
            }
            else
            {
                int value = UnityEngine.Random.Range(0, 5);
                bool result = value == 0 ? true : false;
                if (result)
                {
                    Tanwei = PosManager.Instance.GetRandomTanwei(PosManager.Instance.FindJieSuoedTanWei());
                    if (GuideManager.Instance.isFirstGame)
                    {
                        Tanwei = PosManager.Instance.GetTanWeis().Find(s => s.shop_id == 3);
                    }
                    if (Tanwei != null)
                    { targetTransform = Tanwei.transform; }
                    return true;
                }
                else
                {
                    Tanwei=  PosManager.Instance.GetRandomTanwei(PosManager.Instance.FindNotJieSuoedTanWei());
                    if (Tanwei != null)
                    { 
                        targetTransform = Tanwei.transform;
                        return false;
                    }
                    else
                    {
                      
                        Tanwei = PosManager.Instance.GetRandomTanwei(PosManager.Instance.FindJieSuoedTanWei());
                        if (Tanwei != null)
                        { targetTransform = Tanwei.transform; }
                        return true;
                    }
                }
            
               // return result;
            }
        }
        public void MoveToTarget(Vector3 pos, float speed, float stopDistance)
        {
          //  navAgent.speed = speed;
          //  navAgent.stoppingDistance = stopDistance;
            peopleControl.SetPos(pos);
            animator.speed = 1.2f;
         
          //  navAgent.SetDestination(pos);
        }
        public float GetDistance()
        {
           return peopleControl.ReMainningDistance();
        }
        public void StopMove(bool isStopAnim=false)
        {
            peopleControl.StopMove();
            animator.speed = 0;
            //if (skeletonAnimation != null)
            //{
            //    if (!isStopAnim)
            //        skeletonAnimation.AnimationState.SetAnimation(0, "wanyao", false);
            //}
            //else
            //{
            //    animator.speed = 0;
            //}
        }
        private void OnDisable()
        {
            //CancelInvoke("ResetTarget");
            if (currentState.stateId==FSMStateID.Dead)
            {
                //var triggers = GetComponents<AbstractTrigger>();
                //foreach (var item in triggers)
                //{
                //    item.enabled = false;
                //} 
                //var sensors= GetComponents<AbstractSensor>();
                //foreach (var item in sensors)
                //{
                //    item.enabled = false;
                //}

            }
            else
            {
                //currentState.ExitState(this);
                //currentState = states.Find(p => p.stateId == FSMStateID.Idle);
                //currentStateld = currentState.stateId;
               // PlayAnimation(animParams.Idle);
            }
        }
        #endregion
        #region//4.0
      
        #endregion
        #region//5.0
      
        public bool IsHavePro { get;
            set;
           }
     
       public bool IsIdle
        {
            get;
            set;
        }
        public bool IsWalked
        {
            get;
            set;
        }//是否捡完传单
        public bool IsBuyed
        {
            get;
            set;
        }
        public bool IsCanDestroy
        {
            get;
            set;
        }
        public bool IsPaidui
        {
            get;
            set;
        }
        #endregion
        #region//6.0
        // private AbstractSensor abstractSensor;

        private void AbstractSensor_OnNonPerception()
        {
            targetTransform = null;
        }

        //private void AbstractSensor_OnPerception(List<AbstractTrigger> obj)
        //{
        //    if (obj.Count <= 0 && obj == null) return;
        //    var targets = obj.FindAll(o => Array.IndexOf(targetTags, o.tag) >= 0);
        //    if (targets.Count <= 0 && targets == null) return;
        //    var enemys = targets.FindAll(t => t.GetComponent<CharacterStatus>().HP > 0);
        //    if (enemys.Count <= 0 && enemys == null) return;
        //    var enemyTarget = ArrayHelper.Min(enemys.ToArray(),
        //        e => Vector3.Distance(this.transform.position, e.transform.position));
        //    if (enemyTarget != null)
        //    {
        //        targetTransform = enemyTarget.transform;
        //    }

        //}
        #endregion
    }
}