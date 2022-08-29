using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEngine.U2D;

public class ResourceManager : MonoSingleton<ResourceManager>
{

    Dictionary<string ,Sprite> produceDic=new Dictionary<string, Sprite>();
   

    Dictionary<string, GameObject> proGoDic = new Dictionary<string, GameObject>();

  
 
    SpriteAtlas atlas01;
   // SpriteAtlas atlas02;
    public override void Init()
    {
        base.Init();
        atlas01 = Resources.Load<SpriteAtlas>("ProduceAtlas");
       // atlas02 = Resources.Load<SpriteAtlas>("Other");
    }



    public Sprite GetSprite(string spriteName)
    {
        if (!produceDic.ContainsKey(spriteName))
        {
            //var sprite = Resources.Load<Sprite>("Sprite/" + spriteName);
           // produceDic.Add(spriteName, sprite);
            var sprite = atlas01.GetSprite(spriteName);
            //if (sprite == null)
            //{
            //    sprite = atlas02.GetSprite(spriteName);
            //}
           produceDic.Add(spriteName, sprite);
        }
        else
        {
        
        }
        return produceDic[spriteName];
    }
    public GameObject GetProGo(string goName,string topName= "Prefab/")
    {
        if (!proGoDic.ContainsKey(goName))
        {
            var sprite = Resources.Load<GameObject>(topName + goName);
            proGoDic.Add(goName, sprite);
        }
        else
        {

        }
        return proGoDic[goName];
    }
   

}
