using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles everything in newTutorial scene.
/// Tutorial steps are hardcoded here.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    public static int step;
    public Text tutorialText;
    public GameObject part1;
    public GameObject p1s1;
    public GameObject part2;
    public GameObject p2s2;
    public GameObject rotationGizmo;
    public GameObject part2Button;
    public GameObject nextButton;
    public GameObject FuzeButton;
    public GameObject rotationRemain;
    public GameObject correct;
    public GameObject arrow;
    public GameObject zDownArrow;
    public GameObject finishedPic;
    public GameObject conversation;
    public GameObject cameraRig;
    public GameObject canvas;
    public GameObject partsGroup;
    // RectTransform of the arrow image. The changes are hardcoded. 
    private RectTransform arrowTransform;
    private RectTransform conversationTransform;
    private Camera cam;
    private float canvasWidth;
    private float canvasHeight;

    //data collection
    private ConstructionDataManager dataManager;

    void Start()
    {
        step = 1;
        p1s1.GetComponent<TutorialBlackSurfaceMove>().enabled = false;
        p2s2.GetComponent<TutorialSelect>().enabled = false;
        arrowTransform = arrow.GetComponent<RectTransform>();
        arrowTransform.localRotation = Quaternion.Euler(0, 0, -180);
        arrowTransform.anchoredPosition = new Vector2(-150f, 287f);
        conversationTransform = conversation.GetComponent<RectTransform>();
        conversationTransform.anchoredPosition = new Vector2(0, -25);
        cam = Camera.main;
        partsGroup.transform.position = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2f, cam.pixelHeight / 2f, 72));

        GameObject dataCollectionObj = GameObject.Find("DataCollectionManager");
        if (dataCollectionObj != null)
        {
            dataManager = dataCollectionObj.GetComponent<ConstructionDataManager>();
        }

    }

    void Update()
    {
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;

        // Ensure mouse works...
        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (step == 1)
        {
            tutorialText.text = "This is the target object.";
            Highlighter.Highlight(finishedPic);
            arrow.transform.SetParent(finishedPic.transform);
            arrowTransform.anchoredPosition = finishedPic.GetComponent<RectTransform>().anchoredPosition + new Vector2(280, 160);
            GameObject.Find("tutorialStart").transform.SetPositionAndRotation(new Vector3(-100, 36.8f, 100), new Quaternion());
        }
        else if (step == 2)
        {
            tutorialText.text = "This is the part you start with.";

            part1.SetActive(true);
            p1s1.SetActive(true);
            Highlighter.Highlight(part1);
            Highlighter.Highlight(p1s1);
            Highlighter.Unhighlight(finishedPic);
            arrow.transform.SetParent(canvas.transform);
            arrowTransform.anchoredPosition = new Vector2(canvasWidth / 20f, canvasHeight / 2f);
            conversationTransform.anchoredPosition = new Vector2(156, -81);
        }
        else if (step == 3)
        {
            tutorialText.text = "CLICK to select new parts from the bottom panel.";

            part2Button.SetActive(true);
            nextButton.SetActive(false);
            Highlighter.Unhighlight(part1);
            Highlighter.Unhighlight(p1s1);
            Highlighter.Highlight(part2Button);

            arrowTransform.localRotation = Quaternion.Euler(0, 0, -90);
            arrowTransform.anchoredPosition = new Vector2(-170, 136);
            conversationTransform.anchoredPosition = new Vector2(-28, -81);
        }
        else if (step == 4)
        {
            if(dataManager != null)
            {
                dataManager.AddPartSelected("part2");
            }
            part2.SetActive(true);
            p2s2.SetActive(true);
            rotationGizmo.SetActive(true);
            Highlighter.Highlight(zDownArrow);
            tutorialText.text = "CLICK on the arrow to rotate the part.";
            part2Button.GetComponent<Button>().interactable = false;
            arrowTransform.localRotation = Quaternion.Euler(0, 0, 0);
            //arrowTransform.anchoredPosition = new Vector2(-31, 78);
            arrowTransform.anchoredPosition = new Vector2(0, canvasHeight / 3.4f);
            conversationTransform.anchoredPosition = new Vector2(-28, -45);
        }
        else if (step == 5)
        {
            if(dataManager != null)
            {
                dataManager.AddRotation();
            }
            part2.transform.localRotation = Quaternion.Euler(0, 0, 180);
            p2s2.transform.localRotation = Quaternion.Euler(45, 90, -180);
            zDownArrow.GetComponent<TutorialArrowClick>().enabled = false;
            tutorialText.text = "This shows how many rotations you can make. GAME OVER when it reaches 0!";
            rotationRemain.SetActive(true);
            nextButton.SetActive(true);
            Highlighter.Unhighlight(zDownArrow);
            Highlighter.Highlight(rotationRemain);
            arrowTransform.localRotation = Quaternion.Euler(0, 0, 90);
            arrowTransform.anchoredPosition = new Vector2(393, canvasHeight - 150f);
            conversationTransform.anchoredPosition = new Vector2(121, -16);
        }
        else if (step == 6)
        {
            nextButton.SetActive(false);
            tutorialText.text = "CLICK on the black surface to select it.";
            p2s2.GetComponent<TutorialSelect>().enabled = true;
            Highlighter.Unhighlight(rotationRemain);
            arrowTransform.localRotation = Quaternion.Euler(0, 0, 45);
            //arrowTransform.anchoredPosition = new Vector2(113, 79);
            arrowTransform.anchoredPosition = new Vector2(canvasWidth / 5, canvasHeight / 5);
            conversationTransform.anchoredPosition = new Vector2(83, -37);
        }
        else if (step == 7)
        {
            tutorialText.text = "CLICK on another part's black surface to select it.";
            p1s1.GetComponent<TutorialBlackSurfaceMove>().enabled = true;
            arrowTransform.localRotation = Quaternion.Euler(0, 0, -135);
            //arrowTransform.anchoredPosition = new Vector2(-47, 217);
            arrowTransform.anchoredPosition = new Vector2(0, canvasHeight / 2);
            conversationTransform.anchoredPosition = new Vector2(83, -37);
        }
        else if (step == 8)
        {
            p2s2.transform.localPosition = new Vector3(1.26f, -0.67f, -1.03f);
            part2.transform.localPosition = new Vector3(1.26f, -0.67f, -1.03f);
            rotationGizmo.transform.localPosition = new Vector3(1.26f, -0.67f, -1.03f);
            FuzeButton.SetActive(true);
            Highlighter.Highlight(FuzeButton);
            tutorialText.text = "CLICK on the Fuse button to fuse two parts.";
            arrowTransform.localRotation = Quaternion.Euler(0, 0, -90);
            arrowTransform.anchoredPosition = new Vector2(-90, 126);
            conversationTransform.anchoredPosition = new Vector2(-44, -81);
            //Debug.Log(part2.transform.position);
        }
        else if (step == 9)
        {
            if(dataManager != null)
            {
                dataManager.AddPartFused("part2");
            }
            part2.SetActive(false);
            p2s2.SetActive(false);
            part1.SetActive(false);
            p1s1.SetActive(false);
            rotationGizmo.SetActive(false);
            correct.SetActive(true);
            Highlighter.Unhighlight(FuzeButton);
            tutorialText.text = "Congratulations! You have successfully fused two parts. Click NEXT to end the tutorial!";
            if(dataManager != null) {
                dataManager.SetOutcome("victory");
            }
            arrow.SetActive(false);
            FuzeButton.SetActive(false);
            nextButton.SetActive(true);
            conversationTransform.anchoredPosition = new Vector2(0, 0);
            cameraRig.GetComponent<CameraControls>().enabled = true;
        }
        else if (step == 10)
        {
            tutorialText.text = "If two parts don't match, you cannot fuse them. Rotate one part to make it match the other part.";
            cameraRig.GetComponent<CameraControls>().enabled = true;
        }
    }

}
