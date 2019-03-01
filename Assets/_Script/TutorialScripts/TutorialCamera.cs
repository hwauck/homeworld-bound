using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    private bool cameraMoveTutorialTriggered = false;
    private bool cameraZoomTutorialTriggered = false;
    private Quaternion initialCamRotation;
    private Vector3 initialCamPosition;
    public GameObject camRig;
    public GameObject[] arrows = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject arrow in arrows)
            arrow.SetActive(false);
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
            foreach (GameObject arrow in arrows)
                arrow.SetActive(false);
        }
        if (!cameraZoomTutorialTriggered && (camRig.transform.position.y < 190f))
        {
            cameraZoomTutorialTriggered = true;
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
