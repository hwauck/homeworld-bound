using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class RotationGizmo : MonoBehaviour
{
    public bool limitRotations;
    public bool controlsDisabled;
    public GameObject mainCamera;
	public GameObject toRotate;
	public SelectPart selectPart;
	public GameObject batteryIndicator;
    public Button fuseButton;

	public GameObject xGizmo;
	public GameObject yGizmo;
	public GameObject zGizmo;
	private int xRots = 0;
    private int yRots = 0;
    private int zRots = 0;

    bool rotating = false;

	void Start ()
	{
		//adjuster = EventSystem.current.gameObject.GetComponent<SelectPart>();
		//adjuster = GameObject.Find("EventSystem").GetComponent<SelectPart>();
		Disable();
		batteryIndicator = GameObject.Find("BatteryIndicator");
	}

	void OnEnable()
	{
		// Reset rotation count.
		xRots = 0;
		yRots = 0;
		zRots = 0;
	}

    public void incXRots()
    {
        //SimpleData.WriteDataPoint("Rotate_Object", toRotate.name, "", "", "", "X");
        xRots++;
    }

    public void incYRots()
    {
        //SimpleData.WriteDataPoint("Rotate_Object", toRotate.name, "", "", "", "Y");
        yRots++;
    }

    public void incZRots()
    {
       // SimpleData.WriteDataPoint("Rotate_Object", toRotate.name, "", "", "", "Z");
        zRots++;
    }


    void Update ()
	{
        // TODO: Get rid of RotateBehavior script and all its instances on part prefabs

        // Restarting game while in construction mode.
        // DEMO MODE ONLY.
        // TODO: disable this for the full game version
       // if (Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.R))
		//{
		//	InventoryController.RestartGame();
		//}


		// LookAts.
		Vector3 lookToward = mainCamera.transform.position;

		// X
		xGizmo.transform.LookAt(new Vector3(xGizmo.transform.position.x, lookToward.y, lookToward.z));
		Vector3 xTemp = xGizmo.transform.localEulerAngles;
		xTemp.z = 90;
		xGizmo.transform.localEulerAngles = xTemp;

		// Y
		yGizmo.transform.LookAt(new Vector3(lookToward.x, xGizmo.transform.position.y, lookToward.z));

		// Z
		zGizmo.transform.LookAt(new Vector3(lookToward.x, lookToward.y, xGizmo.transform.position.z));
		Vector3 zTemp = zGizmo.transform.localEulerAngles;
		zTemp.z = 90;
		zGizmo.transform.localEulerAngles = zTemp;

		// Object Itself
		if (toRotate != null)
			transform.position = toRotate.transform.position;

	}

    public IEnumerator Rotate(float x, float y, float z)
	{
        // disable fuse button during rotation to let rotation complete before attachment
        fuseButton.interactable = false;

        // Integration for battery power.
        if (!controlsDisabled && !rotating) {
			BatterySystem.SubPower(1);
		}

		// Can only start rotating if not already rotating. Prevents bugs with part movement.
		if (!rotating)
		{
			// Set rotate flag and start rotating.
			// Flag is reset every frame to ensure the check only runs at the end of all queued rotations.
			rotating = true;
			for (int i = 0; i < 30; i++)
			{
				toRotate.transform.Rotate(x / 30f, y / 30f, z / 30f, Space.World);
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

        // Adjustment of alignment. Do only if there is a selectedFuseTo and selectedObject
        if(selectPart.getSelectedObject() != null && selectPart.getSelectedFuseTo() != null)
        {
            //Debug.Log("toRotate object: " + toRotate);
            //selectPart.getSelectedObject().GetComponent<FaceSelector>().adjustPartAlignment();

            //rotation is done - renable fuse button
            fuseButton.interactable = true;
        }



    }

	//Warning - will probably break on X and Z rotations if used outside of tutorial
	public void runManualRotation(GameObject objectToRotate, float x, float y, float z) {
		toRotate = objectToRotate;
		StartCoroutine(Rotate(x, y, z));

        // update face normal for SelectedEffect ghosts
        if (y == 90f)
        {
            toRotate.GetComponent<OrientationTracker>().adjustFaceDirections("YRight");
        }
        else if (y == -90f)
        {
            toRotate.GetComponent<OrientationTracker>().adjustFaceDirections("Yleft");
        } else if (z == 90f)
        {
            toRotate.GetComponent<OrientationTracker>().adjustFaceDirections("ZLeft");
        } else if (z == -90f)
        {
            toRotate.GetComponent<OrientationTracker>().adjustFaceDirections("ZRight");

        } else if (x == 90f)
        {
            toRotate.GetComponent<OrientationTracker>().adjustFaceDirections("XDown");
        } else if (x == -90f)
        {
            toRotate.GetComponent<OrientationTracker>().adjustFaceDirections("XUp");

        } else
        {
            Debug.Log("ERROR: Invalid rotation direction for manual rotation: (" + x + ", " + y + ", " + z + ")");
        }


    }

    public void Disable()
	{
        Debug.Log("Disabled the rotation gizmo.");
        toRotate = null;
		transform.position = new Vector3(-1000f, -1000f, -1000f);
	}

	public GameObject Enable(GameObject objectToRotate)
	{
		Debug.Log("Enabled the rotation gizmo.");
		toRotate = objectToRotate;
		transform.position = toRotate.transform.position;
		return objectToRotate;
	}

    public GameObject getObjectToRotate()
    {
        return toRotate;
    }

    public bool isRotating()
    {
        return rotating;
    }

	// Checks battery, returns whether this rotation can happen. If it cannot, shows error.
    // This method will likely be deprecated now that battery power is controlled by rotation counter and timer
	bool CheckBattery()
	{
		// Check if we're in the standard game mode and have no power.
		if (BatterySystem.GetPower() == 0 && !controlsDisabled && !FuseEvent.runningJustConstructionMode)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	bool flashing = false;
	IEnumerator FlashBattery()
	{
		if (flashing)
		{
			yield break;
		}
		flashing = true;

		batteryIndicator.transform.localScale = 2f * batteryIndicator.transform.localScale;

		for (int ii = 0; ii < 8; ii++)
		{
			batteryIndicator.transform.Rotate(0f, 90f, 0f);

			// Wait a few frames.
			for (int jj = 0; jj < 8; jj++)
			{
				yield return null;
			}
		}

		batteryIndicator.transform.localScale = 0.5f * batteryIndicator.transform.localScale;
		flashing = false;
	}






}
