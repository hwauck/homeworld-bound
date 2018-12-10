using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class ExplorationLevelResetter : MonoBehaviour {

    public Timer timer;
    private bool taggedFirstPart;
    private bool resetDueToPowerFailure;

    public FadeScreen screenFader;
    public Text lowPowerText;
    public CanvasGroup errorPanel;
    public AudioSource audioSource;
    public ConversationTrigger Exp_firstBattery_b2;
    public ConversationTrigger Exp_firstBattery_b3;
    public ConversationTrigger Exp_firstBattery_b4;

    public Text powerFailureText;
    public CanvasGroup countdownPanel;
    public Text countdownText;
    public Text rechargingText;
    public Text demoFinishedText;
    public Text demoFinishedAltText;
    public Map map;

    private AudioClip powerFailureSound;
    private AudioClip countdownSound;
    private AudioClip finalCountSound;
    private AudioClip rechargingSound;
    private AudioClip powerUpSound;

    public RigidbodyFirstPersonController controller;
    public PartCounter itemPartCounter;
    public BatteryCounter batteryPartCounter;
    public GameObject fuserObject;

    private float forwardSpeed;
    private float backwardSpeed;
    private float strafeSpeed;
    private float jumpForce;
    private float XRotSensitivity;
    private float YRotSensitivity;

    private string whatToBuild;

    public GameObject[] rocketBootParts;

    public UnityEvent gameQuit;

    // Javascript methods imported from browserUnityInteraction.jslib Plugin
    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void HelloString(string str);

    [DllImport("__Internal")]
    private static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    private static extern int AddNumbers(int x, int y);

    [DllImport("__Internal")]
    private static extern string StringReturnValueFunction();

    [DllImport("__Internal")]
    private static extern void BindWebGLTexture(int texture);



    private void Awake()
    {
        taggedFirstPart = false;
        resetDueToPowerFailure = false;
        powerFailureSound = Resources.Load<AudioClip>("Audio/BothModes/msfx_chrono_latency_hammer");
        countdownSound = Resources.Load<AudioClip>("Audio/BothModes/Select02");
        finalCountSound = Resources.Load<AudioClip>("Audio/BothModes/Select04");
        rechargingSound = Resources.Load<AudioClip>("Audio/BothModes/DM-CGS-03");
        powerUpSound = Resources.Load<AudioClip>("Audio/BothModes/Slider3");

        // save original values of all player control variables in RigidBodyFirstPersonController
        forwardSpeed = controller.movementSettings.ForwardSpeed;
        backwardSpeed = controller.movementSettings.BackwardSpeed;
        strafeSpeed = controller.movementSettings.StrafeSpeed;
        jumpForce = controller.movementSettings.JumpForce;
        XRotSensitivity = controller.mouseLook.XSensitivity;
        YRotSensitivity = controller.mouseLook.YSensitivity;
    }

    //reset fadeOutPanel from last scene transition if needed
    private void OnEnable()
    {
        lowPowerText.enabled = false;

        //TODO: add condition for sledgehammer level
        if(ConversationTrigger.GetToken("finished_b4") && !ConversationTrigger.GetToken("finished_RB"))
        {
            disablePlayerControl();

            //reveal all rocket boots parts so player can collect them
            //Debug.Log("Activating Rocket Boots parts!");
            for (int i = 0; i < rocketBootParts.Length; i++)
            {
                //Debug.Log("Activating part " + i + ": " + rocketBootParts[i]);
                rocketBootParts[i].SetActive(true);
            }

            screenFader.fadeIn(1f);

            map.doIntroMap(); // when this is done, it triggers startCountdown() and beginning of timed level
        } else if (ConversationTrigger.GetToken("finished_RB"))
        {
            screenFader.fadeIn(1f);
            enablePlayerControl();
        } else
        {
            enablePlayerControl();
        }


    }

    // Use this for initialization
    void Start () {
		
	}

    // when player reaches highlands level with rocket boots, fade out to DEMO FINISHED words
    public void doFadeToDemoFinished(float seconds, bool playerInitiated)
    {
        gameQuit.Invoke(); // sends out broadcast that game is over; any other scripts can perform actions based on this
        disablePlayerControl();
        StopAllCoroutines();
        timer.stopTimer();
        timer.stopMusic();
        ConversationController.Disable();
        errorPanel.alpha = 0;
        countdownPanel.alpha = 0;
        StartCoroutine(fadeToDemoFinished(seconds, playerInitiated));
    }

    private IEnumerator fadeToDemoFinished(float seconds, bool playerInitiated)
    {
        screenFader.fadeOut(seconds);
        yield return new WaitForSeconds(seconds);

        if(playerInitiated)
        {
            demoFinishedAltText.enabled = true;
            yield return new WaitForSeconds(3f);
            // load next page, however that's done
            Hello();

        } else
        {
            demoFinishedText.enabled = true;
        }
    }

    IEnumerator introTimer()
    {
        // move to center of screen
        RectTransform rrRect = timer.gameObject.GetComponent<RectTransform>();
        rrRect.anchoredPosition = new Vector3(0f, -233f, 0f);
        timer.gameObject.GetComponent<CanvasGroup>().alpha = 1; //show TimeRemainingPanel in center of screen

        Highlighter.Highlight(timer.gameObject);
        yield return new WaitForSeconds(1f);
        Highlighter.Unhighlight(timer.gameObject);

        // zoom TimeRemainingPanel to upper right
        Vector3 startPosition = rrRect.anchoredPosition;
        Vector3 endPosition = new Vector3(0, 0, 0);
        float lerpTime = 0.5f;
        float currentLerpTime = 0f;

        while (Vector3.Distance(rrRect.anchoredPosition, endPosition) > 2)
        {
            rrRect.anchoredPosition = Vector3.Lerp(startPosition, endPosition, currentLerpTime / lerpTime);
            currentLerpTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        rrRect.anchoredPosition = endPosition;
    }

    public void setWhatToBuild(string whatToBuild)
    {
        this.whatToBuild = whatToBuild;

    }

    private void setWhatToBuild()
    {
        // CHANGE this every time a new battery level is added
        if(!ConversationTrigger.GetToken("finished_b1"))
        {
            whatToBuild = "b1";
            Exp_firstBattery_b2.enabled = true;

        }
        else if (!ConversationTrigger.GetToken("finished_b2"))
        {
            whatToBuild = "b2";
            Exp_firstBattery_b3.enabled = true;

        }
        else if (!ConversationTrigger.GetToken("finished_b3"))
        {
            whatToBuild = "b3";
            Exp_firstBattery_b4.enabled = true;

        }
        else if (!ConversationTrigger.GetToken("finished_b4"))
        {
            whatToBuild = "b4";
        } else if (!ConversationTrigger.GetToken("finished_RB"))
        {
            whatToBuild = "rocketBoots";
        } else if (!ConversationTrigger.GetToken("finished_b5"))
        {
            whatToBuild = "b5";
        } else if (!ConversationTrigger.GetToken("finished_b6"))
        {
            whatToBuild = "b6";
        } else if (!ConversationTrigger.GetToken("finished_b7"))
        {
            whatToBuild = "b7";
        } else if (!ConversationTrigger.GetToken("finished_b8"))
        {
            whatToBuild = "b8";
        } else if (!ConversationTrigger.GetToken("finished_sledgehammer"))
        {
            whatToBuild = "sledgehammer";
        } else
        {
            whatToBuild = "none";
        }

        //TESTING ONLY
        whatToBuild = "b4";
        if(ConversationTrigger.GetToken("finished_b4"))
        {
            whatToBuild = "rocketBoots";
        }
    }

    //invoke this method from PartCounter/BatteryCounter whenever all parts are collected (batteries) within time limit (items)
    public void prepareNextLevel()
    {
        setWhatToBuild();

        if (batteryPartCounter.allPartsCollected())
        {
            // play power up noise
            audioSource.PlayOneShot(powerUpSound);
            // player gets "All battery parts collected!" message
            ConversationTrigger.AddToken("all_battery_parts_collected");
            ConversationTrigger.RemoveToken("disable_all_battery_parts_collected");

            batteryPartCounter.resetCounter();

            //remembers which Exploration Mode we were in so we can get back
            InventoryController.levelName = SceneManager.GetActiveScene().name;
            StartCoroutine(waitForEndOfConvoThenLoadLevel(3f, whatToBuild));
  
        } else if (itemPartCounter.allPartsCollected())
        {
            // play power up noise
            audioSource.PlayOneShot(powerUpSound);

            screenFader.fadeOut(3f);
            map.hide();

            itemPartCounter.hideParts();
            itemPartCounter.resetCounter();
            timer.stopTimer();
            timer.resetTimer();

            //remembers which Exploration Mode we were in so we can get back
            InventoryController.levelName = SceneManager.GetActiveScene().name;
            StartCoroutine(waitThenLoadLevel(3f, whatToBuild));
        } else
        {
            Debug.LogError("Error: prepareNextLevel() should not be called before all item/battery parts have been collected");
        }
    }

    private IEnumerator waitForEndOfConvoThenLoadLevel(float seconds, string whatToBuild)
    {
        Quaternion startingRotation = fuserObject.gameObject.transform.rotation;
        Quaternion endingRotation = startingRotation * Quaternion.Euler(0, 0, 90);
        float lerpTime = 1f;
        float currentLerpTime = 0f;
        controller.GetComponent<Fuser>().ActivateFuser();

        //wait until player has closed the "You got all the batteries!" textbox
        while (!ConversationController.currentlyEnabled)
        {
            yield return new WaitForFixedUpdate();
        }
        ConversationTrigger.AddToken("disable_all_battery_parts_collected");
        ConversationTrigger.RemoveToken("all_battery_parts_collected");
        while (ConversationController.currentlyEnabled)
        {

            yield return new WaitForFixedUpdate();
        }

        disablePlayerControl();

        screenFader.fadeOut(0.2f);

        while (Quaternion.Angle(fuserObject.transform.rotation, endingRotation) > 2)
        {
            fuserObject.gameObject.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, currentLerpTime / lerpTime);
            currentLerpTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        //reset Fuser rotation/position for next time Fuser is used
        fuserObject.gameObject.transform.rotation = startingRotation;

        batteryPartCounter.resetCounter();
        InventoryController.levelName = SceneManager.GetActiveScene().name;
        Debug.Log("whatToBuild: " + whatToBuild);

        if(whatToBuild.Equals("b1"))
        {   // first battery level only
            lowPowerText.enabled = true;
            lowPowerText.text = "Welcome to the Fuser X7000 - the premier technology for constructing and crafting!";
            audioSource.PlayOneShot(powerUpSound);
            yield return new WaitForSeconds(4f);
            lowPowerText.text = "Fuser battery parts detected. Activating low power construction mode!";
            audioSource.PlayOneShot(powerUpSound);

        } else if (whatToBuild.StartsWith("b") && whatToBuild.Length == 2)
        {   // all battery levels except first one
            lowPowerText.enabled = true;
            lowPowerText.text = "Welcome to the Fuser X7000!";
            audioSource.PlayOneShot(powerUpSound);
            yield return new WaitForSeconds(2f);
            lowPowerText.text = "Fuser battery parts detected. Activating low power construction mode!";
            audioSource.PlayOneShot(powerUpSound);
        }
            else
        {   //all item levels
            lowPowerText.enabled = true;
            lowPowerText.text = "Welcome to the Fuser X7000!";
            audioSource.PlayOneShot(powerUpSound);
            yield return new WaitForSeconds(2f);
            lowPowerText.text = "New parts detected. Activating full power construction mode!";
            audioSource.PlayOneShot(powerUpSound);
        }

        StartCoroutine(waitThenLoadLevel(seconds, whatToBuild));
    }

    //call this method whenever there's a power failure
    public void powerFailure()
    {
        //disable player controls
        disablePlayerControl();
        //flash warning: power failure!
        powerFailureText.enabled = true;
        errorPanel.alpha = 1;
        audioSource.PlayOneShot(powerFailureSound);

        //start fading to black and then recharge
        StartCoroutine(waitThenContinueFailure(1f, 3f));


        //wait 6 seconds then Dresha tells you to start looking again and that timer will start once you've tagged first part
    }

    private IEnumerator waitThenContinueFailure(float waitSeconds, float fadeSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        screenFader.fadeOut(fadeSeconds);
        map.hide();
        powerFailureText.enabled = false;
        errorPanel.alpha = 0;
        yield return new WaitForSeconds(fadeSeconds);
        itemPartCounter.resetCounter();
        taggedFirstPart = false;

        //untagAllPartsInLevel();

        ConversationTrigger.AddToken("outOfPower");
    }

    //obsolete
    private void untagAllPartsInLevel()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");
        for (int i = 0; i < pickups.Length; i++)
        {
            pickups[i].GetComponent<Collider>().enabled = true;
            ParticleSystem ps = pickups[i].GetComponent<ParticleSystem>();
            ParticleSystem.MainModule psMain = pickups[i].GetComponent<ParticleSystem>().main;

            //turn the particle effect back to original white/magenta
            if (pickups[i].GetComponent<PickUp>().type == PickUp.PickupType.Battery)
            {
                psMain.startColor = new Color(255f, 0f, 255f, 255f);

            }
            else
            {
                psMain.startColor = new Color(255f, 255f, 255f, 255f);
            }
        }
    }

    private IEnumerator waitThenRestart(float seconds, string token)
    {
        yield return new WaitForSeconds(seconds);
        map.show();
        Debug.Log("ADDING CONVO TOKEN " + token + "!");
        ConversationTrigger.AddToken(token);
    }

    private IEnumerator rechargingAndRestart()
    {
        rechargingText.enabled = true;

        for (int i = 0; i < 3; i++)
        {
            rechargingText.text = "Recharging.  ";
            yield return new WaitForSeconds(0.25f);
            rechargingText.text = "Recharging.. ";
            yield return new WaitForSeconds(0.25f);
            rechargingText.text = "Recharging...";
            audioSource.PlayOneShot(rechargingSound);
            yield return new WaitForSeconds(0.5f);
            rechargingText.text = "Recharging   ";
        }
        rechargingText.enabled = false;
        for(int i = 0; i < rocketBootParts.Length; i++)
        {
            rocketBootParts[i].SetActive(true);
        }
        timer.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        timer.resetTimer();
        screenFader.fadeIn(3f);
        StartCoroutine(waitThenRestart(3f, "letsRestart"));

    }

    private void disablePlayerControl()
    {
        controller.movementSettings.ForwardSpeed = 0f;
        controller.movementSettings.BackwardSpeed = 0f;
        controller.movementSettings.StrafeSpeed = 0f;
        controller.movementSettings.JumpForce = 0f;

        //doesn't work?
        controller.mouseLook.XSensitivity = 0;
        controller.mouseLook.YSensitivity = 0;
    }

    private void enablePlayerControl()
    {
        controller.movementSettings.ForwardSpeed = forwardSpeed;
        controller.movementSettings.BackwardSpeed = backwardSpeed;
        controller.movementSettings.StrafeSpeed = strafeSpeed;
        controller.movementSettings.JumpForce = jumpForce;

        controller.mouseLook.XSensitivity = XRotSensitivity;
        controller.mouseLook.YSensitivity = YRotSensitivity;
    }

    public void startCountdown()
    {
        ConversationTrigger.AddToken("introTimer");
        StartCoroutine(doCountdownAndEnableControls());
    }

    public bool hasTaggedFirstPart()
    {
        return taggedFirstPart;
    }

    public void setTaggedFirstPart(bool taggedFirstPart)
    {
        this.taggedFirstPart = taggedFirstPart;
    }

    private IEnumerator doCountdownAndEnableControls()
    {
     //   while(!ConversationTrigger.GetToken("doneRestarting"))
     //   {
     //       Debug.Log("Waiting for doneRestarting token!");
     //       yield return new WaitForFixedUpdate();
     //   }
     //   ConversationTrigger.RemoveToken("doneRestarting");
        ConversationTrigger.RemoveToken("letsRestart");
        ConversationTrigger.RemoveToken("introTimer");
        StartCoroutine(introTimer()); 

        yield return new WaitForSeconds(3f);

        countdownPanel.alpha = 1;
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = "" + i;
            audioSource.PlayOneShot(countdownSound);
            yield return new WaitForSeconds(1f);

        }
        countdownText.text = "GO!";
        audioSource.PlayOneShot(finalCountSound);
        yield return new WaitForSeconds(1f);
        countdownPanel.alpha = 0;

        enablePlayerControl();

        timer.startTimer();
    }

    private IEnumerator waitThenLoadLevel(float seconds, string level)
    {
        yield return new WaitForSeconds(seconds);

        LoadUtils.LoadScene(level);

    }

    // Update is called once per frame
    void Update () {

        // player presses P key, game ends and player sees demo finished screen, goes to post-game survey
        if(Input.GetKeyDown(KeyCode.P))
        {
            doFadeToDemoFinished(3f, true);
            
        }

        //Debug.Log(ConversationTrigger.GetToken("firstPickup"));
        // finished recharging after power failure
        if (ConversationTrigger.GetToken("outOfPower") && ConversationTrigger.GetToken("hasPower"))
        {
            Debug.Log("Finished recharging after power failure!");
            ConversationTrigger.RemoveToken("outOfPower");
            ConversationTrigger.RemoveToken("hasPower");
            StartCoroutine(rechargingAndRestart());

        }
        // when the restart message finishes, reenable controls and start countdown
        else if (ConversationTrigger.GetToken("doneRestarting"))
        {
            ConversationTrigger.RemoveToken("doneRestarting");
            Debug.Log("Beginning countdown!");
            StartCoroutine(doCountdownAndEnableControls());
        }
        // first time level is started: may want this for as soon as the tutorial before first part is over
        else if (ConversationTrigger.GetToken("startBeginningConvo") && ConversationTrigger.GetToken("doneWithBeginningConvo"))
        {
            Debug.Log("This shouldn't be triggering");
            ConversationTrigger.RemoveToken("startBeginningConvo");
            ConversationTrigger.RemoveToken("doneWithBeginningConvo");
            StartCoroutine(doCountdownAndEnableControls());
        }
    }
}
