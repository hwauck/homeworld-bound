﻿using System.Collections;
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

        if (scene.name.Equals("RuinedCity"))
        {
            // reassign missing variables
            GameObject eventSystem = GameObject.Find("EventSystem");

            ExplorationLevelResetter resetter = eventSystem.GetComponent<ExplorationLevelResetter>();
            ScrollingText scrollingText = GameObject.Find("ConversationSystem").GetComponentInChildren<ScrollingText>();
            Timer timer = GameObject.Find("TimeRemainingPanel").GetComponent<Timer>();
            GameObject newAudioSourceObj = GameObject.Find("SFX");
            if (newAudioSourceObj != null)
            {
                if (newAudioSourceObj.GetComponent<AudioSource>() != null)
                {
                    resetter.audioSource = newAudioSourceObj.GetComponent<AudioSource>();
                    scrollingText.audioSource = newAudioSourceObj.GetComponent<AudioSource>();
                }
                else
                {
                    Debug.Log("WARNING: No Audio Source found in scene!");
                }
            }
            else
            {
                Debug.Log("WARNING: no Audio Source Object found in scene!");
            }

            GameObject newMusicSourceObj = GameObject.Find("Music");
            if (newMusicSourceObj != null)
            {
                if (newMusicSourceObj.GetComponent<AudioSource>() != null)
                {
                    resetter.musicSource = newMusicSourceObj.GetComponent<AudioSource>();
                    timer.MusicSource = newMusicSourceObj.GetComponent<AudioSource>();
                }
                else
                {
                    Debug.Log("WARNING: No Music Source found!");

                }
            }
            else
            {
                Debug.Log("WARNING: No Music Source Object found!");
            }

            resetter.gameQuit.AddListener(this.saveAndSendToServer);


            GameObject[] clues = GameObject.FindGameObjectsWithTag("clue");
            //Debug.Log("clues size: " + clues.Length);
            for (int i = 0; i < clues.Length; i++)
            {
                PickUp pickup = clues[i].GetComponent<PickUp>();
                pickup.levelResetter = resetter;
            }

            GameObject[] key1Parts = GameObject.FindGameObjectsWithTag("key1");
            //Debug.Log("key1Parts size: " + key1Parts.Length);
            for(int i = 0; i < key1Parts.Length; i++)
            {
                PickUp pickup = key1Parts[i].GetComponent<PickUp>();
                pickup.levelResetter = resetter;
                pickup.partCounterObj = GameObject.Find("PartsFound");
            }
        } 

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

        if (!scene.name.Equals("Canyon2") && !scene.name.Equals("RuinedCity"))
        {
            // we're in Construction Mode, not loading from a save (or was loaded from Exploration Mode upon loading a save
            expDataManager.enabled = false;
            constDataManager.enabled = true;
            constDataManager.AddNewAttempt(scene.name, true);
            Debug.Log("currently in Construction Mode! Adding new attempt");


        }
        else
        {

            // Load save when inventory controller activates. Has to happen here because we need to wait till the scene is loaded to load the saved data
            if(!SaveController.alreadyLoaded)
            {
                SaveController.Load();
                SaveController.alreadyLoaded = true;
                Debug.Log("Successfully loaded saved game data");
            }

            constDataManager.enabled = false;
            expDataManager.enabled = true;
            ExplorationLevelResetter expLevelResetter = GameObject.Find("EventSystem").GetComponent<ExplorationLevelResetter>();
            expDataManager.AddNewAttempt(scene.name, true);
            expLevelResetter.setWhatToBuild();
            string whatToBuild = expLevelResetter.getWhatToBuild();
            Debug.Log("New attempt for level " + expDataManager.GetCurrAttempt().level + " added!");
            Debug.Log("Data Aggregator: WhatToBuild = " + whatToBuild);

            // loaded saves always start in Exploration Mode, so if the player was in the middle of a Construction Mode level,
            // Construction Mode will start up here from Exploration Mode
            if (ConversationTrigger.GetToken("battery_const_in_progress") 
                || ConversationTrigger.GetToken("item_const_in_progress") 
                || (ConversationTrigger.GetToken("finished_b4") && ConversationTrigger.GetToken("not_finished_const_map_intro"))
                || (ConversationTrigger.GetToken("finished_b8") && ConversationTrigger.GetToken("not_finished_const_map_intro")))
            {
                expLevelResetter.loadConstModeIfConstInProgress();
            } else
            {
                expLevelResetter.setUpCurrentLevel();
            }



        }
    }

    // Use this for initialization
    void Start () {
		
	}

    // called when P is pressed during Exploration or Construction Mode via events
    // TODO: uncomment when ready for testing on server
    public void saveAndSendToServer()
    {
        string allData = "BEGIN_HB_PLAYERDATA,";
        allData += expDataManager.saveAllData();
        allData += constDataManager.saveAllData();
        allData += "END_HB_PLAYERDATA";
        //Debug.Log("SENDING TO SERVER: " + allData);
        sendToDB(allData);
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
