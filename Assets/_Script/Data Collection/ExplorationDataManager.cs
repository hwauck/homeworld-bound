using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;


public class ExplorationDataManager : MonoBehaviour {

    private bool pauseGameplay;
    private bool isReadingText;
    private float total_time;
    private float total_standTime;
    private float total_jumpTime;
    private float total_runTime;
    private float total_walkTime;
    private float total_readTime;
    private int attemptIndex; // total attempt count across all exploration mode levels/tasks/areas
    private int attemptCount; // attempt count, level-specific - starts over in each new collection task/area. 
    private int numLevelsCompleted;
    private int numRanOutOfTime;
    private int total_jumps;

    public RigidbodyFirstPersonController playerController;

    private List<Attempt> attempts;

    public class Attempt
    {
        // starts over every time "level" value changes - so every time exploration mode is entered/exited, but not when
        // player runs out of time on a timed level
        public int attemptNum; 
        public string level;
        public float playTime;
        public float standTime;
        public float jumpTime;
        public float runTime;
        public float walkTime;
        public float readTime;
        public int jumps;
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
            readTime = 0;
            jumps = 0;
            partCollectionOrder = ":";
            outcome = "None"; // will be either "time" (ran out of time), "victory", "quit" (ended game), or "finishedDemo"
        }

        public string toString()
        {
            return "Attempt" + attemptNum + ";level:" + level + ";playTime:" + playTime + ";standTime:" + standTime + ";jumpTime:" + jumpTime + ";runTime:"+ runTime 
                + ";walkTime:" + walkTime + ";readTime:" + readTime + ";jumps:" + jumps + ";partCollOrder:" + partCollectionOrder + ";outcome:" + outcome + "\n";
        }

    }

    // should happen only once during the game, in DataAggregator
    public void initializeDataVars()
    {
        pauseGameplay = false;
        total_time = 0;
        total_standTime = 0;
        total_jumpTime = 0;
        total_runTime = 0;
        total_walkTime = 0;
        total_readTime = 0;
        total_jumps = 0;
        attemptIndex = -1;
        numLevelsCompleted = 0;
        numRanOutOfTime = 0;
        attempts = new List<Attempt>();
        attemptCount = 1;
    }

    private void OnEnable()
    {
        pauseGameplay = false;
    }

    void Update () {
        if(!pauseGameplay)
        {
            GetCurrAttempt().playTime += Time.deltaTime;
            
            if(isReadingText)
            {
                GetCurrAttempt().readTime += Time.deltaTime;
                //Debug.Log("READING: " + GetCurrAttempt().readTime);
            }
            else if (playerController.Velocity.Equals(new Vector3(0, 0, 0)))
            {
                GetCurrAttempt().standTime += Time.deltaTime;
                //Debug.Log("STANDING: " + GetCurrAttempt().standTime);
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

            if(Input.GetKeyUp(KeyCode.Space))
            {
                GetCurrAttempt().jumps++;
            }
        }


	}

    // other scripts can tell this one when playtime should not be incremented - when gameplay is "paused"
    // e.g. when the player is in Fuser mode
    public void setPauseGameplay(bool currentlyPaused)
    {
        pauseGameplay = currentlyPaused;
        if (currentlyPaused)
        {
            Debug.Log("PAUSED PLAYTIME COLLECTION");
        } else
        {
            Debug.Log("UNPAUSED PLAYTIME COLLECTION");
        }
    }

    public bool getPauseGameplay()
    {
        return pauseGameplay;
    }

    public void setIsReadingText(bool readingText)
    {
        isReadingText = readingText;
 
    }

    // needs to be called every time player runs of out of time (so only on timed levels). 
    // Is automatically called each time Canyon2 scene is entered via DataAggregator
    public void AddNewAttempt(string sceneName, bool restartAttemptCount)
    {
        if(restartAttemptCount)
        {
            attemptCount = 1;
        } else
        {
            attemptCount++;
        }
        attemptIndex++;
        Attempt newAttempt = new Attempt(sceneName, attemptCount);
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

    public void setLevelSuffix(string suffix)
    {
        GetCurrAttempt().level += "_" + suffix;
    }

    public Attempt GetCurrAttempt()
    {
        return attempts[attemptIndex];
    }

    public string saveAllData()
    {
        // setting these to zero again so this can recalculate and save totals multiple times during a game
        total_time = 0;
        total_standTime = 0;
        total_jumpTime = 0;
        total_runTime = 0;
        total_walkTime = 0;
        total_readTime = 0;
        total_jumps = 0;
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
            total_readTime += attempts[i].readTime;
            total_jumps += attempts[i].jumps;

            if (attempts[i].outcome.Equals("victory"))
            {
                numLevelsCompleted++;
            }
            else if (attempts[i].outcome.Equals("time"))
            {
                numRanOutOfTime++;
            }
            else if (!attempts[i].outcome.Equals("quit") && !attempts[i].outcome.Equals("finishedDemo"))
            {
                Debug.Log("ERROR: invalid outcome code in attempt " + (i+1) + " in level " + attempts[i].level + ": " + attempts[i].outcome);
            }


        }

        //header

        string allData = "BEGIN_EXPLORATION,";
        allData += total_time + "," + total_standTime + "," + total_jumpTime + "," + total_runTime + "," + total_walkTime + "," + total_readTime + "," + total_jumps 
            + "," + (attemptIndex+1) + "," + numLevelsCompleted + "," + numRanOutOfTime + ",";

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
