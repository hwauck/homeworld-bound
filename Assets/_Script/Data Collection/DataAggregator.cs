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
        initializeDataCollection(scene);
    }

    // here, "loading" refers to both the act of loading a new scene (e.g. a Construction mode level) 
    // and switching to an already loaded scene (e.g. the current Exploration Mode scene after finishing a Construction Mode level)
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

        string currentScene;

        if (LoadUtils.loadedScenes.Count < 2)
        {
            currentScene = SceneManager.GetActiveScene().name;
        }
        else
        {
            currentScene = LoadUtils.currentSceneName;
        }

        if (!scene.name.Equals("Canyon2"))
        {
            expDataManager.enabled = false;
            constDataManager.enabled = true;
            constDataManager.AddNewAttempt(scene.name, true);
            Debug.Log("Finished Loading Scene " + scene.name + ", ConstructionDataManager active");

        }
        else
        {
            constDataManager.enabled = false;
            expDataManager.enabled = true;
            ExplorationLevelResetter expLevelResetter = GameObject.Find("EventSystem").GetComponent<ExplorationLevelResetter>();
            expDataManager.AddNewAttempt(scene.name, true);
            expLevelResetter.setWhatToBuild();
            Debug.Log("Finished Loading Scene " + scene.name + ", ExplorationDataManager active");

        }
    }

    // Use this for initialization
    void Start () {
		
	}

    // called when P is pressed during Exploration or Construction Mode via events
    public void saveAndSendToServer()
    {
        string allData = "BEGIN_HB_PLAYERDATA,";
        allData += expDataManager.saveAllData();
        allData += constDataManager.saveAllData();
        allData += "END_HB_PLAYERDATA";
        Debug.Log("SENDING TO SERVER: " + allData);
        sendToDB(allData);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
