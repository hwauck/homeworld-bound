using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;


public class ExplorationDataManager : MonoBehaviour {

    private float total_time;
    private float total_standTime;
    private float total_jumpTime;
    private float total_runTime;
    private float total_walkTime;
    private int attemptIndex;
    private int numLevelsCompleted;
    private int numRanOutOfTime;

    public RigidbodyFirstPersonController playerController;

    private List<Attempt> attempts;

    public class Attempt
    {
        public int attemptNum;
        public string level;
        public float playTime;
        public float standTime;
        public float jumpTime;
        public float runTime;
        public float walkTime;
        public string partCollectionOrder;
        public string outcome;

        public Attempt(string levelName, int currentAttemptNum)
        {
            attemptNum = currentAttemptNum;
            level = levelName;
            playTime = 0;
            standTime = 0;
            jumpTime = 0;
            runTime = 0;
            walkTime = 0;
            partCollectionOrder = ":";
            outcome = "None"; // will be either "time" (ran out of time), "victory", "quit" (ended game), or "finishedDemo"
        }

        public string toString()
        {
            return "Attempt" + attemptNum + ";level:" + level + ";playTime:" + playTime + ";standTime:" + standTime + ";jumpTime:" + jumpTime + ";runTime:"+ runTime 
                + ";walkTime:" + walkTime + ";partCollOrder:" + partCollectionOrder + ";outcome:" + outcome + "\n";
        }

    }

    public void initializeDataVars()
    {
        total_time = 0;
        total_standTime = 0;
        total_jumpTime = 0;
        total_runTime = 0;
        total_walkTime = 0;
        attemptIndex = -1;
        numLevelsCompleted = 0;
        numRanOutOfTime = 0;
        attempts = new List<Attempt>();
    }


    void Update () {
        GetCurrAttempt().playTime += Time.deltaTime;
        if (playerController.Velocity.Equals(new Vector3(0, 0, 0)))
        {
            GetCurrAttempt().standTime += Time.deltaTime;
        }
        else if (playerController.Jumping)
        {
            GetCurrAttempt().jumpTime += Time.deltaTime;
        }
        else if (playerController.Running)
        {
            GetCurrAttempt().runTime += Time.deltaTime;
        }
        else
        {
            GetCurrAttempt().walkTime += Time.deltaTime;
        }
	}

    // needs to be called every time player runs of out of time (so only on timed levels). 
    // Is automatically called each time Canyon2 scene is entered via OnEnable()
    public void AddNewAttempt(string sceneName)
    {
        attemptIndex++;
        Attempt newAttempt = new Attempt(sceneName, attemptIndex);
        attempts.Add(newAttempt);
    }

    // called every time player collects a part in exploration mode
    public void AddPartCollected(string partName)
    {
        GetCurrAttempt().partCollectionOrder += partName + ":";
    }

    // set to "victory" when player collects all parts (within the time limit if there is one) - or if they complete the whole demo
    // set to "quit" if player ends game
    // set to "time" if player runs out of time on timed levels
    public void setOutcome(string outcome)
    {
        GetCurrAttempt().outcome = outcome;
    }

    public Attempt GetCurrAttempt()
    {
        return attempts[attemptIndex];
    }

    public string GetDataString()
    {
        string retval = "Playtime:" + total_time.ToString() + ",";
        retval += "StandingTime:" + total_standTime.ToString() + ",";
        retval += "JumpTime:" + total_jumpTime.ToString() + ",";
        retval += "RunningTime:" + total_runTime.ToString() + ",";
        retval += "WalkingTime:" + total_walkTime.ToString() + "\n";
        return retval;
    }

    // call this
    public string saveAllData()
    {
        // setting these to zero again so this can recalculate and save totals multiple times during a game
        total_time = 0;
        total_standTime = 0;
        total_jumpTime = 0;
        total_runTime = 0;
        total_walkTime = 0;
        numLevelsCompleted = 0;
        numRanOutOfTime = 0;

        // calculate totals across levels/attempts
        for (int i = 0; i < attempts.Count; i++)
        {
            total_time += attempts[i].playTime;
            total_standTime += attempts[i].standTime;
            total_jumpTime += attempts[i].jumpTime;
            total_runTime += attempts[i].runTime;
            total_walkTime += attempts[i].walkTime;

            string thisAttemptPartOrder = attempts[i].partCollectionOrder;
            attempts[i].partCollectionOrder = thisAttemptPartOrder.Substring(0,thisAttemptPartOrder.Length-1); // get rid of extra colon at end

            if (attempts[i].outcome.Equals("victory"))
            {
                numLevelsCompleted++;
            }
            else if (attempts[i].outcome.Equals("time"))
            {
                numRanOutOfTime++;
            }
            else if (!attempts[i].outcome.Equals("quit"))
            {
                Debug.Log("ERROR: invalid outcome code in attempt " + i + " in level " + attempts[i].level + ": " + attempts[i].outcome);
            }


        }

        //header
        //string allData = "totalTime,total_standTime,total_jumpTime,total_runTime,total_walkTime,totalAttempts,numLevelsCompleted,numRanOutOfTime";

        string allData = "BEGIN_EXPLORATION,";
        allData += total_time + "," + total_standTime + "," + total_jumpTime + "," + total_runTime + "," + total_walkTime + "," + attemptIndex + ",";
        allData += numLevelsCompleted + "," + numRanOutOfTime + ",";

        //attempts data
        for (int i = 0; i < attempts.Count; i++)
        {
            allData += attempts[i].toString();
        }

        allData += "END_EXPLORATION,";

        Debug.Log(allData);
        return allData;
        // This is where we send data to the server - use Javascript library added to project
    }
}
