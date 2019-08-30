using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationTracker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    //whenever a part is rotated, this method is called from RotationGizmo to update the vector directions
    //of all black faces on the part so they'll be display SelectedEffects correctly when selected
    // if Yleft,  forward -> left, left -> back, back -> right, right -> foward
    // up and down stay the same
    // if Yright, forward -> right, right -> back, back -> left, left -> forward
    // up and down stay the same
    // if Xup, forward -> up, up -> back, back -> down, down -> forward
    // left and right stay the same
    // if Xdown, forward -> down, down -> back, back ->up, up -> forward
    // left and right stay the same
    // if Zleft, up -> left, left -> down, down -> right, right -> up
    // forward and back stay the same
    // if Zright, up -> right, right -> down, down -> left, left -> up
    // forward and back stay the same
    // Would be nice to find an easier way of doing this
    public void adjustFaceDirections(string dir)
    {
        FaceSelector[] childFaces = GetComponentsInChildren<FaceSelector>();
        for(int i = 0; i < childFaces.Length; i++)
        {
            if(dir.Equals("XUp"))
            {
                if(childFaces[i].selectedNormal == Vector3.forward)
                {
                    childFaces[i].selectedNormal = Vector3.up;
                }
                else if (childFaces[i].selectedNormal == Vector3.up)
                {
                    childFaces[i].selectedNormal = Vector3.back;
                }
                else if (childFaces[i].selectedNormal == Vector3.back)
                {
                    childFaces[i].selectedNormal = Vector3.down;
                }
                else if (childFaces[i].selectedNormal == Vector3.down)
                {
                    childFaces[i].selectedNormal = Vector3.forward;
                }
            }
            else if (dir.Equals("XDown"))
            {
                if (childFaces[i].selectedNormal == Vector3.forward)
                {
                    childFaces[i].selectedNormal = Vector3.down;
                }
                else if (childFaces[i].selectedNormal == Vector3.down)
                {
                    childFaces[i].selectedNormal = Vector3.back;
                }
                else if (childFaces[i].selectedNormal == Vector3.back)
                {
                    childFaces[i].selectedNormal = Vector3.up;
                }
                else if (childFaces[i].selectedNormal == Vector3.up)
                {
                    childFaces[i].selectedNormal = Vector3.forward;
                }
            }
            else if(dir.Equals("YLeft"))
            {
                if (childFaces[i].selectedNormal == Vector3.forward)
                {
                    childFaces[i].selectedNormal = Vector3.left;
                }
                else if (childFaces[i].selectedNormal == Vector3.left)
                {
                    childFaces[i].selectedNormal = Vector3.back;
                }
                else if (childFaces[i].selectedNormal == Vector3.back)
                {
                    childFaces[i].selectedNormal = Vector3.right;
                }
                else if (childFaces[i].selectedNormal == Vector3.right)
                {
                    childFaces[i].selectedNormal = Vector3.forward;
                }
            }
            else if(dir.Equals("YRight"))
            {
                if (childFaces[i].selectedNormal == Vector3.forward)
                {
                    childFaces[i].selectedNormal = Vector3.right;
                }
                else if (childFaces[i].selectedNormal == Vector3.right)
                {
                    childFaces[i].selectedNormal = Vector3.back;
                }
                else if (childFaces[i].selectedNormal == Vector3.back)
                {
                    childFaces[i].selectedNormal = Vector3.left;
                }
                else if (childFaces[i].selectedNormal == Vector3.left)
                {
                    childFaces[i].selectedNormal = Vector3.forward;
                }
            }
            else if(dir.Equals("ZLeft"))
            {
                if (childFaces[i].selectedNormal == Vector3.up)
                {
                    childFaces[i].selectedNormal = Vector3.left;
                }
                else if (childFaces[i].selectedNormal == Vector3.left)
                {
                    childFaces[i].selectedNormal = Vector3.down;
                }
                else if (childFaces[i].selectedNormal == Vector3.down)
                {
                    childFaces[i].selectedNormal = Vector3.right;
                }
                else if (childFaces[i].selectedNormal == Vector3.right)
                {
                    childFaces[i].selectedNormal = Vector3.up;
                }
            }
            else if(dir.Equals("ZRight"))
            {
                if (childFaces[i].selectedNormal == Vector3.up)
                {
                    childFaces[i].selectedNormal = Vector3.right;
                }
                else if (childFaces[i].selectedNormal == Vector3.right)
                {
                    childFaces[i].selectedNormal = Vector3.down;
                }
                else if (childFaces[i].selectedNormal == Vector3.down)
                {
                    childFaces[i].selectedNormal = Vector3.left;
                }
                else if (childFaces[i].selectedNormal == Vector3.left)
                {
                    childFaces[i].selectedNormal = Vector3.up;
                }
            }
        }
    }

  
}
