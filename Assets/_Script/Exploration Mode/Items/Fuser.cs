using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fuser : ItemBase {
    public GameObject fuser;

    static bool fuserActive = false;
    static GameObject fuserStatic;
    public FadeScreen screenFader;
    public Text lowPowerText;
    public AudioSource audioSource;
    private AudioClip bootingUpSound;

    // Use this for initialization
    void Start () {
        fuserStatic = fuser;
        bootingUpSound = Resources.Load<AudioClip>("Audio/BothModes/DM-CGS-03");

        // Check token and activate if unlocked, but deselected.
        if (ConversationTrigger.GetToken("gear_fuser"))
        {
            ActivateFuser();
            //Deselect();
        }
        else
        {
            // Disable if not unlocked.
            fuserStatic.SetActive(false);
        }
    }

    public void ActivateFuser()
    {
        fuserActive = true;
        fuserStatic.SetActive(true);
        ConversationTrigger.AddToken("gear_fuser");
        StartCoroutine(lookAtFuser());
    }

    public override void Deselect()
    {
        if (fuserActive)
        {
            GetRef();
            // Hide the sledge when deselected, and set a flag.
            fuserStatic.gameObject.SetActive(false);

        }
    }
    public override void Select()
    {
        if (fuserActive)
        {
            GetRef();
            // Re-show the sledgewhen selected, and set a flag.
            fuserStatic.gameObject.SetActive(true);

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

    IEnumerator lookAtFuser()
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
            Debug.Log(ConversationController.currentlyEnabled);

            yield return new WaitForFixedUpdate();
        }

        screenFader.fadeOut(1f);

        while (Quaternion.Angle(fuser.transform.rotation, endingRotation) > 2)
        {
            fuser.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, currentLerpTime/lerpTime);
            currentLerpTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

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

    }

    // Update is called once per frame
    void Update () {
		
	}
}
