using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RotationCounter : MonoBehaviour {

    public int remainingRotations;
    private int ROTATIONS;
    public Text remainingRotationsText;
    public UnityEvent powerFailure;

	// Use this for initialization
	void Start () {
        ROTATIONS = remainingRotations;
	}

    // called by RotationGizmo every time a rotation is performed
    public void decrementRotations()
    {
        remainingRotations--;
        remainingRotationsText.text = "" + remainingRotations;
        if (remainingRotations <= 0)
        {
            powerFailure.Invoke();
        }
    }

    public bool rotationsRemaining()
    {
        if (remainingRotations <= 0)
        {

            return false;
        }
        return true;
    }

    public void resetRotations()
    {
        remainingRotations = ROTATIONS;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
