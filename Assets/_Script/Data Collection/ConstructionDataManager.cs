using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines all the data that need to be collected in construction mode.
/// </summary>
public class ConstructionDataManager : MonoBehaviour {

    public int attemptCount;
    public List<Attempt> attempts;

    public class Attempt
    {
        public float playtime { get; set; }
        public int rotateErrors { get; set; }
        public int faceErrors { get; set; }
        public int rotationCount { get; set; }
        public float timeRotatingCamera { get; set; }
        public string failType { get; set; }

        public Attempt()
        {
            playtime = 0;
            rotateErrors = 0;
            faceErrors = 0;
            rotationCount = 0;
            timeRotatingCamera = 0;
            failType = "No";
        }
        
    }

    void Start()
    {
        attempts = new List<Attempt>();
        Attempt initialAttempt = new Attempt();
        attempts.Add(initialAttempt);
        attemptCount = 0;
    }

    void Update()
    {
        // Update playtime for current attempt
        GetCurrAttempt().playtime += Time.deltaTime;
    }

    public void AddNewAttempt()
    {
        Attempt newAttempt = new Attempt();
        attempts.Add(newAttempt);
        attemptCount++;
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

    // Set playtime to be the value assigned by developer
    public void SetPlaytime(float playtime)
    {
        GetCurrAttempt().playtime = playtime;
    }

    public void SetRotateTime(float rotateTime)
    {
        GetCurrAttempt().timeRotatingCamera = rotateTime;
    }

    public void UpdateRotateTime()
    {
        GetCurrAttempt().timeRotatingCamera += Time.deltaTime;
    }

    public void SetFailType(string failType)
    {
        GetCurrAttempt().failType = failType;
    }

    // Get info of each attempt
    public override string ToString()
    {
        string retval = "Attempts:" + (attemptCount + 1).ToString() + ",";
        foreach (Attempt attempt in attempts)
        {
            retval += ("Playtime:" + attempt.playtime.ToString() + ",");
            retval += ("RotationErrors:" + attempt.rotateErrors.ToString() + ",");
            retval += ("FaceErrors:" + attempt.faceErrors.ToString() + ",");
            retval += ("Rotations:" + attempt.rotationCount.ToString() + ",");
            retval += ("CameraRotationTime:" + attempt.timeRotatingCamera.ToString() + ",");
            retval += ("FailType:" + attempt.failType + "\n");
        }
        return retval;
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
            totalPlaytime += attempt.playtime;
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
