using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    public AudioClip powerFailureSound;

    public CanvasGroup bottomPanel;
    public CameraControls cameraControls;
    public RotationGizmo rotationScript;
    public SelectPart selectPart;
    public Image fadeOutScreen;
    public Text rechargingText;

    public CanvasGroup countdownPanel;
    public Text countdownText;
    public CanvasGroup timeRemainingPanel;
    public CanvasGroup rotationsRemainingPanel;

    public AudioClip countdownSound;
    public AudioClip finalCountSound;
    public AudioClip rechargingSound;

    // need this to get the "mode" so I can get the correct CreatePart script, bleh
    public GameObject eventSystem;

    public GameObject startingPart;
    private Vector3 startingPartFinalPos = new Vector3(-100, 30, 100);
    private Vector3 startingPartOffscreenPos = new Vector3(-100, -40, 100);

    private const float MOVEMENT_SPEED = 100f;



    // Use this for initialization
    void Start () {
        ConversationTrigger.RemoveToken("outOfPower");
        ConversationTrigger.AddToken("hasPower");
    }

    public void resetLevel()
    {
        eventSystem.GetComponent<FuseEvent>().stopMusic();
        powerFailureText.enabled = true;
        errorPanel.alpha = 1;
        audioSource.PlayOneShot(powerFailureSound);
        disablePlayerControls();
        StartCoroutine(resetConstruction());

    }

    // makes the parts in the scene fall apart and downwards
    private IEnumerator resetConstruction()
    {
        yield return new WaitForSeconds(1f);

        Vector3 rbPos;
        Vector3 explosionPosition;
        // all parts from level b2 and later should have the tag "part" on them for this to work correctly
        GameObject[] parts = GameObject.FindGameObjectsWithTag("part");
        for(int i = 0; i < parts.Length; i++)
        {
            // first, set all meshcolliders to convex to avoid bad interaction with Rigidbody
            MeshCollider[] meshColliders = parts[i].GetComponentsInChildren<MeshCollider>();
            for(int j = 0; j < meshColliders.Length; j++)
            {
                meshColliders[j].convex = true;
            }
            // then, add Rigidbodies to apply a downward explosive force
            parts[i].AddComponent<Rigidbody>();
            parts[i].GetComponent<Rigidbody>().useGravity = false;
            rbPos = parts[i].transform.position;
            explosionPosition = new Vector3(rbPos.x, rbPos.y + 10f, rbPos.z);
            parts[i].GetComponent<Rigidbody>().AddExplosionForce(1000f, explosionPosition, 20f, 0f);

        }


        //flicker screen, then go to black
        float flickeringTime = 0.5f;
        float flickerLength;
        while (flickeringTime > 0)
        {
            flickerLength = Random.Range(0.01f, 0.1f);
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
        ConversationTrigger.RemoveToken("hasPower");

        rechargingText.enabled = true;

        //stop downward movement of parts
        for (int i = 0; i < parts.Length; i++)
        {
            Destroy(parts[i].GetComponent<Rigidbody>());
        }

        // destroy all parts except starting part
        // CHANGE this to add the new level string each time a new level is added
        string mode = eventSystem.GetComponent<FuseEvent>().mode;
        switch (mode)
        {
            case "b2":
                eventSystem.GetComponent<CreatePartB2>().clearPartsCreated();
                break;
            case "boot":
                eventSystem.GetComponent<CreatePart>().clearPartsCreated();
                break;
            default:
                break;
        }

        // simple ... progress animation for recharging text
        for(int i = 0; i < 3; i++)
        {
            audioSource.PlayOneShot(rechargingSound);
            rechargingText.text += ".";
            yield return new WaitForSeconds(0.25f);
            rechargingText.text += ".";
            yield return new WaitForSeconds(0.25f);
            rechargingText.text += ".";
            yield return new WaitForSeconds(0.5f);
            rechargingText.text = "Recharging emergency power";
        }
        rechargingText.enabled = false;
        yield return new WaitForSeconds(1f);

        // TODO: add a button to restart level to let the player catch their breath!

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

        // put starting part back to where it was
        // and reset camera
        startingPart.transform.position = startingPartOffscreenPos;
        cameraControls.gameObject.transform.SetPositionAndRotation(new Vector3(-90, 45, -3.36f), Quaternion.Euler(0, 0, 0));

        yield return new WaitForSeconds(1f);

        StartCoroutine(startingPartZoomUp());

        //Dresha talks and part zooms up
        yield return new WaitForSeconds(1f);
        ConversationTrigger.AddToken("letsRestart");

        // do countdown again and start timer, start level

    }

    public void disablePlayerControls()
    {
        bottomPanel.blocksRaycasts = false;
        cameraControls.tutorialMode = true;
        rotationScript.tutorialMode = true;
        selectPart.tutorialMode = true;
    }

    private IEnumerator startingPartZoomUp()
    {
        float step = 0.01f; //move 70 units up in 1 second
        while (!startingPart.transform.position.Equals(startingPartFinalPos))
        {
            startingPart.transform.position = Vector3.MoveTowards(startingPart.transform.position, startingPartOffscreenPos, step);
            step += 0.01f;
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

        eventSystem.GetComponent<FuseEvent>().startMusic();
        timeRemainingPanel.GetComponent<Timer>().startTimer();
        rotationsRemainingPanel.GetComponent<RotationCounter>().resetRotations();
    }

    private void enablePlayerControls()
    {
        bottomPanel.blocksRaycasts = true;
        cameraControls.tutorialMode = false;
        rotationScript.tutorialMode = false;
        selectPart.tutorialMode = false;
    }

    // Update is called once per frame
    void Update () {

        if (ConversationTrigger.GetToken("finishedConst_restart"))
        {
            ConversationTrigger.RemoveToken("letsRestart");
            StartCoroutine(doCountdownAndEnableControls());
        }
	}
}
