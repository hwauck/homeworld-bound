using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PartCounter : MonoBehaviour {

    private int partsFound;
    private int partsNeeded;
    private string partsNeededText;
    public Text partsFoundText;
    private string whatToBuild;
    private string objectToBuild;
    public BatteryCounter batteryCounter;
    public Timer levelTimer;
    public FadeScreen fadeScreen;
    private bool partsDone = false; // has the player collected all the parts they need yet? 

	// Use this for initialization
	void Start () {
        partsFound = 5;
        partsNeededText = "??";
        objectToBuild = "???";
	}

    public void incParts()
    {
        partsFound++;
        partsFoundText.text = objectToBuild + " Parts: " + partsFound + "/" + partsNeededText;
        if(partsFound==partsNeeded)
        {
            partsDone = true;
        }

        Debug.Log("PART COUNTER items parts done? " + partsDone + "item parts found: " + partsFound);
        Debug.Log("PART COUNTER battery parts done? " + batteryCounter.allPartsCollected());
        if (partsDone && batteryCounter.allPartsCollected()) {
            fadeScreen.fadeOut(3f);
            resetCounter();
            batteryCounter.resetCounter();
            levelTimer.stopTimer();
            levelTimer.resetTimer();
            InventoryController.levelName = SceneManager.GetActiveScene().name;
            StartCoroutine(waitThenLoadLevel(3f, whatToBuild));
        }
    }

    public bool allPartsCollected()
    {
        return partsDone;
    }

    private IEnumerator waitThenLoadLevel(float seconds, string level)
    {
        yield return new WaitForSeconds(seconds);
        LoadUtils.LoadScene(level);

    }

    public void resetCounter()
    {
        partsFound = 0;
        partsDone = false;
        objectToBuild = "???";
        partsNeededText = "??";
    }

    public void setWhatToBuild(string whatToBuild, string objectToBuild)
    {
        this.whatToBuild = whatToBuild;
        batteryCounter.setWhatToBuild(whatToBuild);
        this.objectToBuild = objectToBuild;

        // CHANGE for each new Item construction level added
        if(whatToBuild.Equals("b1")) {
            setPartsNeeded(7);
            batteryCounter.setPartsNeeded(14);
        }
        else if (whatToBuild.Equals("b5"))
        {
            setPartsNeeded(12);
            //batteryCounter.setPartsNeeded()
        }
    }

    public void setPartsNeeded(int partsNeeded)
    {
        this.partsNeeded = partsNeeded;
        partsNeededText = partsNeeded + "";
        partsFoundText.text = objectToBuild + " Parts: " + partsFound + "/" + partsNeededText;

    }



    // Update is called once per frame
    void Update () {
		
	}
}
