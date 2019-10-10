using UnityEngine;
using UnityEngine.UI;

// any UI element with this attached needs PointerClickDetector attached too with the tooltip variable set to this
// Because Unity is bad at handling Pointer events with UI elements
public class Tooltip : MonoBehaviour {

    // Conversation token for triggering this Tooltip's ConversationTrigger (its whitelist token)
    private ConversationController convoController;
    public string[] displayTokens;
    private bool selectedObjectFace;
    private bool selectedFuseToFace;
    private SelectPart selectPart;
    public CreatePartB1 createPartB1Trigger; // only set if this tooltip is triggered by part instantiation
    public string[] selectedFuseToTriggers;
    public string[] selectedObjectTriggers;
    public Button buttonTrigger; // used for tooltips that are dependent on whether a button is interactable or not

    // Use this for initialization
    void Start()
    {
        
    }
    void OnEnable () {
        convoController = GameObject.Find("ConversationSystem").GetComponent<ConversationController>();
        selectPart = GameObject.Find("EventSystem").GetComponent<SelectPart>();
        //Debug.Log("setting convoController and selectPart variables for " + gameObject + "!");
    }

    public void OnMouseEnter()
    {
        // turns out disabling a Component script doesn't prevent its methods from firing!
        // Need this to fix that
        if (this.enabled)
        {
            // move tooltip box to the correct location
            // ugh really? Why is anchored position needed for RectTransform? Why can't we just change position of the GameObject?
            //convoController.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(location.x, location.y);

            if (selectedFuseToTriggers.Length == 0 && selectedObjectTriggers.Length == 0 && buttonTrigger == null && createPartB1Trigger == null)
            {
                ConversationTrigger.AddToken(displayTokens[0]);
            }
            else if (selectedObjectTriggers.Length > 0)
            {   // this condition is for the  construction tooltip
                selectedObjectFace = false;
                for (int i = 0; i < selectedObjectTriggers.Length; i++)
                {
                    if (selectPart.getSelectedObject() == null)
                    {
                        break;
                    }
                    else if (selectPart.getSelectedObject().name.Equals(selectedObjectTriggers[i]))
                    {
                        selectedObjectFace = true;
                    }
                }

                // add token for convo 
                if (selectedObjectFace)
                {
                    ConversationTrigger.AddToken(displayTokens[0]);
                }
                else
                {
                    ConversationTrigger.AddToken(displayTokens[1]);

                }
            }
            else if (selectedFuseToTriggers.Length > 0)
            {   // this condition is for the part tooltip
                selectedFuseToFace = false;
                for (int i = 0; i < selectedFuseToTriggers.Length; i++)
                {
                    if (selectPart.getSelectedFuseTo() == null)
                    {
                        break;
                    }
                    else if (selectPart.getSelectedFuseTo().name.Equals(selectedFuseToTriggers[i]))
                    {
                        selectedFuseToFace = true;
                    }
                }

                // add token for convo 
                if (selectedFuseToFace)
                {
                    ConversationTrigger.AddToken(displayTokens[0]);
                }
                else
                {
                    ConversationTrigger.AddToken(displayTokens[1]);

                }
            }
            else if (buttonTrigger != null)
            {   // this condition is for the fuse button tooltip and rotation arrow tooltips
                if (buttonTrigger.IsInteractable())
                {
                    ConversationTrigger.AddToken(displayTokens[0]);
                }
                else
                {
                    ConversationTrigger.AddToken(displayTokens[1]);
                }
            }
            else if (createPartB1Trigger != null)
            {   // this condition is for part button tooltips
                GameObject[] instantiated = createPartB1Trigger.getInstantiated();
                bool partCurrentlyActive = false;
                for (int i = 0; i < instantiated.Length; i++)
                {
                    if (instantiated[i] != null && !instantiated[i].GetComponent<IsFused>().isFused)
                    {
                        partCurrentlyActive = true;
                        break;
                    }
                }

                // only display a tooltip if this button is interactable
                if (gameObject.GetComponent<Button>().IsInteractable())
                {
                    if (partCurrentlyActive)
                    {
                        ConversationTrigger.AddToken(displayTokens[0]);
                    }
                    else
                    {
                        ConversationTrigger.AddToken(displayTokens[1]);
                    }
                }
            }
        }
    }

    public void OnMouseExit()
    {
        if (this.enabled)
        {

            ConversationTrigger.RemoveToken(displayTokens[0]);
            if (displayTokens.Length > 1)
            {
                ConversationTrigger.RemoveToken(displayTokens[1]);
            }

            ConversationController.Disable();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
