using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ExplorationLevelResetter : MonoBehaviour {

    public Timer timer;
    private bool taggedFirstPart;

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

    //private float forwardSpeed;
    //private float backwardSpeed;
    private float XRotSensitivity;
    private float YRotSensitivity;

    private void Awake()
    {
        taggedFirstPart = false;
        powerFailureSound = Resources.Load<AudioClip>("Audio/BothModes/msfx_chrono_latency_hammer");
        countdownSound = Resources.Load<AudioClip>("Audio/BothModes/Select02");
        finalCountSound = Resources.Load<AudioClip>("Audio/BothModes/Select04");
        rechargingSound = Resources.Load<AudioClip>("Audio/BothModes/DM-CGS-03");

        XRotSensitivity = controller.mouseLook.XSensitivity;
        YRotSensitivity = controller.mouseLook.YSensitivity;
    }
    // Use this for initialization
    void Start () {
		
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

        //start fading to black
        waitThenContinueFailure(1f, 3f);

        //Recharging...
        StartCoroutine(rechargingAnimation(3f));

        //wait 6 seconds then Dresha tells you to start looking again and that timer will start once you've tagged first part
        waitThenAddToken(9f, "letsRestart");
    }

    private IEnumerator waitThenContinueFailure(float waitSeconds, float fadeSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        screenFader.fadeOut(fadeSeconds);
        powerFailureText.enabled = false;
        errorPanel.alpha = 0;
        ConversationTrigger.AddToken("outOfPower");
    }

    private IEnumerator waitThenAddToken(float seconds, string token)
    {
        yield return new WaitForSeconds(seconds);
        ConversationTrigger.AddToken(token);

    }

    private IEnumerator rechargingAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);

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
        screenFader.fadeIn(3f);
    }

    private void disablePlayerControl()
    {
        //does this work?
        controller.movementSettings.RunKey = KeyCode.None;

        controller.mouseLook.XSensitivity = 0;
        controller.mouseLook.YSensitivity = 0;
    }

    private void enablePlayerControl()
    {
        //does this work?
        controller.movementSettings.RunKey = KeyCode.LeftShift;

        controller.mouseLook.XSensitivity = XRotSensitivity;
        controller.mouseLook.YSensitivity = YRotSensitivity;
    }

    public void startCountdown()
    {
        disablePlayerControl();
        taggedFirstPart = false;
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

    // Update is called once per frame
    void Update () {
        // finished recharging after power failure
        if (ConversationTrigger.GetToken("outOfPower") && ConversationTrigger.GetToken("hasPower"))
        {
            ConversationTrigger.RemoveToken("outOfPower");
            ConversationTrigger.RemoveToken("hasPower");
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
