using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class RotationCounter : MonoBehaviour {

    public int numRotations;
    private int numRemaining;
    public Text remainingRotationsText;
    public UnityEvent powerFailure;
    private AudioClip decrementRotation;
    public AudioSource audioSource;
    public ScrollingText conversationText;
    public Tutorial1 tutorial;
    public ConstructionDataManager dataManager;

    // Use this for initialization
    void Start () {
        numRemaining = numRotations;
        remainingRotationsText.text = "" + numRemaining;
        decrementRotation = Resources.Load<AudioClip>("Audio/BothModes/Select02");
        if (!dataManager)
        {
            if (GameObject.Find("DataCollectionManager"))
                dataManager = GameObject.Find("DataCollectionManager").GetComponent<ConstructionDataManager>();
        }
            
    }

    // called by RotationGizmo every time a rotation is performed
    public void decrementRotations()
    {
        numRemaining--;
        if (numRemaining <= 0)
        {
            // For data collection
            if (dataManager)
            {
                dataManager.SetOutcome("rotation");
            }
            powerFailure.Invoke();
        }
        else
        {
            audioSource.PlayOneShot(decrementRotation);
        }

        remainingRotationsText.text = "" + numRemaining;

    }

    public int getRotationsRemaining()
    {
        return numRemaining;
    }

    public bool rotationsRemaining()
    {
        if (numRemaining <= 0)
        {

            return false;
        }
        return true;
    }

    public void resetRotations()
    {
        numRemaining = numRotations;
        remainingRotationsText.text = "" + numRemaining;
    }

    IEnumerator introRotationsRemaining()
    {

        // move Rotations Remaining Panel to center of screen
        RectTransform rrRect = GetComponent<RectTransform>();
        rrRect.anchoredPosition = new Vector3(-350, -175, 0);

        // show Rotations Remaining Panel
        GetComponent<CanvasGroup>().alpha = 1;

        if (tutorial != null)
        {
            // temporarily disable tooltips so player sees rotations remaining explanation
            tutorial.disableTooltips();
            conversationText.enableScroll = true;
        }

        // show explanation of rotations remaining
        ConversationTrigger.AddToken("intro_rr");
        if (tutorial != null)
        {
            StartCoroutine(waitThenReenableTooltips());
        }
        Highlighter.Highlight(this.gameObject);
        yield return new WaitForSeconds(4f);
        Highlighter.Unhighlight(this.gameObject);

        // zoom Rotations Remaining Panel to upper right
        Vector3 startPosition = rrRect.anchoredPosition;
        Vector3 endPosition = new Vector3(0, 0, 0);
        float lerpTime = 0.5f;
        float currentLerpTime = 0f;

        while (Vector3.Distance(rrRect.anchoredPosition, endPosition) > 2)
        {
            //Debug.Log(rrRect.anchoredPosition);
            rrRect.anchoredPosition = Vector3.Lerp(startPosition, endPosition, currentLerpTime / lerpTime);
            currentLerpTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        rrRect.anchoredPosition = endPosition;
    }

    IEnumerator waitThenReenableTooltips()
    {
        while (!ConversationController.currentlyEnabled)
        {
            yield return new WaitForFixedUpdate();
        }
        while (ConversationController.currentlyEnabled)
        {

            yield return new WaitForFixedUpdate();
        }
        conversationText.enableScroll = false;
        Tooltip[] allTooltips = Object.FindObjectsOfType<Tooltip>();
        for (int i = 0; i < allTooltips.Length; i++)
        {
            allTooltips[i].enabled = true;
        }
    }

    public void doIntroRotationsRemaining()
    {
        StartCoroutine(introRotationsRemaining());
    }

}
