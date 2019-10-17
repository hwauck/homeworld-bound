using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Events;

public class Fuser : ItemBase {
    public GameObject fuser;
    private ExplorationDataManager expDataManager;

    static bool fuserActive = false;
    static GameObject fuserStatic;
    public FadeScreen screenFader;
    public Text lowPowerText;
    public AudioSource audioSource;
    private AudioClip bootingUpSound;
    private AudioClip lowPowerSound;
    private AudioClip powerUpSound;

    public Button putAwayButton;
    public Text putAwayText;

    public UnityEvent fuserLaunched;
    public UnityEvent fuserPutAway;

    //FPS controller variables
    public RigidbodyFirstPersonController controller;
    private float forwardSpeed;
    private float backwardSpeed;
    private float strafeSpeed;
    private float jumpForce;
    private float XRotSensitivity;
    private float YRotSensitivity;


    // Use this for initialization
    void Start () {
        fuserStatic = fuser;
        bootingUpSound = Resources.Load<AudioClip>("Audio/BothModes/DM-CGS-03");
        lowPowerSound = Resources.Load<AudioClip>("Audio/BothModes/LowBattery2");
        powerUpSound = Resources.Load<AudioClip>("Audio/BothModes/Slider3");
        GameObject expDataManagerObj = GameObject.Find("DataCollectionManager");
        if (expDataManagerObj != null)
        {
            expDataManager = expDataManagerObj.GetComponent<ExplorationDataManager>();
        }

        fuserStatic.SetActive(false);

    }

    // activate the fuser and then run the Fuser interface 
    // script for before any batteries have been collected
    public void ActivateFuserFirstLook()
    {
        // if the player already got the fuser the last time they loaded the game, don't make them watch the intro screen again
        if (!ConversationTrigger.GetToken("gear_fuser"))
        {
            fuserActive = true;
            fuserStatic.SetActive(true);

            ConversationTrigger.AddToken("gear_fuser");
            StartCoroutine(firstLookAtFuser());
            fuserLaunched.Invoke();
        }
    }

    // activate the fuser for all subsequent fuser accesses (each time all the parts
    // for an item/battery have been collected and Construction Mode will be launched)
    public void ActivateFuser()
    {
        fuserActive = true;
        fuserStatic.SetActive(true);
        fuserLaunched.Invoke();

    }

    public override void Deselect()
    {
        if (fuserActive)
        {
            GetRef();
            // Hide the fuser when deselected, and set a flag.
            fuserStatic.gameObject.SetActive(false);
            fuserPutAway.Invoke();

        }
    }
    public override void Select()
    {
        if (fuserActive)
        {
            GetRef();
            // Re-show the fuser when selected, and set a flag.
            fuserStatic.gameObject.SetActive(true);
            fuserLaunched.Invoke();


        }
    }

    // Sometimes, things don't happen in time. Specifically, this static reference conversion.
    void GetRef()
    {
        if (fuserStatic == null)
        {
            fuserStatic = fuser;
        }
    }

    // Restart when re-enabled. Fixes saving bugs.
    void OnEnable()
    {
        Start();
    }


    public void onQuitGame()
    {
        StopAllCoroutines();
        lowPowerText.enabled = false;
        putAwayButton.gameObject.SetActive(false);
    }

    // when the player first finds the Fuser on the ground
    IEnumerator firstLookAtFuser()
    {
        Quaternion startingRotation = fuser.transform.rotation;
        Quaternion endingRotation = startingRotation * Quaternion.Euler(0, 0, 90);
        float lerpTime = 1f;
        float currentLerpTime = 0f;

        //wait until player has closed the "You got the Fuser!" textbox
        while (!ConversationController.currentlyEnabled)
        {
            yield return new WaitForFixedUpdate();
        }
        while (ConversationController.currentlyEnabled)
        {

            yield return new WaitForFixedUpdate();
        }

        disablePlayerControl();
        if(expDataManager != null)
        {
            expDataManager.setPauseGameplay(true);
        }

        screenFader.fadeOut(0.2f);

        while (Quaternion.Angle(fuser.transform.rotation, endingRotation) > 2)
        {
            fuser.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, currentLerpTime/lerpTime);
            currentLerpTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        // set Fuser back to original position/rotation for next time it's used
        fuser.transform.rotation = startingRotation;

        //Booting up... animation
        lowPowerText.enabled = true;

        for (int i = 0; i < 3; i++)
        {
            lowPowerText.text = "Booting up.  ";
            yield return new WaitForSeconds(0.25f);
            lowPowerText.text = "Booting up.. ";
            yield return new WaitForSeconds(0.25f);
            lowPowerText.text = "Booting up...";
            audioSource.PlayOneShot(bootingUpSound);
            yield return new WaitForSeconds(0.5f);
            lowPowerText.text = "Booting up   ";
        }
        lowPowerText.enabled = false;
        yield return new WaitForSeconds(0.5f);

        //Warning: low power animation
        lowPowerText.enabled = true;
        lowPowerText.text = "Welcome to the Fuser X7000, the premier technology for constructing and crafting!";
        audioSource.PlayOneShot(powerUpSound);
        yield return new WaitForSeconds(4f);
        lowPowerText.text = "Warning: low power! Please replace batteries.";
        audioSource.PlayOneShot(lowPowerSound);
        yield return new WaitForSeconds(2f);

        //enable mouse cursor
        ConversationController.AllowMouse();
        putAwayButton.gameObject.SetActive(true);

        // then we continue when the player clicks the put away button

    }

    // called when player clicks the "Put Away Fuser" button
    public void putAwayFuserAndStartTask()
    {
        putAwayButton.gameObject.SetActive(false);
        lowPowerText.enabled = false;
        Deselect();

        //disable mouse cursor
        ConversationController.LockMouse();
        enablePlayerControl();
        if (expDataManager != null)
        {
            expDataManager.setPauseGameplay(false);
        }

        screenFader.fadeIn(0.2f);
        ConversationTrigger.AddToken("findBatteries");
    }

    private void disablePlayerControl()
    {
        // save original values of all player control variables in RigidBodyFirstPersonController
        forwardSpeed = controller.movementSettings.ForwardSpeed;
        backwardSpeed = controller.movementSettings.BackwardSpeed;
        strafeSpeed = controller.movementSettings.StrafeSpeed;
        jumpForce = controller.movementSettings.JumpForce;
        XRotSensitivity = controller.mouseLook.XSensitivity;
        YRotSensitivity = controller.mouseLook.YSensitivity;

        controller.movementSettings.ForwardSpeed = 0f;
        controller.movementSettings.BackwardSpeed = 0f;
        controller.movementSettings.StrafeSpeed = 0f;
        controller.movementSettings.JumpForce = 0f;

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

}
