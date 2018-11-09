using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialClick : MonoBehaviour {
    
	public void NextStep()
    {
        TutorialManager.step++;
    }

}
