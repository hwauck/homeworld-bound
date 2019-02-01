using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialArrowClick : MonoBehaviour, IPointerClickHandler
{
    private bool rotating = false;
    private bool hasRotated = false;
    public GameObject toRotate;
    public GameObject toRotateSurface;
    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("Rotate");
        StartCoroutine(Rotate(0, 0, 90));
    }

    public IEnumerator Rotate(float x, float y, float z)
    {
        // Can only start rotating if not already rotating. Prevents bugs with part movement.
        if (!rotating)
        {
            // Set rotate flag and start rotating.
            // Flag is reset every frame to ensure the check only runs at the end of all queued rotations.
            rotating = true;
            for (int i = 0; i < 30; i++)
            {
                toRotate.transform.Rotate(x / 30f, y / 30f, z / 30f, Space.World);
                toRotateSurface.transform.Rotate(x / 30f, y / 30f, z / 30f, Space.World);
                rotating = true;
                yield return null;
            }
            rotating = false;
            yield return null;  // Wait a frame to see if another active rotation resets this flag to true.
            if (!rotating)
            {
                StartCoroutine(CheckRotation());
            }
        }
    }

    IEnumerator CheckRotation()
    {
        yield return null;

        Vector3 rot = toRotate.transform.eulerAngles;

        // X Rounding
        if (rot.x != 0 && rot.x % 90 < 45)
        {
            rot.x -= (rot.x % 90);
        }
        else if (rot.x != 0 && rot.x % 90 >= 45)
        {
            rot.x += (90 - (rot.x % 90));
        }

        // Y Rounding
        if (rot.y != 0 && rot.y % 90 < 45)
        {
            rot.y -= (rot.y % 90);
        }
        else if (rot.y != 0 && rot.y % 90 >= 45)
        {
            rot.y += (90 - (rot.y % 90));
        }

        // Z Rounding
        if (rot.z != 0 && rot.z % 90 < 45)
        {
            rot.z -= (rot.z % 90);
        }
        else if (rot.z != 0 && rot.z % 90 >= 45)
        {
            rot.z += (90 - (rot.z % 90));
        }

        rot.x = Mathf.RoundToInt(rot.x);
        rot.y = Mathf.RoundToInt(rot.y);
        rot.z = Mathf.RoundToInt(rot.z);

        if (rot.x == 360)
            rot.x = 0;
        if (rot.y == 360)
            rot.y = 0;
        if (rot.z == 360)
            rot.z = 0;

        //Debug.Log(rot);

        toRotate.transform.eulerAngles = rot;
        rotating = false;
        hasRotated = true;
        Debug.Log("Check");
    }

    void Update()
    {
        if (hasRotated)
        {
            TutorialManager.step++;
            TutorialManager.triggerStep = true;
        }
    }
}
