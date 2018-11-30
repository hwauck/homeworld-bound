using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ExplorationDataManager : MonoBehaviour {

    public float playtime;
    public float standTime;
    public float jumpTime;
    public float runningTime;
    public float walkingTime;

    public RigidbodyFirstPersonController playerController;

	void Start () {
        playtime = 0;
        standTime = 0;
        jumpTime = 0;
        runningTime = 0;
        walkingTime = 0;
	}
	
	void Update () {
        playtime += Time.deltaTime;
        if (playerController.Velocity.Equals(new Vector3(0, 0, 0)))
            standTime += Time.deltaTime;
        else if (playerController.Jumping)
            jumpTime += Time.deltaTime;
        else if (playerController.Running)
            runningTime += Time.deltaTime;
        else
            walkingTime += Time.deltaTime;
	}

    public string GetDataString()
    {
        string retval = "Playtime:" + playtime.ToString() + ",";
        retval += "StandingTime:" + standTime.ToString() + ",";
        retval += "JumpTime:" + jumpTime.ToString() + ",";
        retval += "RunningTime:" + runningTime.ToString() + ",";
        retval += "WalkingTime:" + walkingTime.ToString() + "\n";
        return retval;
    }
}
