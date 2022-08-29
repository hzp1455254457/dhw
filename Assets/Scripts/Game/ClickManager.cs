using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.FSM;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager 
{
   
    public static bool IsPointerOverUIObject()
     {
          if (EventSystem.current == null)
             return false;
 
         // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
         // the ray cast appears to require only eventData.position.
         PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
         EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
 
         return results.Count > 0;
     }
    private static bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition)
    {
         if (EventSystem.current == null)
             return false;
 
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
         PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);  
    eventDataCurrentPosition.position = screenPosition;

         GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
         List<RaycastResult> results = new List<RaycastResult>();
         uiRaycaster.Raycast(eventDataCurrentPosition, results);
         return results.Count > 0;
     }


}
