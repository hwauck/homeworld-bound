using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script defines all the data that need to be collected in construction mode.
/// </summary>
public class ConstructionDataManager : MonoBehaviour {

    protected int attemptCount;
    private float total_const_time;
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
            return "Attempt" + attemptNum + ";level:" + level + ";playTime:" + playTime + ";rotateErrors:" + rotateErrors + ";faceErrors:" + faceErrors + ";rotationCount:" + rotationCount
                + ";timeRotatingCamera:" + timeRotatingCamera + ";numPartsFused:" + numPartsFused + ";numPartsSelected:" + numPartsSelected + ";partSelectionOrder:" 
                + partSelectionOrder + ";partFuseOrder:" + partFuseOrder + ";outcome:" + outcome + "\n";
        }

    }

    public void initializeDataVars()
    {
        total_const_time = 0;
        total_const_errors = 0;
        total_face_errors = 0;
        total_rotate_errors = 0;
        total_rotations = 0;
        total_cameraRotate_time = 0;
        const_levels_completed = 0;
        total_parts_selected = 0;
        const_numOutOfTime = 0;
        const_numOutOfRotations = 0;
        attemptCount = -1;

        attempts = new List<Attempt>();
    }

    void Update()
    {
        // Update playtime for current attempt
        GetCurrAttempt().playTime += Time.deltaTime;
    }

    public void AddNewAttempt(string sceneName)
    {
        attemptCount++;
        Attempt newAttempt = new Attempt(sceneName, attemptCount);
        attempts.Add(newAttempt);
    }

    public Attempt GetCurrAttempt()
    {
        return attempts[attemptCount];
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

    // Set playtime to be the value assigned by developer
    public void SetPlaytime(float playtime)
    {
        GetCurrAttempt().playTime = playtime;
    }

    public void SetRotateTime(float rotateTime)
    {
        GetCurrAttempt().timeRotatingCamera = rotateTime;
    }

    public void UpdateRotateTime()
    {
        GetCurrAttempt().timeRotatingCamera += Time.deltaTime;
    }

    // set to "victory" when player fuses all parts in FuseEvent or CreatePart
    // set to "quit" when player presses the key to quit game early
    // set to "time" when timer runs out on timed levels
    // set to "rotation" when rotation counter runs out
    public void SetOutcome(string outcome)
    {
        GetCurrAttempt().outcome = outcome;
    }

    // Get info of each attempt
    public override string ToString()
    {
        string retval = "Attempts:" + (attemptCount + 1).ToString() + ",";
        foreach (Attempt attempt in attempts)
        {
            retval += ("Playtime:" + attempt.playTime.ToString() + ",");
            retval += ("RotationErrors:" + attempt.rotateErrors.ToString() + ",");
            retval += ("FaceErrors:" + attempt.faceErrors.ToString() + ",");
            retval += ("Rotations:" + attempt.rotationCount.ToString() + ",");
            retval += ("CameraRotationTime:" + attempt.timeRotatingCamera.ToString() + ",");
            retval += ("FailType:" + attempt.outcome + "\n");
        }
        return retval;
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
        allData += total_const_time + "," + total_const_errors + "," + total_face_errors + "," + total_rotate_errors + "," + total_rotations + "," 
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

    // Get accumulated attempts info
    public string GetAllAttemptsInfo()
    {
        string retval = "attempts:" + (attemptCount + 1).ToString() + ",";
        float totalPlaytime = 0;
        int totalRotateErrors = 0;
        int totalFaceErrors = 0;
        int totalRotationCount = 0;
        float totalTimeRotatingCamera = 0;
        foreach (Attempt attempt in attempts)
        {
            totalPlaytime += attempt.playTime;
            totalRotateErrors += attempt.rotateErrors;
            totalFaceErrors += attempt.faceErrors;
            totalRotationCount += attempt.rotationCount;
            totalTimeRotatingCamera += attempt.timeRotatingCamera;
        }
        retval += ("TotalPlaytime:" + totalPlaytime.ToString() + ",");
        retval += ("TotalRotationErrors:" + totalRotateErrors.ToString() + ",");
        retval += ("TotalFaceErrors:" + totalFaceErrors.ToString() + ",");
        retval += ("TotalRotations:" + totalRotationCount.ToString() + ",");
        retval += ("TotalCameraRotationTime:" + totalTimeRotatingCamera.ToString() + "\n");
        return retval;
    }
}
