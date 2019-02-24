using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    private bool cameraMoveTutorialTriggered = false;
    private Quaternion initialCamRotation;
    public GameObject camRig;
    public GameObject arrow;
    // Start is called before the first frame update
    void Start()
    {
        arrow.SetActive(false);
        initialCamRotation = camRig.transform.rotation;
        ConversationTrigger.AddToken("camera_zoom");
    }

    // Update is called once per frame
    void Update()
    {
        if (camRig.transform.rotation != initialCamRotation)
        {
            cameraMoveTutorialTriggered = true;
            arrow.SetActive(false);
        }
        if (!cameraMoveTutorialTriggered && camRig.transform.position.z >= -60f && camRig.transform.position.z <= 20f)
        {
            cameraMoveTutorialTriggered = true;
            ConversationTrigger.AddToken("camera_move");
            arrow.SetActive(true);
            //Debug.Log("Move Tutorial");
        }
    }
}
