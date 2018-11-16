using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialClick : MonoBehaviour {
    
	public void NextStep()
    {
        // Goes to next scene if tutorial has ended
        if (TutorialManager.step == 9)
        {
            SceneManager.LoadScene("b1");
        }
        else 
            TutorialManager.step++;
    }

}
