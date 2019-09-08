using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script defines all the data that need to be collected in construction mode.
/// </summary>
public class ConstructionDataManager : MonoBehaviour {

    private bool isPaused; // should gameplay time be incremented (unpaused) or not (paused)?
    private bool isReadingText;
    protected int attemptIndex; // across Construction Mode, how many total attempts were there? (attemptIndex+1)
    private int attemptCount; // for each Construction Mode level, how many attempts were there? Starts over for each new level.
    private float total_const_time;
    private float total_readTime;
    private int total_const_errors;
    private int total_face_errors;
    private int total_rotate_errors;
    private int total_rotations;
    private float total_cameraRotate_time;
    private int const_levels_completed;
    private int total_parts_selected;
    private int const_numOutOfTime;
    private int const_numOutOfRotations;
    private List<Attempt> attempts;

    public class Attempt
    {
        public int attemptNum;
        public string level;
        public float playTime;
        public float readTime;
        public int rotateErrors;
        public int faceErrors;
        public int rotationCount;
        public float timeRotatingCamera;
        public int numPartsFused;
        public int numPartsSelected;
        public string partSelectionOrder;
        public string partFuseOrder;
        public string outcome; // "victory" or "rotation" or "time" or "quit"

        public Attempt(string levelName, int currentAttemptNum)
        {
            attemptNum = currentAttemptNum;
            level = levelName;
            playTime = 0;
            readTime = 0;
            rotateErrors = 0;
            faceErrors = 0;
            rotationCount = 0;
            timeRotatingCamera = 0;
            numPartsFused = 0;
            numPartsSelected = 0;
            partSelectionOrder = ":";
            partFuseOrder = ":";
            outcome = "None"; // either "quit" (ended game), "time" (ran out of time on timed levels), "rotation" (ran out of rotations), or "victory"
        }

        public string toString()
        {
            return "Attempt" + attemptNum + ";level:" + level + ";playTime:" + playTime + ";readTime:" + readTime + ";rotateErrors:" + rotateErrors + ";faceErrors:" + faceErrors + ";rotationCount:" + rotationCount
                + ";timeRotatingCamera:" + timeRotatingCamera + ";numPartsFused:" + numPartsFused + ";numPartsSelected:" + numPartsSelected + ";partSelectionOrder:" 
                + partSelectionOrder + ";partFuseOrder:" + partFuseOrder + ";outcome:" + outcome + "\n";
        }

    }

    public void initializeDataVars()
    {
        isPaused = false;
        isReadingText = false;
        total_const_time = 0;
        total_readTime = 0;
        total_const_errors = 0;
        total_face_errors = 0;
        total_rotate_errors = 0;
        total_rotations = 0;
        total_cameraRotate_time = 0;
        const_levels_completed = 0;
        total_parts_selected = 0;
        const_numOutOfTime = 0;
        const_numOutOfRotations = 0;
        attemptIndex = -1;
        attemptCount = 1;

        attempts = new List<Attempt>();
    }

    private void OnEnable()
    {
        isPaused = false;
    }

    void Update()
    {
        // Update playtime for current attempt
        if(!isPaused)
        {
            GetCurrAttempt().playTime += Time.deltaTime;
        }

        if(isReadingText)
        {
            GetCurrAttempt().readTime += Time.deltaTime;
            //Debug.Log("READING: " + GetCurrAttempt().readTime);
            if(isPaused)
            {
                GetCurrAttempt().playTime += Time.deltaTime;
            }
        }
    }

    public void AddNewAttempt(string sceneName, bool restartAttemptCount)
    {
        if (restartAttemptCount)
        {
            attemptCount = 1;
        }
        else
        {
            attemptCount++;
        }
        attemptIndex++;
        Attempt newAttempt = new Attempt(sceneName, attemptCount);
        attempts.Add(newAttempt);
    }

    public Attempt GetCurrAttempt()
    {
        return attempts[attemptIndex];
    }

    public void AddFaceError()
    {
        GetCurrAttempt().faceErrors++;
    }

    public void AddRotateError()
    {
        GetCurrAttempt().rotateErrors++;
    }

    public void AddRotation()
    {
        GetCurrAttempt().rotationCount++;
    }

    //TODO add to FuseEvent script
    public void AddPartFused(string partName)
    {
        GetCurrAttempt().numPartsFused++;
        GetCurrAttempt().partFuseOrder += partName + ":";
    }

    //TODO: add to CreatePart scripts
    public void AddPartSelected(string partName)
    {
        GetCurrAttempt().numPartsSelected++;
        GetCurrAttempt().partSelectionOrder += partName + ":";
    }

    public void setIsReadingText(bool isReadingText)
    {
        this.isReadingText = isReadingText;
    }

    // Set playtime to be the value assigned by developer
    public void SetPlaytime(float playtime)
    {
        GetCurrAttempt().playTime = playtime;
    }

    public void SetRotateTime(float rotateTime)
    {
        GetCurrAttempt().timeRotatingCamera = rotateTime;
    }

    public void setPauseGameplay(bool isPaused)
    {
        this.isPaused = isPaused;
    }

    public bool getPauseGameplay()
    {
        return isPaused;
    }

    public void UpdateRotateTime()
    {
        GetCurrAttempt().timeRotatingCamera += Time.deltaTime;
    }

    // set to "victory" when player fuses all parts in FuseEvent or CreatePart (or when a levvel had to be skipped for debug reasons)
    // set to "quit" when player presses the key to quit game early
    // set to "time" when timer runs out on timed levels
    // set to "rotation" when rotation counter runs out
    public void SetOutcome(string outcome)
    {
        GetCurrAttempt().outcome = outcome;
    }

    public string saveAllData()
    {
        // setting these to zero again so this can recalculate and save totals multiple times during a game
        total_const_time = 0;
        total_const_errors = 0;
        total_face_errors = 0;
        total_rotate_errors = 0;
        total_rotations = 0;
        total_cameraRotate_time = 0;
        const_levels_completed = 0;
        const_numOutOfTime = 0;
        const_numOutOfRotations = 0;
        total_readTime = 0;
        total_parts_selected = 0;

        // calculate totals across levels/attempts
        for (int i = 0; i < attempts.Count; i++)
        {
            total_const_time += attempts[i].playTime;
            total_face_errors += attempts[i].faceErrors;
            total_rotate_errors += attempts[i].rotateErrors;
            total_const_errors = total_face_errors + total_rotate_errors;
            total_rotations += attempts[i].rotationCount;
            total_cameraRotate_time += attempts[i].timeRotatingCamera;
            total_parts_selected += attempts[i].numPartsSelected;
            total_readTime += attempts[i].readTime;

            string thisAttemptPartSelectionOrder = attempts[i].partSelectionOrder;
            attempts[i].partSelectionOrder = thisAttemptPartSelectionOrder.Substring(0, thisAttemptPartSelectionOrder.Length - 1); // get rid of extra colon at end
            string thisAttemptPartFuseOrder = attempts[i].partFuseOrder;
            attempts[i].partFuseOrder = thisAttemptPartFuseOrder.Substring(0, thisAttemptPartFuseOrder.Length - 1); // get rid of extra colon at end

            if (attempts[i].outcome.Equals("victory"))
            {
                const_levels_completed++;
            }
            else if (attempts[i].outcome.Equals("time"))
            {
                const_numOutOfTime++;
            }
            else if (attempts[i].outcome.Equals("rotation"))
            {
                const_numOutOfRotations++;
            }
            else if (!attempts[i].outcome.Equals("quit"))
            {
                Debug.Log("ERROR: invalid outcome code in attempt " + i + " in level " + attempts[i].level + ": " + attempts[i].outcome);
            }


        }

        string allData = "BEGIN_CONSTRUCTION,";
        allData += total_const_time + "," + total_readTime + "," + total_const_errors + "," + total_face_errors + "," + total_rotate_errors + "," + total_rotations + "," 
            + total_cameraRotate_time + ","+ const_levels_completed + "," + const_numOutOfTime + "," + const_numOutOfRotations + "," + total_parts_selected + ",";

        //attempts data
        for (int i = 0; i < attempts.Count; i++)
        {
            allData += attempts[i].toString();
        }

        allData += "END_CONSTRUCTION,";

        Debug.Log(allData);
        return allData;
        // This is where we send data to the server - use Javascript library added to project
    }

}
