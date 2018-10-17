using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour {

    // Conversation token for triggering this Tooltip's ConversationTrigger (its whitelist token)
    // So, any object with the Tooltip component needs to also have a ConversationTrigger component attached
    public string[] displayTokens;
    private bool selectedObjectFace;
    private bool selectedFuseToFace;
    public SelectPart selectPart;
    public GameObject[] selectedFuseToTriggers;
    public GameObject[] selectedObjectTriggers;
    public Vector3 location;


    // Use this for initialization
    void Start () {

    }

    void OnMouseEnter()
    {
        if (selectedFuseToTriggers.Length == 0 && selectedObjectTriggers.Length == 0)
        {
            ConversationTrigger.AddToken(displayTokens[0]);
        } else if (selectedObjectTriggers.Length > 0)
        {
            selectedObjectFace = false;
            for(int i = 0; i < selectedObjectTriggers.Length; i++)
            {
                if(selectPart.getSelectedObject().name.Equals(selectedObjectTriggers[i].name))
                {
                    selectedObjectFace = true;
                }
            }

            // add token for convo and move conversionController to appropriate place
            if(selectedObjectFace)
            {
                ConversationTrigger.AddToken(displayTokens[0]);
            } else
            {
                ConversationTrigger.AddToken(displayTokens[1]);

            }
        }
    }

    void OnMouseExit()
    {
        ConversationTrigger.RemoveToken(displayTokens[0]);
        if (displayTokens.Length > 1)
        {
            ConversationTrigger.RemoveToken(displayTokens[1]);
        }

        ConversationController.Disable();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
