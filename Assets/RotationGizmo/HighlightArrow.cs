using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightArrow : MonoBehaviour, IPointerClickHandler
{

    public RotationGizmo rotationGizmo;
    public RotationCounter rotationCounter;

	// Use this for initialization
	void Start () {
		
	}

    private void OnMouseOver()
    {
        if(!rotationGizmo.controlsDisabled)
        {
            Highlighter.Highlight(this.gameObject);
        }
    }

    private void OnMouseExit()
    {
        if(!rotationGizmo.controlsDisabled)
        {
            Highlighter.Unhighlight(this.gameObject);

        }
    }

    // only executes if this gameobject was the first one hit by mouse's raycast
    // so it won't fire if UI element is clicked and object is behind it, yay
    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log(this.gameObject + "clicked!");
        if (!rotationGizmo.controlsDisabled)
        {
            switch (this.gameObject.name)
            {
                case "XUp2":
                    rotationGizmo.incXRots();
                    if (rotationGizmo.limitRotations)
                    {
                        rotationCounter.decrementRotations();
                    }

                    // if (Mathf.Approximately(xGizmo.transform.localEulerAngles.y, 180f))
                    //{
                    //     StartCoroutine(Rotate(90f, 0f, 0f));
                    //    toRotate.GetComponent<OrientationTracker>().adjustFaceDirections("XDown");
                    // }
                    // else
                    //  {
                    StartCoroutine(rotationGizmo.Rotate(90f, 0f, 0f));
                    rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("XUp");
                    // }
                    break;

                case "XDown2":
                    //if (!CheckBattery())
                    //    break;
                    rotationGizmo.incXRots();
                    if (rotationGizmo.limitRotations)
                    {
                        rotationCounter.decrementRotations();
                    }

                    // if (Mathf.Approximately(xGizmo.transform.localEulerAngles.y, 180f))
                    // {
                    //    StartCoroutine(rotationGizmo.Rotate(-90f, 0f, 0f));
                    //    rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("XUp");
                    // }
                    //else
                    // {
                    StartCoroutine(rotationGizmo.Rotate(-90f, 0f, 0f));
                    rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("XDown");
                    // }
                    break;

                case "YLeft2":
                    //   if (!CheckBattery())
                    //      break;
                    rotationGizmo.incYRots();
                    if (rotationGizmo.limitRotations)
                    {
                        rotationCounter.decrementRotations();
                    }

                    StartCoroutine(rotationGizmo.Rotate(0f, 90f, 0f));
                    rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("YRight");
                    break;

                case "YRight2":
                    //   if (!CheckBattery())
                    //        break;
                    rotationGizmo.incYRots();
                    if (rotationGizmo.limitRotations)
                    {
                        rotationCounter.decrementRotations();
                    }

                    StartCoroutine(rotationGizmo.Rotate(0f, -90f, 0f));
                    rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("YLeft");
                    break;

                case "ZUp2":
                    //   if (!CheckBattery())
                    //       break;
                    rotationGizmo.incZRots();
                    if (rotationGizmo.limitRotations)
                    {
                        rotationCounter.decrementRotations();
                    }

                    if (Mathf.Approximately(this.transform.parent.transform.localEulerAngles.y, 270f))
                    {
                         StartCoroutine(rotationGizmo.Rotate(0f, 0f, 90f));
                        rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("ZLeft");
                     }
                     else
                     {
                        StartCoroutine(rotationGizmo.Rotate(0f, 0f, -90f));
                        rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("ZRight");
                    }
                    break;

                case "ZDown2":
                    //   if (!CheckBattery())
                    //       break;
                    rotationGizmo.incZRots();
                    if (rotationGizmo.limitRotations)
                    {
                        rotationCounter.decrementRotations();
                    }
                    SimpleData.WriteDataPoint("Rotate_Object", rotationGizmo.getObjectToRotate().name, "", "", "", "Z");
                      if (Mathf.Approximately(this.transform.parent.transform.localEulerAngles.y, 270f))
                      {
                         StartCoroutine(rotationGizmo.Rotate(0f, 0f, -90f));
                         rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("ZRight");
                     }
                     else
                     {
                        StartCoroutine(rotationGizmo.Rotate(0f, 0f, 90f));
                        rotationGizmo.getObjectToRotate().GetComponent<OrientationTracker>().adjustFaceDirections("ZLeft");
                     }
                    break;

                default:
                    break;
            }
        }
    }

        // Update is called once per frame
        void Update () {
		
	}
}
