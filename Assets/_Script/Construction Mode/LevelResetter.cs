using UnityEngine;
using UnityEngine.UI;

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

    public GameObject startingPart;

	// Use this for initialization
	void Start () {
	}

    public void resetLevel()
    {
        powerFailureText.enabled = true;
        errorPanel.alpha = 1;
        audioSource.PlayOneShot(powerFailureSound);
        disablePlayerControls();

    }

    // makes the parts in the scene fall apart and downwards
    private void applyGravity()
    {
        // all parts from level b2 and later should have the tag "part" on them for this to work correctly
        GameObject[] parts = GameObject.FindGameObjectsWithTag("part");
        for(int i = 0; i < parts.Length; i++)
        {
            // first, set all meshcolliders to convex to avoid bad interaction with Rigidbody
            //MeshCollider[] meshColliders = parts[i].GetComponentsInChildren<MeshCollider>();
           // for(int j = 0; j < meshColliders.Length; j++)
           // {
            //    meshColliders[i].convex = true;
            //}
            // then, add Rigidbodies to apply a downward explosive force
            parts[i].AddComponent<Rigidbody>();
            parts[i].GetComponent<Rigidbody>().useGravity = false;

        }
        Vector3 rbPos = startingPart.transform.position;
        Vector3 explosionPosition = new Vector3(rbPos.x, rbPos.y + 10f, rbPos.z);
        startingPart.GetComponent<Rigidbody>().AddExplosionForce(1000f, explosionPosition, 20f, 0f);
    }

    public void disablePlayerControls()
    {
        bottomPanel.blocksRaycasts = false;
        cameraControls.tutorialMode = true;
        rotationScript.tutorialMode = true;
        selectPart.tutorialMode = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
