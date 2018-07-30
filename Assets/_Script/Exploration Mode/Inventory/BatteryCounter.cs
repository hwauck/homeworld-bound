using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

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

    public UnityEvent readyForNextLevel;

    // Use this for initialization
    void Start () {
        partsFound = 0;
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
            readyForNextLevel.Invoke();
        }           


        //if (partsDone && partCounter.allPartsCollected()) {
        //    fadeScreen.fadeOut(3f);
        //    resetCounter();
        //    partCounter.resetCounter();
        //    levelTimer.stopTimer();
        //    levelTimer.resetTimer();
        //    InventoryController.levelName = SceneManager.GetActiveScene().name;
        //    StartCoroutine(waitThenLoadLevel(3f, whatToBuild));
        //}
    }

    public void resetCounter()
    {
        // if this reset is due to completing the level, reset object text to ???
        // if this reset is due to power failure (running out of time), keep object text
        if (partsDone)
        {
            // CHANGE when more levels are added
            if (!RocketBoots.GetBootsActive())
            {
                partsNeededText = 14 + "";

            }
            else
            {
                partsNeededText = "??";

            }
        }
        partsFound = 0;
        partsDone = false;

        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeededText;

    }

    public bool allPartsCollected()
    {
        return partsDone;
    }

    public void setWhatToBuild(string whatToBuild)
    {
        this.whatToBuild = whatToBuild;

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
