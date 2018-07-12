using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RotationCounter : MonoBehaviour {

    public int numRotations;
    private int numRemaining;
    public Text remainingRotationsText;
    public UnityEvent powerFailure;

	// Use this for initialization
	void Start () {
        numRemaining = numRotations;
        remainingRotationsText.text = "" + numRemaining;
	}

    // called by RotationGizmo every time a rotation is performed
    public void decrementRotations()
    {
        numRemaining--;
        if (numRemaining < 0)
        {
            powerFailure.Invoke();
        }
        else
        {
            remainingRotationsText.text = "" + numRemaining;
        }
    }

    public bool rotationsRemaining()
    {
        if (numRemaining <= 0)
        {

            return false;
        }
        return true;
    }

    public void resetRotations()
    {
        numRemaining = numRotations;
        remainingRotationsText.text = "" + numRemaining;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
