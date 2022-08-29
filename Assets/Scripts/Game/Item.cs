using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public Image img;
    public Text nameText;
    public void Awake()
    {
        img = Global.FindChild<Image>(transform, "img");
        nameText = Global.FindChild<Text>(transform, "nameText");
    }
    public void SetItem(Sprite sprite ,string value)
    {
        img.sprite = sprite;
        nameText.text = value; 
    }
}
