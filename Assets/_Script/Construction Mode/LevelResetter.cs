using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


// This script is used to keep track of when the player has run out of battery power.
// Once this happens, a warning saying that the system is shutting down due to low power should appear
// at the same time, player controls become disabled
// Then, all parts on the screen should have gravity applied to them and fall downwards until offscreen
// delete all created parts except starting part
// Then, screen sputters and flickers, goes black
// message from "SYSTEM" says "Recharging emergency power..." for a couple seconds
// Button appears: Restart Construction
// then, level is reset: starting part rigidbody removed, correctly rotated
// interface reappears, Dresha says stuff (Const_restart)
// starting part zooms up into place again
// Countdown begins again
public class LevelResetter : MonoBehaviour {

    public CanvasGroup errorPanel;
    public Text powerFailureText;
    public AudioSource audioSource;
    private AudioClip powerFailureSound;

    public CanvasGroup bottomPanel;
    public CameraControls cameraControls;
    public RotationGizmo rotationScript;
    public SelectPart selectPart;
    public FuseEvent fuseEvent;
    public Image fadeOutScreen;
    public Text rechargingText;
    public Button tryAgainButton;

    public CanvasGroup countdownPanel;
    public Text countdownText;
    public CanvasGroup timeRemainingPanel;
    public CanvasGroup rotationsRemainingPanel;

    private AudioClip countdownSound;
    private AudioClip finalCountSound;
    private AudioClip rechargingSound;

    private float flickeringTime;
    private float flickerLength;
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

    private void Awake()
    {
        powerFailureSound = Resources.Load<AudioClip>("Audio/BothModes/msfx_chrono_latency_hammer");
        countdownSound = Resources.Load<AudioClip>("Audio/BothModes/Select02");
        finalCountSound = Resources.Load<AudioClip>("Audio/BothModes/Select04");
        rechargingSound = Resources.Load<AudioClip>("Audio/BothModes/DM-CGS-03");
        originalColliderCenter = startingPart.GetComponent<BoxCollider>().center;
        originalColliderSize = startingPart.GetComponent<BoxCollider>().size;
        startingPartRotation = startingPart.transform.rotation;

        // make sure player controls are always disabled at beginning before countdown begins
        disablePlayerControls();

        if(InventoryController.levelName == "")
        {
            runningJustConstructionMode = true;
        }

    }

    // Use this for initialization
    void Start () {

    }

    void OnEnable()
    {
        //display Recharging screen while level loads
        //note: fadeOutPanel Image needs to be enabled in Inspector for it to look right
        if(fadeOutScreen.enabled) // b2 shouldn't recharge on startup, so don't do animation or sounds
        {
            Debug.Log("OnEnable() method in LevelResetter - starting recharging animation!");
            StartCoroutine(rechargingAnimation());
            StartCoroutine(waitAndThenZoomUpPart(4f));
            StartCoroutine(waitAndThenAddToken(4, "startBeginningConvo"));
        } else
        {
            StartCoroutine(waitAndThenZoomUpPart(1f));
            StartCoroutine(waitAndThenAddToken(1, "startBeginningConvo"));
        }

    }

    private IEnumerator waitAndThenAddToken(float seconds, string token)
    {
        //this, combined with the doneRestarting token from the opening conversation, will start the
        //level countdown after the opening conversation is complete
        // if there is no opening conversation, simply add the line
        // ConversationTrigger.AddToken("doneRestarting")
        yield return new WaitForSeconds(seconds);
        ConversationTrigger.AddToken(token);
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
        fadeOutScreen.enabled = false;
    }

    public void resetLevel()
    {
        fuseEvent.stopMusic();
        powerFailureText.enabled = true;
        errorPanel.alpha = 1;
        audioSource.PlayOneShot(powerFailureSound);
        disablePlayerControls();
        timeRemainingPanel.GetComponent<Timer>().stopTimer();
        StartCoroutine(doPowerFailure());

    }

    // makes the parts in the scene fall apart and downwards - only happens for item levels, not battery levels
    private IEnumerator doPowerFailure()
    {
        yield return new WaitForSeconds(1f);

        Vector3 rbPos;
        Vector3 explosionPosition;
        // all parts from level b2 and later should have the tag "part" on them for this to work correctly
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
            explosionPosition = new Vector3(rbPos.x, rbPos.y + 5f, rbPos.z);
            parts[i].GetComponent<Rigidbody>().AddExplosionForce(1000f, explosionPosition, 20f, 0f);

        }

        yield return new WaitForSeconds(1f);


        //flicker screen, then go to black
        flickeringTime = 0.5f;
        while (flickeringTime > 0)
        {
            flickerLength = Random.Range(0.05f, 0.15f);
            fadeOutScreen.enabled = true;
            yield return new WaitForSeconds(flickerLength);
            fadeOutScreen.enabled = false;
            flickeringTime -= flickerLength;
            yield return new WaitForSeconds(flickerLength);
        }
        fadeOutScreen.enabled = true;
        powerFailureText.enabled = false;
        errorPanel.alpha = 0;

        yield return new WaitForSeconds(1f);

        //Dresha gives you the failure message
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
        // destroy all parts except starting part
        // CHANGE this to add the new level string each time a new level is added
        fuseEvent.fuseCleanUp();

        string currentLevel;
        // if testing individual levels, use if(Application.isEditor). If testing levels in
        // sequence, use if(false)
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
        switch (currentLevel)
        {
            case "rocketBoots":
                eventSystem.GetComponent<CreatePartRB>().destroyAllCreatedParts();
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

        // and reset camera
        cameraControls.gameObject.transform.SetPositionAndRotation(new Vector3(-90, 45, -3.36f), Quaternion.Euler(0, 0, 0));

        // and reset the number of rotations, time remaining, and fuseCount
        rotationsRemainingPanel.GetComponent<RotationCounter>().resetRotations();
        timeRemainingPanel.GetComponent<Timer>().resetTimer();
        fuseEvent.resetFuseCount();
        yield return new WaitForSeconds(4f);

        //flicker screen back in
        flickeringTime = 0.5f;
        while (flickeringTime > 0)
        {
            flickerLength = Random.Range(0.01f, 0.1f);
            fadeOutScreen.enabled = false;
            yield return new WaitForSeconds(flickerLength);
            fadeOutScreen.enabled = true;
            flickeringTime -= flickerLength;
        }
        fadeOutScreen.enabled = false;

        yield return new WaitForSeconds(1f);

        StartCoroutine(startingPartZoomUp());

        //Dresha talks and part zooms up
        yield return new WaitForSeconds(1f);
        ConversationTrigger.AddToken("letsRestart");
    }

    //triggered by click of the tryAgainButton
    public void doResetConstruction()
    {
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
            Debug.Log("ZOOM " + step);
            yield return new WaitForSeconds(0.01f);
        }

    }

    private IEnumerator doCountdownAndEnableControls()
    {
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

        enablePlayerControls();

        fuseEvent.startMusic();
        timeRemainingPanel.GetComponent<Timer>().startTimer();
    }

    private void enablePlayerControls()
    {
        bottomPanel.blocksRaycasts = true;
        cameraControls.controlsDisabled = false;
        rotationScript.controlsDisabled = false;
        selectPart.controlsDisabled = false;
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("startBeginningConvo is already here? " + ConversationTrigger.GetToken("startBeginningConvo"));
        // finished recharging after power failure, show Try Again? button to restart level
        if (ConversationTrigger.GetToken("outOfPower") && ConversationTrigger.GetToken("hasPower"))
        {
            ConversationTrigger.RemoveToken("outOfPower");
            ConversationTrigger.RemoveToken("hasPower");
            showTryAgainButton();
        }
        // when Dresha has finished the restart message, reenable controls and start level again with countdown
        else if (ConversationTrigger.GetToken("doneRestarting") && ConversationTrigger.GetToken("letsRestart"))
        { 
            ConversationTrigger.RemoveToken("letsRestart");
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
