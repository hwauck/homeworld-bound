using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour {
    public Text timerLabel;
    public float timeGiven;
    private float timeRemaining; // saves the timeRemaining value entered in inspector for when the level is restarted
    private int minutes;
    private int seconds;
    private bool timerStarted = false;
    public UnityEvent powerFailure;
    private int numRanOutOfTime; // this should be recorded by the appropriate level and then reset to 0 each level.
    public AudioClip ExploreM;
    public AudioClip ExploreMLOW;
    public AudioClip ExploreNonTimed;
    public AudioSource MusicSource;
    private bool sinisterMusicstarted = false;
    public UnityEvent isTimedLevel;
    // For data collection
    public bool isConstructionTimer; // is this timer for construction mode levels or exploration mode levels?
    public ConstructionDataManager constructDataManager;
    public ExplorationDataManager exploreDataManager;

    void Awake()
    {
        isTimedLevel.Invoke(); // prompts LevelResetter to disablePlayerControls()
        numRanOutOfTime = 0;
    }

    // Use this for initialization
    void Start () {
        timeRemaining = timeGiven;
        minutes = Mathf.FloorToInt(timeRemaining / 60F);
        seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);
        timerLabel.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        //play regular music during battery levels
        MusicSource.Play();

        // For data collection
        if (isConstructionTimer)
        {
            GameObject dataCollectionManager = GameObject.Find("DataCollectionManager");
            if(dataCollectionManager != null)
            {
                constructDataManager = GameObject.Find("DataCollectionManager").GetComponent<ConstructionDataManager>();
                if(constructDataManager == null)
                {
                    Debug.LogError("ERROR: ConstructionDataManager component missing from DataCollectonManager!");
                }
            }
        } else
        {
            exploreDataManager = GameObject.Find("DataCollectionManager").GetComponent<ExplorationDataManager>();
        }
    }

    public void startTimer()
    {
        timerStarted = true;
        MusicSource.Stop();
        MusicSource.clip = ExploreM;
        MusicSource.Play();
    }

    public void stopTimer()
    {
        timerStarted = false;

    }

    public void resetTimer()
    {
        timeRemaining = timeGiven;
        minutes = Mathf.FloorToInt(timeRemaining / 60F);
        seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);
        timerLabel.text = string.Format("{0:0}:{1:00}", minutes, seconds);

    }

    public int getNumRanOutOfTime()
    {
        return numRanOutOfTime;
    }

    public void stopMusic()
    {
        MusicSource.Stop();
        sinisterMusicstarted = false;
    }

    public void startMusic()
    {
        MusicSource.Play();
    }

    void Update()
    {
        if (timerStarted)
        {

            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
            }

            if (timeRemaining < 60)

            {
                if (!sinisterMusicstarted)
                {
                    sinisterMusicstarted = true;


                    Debug.Log("setting sinister Musictotrue");
                    MusicSource.Stop();


                    MusicSource.clip = ExploreMLOW;
                    MusicSource.Play();
                    Debug.Log("changing clip and playing");
                }
            }

            if (timeRemaining < 0)
            {
                stopTimer();
                powerFailure.Invoke();
                stopMusic();
                // For data collection
                numRanOutOfTime++;
                if (isConstructionTimer)
                {
                    if (constructDataManager != null) { 
                        constructDataManager.SetOutcome("time");
                    }
                } else
                {
                    exploreDataManager.setOutcome("time");
                }

                minutes = 0;
                seconds = 0;
            }
      
            else
            {
                minutes = Mathf.FloorToInt(timeRemaining / 60F);
                seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);
 
            }

            if (timeRemaining >= 0)
            {
                //update the label value
                timerLabel.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            }
        }
    }
}
