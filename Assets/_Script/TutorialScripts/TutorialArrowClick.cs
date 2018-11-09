using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialArrowClick : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("Rotate");
        TutorialManager.step++;
    }
	
}
