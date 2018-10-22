using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// IMPORTANT: ConversationController needs to have earlier draw order than UI elements with Tooltips
// in order for OnPointerEnter/Exit to function correctly. No idea why, may have something to do with 
// where ConversationController is put when it's in FakeActive mode.
public class PointerClickDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Tooltip tooltip;

	// Use this for initialization
	void Start () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.OnMouseExit();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
