using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase :PanelAnimation,IHideSomeUI
{
    public abstract void HideRed();

  protected  bool IsShowRed=true;

    protected virtual void SetRedStates(bool value=true)
    {

        IsShowRed = value;
        if (!IsShowRed)
        {
            HideRed();
        }
    }

  
    // Update is called once per frame
  
}
