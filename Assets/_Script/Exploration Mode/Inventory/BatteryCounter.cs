using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BatteryCounter : MonoBehaviour {

    private int partsFound;
    private int partsNeeded;
    private string partsNeededText;
    public Text partsFoundText;
    public PartCounter partCounter;
    public Timer levelTimer;
    public FadeScreen fadeScreen;

    private string whatToBuild;
    private bool partsDone = false; // has the player collected all the parts they need yet? 

    // Use this for initialization
    void Start () {
        partsFound = 12;
        if (!RocketBoots.GetBootsActive())
        {
            partsNeededText = 14 + "";

        }
        else
        {
            partsNeededText = "??";

        }
    }

    public void incParts()
    {
        partsFound++;
        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeededText;
        if (partsFound == partsNeeded)
        {
            partsDone = true;
        }

        Debug.Log("BATTERY COUNTER battery parts done? " + partsDone);
        Debug.Log("BATTERY COUNTER item parts done? " + partCounter.allPartsCollected());
        if (partsDone && partCounter.allPartsCollected()) {
            fadeScreen.fadeOut(3f);
            resetCounter();
            partCounter.resetCounter();
            levelTimer.stopTimer();
            levelTimer.resetTimer();
            InventoryController.levelName = SceneManager.GetActiveScene().name;
            StartCoroutine(waitThenLoadLevel(3f, whatToBuild));
        }
    }

    public void resetCounter()
    {
        partsFound = 0;
        partsDone = false;
        // CHANGE when more levels are added
        if (!RocketBoots.GetBootsActive())
        {
            partsNeededText = 14 + "";

        } else
        {
            partsNeededText = "??";

        }
    }

    public bool allPartsCollected()
    {
        return partsDone;
    }

    public void setWhatToBuild(string whatToBuild)
    {
        this.whatToBuild = whatToBuild;

    }

    private IEnumerator waitThenLoadLevel(float seconds, string level)
    {
        yield return new WaitForSeconds(seconds);
        LoadUtils.LoadScene(level);

    }

    public void setPartsNeeded(int partsNeeded)
    {
        this.partsNeeded = partsNeeded;
        partsNeededText = partsNeeded + "";
        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeededText;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
