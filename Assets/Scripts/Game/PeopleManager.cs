using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoSingleton<PeopleManager>
{
    // Start is called before the first frame update

    public List<PeopleControl> peopleControls;
    public List<PeopleControl> GoTargetpeopleControls;
    public List<PeopleControl> paiduiPeopleControls;
    public PeopleControl[] peopleControl;
   public float[] wayPoint;
    public string[] stringValues = new string[] { "进去看看", "买点啥", "扫货去!", "凑凑热闹", "买买买!" };
    public string GetStringValue()
    {
      return stringValues[Random.Range(0, stringValues.Length)];
    }
    public void InitList()
    {
        peopleControls = new List<PeopleControl>();
        paiduiPeopleControls= new List<PeopleControl>();
        GoTargetpeopleControls = new List<PeopleControl>();
        peopleControl = Resources.LoadAll<PeopleControl>("Prefab/Players");
        
    }
    private void Awake()
    {
        Debug.LogError("初始化peopleManager");
        InitList();
        wayPoint = new float[2];
        wayPoint[0] = (-5.4f * 3);
        wayPoint[1] = (5.4f * 3+1f);
    }
 
    public PeopleControl CreactPeople(BornType bornType,bool isFirst=false)
    {
       var people= Instantiate<PeopleControl>(peopleControl[Random.Range(0,peopleControl.Length)]);
    float y= Random.Range(0, -5.4f);
        people.SetBornType(bornType, y);
        PeopleData peopleData = new PeopleData();
            peopleData.SetData(1, 1,Random.Range(2,4));
     
        int value = bornType == BornType.Left ? 0 : 1;
        float offsetX= bornType == BornType.Left ? -1f : 1f;
        float x = wayPoint[value]+ offsetX;
        if (!isFirst)
        {
            peopleData.SetData(1, 1, Random.Range(2, 4));
            people.transform.position = new Vector2(x, GetYPos());
        }
      
        else
        {//教程逻辑
            peopleData.SetData(1, 1, 2);
            people.transform.position = new Vector2(x, -6f);
            people.GetComponentInChildren<Collider2D>().enabled = false;
        }
        people.SetData(peopleData);
        AddPeople(people);
        return people;
    }
    public void AddGoTargetPeople(AI.FSM.BaseFSM fSM)
    {
        if (!GoTargetpeopleControls.Contains(fSM.peopleControl))
        {
            GoTargetpeopleControls.Add(fSM.peopleControl);
           // fSM.MoveToTarget(PosManager.Instance.GetKonXianPos().vector2, 1, 1);
        }
    }
    public void RemoveGoTargetPeople(AI.FSM.BaseFSM fSM)
    {
        if (GoTargetpeopleControls.Contains(fSM.peopleControl))
        {
            GoTargetpeopleControls.Remove(fSM.peopleControl);

            //peopleControl.Destroy();
        }
    }
    public void AddPaiXu()
    {

    }
    public void AddPaiDuiP(AI.FSM.BaseFSM fSM)
    {
        if (!paiduiPeopleControls.Contains(fSM.peopleControl))
        {
            paiduiPeopleControls .Add(fSM.peopleControl);
          
            fSM.peopleControl.posData = PosManager.Instance.GetKonXianPos();
            fSM.MoveToTarget(fSM.peopleControl.posData.vector2, 1, 1);
        }
    }
    
    public void RemovePaiDuiPeople(AI.FSM.BaseFSM fSM)
    {
        if (paiduiPeopleControls.Contains(fSM.peopleControl))
        {
            paiduiPeopleControls.Remove(fSM.peopleControl);
            fSM.peopleControl.posData.isHave = false;
           fSM.peopleControl.posData = null;
            //peopleControl.Destroy();
        }
    }
    public float Distance()
    {


       return Mathf.Abs(wayPoint[0]) + Mathf.Abs(wayPoint[1]);
    }
    public float GetYPos()
    {
      return  Random.Range(-5f, -9.4f);
    }
   public void JieSuoTanweiAction(int id)
    {
      var list=  peopleControls.FindAll(s => s.baseFSM.currentStateld == AI.FSM.FSMStateID.Roam && ((AI.FSM.RoamState)(s.baseFSM.CurrentState)).isAchiveRoam == false&&s.baseFSM.Tanwei.shop_id==id);
        foreach (var item in list)
        {
            item.baseFSM.IsHavePro = true;
        }
    }

   public void AddChuanDan(int count)
    {
        MainUI.Instance.StopTiming();
        MainUI.Instance.AddChuanDanCount(count);
    }

    public void SetZero()
    {
     //  MainUI.Instance.shouyinCount = 0;
        var list = paiduiPeopleControls.FindAll(s => s.posData.index < 10);
        if (list.Count <= 10 && list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].baseFSM.SetStates(AI.FSM.FSMStateID.ExitGame);
            }
        }
        var list1 = paiduiPeopleControls.FindAll(s => s.posData.index >= 10);

        if (list1.Count > 0)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                list1[i].baseFSM.SetStates(AI.FSM.FSMStateID.PaiDui);
            }
        }
    }

    public void AddPeople(PeopleControl peopleControl)
    {
        if (!peopleControls.Contains(peopleControl))
        {
            peopleControls.Add(peopleControl);
        }

    }
    float times = 0;
    float times1 = 0;
    bool isFirstCreact = false;
    private void Update()
    {
        times += Time.deltaTime;
        times1 += Time.deltaTime;
        
        if (times >= 1.5f)
        {
            times = 0;
            if(!GuideManager.Instance.isFirstGame)
            CreactPeople(BornType.Right);
        }
        if (times1 >= 2f)
        {
            if (!GuideManager.Instance.isFirstGame)
            { CreactPeople(BornType.Left);
            }
            else
            {
                if (!isFirstCreact)
                { CreactPeople(BornType.Left,true);
                    isFirstCreact = true;
                }
            }
            times1 = 0;
        }
    }
   // public AstarPath pathfinding;
  
    public void RemovePeople(PeopleControl peopleControl)
    {
        if (peopleControls.Contains(peopleControl))
        {
            peopleControls.Remove(peopleControl);
            peopleControl.Destroy();
        }
    }
}
public enum BornType
{
    Left,
    Right
}
[System.Serializable]
public class PeopleData
{
    public int map_id;
    public int custom_id;
    public int custom_need_click;
    public void SetData(int mapId,int cusId,int click)
    {
        map_id = mapId;
        custom_id = cusId;
        custom_need_click = click;
    }
}
    