using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCamera : MonoBehaviour
{
    private bool cameraMoveTutorialTriggered = false;
    private bool cameraZoomTutorialTriggered = false;
    public CameraControls cameraControls;
    private Quaternion initialCamRotation;
    private Vector3 initialCamPosition;
    public GameObject camRig;
    public GameObject[] arrows = new GameObject[4];

    public Button[] partButtons;
    public Button controlsButton;

    private bool buttonsEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject arrow in arrows)
            arrow.SetActive(false);
        foreach (Button partButton in partButtons)
        {
            partButton.interactable = false;
        }
        initialCamRotation = camRig.transform.rotation;
        initialCamPosition = camRig.transform.position;
        ConversationTrigger.AddToken("camera_zoom");
    }

    // Update is called once per frame
    void Update()
    {
        if (camRig.transform.rotation != initialCamRotation)
        {

            cameraMoveTutorialTriggered = true;


            if (!buttonsEnabled) {
                foreach (Button partButton in partButtons)
                {
                    partButton.interactable = true;
                }
                foreach (GameObject arrow in arrows)
                    arrow.SetActive(false);
                buttonsEnabled = true;

            }
        }

        if (!cameraZoomTutorialTriggered && (camRig.transform.position.y < 190f))
        {
            cameraZoomTutorialTriggered = true;
            cameraControls.movementDisabled = false;
        }
        if (!cameraMoveTutorialTriggered && cameraZoomTutorialTriggered)
        {
            cameraMoveTutorialTriggered = true;
            ConversationTrigger.AddToken("camera_move");
            Debug.Log("triggered");
            foreach (GameObject arrow in arrows)
                arrow.SetActive(true);
            //Debug.Log("Move Tutorial");
        }
    }
}
