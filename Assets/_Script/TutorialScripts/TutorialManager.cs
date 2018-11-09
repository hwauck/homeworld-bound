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
    // RectTransform of the arrow image. The changes are hardcoded. 
    private RectTransform arrowTransform;
    void Start()
    {
        step = 1;
        p1s1.GetComponent<TutorialSelect>().enabled = false;
        p2s2.GetComponent<TutorialSelect>().enabled = false;
        arrowTransform = arrow.GetComponent<RectTransform>();
        arrowTransform.localRotation = Quaternion.Euler(0, 0, -180);
        arrowTransform.anchoredPosition = new Vector2(-150f, 287f);
    }

    void Update()
    {
        if (step == 1)
        {
            tutorialText.text = "This is the target object";
            
        }
        else if (step == 2)
        {
            tutorialText.text = "This is the part you have";
            part1.SetActive(true);
            p1s1.SetActive(true);
            arrowTransform.anchoredPosition = new Vector2(35, 190);
        }
        else if (step == 3)
        {
            tutorialText.text = "CLICK to select new parts from the bottom panel";
            part2Button.SetActive(true);
            nextButton.SetActive(false);
            arrowTransform.localRotation = Quaternion.Euler(0, 0, -90);
            arrowTransform.anchoredPosition = new Vector2(-177, 136);
        }
        else if (step == 4)
        {
            part2.SetActive(true);
            p2s2.SetActive(true);
            rotationGizmo.SetActive(true);
            tutorialText.text = "CLICK on arrow to rotate the object";
            part2Button.GetComponent<Button>().interactable = false;
            arrowTransform.localRotation = Quaternion.Euler(0, 0, -180);
            arrowTransform.anchoredPosition = new Vector2(311, 197);
        }
        else if (step == 5)
        {
            part2.transform.localRotation = Quaternion.Euler(0, 0, 180);
            p2s2.transform.localRotation = Quaternion.Euler(45, 90, -180);
            GameObject.Find("ZUp").GetComponent<TutorialArrowClick>().enabled = false;
            tutorialText.text = "This shows how many rotations you can make. GAME OVER when it reaches 0!";
            rotationRemain.SetActive(true);
            nextButton.SetActive(true);
            arrowTransform.localRotation = Quaternion.Euler(0, 0, 90);
            arrowTransform.anchoredPosition = new Vector2(393, 250);
        }
        else if (step == 6)
        {
            nextButton.SetActive(false);
            tutorialText.text = "CLICK on the black surface to select it";
            p2s2.GetComponent<TutorialSelect>().enabled = true;
            arrowTransform.localRotation = Quaternion.Euler(0, 0, 45);
            arrowTransform.anchoredPosition = new Vector2(113, 79);
        }
        else if (step == 7)
        {
            tutorialText.text = "CLICK on another part's black surface to select it";
            p1s1.GetComponent<TutorialSelect>().enabled = true;
            arrowTransform.localRotation = Quaternion.Euler(0, 0, -135);
            arrowTransform.anchoredPosition = new Vector2(-47, 217);
        }
        else if (step == 8)
        {
            p2s2.transform.localPosition = new Vector3(1.26f, -0.67f, -1.03f);
            part2.transform.localPosition = new Vector3(1.26f, -0.67f, -1.03f);
            rotationGizmo.transform.localPosition = new Vector3(1.26f, -0.67f, -1.03f);
            FuzeButton.SetActive(true);
            tutorialText.text = "CLICK on Fuze button to fuze two parts";
            arrowTransform.localRotation = Quaternion.Euler(0, 0, -90);
            arrowTransform.anchoredPosition = new Vector2(-90, 126);
        }
        else if (step == 9)
        {
            part2.SetActive(false);
            p2s2.SetActive(false);
            part1.SetActive(false);
            p1s1.SetActive(false);
            rotationGizmo.SetActive(false);
            correct.SetActive(true);
            tutorialText.text = "Congratulations! You have successfully fuzed two parts";
            arrow.SetActive(false);
        }
        else if (step == 10)
        {
            tutorialText.text = "Note that if two parts don't match, you cannot fuze them. Rotate one part to make it match the other part.";
        }
    }

}
