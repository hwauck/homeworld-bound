using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;

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

    public UnityEvent readyForNextLevel;


    // Use this for initialization
    void Start () {
        partsFound = 0;
        partsNeededText = "??";
        objectToBuild = "???";
	}

    public void incParts()
    {
        partsFound++;
        partsFoundText.text = objectToBuild + " Parts: " + partsFound + "/" + partsNeededText;
        showParts();
        if(partsFound==partsNeeded)
        {
            partsDone = true;

            Debug.Log("Setting batteriesBuilt back to 0!");
            batteryCounter.setBatteriesBuilt(0); // reset batteries built since we used all of the previous ones up building the Rocket Boots

            StartCoroutine(waitThenHide(6));

            readyForNextLevel.Invoke();
        }

        //if (partsDone && batteryCounter.allPartsCollected()) {
        //    fadeScreen.fadeOut(3f);
        //    resetCounter();
        //    batteryCounter.resetCounter();
        //    levelTimer.stopTimer();
        //    levelTimer.resetTimer();
        //    InventoryController.levelName = SceneManager.GetActiveScene().name;
        //    StartCoroutine(waitThenLoadLevel(3f, whatToBuild));
        //}
    }

    private IEnumerator waitThenHide(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hideParts();
    }

    public bool allPartsCollected()
    {
        return partsDone;
    }

    public void resetCounter()
    {
        // if this reset is due to completing the level, reset object text to ???
        // if this reset is due to power failure (running out of time), keep object text
        if (partsDone)
        {
            objectToBuild = "???";
            partsNeededText = "??";
        }
        partsFound = 0;
        partsDone = false;
        partsFoundText.text = objectToBuild + " Parts: " + partsFound + "/" + partsNeededText;

    }

    public void setObjectToBuild(string objectToBuild)
    {
        this.objectToBuild = objectToBuild;
    }

    public void setPartsNeeded(int partsNeeded)
    {
        this.partsNeeded = partsNeeded;
        partsNeededText = partsNeeded + "";
        partsFoundText.text = objectToBuild + " Parts: " + partsFound + "/" + partsNeededText;

    }

    // make PartCounter UI element visible
    public void showParts()
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.transform.GetComponentInChildren<Text>().enabled = true;

    }

    // make PartCounter UI element invisible
    public void hideParts()
    {
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.transform.GetComponentInChildren<Text>().enabled = false;
    }

}
