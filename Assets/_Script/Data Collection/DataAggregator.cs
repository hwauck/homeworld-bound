using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

// this class waits until the player finishes the demo or chooses to quit the game, then takes data from 
// both the ExplorationDataManager and ConstructionDataManager. 
public class DataAggregator : MonoBehaviour {

    private static DataAggregator instance;
    public ExplorationDataManager expDataManager;
    public ConstructionDataManager constDataManager;

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
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.name.Equals("Canyon2"))
        {
            GameObject.Find("ExplorationDataManager").SetActive(false);
            GameObject.Find("ConstructionDataManager").SetActive(true);
            constDataManager.AddNewAttempt(scene.name);
            Debug.Log("Finished Loading Scene " + scene.name + ", ExplorationDataManager active");


        }
        else
        {
            GameObject.Find("ConstructionDataManager").SetActive(false);
            GameObject.Find("ExplorationDataManager").SetActive(true);
            expDataManager.AddNewAttempt(scene.name);
            Debug.Log("Finished Loading Scene " + scene.name + ", ConstructionDataManager active");

        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
