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
    public Text timeRemaining;
    public Text rotationsRemaining;
    private bool enabledControls;
    private CameraControls cameraControls;
    public GameObject rotationGizmo;
    private RotationGizmo rotationScript;
    public Button fuseButton;
    public Button controlsButton;
    public AudioSource audioSource;
    public AudioClip lowPowerSound;
    public Text lowPowerText;

    private bool done;

	void Awake() {

	}

	// Use this for initialization
	void Start () {

        rotationScript = rotationGizmo.GetComponent<RotationGizmo>();
        selectPart = eventSystem.GetComponent<SelectPart>();
		fuseEvent = eventSystem.GetComponent<FuseEvent>();
		conversationSystem = GameObject.Find("ConversationSystem");
        enabledControls = false;

        if (disableTutorial)
        {
            enabledControls = true;
            bottomPanel.blocksRaycasts = true;
            cameraControls.tutorialMode = false;
            rotationScript.tutorialMode = false;
            selectPart.tutorialMode = false;
            ConversationTrigger.AddToken("finishedEnablingControls2");
        }
    }
	
	// Update is called once per frame
	void Update () {

        // low power warning flashes on screen
        if(!ConversationTrigger.GetToken("finishedConst_21"))
        {
            StartCoroutine(flashLowPowerAndAddToken());
        }
        // show timer at top
        else if(!ConversationTrigger.GetToken("finishedConst_22") && ConversationTrigger.GetToken("finishedConst_21"))
        {
            //set canvas group alpha to 1 for both time and rotations
            //set blocks raycasts = true from both time and rotations
            timeRemainingPanel.alpha = 1;
            rotationsRemainingPanel.alpha = 1;
            timeRemainingPanel.blocksRaycasts = true;
            rotationsRemainingPanel.blocksRaycasts = true;
        }
	}

    private IEnumerator flashLowPowerAndAddToken()
    {
        for (int i = 0; i < 3; i++)
        {
            audioSource.PlayOneShot(lowPowerSound);
            lowPowerText.enabled = true;
            yield return new WaitForSeconds(1f);
            lowPowerText.enabled = false;
            yield return new WaitForSeconds(1f);
        }
        ConversationTrigger.AddToken("finishedLowPowerWarning");
    }

    private void flashControlsAndAddToken(string token)
    {
        highlightGizmo(1f);
        for (int i = 0; i < partButtons.Length; i++)
        {
            highlighter.HighlightTimed(partButtons[i].gameObject, 1f);

        }
        highlighter.HighlightTimed(fuseButton.gameObject, 1f);
        highlighter.HighlightTimed(controlsButton.gameObject, 1f);
        ConversationTrigger.AddToken(token);
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
