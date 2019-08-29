using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

// this class waits until the player finishes the demo or chooses to quit the game, then takes data from 
// both the ExplorationDataManager and ConstructionDataManager. 
public class DataAggregator : MonoBehaviour {

    //private static DataAggregator instance;
    private ExplorationDataManager expDataManager;
    private ConstructionDataManager constDataManager;

    [DllImport("__Internal")]
    private static extern void sendToDB(string playerData);

    void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //makes the GameObject with this script attached to it persist across game levels
        //DontDestroyOnLoad(transform.gameObject);

        expDataManager = GetComponent<ExplorationDataManager>();
        constDataManager = GetComponent<ConstructionDataManager>();
        expDataManager.initializeDataVars();
        constDataManager.initializeDataVars();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Finished Loading Scene " + scene.name);
        initializeDataCollection(scene);
    }
 
    // this is called in three cases:
    // 1. new level loaded
    // 2. switch level back to already loaded level (Construction -> Exploration)
    // 3. Current level reset due to running out of time or rotations
    private void initializeDataCollection(Scene scene)
    {
        //if(LoadUtils.loadedScenes.Count > 0)
        //{
        //    while (!LoadUtils.isSceneLoaded)
        //    {
        //        yield return null;
        //    }
        //}

        ////reset isSceneLoaded for next level load
        //LoadUtils.isSceneLoaded = false;

        // stop any conversation controller's reading text data collection in case scene switched before conversation ended
        expDataManager.setIsReadingText(false);
        constDataManager.setIsReadingText(false);

        if (!scene.name.Equals("Canyon2"))
        {
            expDataManager.enabled = false;
            constDataManager.enabled = true;
            constDataManager.AddNewAttempt(scene.name, true);

        }
        else
        {
            constDataManager.enabled = false;
            expDataManager.enabled = true;
            ExplorationLevelResetter expLevelResetter = GameObject.Find("EventSystem").GetComponent<ExplorationLevelResetter>();
            expDataManager.AddNewAttempt(scene.name, true);
            expLevelResetter.setWhatToBuild();
            Debug.Log("New attempt for level " + expDataManager.GetCurrAttempt().level + " added!");
        }
    }

    // Use this for initialization
    void Start () {
		
	}

    // called when P is pressed during Exploration or Construction Mode via events
    // TODO: uncomment when ready for testing on server
    public void saveAndSendToServer()
    {
        //string allData = "BEGIN_HB_PLAYERDATA,";
        //allData += expDataManager.saveAllData();
        //allData += constDataManager.saveAllData();
        //allData += "END_HB_PLAYERDATA";
        //Debug.Log("SENDING TO SERVER: " + allData);
        //sendToDB(allData);
    }
	
	// Update is called once per frame
	void Update () {

        // can't figure out a way to set up an event listener for this kind of event, so I'll just do it the clunky way
		if(LoadUtils.sceneSwitched)
        {
            LoadUtils.sceneSwitched = false;

            // we went back to an already loaded exploration mode scene from a finished Construction Mode
            // scene, so add a new attempt
            Scene currentScene;

            if (LoadUtils.loadedScenes.Count < 2)
            {
                currentScene = SceneManager.GetActiveScene();
            }
            else
            {
                string sceneName = LoadUtils.currentSceneName;
                currentScene = SceneManager.GetSceneByName(sceneName);
            }
            initializeDataCollection(currentScene);
        }
	}
}
