using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

// this class waits until the player finishes the demo or chooses to quit the game, then takes data from 
// both the ExplorationDataManager and ConstructionDataManager. 
public class DataAggregator : MonoBehaviour {

    private static DataAggregator instance;
    private ExplorationDataManager expDataManager;
    private ConstructionDataManager constDataManager;

    [DllImport("__Internal")]
    private static extern void sendToDB(string playerData);

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        //makes the GameObject with this script attached to it persist across game levels
        DontDestroyOnLoad(transform.gameObject);

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
        if (!scene.name.Equals("Canyon2"))
        {
            expDataManager.enabled = false;
            constDataManager.enabled = true;
            constDataManager.AddNewAttempt(scene.name);
            Debug.Log("Finished Loading Scene " + scene.name + ", ExplorationDataManager active");


        }
        else
        {
            constDataManager.enabled = false;
            expDataManager.enabled = true;
            expDataManager.AddNewAttempt(scene.name);
            Debug.Log("Finished Loading Scene " + scene.name + ", ConstructionDataManager active");

        }
    }

    // Use this for initialization
    void Start () {
		
	}

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
