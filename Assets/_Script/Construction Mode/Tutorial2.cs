using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour {
    public bool disableTutorial;

    public GameObject finishedImage;
	public Button[] partButtons;

	public Highlighter highlighter;

	public Text congrats;
	public Button goToRocketBootsLevel;
	public GameObject eventSystem;
	private SelectPart selectPart;
	private FuseEvent fuseEvent;
	private GameObject selectedObj;
	private GameObject conversationSystem;

    public CanvasGroup bottomPanel;
    public CanvasGroup timeRemainingPanel;
    public CanvasGroup rotationsRemainingPanel;
    public CanvasGroup errorPanel;
    public Text timeRemaining;
    public Text rotationsRemaining;
    public Image arrowToTimePanel;
    public Image arrowToRotationsPanel;

    private bool enabledControls;
    public CameraControls cameraControls;
    public GameObject rotationGizmo;
    private RotationGizmo rotationScript;
    public Button fuseButton;
    public Button controlsButton;

    public AudioSource audioSource;
    public AudioClip lowPowerSound;
    public AudioClip countdownSound;
    public AudioClip finalCountSound;
    public Text lowPowerText;

    public CanvasGroup countdownPanel;
    public Text countdownText;

    private const float SHOW_IMAGE_DURATION = 2f;

    private bool displayedTimerAndRotations;
    private bool flashedTimerAndRotations;
    private bool flashedLowPowerWarning;
    private bool startedCountdown;

    void Awake() {

	}

	// Use this for initialization
	void Start () {

        rotationScript = rotationGizmo.GetComponent<RotationGizmo>();
        selectPart = eventSystem.GetComponent<SelectPart>();
		fuseEvent = eventSystem.GetComponent<FuseEvent>();
		conversationSystem = GameObject.Find("ConversationSystem");

        enabledControls = false;
        displayedTimerAndRotations = false;
        flashedTimerAndRotations = false;
        flashedLowPowerWarning = false;

        if (disableTutorial)
        {
            enabledControls = true;
            bottomPanel.blocksRaycasts = true;
            cameraControls.tutorialMode = false;
            rotationScript.tutorialMode = false;
            selectPart.tutorialMode = false;
            fuseEvent.startMusic();
            timeRemainingPanel.GetComponent<Timer>().startTimer();
        }
    }
	
	// Update is called once per frame
	void Update () {

        // low power warning flashes on screen
        if(!flashedLowPowerWarning)
        {
            flashedLowPowerWarning = true;
            StartCoroutine(flashLowPowerAndAddToken());
        }
        // show timer and number of rotations remaining at top
        else if(!displayedTimerAndRotations && ConversationTrigger.GetToken("finishedConst_21"))
        {
            displayedTimerAndRotations = true;
            timeRemainingPanel.alpha = 1;
            rotationsRemainingPanel.alpha = 1;
            timeRemainingPanel.blocksRaycasts = true;
            rotationsRemainingPanel.blocksRaycasts = true;
        }
        // draw player's attention to the timer and rotation limit
        else if(!flashedTimerAndRotations && ConversationTrigger.GetToken("finishedConst_22"))
        {
            flashedTimerAndRotations = true;
            StartCoroutine(showImageAndAddToken(arrowToTimePanel, SHOW_IMAGE_DURATION, "finishedFlashingTimerAndRotations"));
            StartCoroutine(showImage(arrowToRotationsPanel, SHOW_IMAGE_DURATION));
            highlighter.HighlightTimed(timeRemainingPanel.gameObject, 2);
            highlighter.HighlightTimed(rotationsRemainingPanel.gameObject, 2);
        }
        // player just clicked Ready! answer, so start countdown
        else if (!startedCountdown && ConversationTrigger.GetToken("finishedConst_23"))
        {
            startedCountdown = true;
            // do countdown
            // as soon as countdown finishes, enable player controls with one flash
            StartCoroutine(doCountdownAndEnableControls());
        } 
        // if player runs out of time or rotations, start level over
        // should put checks for this in timer and rotation panels, replace BatteryIndicator with them
        // in RotationGizmo stuff
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
        enabledControls = true;
        bottomPanel.blocksRaycasts = true;
        cameraControls.tutorialMode = false;
        rotationScript.tutorialMode = false;
        selectPart.tutorialMode = false;
        flashControls();

        fuseEvent.startMusic();
        timeRemainingPanel.GetComponent<Timer>().startTimer();
    }

    private IEnumerator showImage(Image imgToFlash, float time)
    {
        imgToFlash.enabled = true;
        yield return new WaitForSeconds(time);
        imgToFlash.enabled = false;

    }

    IEnumerator showImageAndAddToken(Image imgToFlash, float time, string token)
    {
        imgToFlash.enabled = true;
        yield return new WaitForSeconds(time);
        imgToFlash.enabled = false;

        ConversationTrigger.AddToken(token);
    }

    private IEnumerator flashLowPowerAndAddToken()
    {
        for (int i = 0; i < 3; i++)
        {
            audioSource.PlayOneShot(lowPowerSound);
            errorPanel.alpha = 1;
            lowPowerText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            errorPanel.alpha = 0;
            lowPowerText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
        ConversationTrigger.AddToken("finishedLowPowerWarning");
    }

    private void flashControls()
    {
        highlightGizmo(0.5f);
        for (int i = 0; i < partButtons.Length; i++)
        {
            highlighter.HighlightTimed(partButtons[i].gameObject, 0.5f);

        }
        highlighter.HighlightTimed(fuseButton.gameObject, 0.5f);
        highlighter.HighlightTimed(controlsButton.gameObject, 0.5f);
    }

    private void highlightGizmo(float seconds)
    {
        // maybe should highlight only the sliders instead?
        foreach (Transform child in rotationGizmo.transform)
        {
            highlighter.HighlightTimed(child.gameObject, seconds);
        }
    }

    private void highlightPartButtons(float sec) {
		foreach (Button b in partButtons) {
			highlighter.HighlightTimed(b.gameObject, sec);
		}
	}
		
}
