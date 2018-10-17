﻿using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BatteryCounter : MonoBehaviour {

    private int partsFound;
    private int partsNeeded;
    private int batteriesBuilt;
    private int batteriesNeeded;
    public Text partsFoundText;
    public PartCounter partCounter;
    public Image batteriesBuiltBG;
    public Text batteriesBuiltText;
    public Timer levelTimer;
    public FadeScreen fadeScreen;

    private string whatToBuild;
    private bool partsDone = false; // has the player collected all the parts they need yet? 

    public UnityEvent readyForNextLevel;

    // Use this for initialization
    void Start () {
        partsFound = 0;
        batteriesBuilt = 0;
        if (!RocketBoots.GetBootsActive())
        {
            partsNeeded = 4;
            batteriesNeeded = 4;
        }

    }

    // make BatteryCounter UI element invisible
    public void hide()
    {
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.transform.GetComponentInChildren<Text>().enabled = false;
    }

    // make BatteryCounter UI element visible
    public void show()
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.transform.GetComponentInChildren<Text>().enabled = true;
        if(batteriesBuilt > 0)
        {
            batteriesBuiltText.text = "Batteries Built: " + batteriesBuilt + "/" + batteriesNeeded;
            batteriesBuiltBG.enabled = true;
            batteriesBuiltText.enabled = true;
        }
    }

    public int getBatteriesBuilt()
    {
        return batteriesBuilt;
    }

    public void incParts()
    {
        partsFound++;
        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeeded;
        show();
        Debug.Log("partsFound in incParts: " + partsFound);

        if (partsFound == partsNeeded)
        {
            partsDone = true;
            // reset battery pickup conversation for next battery level
            ConversationTrigger.RemoveToken("picked_up_a_battery");
            batteriesBuilt++; // technically, they're not built yet. But they will be when the player returns to scene.

            StartCoroutine(waitThenHide(6));

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

    private IEnumerator waitThenHide(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hide();
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
                partsNeeded = 4;

            }

        }
        partsFound = 0;
        partsDone = false;

        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeeded;
        Debug.Log("partsFoundText in resetCounter: " + partsFoundText.text);

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
        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeeded;
        Debug.Log("partsFoundText in setPartsNeeded: " + partsFoundText.text);

    }

    // Update is called once per frame
    void Update () {
		
	}
}