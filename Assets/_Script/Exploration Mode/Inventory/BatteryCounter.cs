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

    public readonly int[] PARTS_NEEDED = { 4, 4, 7, 6, 5, 7, 7, 7 };


    private string whatToBuild;
    private bool partsDone = false; // has the player collected all the parts they need yet? 

    public UnityEvent readyForNextLevel;
    public UnityEvent newBatteryBuilt; // triggers addition of battery to ExplorationLevelResetter's numBatteriesBuilt variable

    // Use this for initialization
    void Start () {
        partsFound = 0;
        batteriesBuilt = 0;
        // for testing only!
        //batteriesBuilt = 3;
        setCounterMaximums();
        hideBatteryParts();




    }

    // called when finishedLoad event is invoked by InventoryController
    public void loadSavedBatteryParts()
    {
        // figure out how many batteries player should start with
        int savedBatteries = InventoryController.getSavedBatteryPartCount();
    }

    // sets what partsNeeded and batteriesBuilt should be depending on
    // player's progress
    private void setCounterMaximums()
    {
        // CHANGE when more levels are added
        if (!ConversationTrigger.GetToken("finished_b1"))
        {
            partsNeeded = PARTS_NEEDED[0];
            batteriesNeeded = 4;
        }
        else if (!ConversationTrigger.GetToken("finished_b2"))
        {
            partsNeeded = PARTS_NEEDED[1];
            batteriesNeeded = 4;
        }
        else if (!ConversationTrigger.GetToken("finished_b3"))
        {
            partsNeeded = PARTS_NEEDED[2];
            batteriesNeeded = 4;
        }
        else if (!ConversationTrigger.GetToken("finished_b4"))
        {
            partsNeeded = PARTS_NEEDED[3];
            batteriesNeeded = 4;
        }
        else if (!ConversationTrigger.GetToken("finished_b5"))
        {
            partsNeeded = PARTS_NEEDED[4];
            batteriesNeeded = 4;

        }
        else if (!ConversationTrigger.GetToken("finished_b6"))
        {
            partsNeeded = PARTS_NEEDED[5];
            batteriesNeeded = 4;
        }
        else if (!ConversationTrigger.GetToken("finished_b7"))
        {
            partsNeeded = PARTS_NEEDED[6];
            batteriesNeeded = 4;
        }
        else if (!ConversationTrigger.GetToken("finished_b8"))
        {
            partsNeeded = PARTS_NEEDED[7];
            batteriesNeeded = 4;
        }
        else
        {
            partsNeeded = 0;
            batteriesNeeded = 0;
        }
    }

    // make BatteryCounter UI element invisible
    public void hideBatteryParts()
    {
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.transform.GetComponentInChildren<Text>().enabled = false;
    }

    public void hideBatteriesBuilt()
    {
        batteriesBuiltBG.enabled = false;
        batteriesBuiltText.enabled = false;
    }

    // make BatteryCounter UI element visible
    public void showBatteryParts()
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.transform.GetComponentInChildren<Text>().enabled = true;

    }

    public void showBatteriesBuilt()
    {
        batteriesBuiltText.text = "Batteries Built: " + batteriesBuilt + "/" + batteriesNeeded;
        batteriesBuiltBG.enabled = true;
        batteriesBuiltText.enabled = true;
    }

    public int getBatteriesBuilt()
    {
        return batteriesBuilt;
    }

    public void setBatteriesBuilt(int num)
    {
        batteriesBuilt = num;
    }

    public void setBatteryParts(int num)
    {
        partsFound = num;
    }

    public void incParts(bool isFromSave)
    {
        if(partsFound == 0)
        {
            setCounterMaximums(); // figure out how many batteries are needed for next Construction level
        }
        partsFound++;
        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeeded;
        Debug.Log("Battery parts found: " + partsFound);
        Debug.Log("Battery parts needed: " + partsNeeded);
        showBatteryParts();

        // This condition is only met when loading from a save where a battery level was just completed and the 
        // player now needs to collect more parts
        Debug.Log("incParts: What to Build: " + getWhatToBuild());
        if (partsFound == partsNeeded && isFromSave && ConversationTrigger.GetToken("finished_" + getWhatToBuild()) && !ConversationTrigger.GetToken("not_finished_const_map_intro"))
        {
            // do nothing
            batteriesBuilt++;
            Debug.Log("Incrementing batteriesBuilt from BatteryCounter.incParts(" + isFromSave + ") - now " + batteriesBuilt);

            resetCounter();
            batteriesBuiltBG.gameObject.SetActive(true); // ExplorationLevelResetter normally triggers this on level load
            newBatteryBuilt.Invoke();

            // reset battery pickup conversation for next battery level
            ConversationTrigger.RemoveToken("picked_up_a_battery");

            ConversationTrigger.RemoveToken("not_finished_collecting_" + getWhatToBuild());


            Debug.Log("Don't Load Next level: Battery Counter's WhatToBuild: " + getWhatToBuild());

        } else if (partsFound == partsNeeded && isFromSave && ConversationTrigger.GetToken("finished_b4") && ConversationTrigger.GetToken("not_finished_const_map_intro"))
        {
            ConversationTrigger.RemoveToken("read_fuser_log");
            ConversationTrigger.RemoveToken("show_locate_button");
            ConversationTrigger.RemoveToken("HardInstant_Const_fuserLog");
            ConversationTrigger.RemoveToken("oneShot_locate_button");

            StartCoroutine(waitThenHide(6));

            readyForNextLevel.Invoke();
        } else if (partsFound == partsNeeded)
        {
            Debug.Log("Go to Next Level: Battery Counter's WhatToBuild: " + getWhatToBuild());
            partsDone = true;

            // reset battery pickup conversation for next battery level
            ConversationTrigger.RemoveToken("picked_up_a_battery");

            batteriesBuilt++; // technically, they're not built yet. But they will be when the player returns to scene.
            Debug.Log("Incrementing batteriesBuilt from BatteryCounter.incParts(" + isFromSave + ") - now " + batteriesBuilt);

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
        hideBatteryParts();
    }

    public void resetCounter()
    {
        // if this reset is due to completing the level, reset object text to ???
        // if this reset is due to power failure (running out of time), keep object text
 //       if (partsDone)
  //      {
  //          setCounterMaximums();

  //      }
        
        // normally, we reset the counter to 0 every time all parts are collected. But if the player somehow got
        // 2 batteries at the same time (some battery pairs are close enough together that this might be possible),
        // keep the extra battery to carry over to the next level
        if(partsFound > partsNeeded)
        {
            partsFound = 1;
        } else
        {
            partsFound = 0;
        }
        partsDone = false;

        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeeded;

    }

    public bool allPartsCollected()
    {
        return partsDone;
    }

    public void setWhatToBuild(string whatToBuild)
    {
        this.whatToBuild = whatToBuild;

    }

    public string getWhatToBuild()
    {
        return whatToBuild;
    }

    public void setPartsNeeded(int partsNeeded)
    {
        this.partsNeeded = partsNeeded;
        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeeded;

    }

}
