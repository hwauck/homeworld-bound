using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;


public class ExplorationLevelResetter : MonoBehaviour {

    public Timer timer;
    private bool taggedFirstPart;
    private bool resetDueToPowerFailure;

    public FadeScreen screenFader;
    public CanvasGroup errorPanel;
    public AudioSource audioSource;

    public Text powerFailureText;
    public CanvasGroup countdownPanel;
    public Text countdownText;
    public Text rechargingText;

    private AudioClip powerFailureSound;
    private AudioClip countdownSound;
    private AudioClip finalCountSound;
    private AudioClip rechargingSound;

    public RigidbodyFirstPersonController controller;
    public PartCounter itemPartCounter;
    public BatteryCounter batteryPartCounter;

    private float forwardSpeed;
    private float backwardSpeed;
    private float strafeSpeed;
    private float jumpForce;
    private float XRotSensitivity;
    private float YRotSensitivity;

    private string whatToBuild;

    private void Awake()
    {
        taggedFirstPart = false;
        resetDueToPowerFailure = false;
        powerFailureSound = Resources.Load<AudioClip>("Audio/BothModes/msfx_chrono_latency_hammer");
        countdownSound = Resources.Load<AudioClip>("Audio/BothModes/Select02");
        finalCountSound = Resources.Load<AudioClip>("Audio/BothModes/Select04");
        rechargingSound = Resources.Load<AudioClip>("Audio/BothModes/DM-CGS-03");

        // save original values of all player control variables in RigidBodyFirstPersonController
        forwardSpeed = controller.movementSettings.ForwardSpeed;
        backwardSpeed = controller.movementSettings.BackwardSpeed;
        strafeSpeed = controller.movementSettings.StrafeSpeed;
        jumpForce = controller.movementSettings.JumpForce;
        XRotSensitivity = controller.mouseLook.XSensitivity;
        YRotSensitivity = controller.mouseLook.YSensitivity;
    }
    // Use this for initialization
    void Start () {
		
	}

    public void setWhatToBuild(string whatToBuild, string objectToBuild)
    {
        this.whatToBuild = whatToBuild;
        itemPartCounter.setObjectToBuild(objectToBuild);

        // CHANGE for each new Item construction level added
        if (whatToBuild.Equals("b1"))
        {
            itemPartCounter.setPartsNeeded(7);
            batteryPartCounter.setPartsNeeded(14);
        }
        else if (whatToBuild.Equals("b5"))
        {
            itemPartCounter.setPartsNeeded(12);
            //batteryCounter.setPartsNeeded()
        }
    }

    //invoke this method from PartCounter/BatteryCounter whenever all parts are successfully tagged within time limit
    public void prepareNextLevel()
    {
        if (itemPartCounter.allPartsCollected() && batteryPartCounter.allPartsCollected())
        {
            screenFader.fadeOut(3f);
            itemPartCounter.resetCounter();
            batteryPartCounter.resetCounter();
            timer.stopTimer();
            timer.resetTimer();
            InventoryController.levelName = SceneManager.GetActiveScene().name;
            StartCoroutine(waitThenLoadLevel(3f, whatToBuild));
        }
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
        powerFailureText.enabled = false;
        errorPanel.alpha = 0;
        yield return new WaitForSeconds(fadeSeconds);
        itemPartCounter.resetCounter();
        batteryPartCounter.resetCounter();
        taggedFirstPart = false;

        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");
        for(int i = 0; i < pickups.Length; i++)
        {
            pickups[i].GetComponent<Collider>().enabled = true;
            ParticleSystem ps = pickups[i].GetComponent<ParticleSystem>();
            ParticleSystem.MainModule psMain = pickups[i].GetComponent<ParticleSystem>().main;

            //turn the particle effect back to original white/magenta
            if(pickups[i].GetComponent<PickUp>().type == PickUp.PickupType.Battery)
            {
                psMain.startColor = new Color(255f, 0f, 255f, 255f);

            } else
            {
                psMain.startColor = new Color(255f, 255f, 255f, 255f);
            }
        }
        ConversationTrigger.AddToken("outOfPower");
    }

    private IEnumerator waitThenRestart(float seconds, string token)
    {
        yield return new WaitForSeconds(seconds);
        ConversationTrigger.AddToken(token);
        enablePlayerControl();
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
        disablePlayerControl();
        // firstPickup should always be an item part. So the first time the player tries the level, 
        // they'll get the Exp_firstRBPart message.
        // But if it's not their first time, they'll just get the startTimer message

        if (timer.getNumRanOutOfTime() > 0)
        {
            ConversationTrigger.AddToken("firstPickup");
        }

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
        while(!ConversationTrigger.GetToken("readyToStartTimer"))
        {
            yield return new WaitForFixedUpdate();
        }
        ConversationTrigger.RemoveToken("firstPickup");
        ConversationTrigger.RemoveToken("readyToStartTimer");
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
        //Debug.Log(ConversationTrigger.GetToken("firstPickup"));
        // finished recharging after power failure
        if (ConversationTrigger.GetToken("outOfPower") && ConversationTrigger.GetToken("hasPower"))
        {
            ConversationTrigger.RemoveToken("outOfPower");
            ConversationTrigger.RemoveToken("hasPower");
            StartCoroutine(rechargingAndRestart());

        }
        // when Dresha has finished the restart message, reenable controls
        else if (ConversationTrigger.GetToken("doneRestarting") && ConversationTrigger.GetToken("letsRestart"))
        {
            ConversationTrigger.RemoveToken("letsRestart");
            ConversationTrigger.RemoveToken("doneRestarting");
            enablePlayerControl();
        }
        // first time level is started: may want this for as soon as the tutorial before first part is over
        else if (ConversationTrigger.GetToken("startBeginningConvo") && ConversationTrigger.GetToken("doneWithBeginningConvo"))
        {
            ConversationTrigger.RemoveToken("startBeginningConvo");
            ConversationTrigger.RemoveToken("doneWithBeginningConvo");
            StartCoroutine(doCountdownAndEnableControls());
        }
    }
}
