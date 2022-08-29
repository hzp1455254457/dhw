using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanweiClick : MonoBehaviour
{
    ConfigData.Tanwei tanwei;
    // Start is called before the first frame update
    void Start()
    {
        tanwei = GetComponentInParent<ConfigData.Tanwei>();
    }

    // Update is called once per frame
 
    private void OnMouseDown()
    {
        tanwei.ShopClick();
    }
}
