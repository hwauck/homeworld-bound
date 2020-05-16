using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// This script is used to reset the level when the player has run out of battery power
// and when the player has run out of time in timed Construction Mode levels.
// It also keeps track of some other specific level-agnostic things within 
// Construction Mode: transitioning from Construction Mode level to timed Exploration
// Mode level, telemetry data collection (in tandem with FuseEvent), and game quitting in Construction Mode.
// If you can't find Construction Mode level-agnostic operations here, try the FuseEvent script.
public class LevelResetter : MonoBehaviour {

    public Tutorial1 tutorial;
    public CanvasGroup errorPanel;
    public Text powerFailureText;
    public AudioSource audioSource;
    private AudioClip powerFailureSound;

    public CanvasGroup bottomPanel;
    public CameraControls cameraControls;
    public RotationGizmo rotationScript;
    public SelectPart selectPart;
    public FuseEvent fuseEvent;

    public Text rechargingText;
    public Button tryAgainButton;

    public CanvasGroup countdownPanel;
    public Text countdownText;
    public CanvasGroup timeRemainingPanel;
    public CanvasGroup rotationsRemainingPanel;

    private AudioClip countdownSound;
    private AudioClip finalCountSound;
    private AudioClip rechargingSound;
    private AudioClip hiddenMaterialsFoundSound;

    private GameObject[] parts;
    private MeshCollider[] meshColliders;

    // need this to get the "mode" so I can get the correct CreatePart script, bleh
    public GameObject eventSystem;

    public GameObject startingPart;
    private Vector3 startingPartFinalPos = new Vector3(-100, 30, 100);
    private Vector3 startingPartOffscreenPos = new Vector3(-100, -40, 100);
    private Quaternion startingPartRotation;
    private Vector3 originalColliderCenter;
    private Vector3 originalColliderSize;

    private const float MOVEMENT_SPEED = 100f;

    private bool runningJustConstructionMode = false;

    // variables for transition to timed Exploration Mode levels
    public Image fadeOutScreen;
    public Image map;
    public Text gameFinishedText;
    public FadeScreen screenFader;
    private AudioClip fullyChargedSound;
    private AudioClip logMessageSound;
    public Button readButton;
    public Button locateButton;
    public Button startButton;
    public Button claimButton;
    public Text showMapText;
    public ConversationController controller;

    // Data collection
    public ConstructionDataManager dataManager;

    public CanvasGroup confirmQuitPanel;
    public Button yesQuitButton;
    public Button noDontQuitButton;

    public UnityEvent gameQuit;

    private void Awake()
    {
        powerFailureSound = Resources.Load<AudioClip>("Audio/BothModes/msfx_chrono_latency_hammer");
        countdownSound = Resources.Load<AudioClip>("Audio/BothModes/Select02");
        finalCountSound = Resources.Load<AudioClip>("Audio/BothModes/Select04");
        rechargingSound = Resources.Load<AudioClip>("Audio/BothModes/DM-CGS-03");
        fullyChargedSound = Resources.Load<AudioClip>("Audio/ConstModeMusic/sfx_shield");
        logMessageSound = Resources.Load<AudioClip>("Audio/BothModes/Denied3");
        hiddenMaterialsFoundSound = Resources.Load<AudioClip>("Audio/ConstModeMusic/StartMap2");
        originalColliderCenter = startingPart.GetComponent<BoxCollider>().center;
        originalColliderSize = startingPart.GetComponent<BoxCollider>().size;
        startingPartRotation = startingPart.transform.rotation;


        if(InventoryController.levelName == "")
        {
            runningJustConstructionMode = true;

        }

        // For data collection.
        if (!dataManager)
        {
            GameObject dataManagerObject = GameObject.Find("DataCollectionManager");
            if (dataManagerObject != null) {
                dataManager = dataManagerObject.GetComponent<ConstructionDataManager>();
                gameQuit.AddListener(dataManagerObject.GetComponent<DataAggregator>().saveAndSendToServer);
            }
        }

    }

    // Use this for initialization
    void Start () {
        
    }

    public void setUpCurrentLevel()
    {
        StartCoroutine(waitAndThenZoomUpPart(1f));
        StartCoroutine(waitAndThenAddToken(1, "doneRestarting"));
        if (LoadUtils.currentSceneName.Equals("b1"))
        {
            StartCoroutine(waitAndThenAddToken(1, "startCameraControls"));
        }
        else if (LoadUtils.currentSceneName.Equals("b4") && ConversationTrigger.GetToken("finished_b4")) // hasn't completed the read fuser log/const intro map section yet
        {
            doTransitionToFuserLog();
        }
        else if (LoadUtils.currentSceneName.Equals("b8") && ConversationTrigger.GetToken("finished_b8")) // hasn't completed the read fuser log/const intro map section yet
        {
            doTransitionToFuserLog();

        }
    }

    //called by Claim Item button in b4 and b8 (and any battery level that comes right before a timed Exploration Mode level with a map)
    public void doTransitionToFuserLog()
    {
        if (LoadUtils.currentSceneName.Equals("b4"))
        {
            StartCoroutine(transitionToFuserLogFirstTime());
        }
        else
        {
            StartCoroutine(transitionToFuserLog());

        }
    }

    // the first time a timed map Exploration Mode level is introduced
    private IEnumerator transitionToFuserLogFirstTime()
    {
        claimButton.gameObject.SetActive(false);
        disablePlayerControls();
        if(dataManager)
        {
            dataManager.setPauseGameplay(true);
        }
        screenFader.fadeOut(1f);
        yield return new WaitForSeconds(1f);

        rechargingText.enabled = true;
        rechargingText.text = "Fuser is now fully charged!";
        audioSource.PlayOneShot(fullyChargedSound);
        yield return new WaitForSeconds(3f);
        rechargingText.text = "New log message detected.";
        audioSource.PlayOneShot(logMessageSound);
        readButton.gameObject.SetActive(true);
        // now wait for player input


    }

    // skips the Read Log part of the map intro if this isn't the first timed map Exploration Mode level
    private IEnumerator transitionToFuserLog()
    {
        claimButton.gameObject.SetActive(false);
        disablePlayerControls();
        if (dataManager)
        {
            dataManager.setPauseGameplay(true);
        }
        screenFader.fadeOut(1f);
        yield return new WaitForSeconds(1f);

        rechargingText.enabled = true;
        rechargingText.text = "Fuser is now fully charged!";
        audioSource.PlayOneShot(fullyChargedSound);
        yield return new WaitForSeconds(3f);

        audioSource.PlayOneShot(logMessageSound);
        ConversationTrigger.AddToken("read_fuser_log");
        ConversationTrigger.AddToken("show_locate_button");
        locateButton.gameObject.SetActive(true);
        // now wait for player input


    }

    //called by clicking on "Read" button - reads Fuser log
    // only for b4 level
    public void doReadLog()
    {
        rechargingText.enabled = false;
        readButton.gameObject.SetActive(false);

        // move controller to center of screen
        RectTransform controllerRect = controller.GetComponent<RectTransform>();
        controllerRect.anchorMin = new Vector2(0.5f, 0.5f);
        controllerRect.anchorMax = new Vector2(0.5f, 0.5f);
        controllerRect.anchoredPosition = new Vector2(0, 0);

        StartCoroutine(readLog());
 
    }

    private IEnumerator readLog()
    {
        // start log display
        ConversationTrigger.AddToken("read_fuser_log");
        Debug.Log("Added read_fuser_log token - should trigger Const_fuserlog convo");

        // wait till conversation finishes
        while (!ConversationTrigger.GetToken("show_locate_button"))
        {
            yield return new WaitForFixedUpdate();
        }

        //once convo is finished, Locate Hidden Materials button appears, triggered
        // by token given by read fuser log conversation
        locateButton.gameObject.SetActive(true);
    }

    //called by clicking on the Locate Hidden Materials button
    // only for levels transitioning to timed Exploration Mode
    public void doShowMap()
    {
        locateButton.gameObject.SetActive(false);
        StartCoroutine(showMap());
;    }

    private IEnumerator showMap()
    {
        rechargingText.text = "";
        rechargingText.enabled = true;
        for (int i = 0; i < 3; i++)
        {
            rechargingText.text = "Searching.  ";
            yield return new WaitForSeconds(0.25f);
            rechargingText.text = "Searching.. ";
            yield return new WaitForSeconds(0.25f);
            rechargingText.text = "Searching...";
            audioSource.PlayOneShot(rechargingSound);
            yield return new WaitForSeconds(0.5f);
            rechargingText.text = "Searching   ";
        }
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(hiddenMaterialsFoundSound);
        rechargingText.text = "Hidden materials located. Generating area map...";
        yield return new WaitForSeconds(3f);

        rechargingText.enabled = false;

        // show the map, explanation of map
        showMapText.gameObject.SetActive(true); // does this make sprite text appear?
        map.gameObject.SetActive(true);

        // and finally show Start button and tell save file that const_map_intro has been completed
        yield return new WaitForSeconds(2f);
        ConversationTrigger.RemoveToken("not_finished_const_map_intro");

        // wait until b4's map intro is complete before declaring the construction mode level "finished"
        ConversationTrigger.RemoveToken("battery_const_in_progress");
        Debug.Log("Removed not_finished_const_map_intro token");
        startButton.gameObject.SetActive(true);
    }

    private IEnumerator waitAndThenAddToken(float seconds, string token)
    {
        //this, combined with the doneRestarting token from the opening conversation, will start the
        //level countdown after the opening conversation is complete
        // if there is no opening conversation, simply add the line
        // ConversationTrigger.AddToken("doneRestarting")
        yield return new WaitForSeconds(seconds);
        ConversationTrigger.AddToken(token);
        Debug.Log("Added token " + token);
    }

    private IEnumerator waitAndThenZoomUpPart(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(startingPartZoomUp());

    }

    private IEnumerator rechargingAnimation()
    {
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
    }

    public void resetLevel()
    {
        //Debug.Log("RESETTING LEVEL!");
        if(dataManager)
        {
            dataManager.setPauseGameplay(true);
        }
        fuseEvent.stopMusic();
        powerFailureText.enabled = true;
        errorPanel.alpha = 1;
        audioSource.PlayOneShot(powerFailureSound);
        disablePlayerControls();
        if(tutorial != null)
        {
            tutorial.disableTooltips();
        }
        if (timeRemainingPanel != null)
        {
            timeRemainingPanel.GetComponent<Timer>().stopTimer();
        }

        RotationArrow[] rotationArrows = rotationScript.gameObject.GetComponentsInChildren<RotationArrow>();
        foreach(RotationArrow arrow in rotationArrows)
        {
            arrow.UnhighlightArrow();
        }
        StartCoroutine(doPowerFailure());

    }

    // makes the parts in the scene fall apart and downwards - only happens for item levels, not battery levels
    private IEnumerator doPowerFailure()
    {
        yield return new WaitForSeconds(1f);

        Vector3 rbPos;
        Vector3 explosionPosition;
        float xExpOffset, yExpOffset, zExpOffset;
        // all parts including the starting part should have the tag "part" on them for this to work correctly
        parts = GameObject.FindGameObjectsWithTag("part");
        for (int i = 0; i < parts.Length; i++)
        {
            // first, set all meshcolliders to convex to avoid bad interaction with Rigidbody
            meshColliders = parts[i].GetComponentsInChildren<MeshCollider>();
            for (int j = 0; j < meshColliders.Length; j++)
            {
                if (meshColliders[j].GetComponent<FuseBehavior>() == null)
                {
                    // is not an attachment region - make meshCollider convex to avoid errors with Rigidbodies
                    meshColliders[j].convex = true;
                } else 
                {
                    // is an attachment region - causes wonky behavior sometimes if meshcollider is set to convex
                    // so just disable it instead
                    meshColliders[j].enabled = false;
                }
            }
            // then, add Rigidbodies to apply a downward explosive force
            parts[i].AddComponent<Rigidbody>();
            parts[i].GetComponent<Rigidbody>().useGravity = false;
            rbPos = parts[i].transform.position;
            xExpOffset = (Random.Range(0, 1) * 2 - 1) * Random.Range(5, 15);
            yExpOffset = 5f;
            zExpOffset = (Random.Range(0, 1) * 2 - 1) * Random.Range(5, 15);

            explosionPosition = new Vector3(rbPos.x + xExpOffset, rbPos.y + yExpOffset, rbPos.z + zExpOffset);

            parts[i].GetComponent<Rigidbody>().AddExplosionForce(1000f, explosionPosition, 20f, 0f);

        }

        yield return new WaitForSeconds(1f);


        fadeOutScreen.enabled = true;
        screenFader.fadeOut(0.5f);
        powerFailureText.enabled = false;
        errorPanel.alpha = 0;

        yield return new WaitForSeconds(1f);

        //Try Again button appears
        ConversationTrigger.AddToken("outOfPower");

        //stop downward movement of parts
        for (int i = 0; i < parts.Length; i++)
        {
            Destroy(parts[i].GetComponent<Rigidbody>());
        }

        //need to wait till end of frame for the Destroy actions to go into effect
        yield return new WaitForEndOfFrame();

        meshColliders = startingPart.GetComponentsInChildren<MeshCollider>();

        for (int i = 0; i < meshColliders.Length; i++)
        {
            //change meshcolliders on startingPart back to non-convex if they weren't before
            if(!meshColliders[i].enabled)
            {
                meshColliders[i].enabled = true;
            }else if (meshColliders[i].gameObject.GetComponent<Convexity>() == null)
            {
                meshColliders[i].convex = false;
            }
        }

        fuseEvent.fuseCleanUp();

        string currentLevel;
        print("Running just construction mode? " + runningJustConstructionMode);
        print("SceneManager.GetActiveScene().name: " + SceneManager.GetActiveScene().name);
        print("LoadUtils.currentSceneName: " + LoadUtils.currentSceneName);
        if (runningJustConstructionMode)
        {
            currentLevel = SceneManager.GetActiveScene().name;
        } else
        {
            currentLevel = LoadUtils.currentSceneName;
        }

        // destroy all parts except starting part
        // CHANGE this to add the new level string each time a new level is added
        switch (currentLevel)
        {
            case "b1":
                eventSystem.GetComponent<CreatePartB1>().destroyAllCreatedParts();
                break;
            case "b2":
                eventSystem.GetComponent<CreatePartB2_harder>().destroyAllCreatedParts();
                break;
            case "b3":
                eventSystem.GetComponent<CreatePartB3_harder>().destroyAllCreatedParts();
                break;
            case "b4":
                eventSystem.GetComponent<CreatePartB4_harder>().destroyAllCreatedParts();
                break;
            case "rocketBoots":
                eventSystem.GetComponent<CreatePartRB>().destroyAllCreatedParts();
                break;
            case "b5":
                eventSystem.GetComponent<CreatePartB5>().destroyAllCreatedParts();
                break;
            case "b6":
                eventSystem.GetComponent<CreatePartB6>().destroyAllCreatedParts();
                break;
            case "b7":
                eventSystem.GetComponent<CreatePartB7>().destroyAllCreatedParts();
                break;
            case "b8":
                eventSystem.GetComponent<CreatePartB8>().destroyAllCreatedParts();
                break;
            case "sledgehammer":
                eventSystem.GetComponent<CreatePartSledge>().destroyAllCreatedParts();
                break;
            default:
                break;
        }

        // reset the starting part's box collider to its original size 
        Destroy(startingPart.GetComponent<BoxCollider>());
        BoxCollider newCollider = startingPart.AddComponent<BoxCollider>();
        newCollider.enabled = false;
        newCollider.center = originalColliderCenter;
        newCollider.size = originalColliderSize;
    }

    public void showTryAgainButton()
    {
        tryAgainButton.gameObject.SetActive(true);
    }

    private IEnumerator resetConstruction()
    {
        tryAgainButton.gameObject.SetActive(false);
        rechargingText.enabled = true;
        // simple ... progress animation for recharging text
        // takes 3 seconds for recharging animation to complete
        StartCoroutine(rechargingAnimation());

        // put starting part back to where it was
        startingPart.transform.SetPositionAndRotation(startingPartOffscreenPos, startingPartRotation);

        // reset victoryPrefab, otherwise it does weird stuff once level is complete
        fuseEvent.resetVictoryPrefab();

        // and reset camera
        cameraControls.gameObject.transform.SetPositionAndRotation(new Vector3(-90, 45, -3.36f), Quaternion.Euler(0, 0, 0));

        // and reset the number of rotations, time remaining, and fuseCount
        rotationsRemainingPanel.GetComponent<RotationCounter>().resetRotations();
        if (timeRemainingPanel != null)
        {
            timeRemainingPanel.GetComponent<Timer>().resetTimer();
        }
        fuseEvent.resetFuseCount();
        yield return new WaitForSeconds(4f);

        screenFader.fadeIn(0.5f);


        if (tutorial != null)
        {
            tutorial.enableTooltips();
        }
        yield return new WaitForSeconds(1f);

        StartCoroutine(startingPartZoomUp());

        //part zooms up
        yield return new WaitForSeconds(1f);
        ConversationTrigger.AddToken("doneRestarting");
    }

    //triggered by click of the tryAgainButton
    public void doResetConstruction()
    {
        if (dataManager)
        {
            string currentLevel;
            if (runningJustConstructionMode)
            {
                currentLevel = SceneManager.GetActiveScene().name;
            }
            else
            {
                currentLevel = LoadUtils.currentSceneName;
            }
            dataManager.AddNewAttempt(currentLevel,false);
        }
        StartCoroutine(resetConstruction());
    }

    public void disablePlayerControls()
    {
        bottomPanel.blocksRaycasts = false;
        cameraControls.controlsDisabled = true;
        rotationScript.controlsDisabled = true;
        selectPart.controlsDisabled = true;
    }

    private IEnumerator startingPartZoomUp()
    {
        float step = 0.01f; //move all the way up in one second
        while (!startingPart.transform.position.Equals(startingPartFinalPos))
        {
            startingPart.transform.position = Vector3.Lerp(startingPartOffscreenPos, startingPartFinalPos, step);
            step *= 1.2f;
            yield return new WaitForSeconds(0.01f);
        }

    }

    IEnumerator introTimerAndRotations()
    {
        // move to center of screen
        RectTransform rrRectTimer = timeRemainingPanel.gameObject.GetComponent<RectTransform>();
        RectTransform rrRectRotations = rotationsRemainingPanel.gameObject.GetComponent<RectTransform>();

        rrRectTimer.anchoredPosition = new Vector3(-400f, -175f, 0f);
        rrRectRotations.anchoredPosition = new Vector3(-300f, -175f, 0f);

        timeRemainingPanel.alpha = 1; //show TimeRemainingPanel in center of screen
        rotationsRemainingPanel.alpha = 1; //show RotationsRemainingPanel in center of screen

        Highlighter.Highlight(timeRemainingPanel.gameObject);
        Highlighter.Highlight(rotationsRemainingPanel.gameObject);
        yield return new WaitForSeconds(1f);
        Highlighter.Unhighlight(timeRemainingPanel.gameObject);
        Highlighter.Unhighlight(rotationsRemainingPanel.gameObject);

        // zoom panels to upper right
        Vector3 startPositionTimer = rrRectTimer.anchoredPosition;
        Vector3 startPositionRotations = rrRectRotations.anchoredPosition;

        Vector3 endPositionTimer = new Vector3(-100, 0, 0);
        Vector3 endPositionRotations = new Vector3(0, 0, 0);

        float lerpTime = 0.5f;
        float currentLerpTime = 0f;

        while (Vector3.Distance(rrRectTimer.anchoredPosition, endPositionTimer) > 2)
        {
            rrRectTimer.anchoredPosition = Vector3.Lerp(startPositionTimer, endPositionTimer, currentLerpTime / lerpTime);
            rrRectRotations.anchoredPosition = Vector3.Lerp(startPositionRotations, endPositionRotations, currentLerpTime / lerpTime);
            currentLerpTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        rrRectTimer.anchoredPosition = endPositionTimer;
        rrRectRotations.anchoredPosition = endPositionRotations;
    }

    private IEnumerator doCountdownAndEnableControls()
    {
        if (timeRemainingPanel != null)
        {
            yield return new WaitForSeconds(1f); // wait for part to finish zooming up

            StartCoroutine(introTimerAndRotations());

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

            timeRemainingPanel.GetComponent<Timer>().startTimer();
        }
        enablePlayerControls();
        if (dataManager != null)
        {
            dataManager.setPauseGameplay(false);
        }
        fuseEvent.startMusic();

    }

    private void enablePlayerControls()
    {
        bottomPanel.blocksRaycasts = true;
        cameraControls.controlsDisabled = false;
        rotationScript.controlsDisabled = false;
        selectPart.controlsDisabled = false;
    }

    // when player presses P key, call this method to end game
    public void doFadeToDemoFinished(float seconds)
    {
        if (dataManager != null)
        {
            dataManager.SetOutcome("quit");
        }


        disablePlayerControls();
        StopAllCoroutines();
        if(timeRemainingPanel != null)
        {
            Timer timer = timeRemainingPanel.gameObject.GetComponent<Timer>();
            timer.stopTimer();
            timer.stopMusic();
            countdownPanel.alpha = 0;
        }

        ConversationController.Disable();
        errorPanel.alpha = 0;
        confirmQuitPanel.alpha = 0;

        // only for non-tutorial levels (everything but newTutorial)
        if (rechargingText != null)
        {
            rechargingText.enabled = false;
            tryAgainButton.gameObject.SetActive(false);
        }

        // only for timed levels
        if (showMapText != null)
        {
            showMapText.enabled = false;
            readButton.gameObject.SetActive(false);
            locateButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
            map.gameObject.SetActive(false);
        }

        StartCoroutine(fadeToDemoFinished(seconds));
    }

    // this function is called when the player selects Yes, Quit on the confirmation dialogue
    public void gameFinished()
    {
        doFadeToDemoFinished(2f);
    }

    private IEnumerator fadeToDemoFinished(float seconds)
    {
        disablePlayerControls();
        screenFader.fadeOut(seconds);
        yield return new WaitForSeconds(seconds);

        gameQuit.Invoke(); // sends out broadcast that game is over; any other scripts can perform actions based on this
                           // might need to tell new tutorial level coroutines to stop too
                           // DataAggregator is listening for gameQuit event, triggers sendDataToDB method
        yield return new WaitForSeconds(2f);
        gameFinishedText.enabled = true;

    }



    // called when player clicks the NoDontQuitButton on the confirmation dialogue
    public void returnToGame()
    {
        confirmQuitPanel.alpha = 0;
        yesQuitButton.gameObject.SetActive(false);
        noDontQuitButton.gameObject.SetActive(false);
        if(dataManager != null)
        {
            dataManager.setPauseGameplay(false);
        }

    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyUp(KeyCode.P))
        {
            yesQuitButton.gameObject.SetActive(true);
            noDontQuitButton.gameObject.SetActive(true);
            confirmQuitPanel.alpha = 1;
            if(dataManager != null)
            {
                dataManager.setPauseGameplay(true);
            }

        }

        // Debugging purposes only. Uncomment if needed.
        //Debug.Log("startBeginningConvo is already here? " + ConversationTrigger.GetToken("startBeginningConvo"));


        // finished recharging after power failure, show Try Again? button to restart level
        if (ConversationTrigger.GetToken("outOfPower"))
        {
            ConversationTrigger.RemoveToken("outOfPower");
            showTryAgainButton();
        }
        // when Dresha has finished the restart message, reenable controls and start level again with countdown
        else if (ConversationTrigger.GetToken("doneRestarting"))
        { 
            ConversationTrigger.RemoveToken("doneRestarting");
            StartCoroutine(doCountdownAndEnableControls());
        } 
        // first time level is started: as soon as recharging animation and starting conversation has finished, start level with countdown
        else if (ConversationTrigger.GetToken("startBeginningConvo") && ConversationTrigger.GetToken("doneWithBeginningConvo"))
        {
            ConversationTrigger.RemoveToken("startBeginningConvo");
            ConversationTrigger.RemoveToken("doneWithBeginningConvo");
            StartCoroutine(doCountdownAndEnableControls());
        }
    }
}
