using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Pathfinding;
using System;
using DG.Tweening;

public class PeopleControl : MonoBehaviour
{
    // Start is called before the first frame update
    AIDestinationSetter aIDestinationSetter;
    Transform Targettransform;
    AILerp aI;
   
     Transform peopleTf;
    public BornType bornType = 0;
    //public float yPos;
    public int Step=0;
    public PeopleData peopleData;
    public PeopleUI peopleUI;
  public  AI.FSM.BaseFSM baseFSM;
    public PosData posData;
    SimpleSmoothModifier smoothModifier;
    Tweener tweener;
    Tweener tweener1;
    Tweener tweener2;
    Tweener tweener3;
    public void SetSpeed(int speed)
    {
        aI.speed = speed;
    }
    public Transform GetPeopleTf()
    {
        return peopleTf;
    }

    public void Animtion()
    {
        if (tweener != null)
        {
            tweener.Kill();
        }
        if (tweener1 != null)
        {
            tweener1.Kill();
        }
        if (tweener2 != null)
        {
            tweener2.Kill();
        }
        if (tweener3 != null)
        {
            tweener3.Kill();
        }
     
        int value = peopleTf.lossyScale.x > 0 ? 1 : -1;
        peopleTf.DOScaleX(1 * value, 0f);
        peopleTf.DOScaleY(1f,0F);
        tweener = peopleTf.DOScaleX(1.2f* value, 0.3f);
        tweener2= peopleTf.DOScaleY(1.2f, 0.3F);
        tweener.onComplete = () =>
        {
            tweener1 = peopleTf.DOScaleX(1.0f * value, 0.3f);
            tweener3= peopleTf.DOScaleY(1f, 0.3F);
        };
    }
    void Awake()
    {
        InitConmpont();
    }
    public void SetData(PeopleData peopleData)
    {
        this.peopleData = peopleData;
    }
    public void SetBornType(BornType type,float y)
    {
        bornType = type;
        //yPos = y;
    }
    private void InitConmpont()
    {if (aI != null) return;
        aIDestinationSetter = GetComponentInChildren<AIDestinationSetter>();
        aI = GetComponentInChildren<AILerp>();
        peopleTf = aI.transform;
        Targettransform = transform.Find("targetTf");
        baseFSM = GetComponentInChildren<AI.FSM.BaseFSM>();
        peopleUI = baseFSM.peopleUI;
        smoothModifier = GetComponentInChildren<SimpleSmoothModifier>();
        smoothModifier.maxSegmentLength = 16;
    }

   public void SetQuXian(SimpleSmoothModifier.SmoothType smoothType)
    {
        smoothModifier.smoothType = smoothType;
    }
   
    public float ReMainningDistance()
    {
        return Vector2.Distance(peopleTf.position, Targettransform.position);
    }
    bool isAction = false;
   
    public void SetPos(Vector3 target)
    {
        InitConmpont();
        isAction = false;
        Targettransform.position = target;
        if (aIDestinationSetter.target==null)
            aIDestinationSetter.target = Targettransform;
        IsMove = true;
        stopScale = false;
        if (!aI.enabled)
        {
            aI.enabled = true;
            aIDestinationSetter.enabled = true;
        }
    }
    public void StopMove()
    {
        aI.enabled = false;
        aIDestinationSetter.enabled = false;
        IsMove = false;
        stopScale = true;
    }
    public void Destroy()
    {
        StopMove();
        Destroy(gameObject);
    }
    bool IsMove = false;
    bool stopScale = false;

 public void SetScale(bool value=true)
    {
        int scale = value == true ? 1 : -1;
        peopleTf.localScale = new Vector3(Mathf.Abs( peopleTf.localScale.x)* scale, peopleTf.localScale.y, peopleTf.localScale.z);
        stopScale = true;
    }
    private void Update()
    {if (!IsMove) return;
        if (stopScale) return;
       if(aI.Gettangent().x < -0.1f)
        {
           if(peopleTf.localScale.x < 0)
            {
                peopleTf.localScale = new Vector3(peopleTf.localScale.x * -1, peopleTf.localScale.y, peopleTf.localScale.z);
            }
        }
        else if(aI.Gettangent().x >0.1f)
        {
            if (peopleTf.localScale.x > 0)
            {
                peopleTf.localScale = new Vector3(peopleTf.localScale.x * -1, peopleTf.localScale.y, peopleTf.localScale.z);
            }
        }
    }
   
}
