using UnityEngine;
using System.Collections;

public class ProgressChecker : MonoBehaviour
{
    // When loading a save, this script checks to see if the player should be in a different level.
    // If they should be, it relocates them.

    // For some reason, moving this LoadNewExplorationLevel command to a different script causes the game to crash.
    // Not sure why. So I do it here.

    void Start()
    {


        // Later levels should be higher in this list.
        if (ConversationTrigger.GetToken("reachedLevel_CityVault"))
        {
            LoadUtils.LoadNewExplorationLevel("CityVault", new Vector3(0, 5, 0));
            return;
        }

        if (ConversationTrigger.GetToken("reachedLevel_RuinedCity"))
        {
            LoadUtils.LoadNewExplorationLevel("RuinedCity", new Vector3(0, 5, 0));
            return;
        }
        else
        {
            transform.position = new Vector3(-160, -32, 60);
            Debug.Log("transformierung");
        }


    }
}
