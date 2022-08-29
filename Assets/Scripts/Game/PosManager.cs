using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConfigData;

public class PosManager :MonoSingleton<PosManager>
{
    // Start is called before the first frame update
    public Transform tf;
    public List<Transform> bornTfs;
    public Transform enterTransForm,exitTransForm,shoutinTf;
    public List<PosData> posDatas=new List<PosData>();
    public PosData konXianPosData;//列表满了之后的位置

    public List<Tanwei> tanweis;
    bool isInit = false;
    public List<Tanwei> GetTanWeis()
    {
        if (tanweis==null||tanweis.Count<=0)
        { 
            
            tanweis = new List<Tanwei>(GetComponentsInChildren<Tanwei>()); 
        
        }
        return tanweis;
    }
   public void InitPos()
    {if (isInit) return;
        bornTfs = new List<Transform>();
        
        foreach (var item in tf)
        {
           
            bornTfs.Add((Transform)item);
          


        }
        for (int i = 0; i < bornTfs.Count; i++)
        {
            bornTfs[i].gameObject.name = (i + 1).ToString();
        }
        GetTanWeis();
        for (int i = 0; i < 10; i++)
        {
            // GameObject go = new GameObject("paiduiTf" + (i + 1));
            //go.transform.SetParent(exitTransForm);
            posDatas.Add(new PosData(false, new Vector2(exitTransForm.position.x-0.5f*(i+1), exitTransForm.position.y),i));
            
        }
        konXianPosData = new PosData(false, new Vector2(posDatas[posDatas.Count - 1].vector2.x - 0.5f, exitTransForm.position.y), posDatas.Count);
        isInit = true;
    }
    public List<Tanwei> FindJieSuoedTanWei()
    {
        var list = tanweis.FindAll(s => s.GetLockStatus() == 1);
        return list;
    }
    public List<Tanwei> FindNotJieSuoedTanWei()
    {
        var list = tanweis.FindAll(s => s.GetLockStatus() != 1);
        return list;
    }
    public Tanwei GetRandomTanwei(List<Tanwei> list)
    {if (list != null && list.Count > 0)
        { int index = Random.Range(0, list.Count);
            return list[index];
        }
    else if(list == null || list.Count <= 0)
        {
            return null;
        }
        return null;
    }
    public PosData GetKonXianPos(bool isLast)
    {
        return konXianPosData;
    }
   public PosData GetKonXianPos()
    {
     var pos= posDatas.Find(s => s.isHave ==false);
        if (pos != null)
        {
            pos.SetPosISHave();
            return pos;
        }
        else
        {
            return konXianPosData;
        }
    }
    private void Start()
    {
        //Debug.LogError("初始化POSManager");
        InitPos();
    }
    public Vector3  GetPos(int index)
    {
        InitPos();
        if (index>= bornTfs.Count)
        {
            int newIndex = (index - 1 )% bornTfs.Count;
            int beishu = (index - 1) / bornTfs.Count;
            Vector3 vector3 = new Vector3(beishu * Camera.main.orthographicSize * ((float)Screen.width / (float)Screen.height) * 2, bornTfs[newIndex].position.y, 0);
            return vector3;
        }
        else
        {
            return bornTfs[index - 1].position;
        }
    }
    public Transform GetBornTransform(int index)
    {
        InitPos();
        if (index < bornTfs.Count)
        {
            return bornTfs[index - 1];

        }
        else
        {
            return null;
        }
    }
}
[System.Serializable]
public class PosData
{
    public bool isHave;
    public Vector2 vector2;
    public int index;
    public PosData(bool ishave,Vector2 vector,int index)
    {
        isHave = ishave;
        vector2 = vector;
        this.index = index;
    }
    public void SetPosISHave()
    {
        isHave = true;
    }
}
