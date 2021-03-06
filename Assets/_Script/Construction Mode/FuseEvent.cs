﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;


// This class has turned into a catch-all place for various Construction Mode level-agnostic operations
// and as a result is kind of an ugly monster to read through. If you're looking for code that operates 
// in every Construction Mode level and can't be found in a script attached to Construction Mode prefabs
// or UI elements, or in the LevelResetter script, it's probably here. This class primarily handles fusing,
// checking whether a fuse has been performed correctly or not, and transitioning to the next level after
// a Construction Mode level is successfully completed, but also handles Construction Mode telemetry data
// collection, music, rotation and fuse error messages, debug cheat codes, and level loading 
// (the latter in tandem with LevelResetter).
public class FuseEvent : MonoBehaviour {

    //tutorial variables
    // Some of these tutorial variables may be unused in this version of the game, but play a role in some of the older,
    // more structured versions of the Construction Mode tutorial for the b1 level
    public static bool runningJustConstructionMode = false;
    private bool firstFuseComplete;
    private bool isFirstLevel = false; // so we don't have to wait for LoadUtils.LoadScene before setting up fuseMapping
    private int loadedLevels = 0;

    //keep track of which objects are currently selected for fusing
    private GameObject selectedObject;
	private GameObject selectedFuseTo;
	//dictionary containing valid fuse pairs of planes (Fusing, FuseTo)
	private Dictionary<string, string> fuseMapping;
	private int middleTRotation;

	//CoRoutine Variables
	private enum Fade {In, Out};
	private float fadeTime = 5.0F;

    //Music
    public AudioSource source;
    public AudioSource musicsource;
    public AudioClip success;
    public AudioClip failure;
    public AudioClip music;
    public AudioClip victorymusic;

    private string fuseStatus;

    //Buttons
    public GameObject[] partButtons;
	public Button connectButton;
    public Button claimItem;    // A button which appears upon completion of an item to claim it in exploration mode. (says "Take")

    // Non-Button UI elements
	public Text shapesWrong;
	public Text rotationWrong;
	public RotationGizmo rotateGizmo;	// When completing a fuse, disable the rotation gizmo.
	public GameObject victoryPrefab;
	public CanvasGroup bottomPanelGroup;
	public CanvasGroup congratsPanelGroup;
	public CanvasGroup errorPanelGroup;
    public GameObject congratsPanel;
	public Image finishedImage;

	public Camera mainCam;
	private GameObject group;
	private int fuseCount;
	private int NUM_FUSES;

    //data collection
    private float levelTimer;
	private string filename; // only for local machine save files.
	//private StreamWriter sr;
	private int numFuseAttempts;
	private int numWrongRotationFails;
	private int numWrongFacesFails;

    // Data collection
    public ConstructionDataManager dataManager;

    public UnityEvent finishedLoadingLevel;

	void OnEnable()
	{
        // For data collection.
        startLevelTimer();
        if (!dataManager)
        {
            if (GameObject.Find("DataCollectionManager"))
                dataManager = GameObject.Find("DataCollectionManager").GetComponent<ConstructionDataManager>();
        }
        musicsource.clip = music;

        //musicsource.Play();

        //wait until level has officially loaded before assigning fuse mappings
        StartCoroutine(createFuseMapping());
    }

	void Awake ()
	{
		// Setup camera reference properly.
		mainCam = Camera.main;

		// Grab the rotation gizmo reference.
		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();



        firstFuseComplete = false;


        if (InventoryController.levelName == "")
        {
            // Special stuff happens because we are just running construction mode without exploration mode.
            runningJustConstructionMode = true;

            //! Is this a really bad idea?
            SaveController.filename += "_CONSTRUCTION-ONLY";

            // This works because levelName will be "" when we aren't coming from any specific level.

 
        }
        else
        {
            runningJustConstructionMode = false;
        }

        // Make sure all this is reset in data collection as needed whenever level is reset
        fuseCount = 0;
		fuseStatus = "none";
		filename = "ConstructionModeData.txt";
		//sr = File.AppendText(filename);
		levelTimer = Time.time;
		numFuseAttempts = 0;
		numWrongFacesFails = 0;
		numWrongRotationFails = 0;

		selectedObject = GetComponent<SelectPart>().getSelectedObject();
		selectedFuseTo = GetComponent<SelectPart>().getSelectedFuseTo();

        NUM_FUSES = 0;
		for(int i = 0; i < partButtons.Length; i++) {
			NUM_FUSES += partButtons[i].GetComponent<Uses>().numUses;
		}


	}

    // re-instantiates the victoryPrefab to avoid weird rotation behavior on level completion after power failure
    public void resetVictoryPrefab()
    {
        GameObject prevVictoryPrefab = GameObject.Find("victoryPrefab(Clone)");
        if (prevVictoryPrefab != null)
        {
            Destroy(prevVictoryPrefab);
        }
        group = Instantiate(victoryPrefab, new Vector3(-100, 30, 100), new Quaternion());

        //to make sure it's created as part of the current construction mode scene
        LoadUtils.InstantiateParenter(group);
    }

    // used by level resetter to reset the fuse count every time level is reset
    public void resetFuseCount()
    {
        fuseCount = 0;
    }


    // called by Claim/Take button after level is finished for all levels except the last battery level
    // before timed Exploration Mode
    public void goToNextScene()
    {

        string currentLevel;  
        if(loadedLevels < 2)
        {
            currentLevel = SceneManager.GetActiveScene().name;
        } else
        {
            currentLevel = LoadUtils.currentSceneName;
        }
        Debug.Log("currentLevel: " + currentLevel);
        // TODO: add new item levels here
        switch (currentLevel)
        {
            case "rocketBoots":
                RocketBoots.ActivateBoots();
                InventoryController.items.Remove("Rocket Boots Body");
                InventoryController.items.Remove("Rocket Boots Calf");
                InventoryController.items.Remove("Rocket Boots Sole");
                InventoryController.items.Remove("Rocket Boots Toe");
                InventoryController.items.Remove("Rocket Boots Toe Sole");
                InventoryController.items.Remove("Rocket Boots Trim");
                InventoryController.items.Remove("Rocket Boots Widening");
                InventoryController.ConvertInventoryToTokens();
                break;
            case "sledgehammer":
                Sledgehammer.ActivateSledgehammer();
                InventoryController.items.Remove("Sledgehammer Trapezoid");
                InventoryController.items.Remove("Sledgehammer Top Point");
                InventoryController.items.Remove("Sledgehammer Shaft");
                InventoryController.items.Remove("Sledgehammer Head");
                InventoryController.items.Remove("Sledgehammer Haft");
                InventoryController.items.Remove("Sledgehammer Bottom Point");
                InventoryController.items.Remove("Sledgehammer Bottom Point Right");
                InventoryController.items.Remove("Sledgehammer Trapezoid");
                InventoryController.items.Remove("Sledgehammer Top Point Right");
                InventoryController.items.Remove("Sledgehammer Small Tip");
                InventoryController.items.Remove("Sledgehammer Small Trap");
                InventoryController.items.Remove("Sledgehammer Tip");
                InventoryController.ConvertInventoryToTokens();
                break;
            case "key1":
                ConversationTrigger.AddToken("player_has_key1");
                InventoryController.items.Remove("Key 1 Dangly T");
                InventoryController.items.Remove("Key 1 Upright L");
                InventoryController.items.Remove("Key 1 Upright Rect");
                InventoryController.items.Remove("Key 1 Upright T");
                InventoryController.items.Remove("Key 1 Walking Pants");
                InventoryController.items.Remove("Key 1 Waluigi");
                InventoryController.ConvertInventoryToTokens();
                break;
            case "ffa":
                ConversationTrigger.AddToken("player_has_ffa");
                InventoryController.items.Remove("FFA Blue Tri");
                InventoryController.items.Remove("FFA Center Box");
                InventoryController.items.Remove("FFA Center Tri");
                InventoryController.items.Remove("FFA Handle Bottom");
                InventoryController.items.Remove("FFA Handle Top");
                InventoryController.items.Remove("FFA Left Tri");
                InventoryController.items.Remove("FFA Right Tri");
                InventoryController.items.Remove("FFA Right Tri Chunk");
                InventoryController.items.Remove("FFA Ring Large");
                InventoryController.items.Remove("FFA Ring Long");
                InventoryController.items.Remove("FFA Ring Small");
                InventoryController.items.Remove("FFA Scalene");
                InventoryController.ConvertInventoryToTokens();
                break;

            default:
                Debug.Log("Not Yet Implemented: " + SceneManager.GetActiveScene().name);
                break;
        }

        LoadUtils.LoadScene(InventoryController.levelName);
        LoadUtils.UnloadScene(currentLevel);


    }

	public bool done() {
		if(fuseCount == NUM_FUSES) {
			return true;
		}
		return false;
	}

	public string getFuseStatus() {
		return fuseStatus;
	}

    //allows other scripts, such as Tutorial scripts, to start music manually
    public void startMusic()
    {
        musicsource.Play();
    }

    //allows other scripts, such as Tutorial scripts, to start music manually
    public void stopMusic()
    {
        musicsource.Stop();
    }

    private IEnumerator createFuseMapping() {

        fuseMapping = new Dictionary<string, string>();
        //wait till next scene has loaded
        if (loadedLevels > 0)
        {
            while (!LoadUtils.isSceneLoaded)
            {

                yield return null;
            }
        }
        loadedLevels++;

        finishedLoadingLevel.Invoke(); // this triggers levelResetter's setUpCurrentLevel() method

        //reset isSceneLoaded for next level load
        LoadUtils.isSceneLoaded = false;

        string currentScene;

        if (loadedLevels < 2)
        {
            currentScene = SceneManager.GetActiveScene().name;
        } else
        {
            currentScene = LoadUtils.currentSceneName;
        }

        //NOW do fuse mappings with the correct level name!
        //fueMapping.Add(active part, fused part)
        //CHANGE this if statement by adding a new else if onto the end of it for your new level.
        // The name of the mode is the name of your level. You need to add key-value pairs to 
        // fuseMapping where the keys are names of active part ACs and the values are
        // the names of all fused part ACs that a given active part AC can attach to.
        // Thus, fuseMapping["blah"] = the fused part AC that the active part "blah" can
        // attach to. 
        // NOTE: when there are parts A and B that can be fused to each other instead of starting part,
        // fuseMapping needs mappings from A -> B AND from B -> A
        Debug.Log("Creating fuse mappings for Scene " + currentScene);

        //also, here we decide whether or not to use the FuseEvent's music setup. If it's a non-timed level,
        //we play the music from FuseEvent. If it's a timed level (with a TimeRemainingPanel), we let the 
        //Timer script on the TimeRemainingPanel handle the music
        if(!currentScene.Equals("rocketBoots") && !currentScene.Equals("sledgehammer"))
        {
            startMusic();
        }
        if (currentScene.Equals("b1"))
        {
            //b1p1 to bb1
            fuseMapping.Add("b1p1_bb1_a1", "bb1_b1p1_a1");

            //b1p2 to bb1
            fuseMapping.Add("b1p2_bb1_a1", "bb1_b1p2_a1");
            fuseMapping.Add("b1p2_bb1_a2", "bb1_b1p2_a2");

            //b1p3 to bb1
            fuseMapping.Add("b1p3_bb1_a1", "bb1_b1p3_a1");

        }
        else if (currentScene.Equals("b2")) // for b2_harder
        {
            //b2p2 to b2p1
            fuseMapping.Add("b2p2_b2p1_a1", "b2p1_b2p2_a1");
            fuseMapping.Add("b2p2_b2p1_a2", "b2p1_b2p2_a2");


            //b2p1 to bb2
            fuseMapping.Add("b2p1_bb2_a1", "bb2_b2p1_a1");
            fuseMapping.Add("b2p1_bb2_a2", "bb2_b2p1_a2");
            fuseMapping.Add("b2p1_bb2_a3", "bb2_b2p1_a3");
            fuseMapping.Add("b2p1_bb2_a4", "bb2_b2p1_a4");


            //b2p3 to b2p1
            fuseMapping.Add("b2p3_b2p1_a1", "b2p1_b2p3_a1");

            //b2p3 to b2p2
            fuseMapping.Add("b2p3_b2p2_a1", "b2p2_b2p3_a1");
            fuseMapping.Add("b2p3_b2p2_a2", "b2p2_b2p3_a2");

            //b2p2 to b2p3
            fuseMapping.Add("b2p2_b2p3_a1", "b2p3_b2p2_a1");
            fuseMapping.Add("b2p2_b2p3_a2", "b2p3_b2p2_a2");

        }
        else if(currentScene.Equals("b3")) // for b3_harder
        {
            //b3p1 to bb3
            fuseMapping.Add("b3p1_bb3_a1", "bb3_b3p1_a1");
            fuseMapping.Add("b3p1_bb3_a2", "bb3_b3p1_a2");

            //b3p2 to bb3
            fuseMapping.Add("b3p2_bb3_a1", "bb3_b3p2_a1");
            fuseMapping.Add("b3p2_bb3_a2", "bb3_b3p2_a2");

            //b3p2 to b3p1
            fuseMapping.Add("b3p2_b3p1_a1", "b3p1_b3p2_a1");

            //b3p1 to b3p2
            fuseMapping.Add("b3p1_b3p2_a1", "b3p2_b3p1_a1");

            //b3p1 to b3p3
            fuseMapping.Add("b3p1_b3p3_a1", "b3p3_b3p1_a1");
            fuseMapping.Add("b3p1_b3p3_a2", "b3p3_b3p1_a2");

            //b3p2 to b3p3
            fuseMapping.Add("b3p2_b3p3_a1", "b3p3_b3p2_a1");
            fuseMapping.Add("b3p2_b3p3_a2", "b3p3_b3p2_a2");

            //b3p3 to bb3
            fuseMapping.Add("b3p3_bb3_a1", "bb3_b3p3_a1");

            //b3p3 to b3p1
            fuseMapping.Add("b3p3_b3p1_a1", "b3p1_b3p3_a1");
            fuseMapping.Add("b3p3_b3p1_a2", "b3p1_b3p3_a2");

            //b3p3 to b3p2
            fuseMapping.Add("b3p3_b3p2_a1", "b3p2_b3p3_a1");
            fuseMapping.Add("b3p3_b3p2_a2", "b3p2_b3p3_a2");

            //b3p3 to b3p4
            fuseMapping.Add("b3p3_b3p4_a1", "b3p4_b3p3_a1");
            fuseMapping.Add("b3p3_b3p4_a2", "b3p4_b3p3_a2");
            fuseMapping.Add("b3p3_b3p4_a3", "b3p4_b3p3_a3");
            fuseMapping.Add("b3p3_b3p4_a4", "b3p4_b3p3_a4");

            //b3p4 to b3p3
            fuseMapping.Add("b3p4_b3p3_a1", "b3p3_b3p4_a1");
            fuseMapping.Add("b3p4_b3p3_a2", "b3p3_b3p4_a2");
            fuseMapping.Add("b3p4_b3p3_a3", "b3p3_b3p4_a3");
            fuseMapping.Add("b3p4_b3p3_a4", "b3p3_b3p4_a4");

            //b3p4 to b3p5
            fuseMapping.Add("b3p4_b3p5_a1", "b3p5_b3p4_a1");
            fuseMapping.Add("b3p4_b3p5_a2", "b3p5_b3p4_a2");
            fuseMapping.Add("b3p4_b3p5_a3", "b3p5_b3p4_a3");

            //b3p5 to b3p4
            fuseMapping.Add("b3p5_b3p4_a1", "b3p4_b3p5_a1");
            fuseMapping.Add("b3p5_b3p4_a2", "b3p4_b3p5_a2");
            fuseMapping.Add("b3p5_b3p4_a3", "b3p4_b3p5_a3");

            //bb3_gray to bb3
            fuseMapping.Add("bb3_gray_bb3_a1", "bb3_bb3_gray_a1");

        }
        else if (currentScene.Equals("b4")) // for b4_harder
        {
            //b4p1 to b4p2
            fuseMapping.Add("b4p1_b4p2_a1", "b4p2_b4p1_a1");

            //b4p1 to b4p3
            fuseMapping.Add("b4p1_b4p3_a1", "b4p3_b4p1_a1");

            //b4p2 to b4p1
            fuseMapping.Add("b4p2_b4p1_a1", "b4p1_b4p2_a1");

            //b4p2 to b4p4
            fuseMapping.Add("b4p2_b4p4_a1", "b4p4_b4p2_a1");

            //b4p3 to bb4
            fuseMapping.Add("b4p3_bb4_a1", "bb4_b4p3_a1");
            fuseMapping.Add("b4p3_bb4_a2", "bb4_b4p3_a2");

            //b4p3 to b4p1
            fuseMapping.Add("b4p3_b4p1_a1", "b4p1_b4p3_a1");

            //b4p3 to b4p4
            fuseMapping.Add("b4p3_b4p4_a1", "b4p4_b4p3_a1");
            fuseMapping.Add("b4p3_b4p4_a2", "b4p4_b4p3_a2");

            //b4p4 to b4p2
            fuseMapping.Add("b4p4_b4p2_a1", "b4p2_b4p4_a1");

            //b4p4 to b4p3
            fuseMapping.Add("b4p4_b4p3_a1", "b4p3_b4p4_a1");
            fuseMapping.Add("b4p4_b4p3_a2", "b4p3_b4p4_a2");

            //b4p4 to b4p5
            fuseMapping.Add("b4p4_b4p5_a1", "b4p5_b4p4_a1");

            //b4p5 to bb4
            fuseMapping.Add("b4p5_bb4_a1", "bb4_b4p5_a1");
            fuseMapping.Add("b4p5_bb4_a2", "bb4_b4p5_a2");

            //b4p5 to b4p4
            fuseMapping.Add("b4p5_b4p4_a1", "b4p4_b4p5_a1");
        }
        else if (currentScene.Equals("rocketBoots"))
        {
            // widening to heel
            fuseMapping.Add("widening_heel_attach", "heel_widening_attach");

            // calf to widening
            fuseMapping.Add("calf_widening_attach", "widening_calf_attach");

            // trim to calf
            fuseMapping.Add("trim_calf_attach", "calf_trim_attach");

            // midfoot to heel
            fuseMapping.Add("midfoot_heel_attach", "heel_midfoot_attach");

            // ballfoot to midfoot
            fuseMapping.Add("ballfoot_midfoot_attach", "midfoot_ballfoot_attach");

            // toe to ballfoot
            fuseMapping.Add("toe_ballfoot_attach", "ballfoot_toe_attach");

        }
        else if (currentScene.Equals("b5"))
        {
            // b5p2 to bb5
            fuseMapping.Add("b5p2_bb5_a1", "bb5_b5p2_a1");

            // b5p3 to bb5
            fuseMapping.Add("b5p3_bb5_a1", "bb5_b5p3_a1");
            fuseMapping.Add("b5p3_bb5_a2", "bb5_b5p3_a2");

            // b5p3 to b5p4
            fuseMapping.Add("b5p3_b5p4_a1", "b5p4_b5p3_a1");

            // b5p3 to b5p5
            fuseMapping.Add("b5p3_b5p5_a1", "b5p5_b5p3_a1");

            // b5p4 to bb5
            fuseMapping.Add("b5p4_bb5_a1", "bb5_b5p4_a1");
            fuseMapping.Add("b5p4_bb5_a2", "bb5_b5p4_a2");
            fuseMapping.Add("b5p4_bb5_a3", "bb5_b5p4_a3");

            // b5p4 to b5p3
            fuseMapping.Add("b5p4_b5p3_a1", "b5p3_b5p4_a1");

            // b5p4 to b5p5
            fuseMapping.Add("b5p4_b5p5_a1", "b5p5_b5p4_a1");
            fuseMapping.Add("b5p4_b5p5_a2", "b5p5_b5p4_a2");

            // b5p5 to bb5
            fuseMapping.Add("b5p5_bb5_a1", "bb5_b5p5_a1");
            fuseMapping.Add("b5p5_bb5_a2", "bb5_b5p5_a2");

            // b5p5 to b5p3
            fuseMapping.Add("b5p5_b5p3_a1", "b5p3_b5p5_a1");

            // b5p5 to b5p4
            fuseMapping.Add("b5p5_b5p4_a1", "b5p4_b5p5_a1");
            fuseMapping.Add("b5p5_b5p4_a2", "b5p4_b5p5_a2");

        }
        else if (currentScene.Equals("b6"))
        {
            // b6p1 to b6p2
            fuseMapping.Add("b6p1_b6p2_a1", "b6p2_b6p1_a1");

            // b6p1 to b6p4
            fuseMapping.Add("b6p1_b6p4_a1", "b6p4_b6p1_a1");
            fuseMapping.Add("b6p1_b6p4_a2", "b6p4_b6p1_a2");
            fuseMapping.Add("b6p1_b6p4_a3", "b6p4_b6p1_a3");
            fuseMapping.Add("b6p1_b6p4_a4", "b6p4_b6p1_a4");

            // b6p1 to b6p6
            fuseMapping.Add("b6p1_b6p6_a1", "b6p6_b6p1_a1");

            // b6p2 to b6p1
            fuseMapping.Add("b6p2_b6p1_a1", "b6p1_b6p2_a1");

            // b6p2 to b6p3
            fuseMapping.Add("b6p2_b6p3_a1", "b6p3_b6p2_a1");
            fuseMapping.Add("b6p2_b6p3_a2", "b6p3_b6p2_a2");
            fuseMapping.Add("b6p2_b6p3_a3", "b6p3_b6p2_a3");
            fuseMapping.Add("b6p2_b6p3_a4", "b6p3_b6p2_a4");

            // b6p2 to b6p5
            fuseMapping.Add("b6p2_b6p5_a1", "b6p5_b6p2_a1");

            // b6p3 to b6p2
            fuseMapping.Add("b6p3_b6p2_a1", "b6p2_b6p3_a1");
            fuseMapping.Add("b6p3_b6p2_a2", "b6p2_b6p3_a2");
            fuseMapping.Add("b6p3_b6p2_a3", "b6p2_b6p3_a3");
            fuseMapping.Add("b6p3_b6p2_a4", "b6p2_b6p3_a4");

            // b6p3 to b6p4
            fuseMapping.Add("b6p3_b6p4_a1", "b6p4_b6p3_a1");

            // b6p3 to b6p5
            fuseMapping.Add("b6p3_b6p5_a1", "b6p5_b6p3_a1");
            fuseMapping.Add("b6p3_b6p5_a2", "b6p5_b6p3_a2");
            fuseMapping.Add("b6p3_b6p5_a3", "b6p5_b6p3_a3");
            fuseMapping.Add("b6p3_b6p5_a4", "b6p5_b6p3_a4");
            fuseMapping.Add("b6p3_b6p5_a5", "b6p5_b6p3_a5");

            // b6p4 to b6p1
            fuseMapping.Add("b6p4_b6p1_a1", "b6p1_b6p4_a1");
            fuseMapping.Add("b6p4_b6p1_a2", "b6p1_b6p4_a2");
            fuseMapping.Add("b6p4_b6p1_a3", "b6p1_b6p4_a3");
            fuseMapping.Add("b6p4_b6p1_a4", "b6p1_b6p4_a4");

            // b6p4 to b6p3
            fuseMapping.Add("b6p4_b6p3_a1", "b6p3_b6p4_a1");

            // b6p4 to b6p6
            fuseMapping.Add("b6p4_b6p6_a1", "b6p6_b6p4_a1");
            fuseMapping.Add("b6p4_b6p6_a2", "b6p6_b6p4_a2");
            fuseMapping.Add("b6p4_b6p6_a3", "b6p6_b6p4_a3");
            fuseMapping.Add("b6p4_b6p6_a4", "b6p6_b6p4_a4");
            fuseMapping.Add("b6p4_b6p6_a5", "b6p6_b6p4_a5");

            // b6p5 to b6p2
            fuseMapping.Add("b6p5_b6p2_a1", "b6p2_b6p5_a1");

            // b6p5 to b6p3
            fuseMapping.Add("b6p5_b6p3_a1", "b6p3_b6p5_a1");
            fuseMapping.Add("b6p5_b6p3_a2", "b6p3_b6p5_a2");
            fuseMapping.Add("b6p5_b6p3_a3", "b6p3_b6p5_a3");
            fuseMapping.Add("b6p5_b6p3_a4", "b6p3_b6p5_a4");
            fuseMapping.Add("b6p5_b6p3_a5", "b6p3_b6p5_a5");

            // b6p5 to b6p6
            fuseMapping.Add("b6p5_b6p6_a1", "b6p6_b6p5_a1");
            fuseMapping.Add("b6p5_b6p6_a2", "b6p6_b6p5_a2");

            // b6p5 to bb6
            fuseMapping.Add("b6p5_bb6_a1", "bb6_b6p5_a1");
            fuseMapping.Add("b6p5_bb6_a2", "bb6_b6p5_a2");

            // b6p6 to b6p1
            fuseMapping.Add("b6p6_b6p1_a1", "b6p1_b6p6_a1");

            // b6p6 to b6p4
            fuseMapping.Add("b6p6_b6p4_a1", "b6p4_b6p6_a1");
            fuseMapping.Add("b6p6_b6p4_a2", "b6p4_b6p6_a2");
            fuseMapping.Add("b6p6_b6p4_a3", "b6p4_b6p6_a3");
            fuseMapping.Add("b6p6_b6p4_a4", "b6p4_b6p6_a4");
            fuseMapping.Add("b6p6_b6p4_a5", "b6p4_b6p6_a5");

            // b6p6 to b6p5
            fuseMapping.Add("b6p6_b6p5_a1", "b6p5_b6p6_a1");
            fuseMapping.Add("b6p6_b6p5_a2", "b6p5_b6p6_a2");

            // b6p6 to bb6
            fuseMapping.Add("b6p6_bb6_a1", "bb6_b6p6_a1");
            fuseMapping.Add("b6p6_bb6_a2", "bb6_b6p6_a2");

        }
        else if (currentScene.Equals("b7"))
        {
            // b7p1 to bb7
            fuseMapping.Add("b7p1_bb7_a1", "bb7_b7p1_a1");
            fuseMapping.Add("b7p1_bb7_a2", "bb7_b7p1_a2");
            fuseMapping.Add("b7p1_bb7_a3", "bb7_b7p1_a3");
            fuseMapping.Add("b7p1_bb7_a4", "bb7_b7p1_a4");

            // b7p1 to b7p2
            fuseMapping.Add("b7p1_b7p2_a1", "b7p2_b7p1_a1");
            fuseMapping.Add("b7p1_b7p2_a2", "b7p2_b7p1_a2");

            // b7p1 to b7p3
            fuseMapping.Add("b7p1_b7p3_a1", "b7p3_b7p1_a1");

            // b7p1 to b7p4
            fuseMapping.Add("b7p1_b7p4_a1", "b7p4_b7p1_a1");

            // b7p1 to b7p5
            fuseMapping.Add("b7p1_b7p5_a1", "b7p5_b7p1_a1");

            // b7p1 to b7p6
            fuseMapping.Add("b7p1_b7p6_a1", "b7p6_b7p1_a1");

            // b7p2 to bb7
            fuseMapping.Add("b7p2_bb7_a1", "bb7_b7p2_a1");
            fuseMapping.Add("b7p2_bb7_a2", "bb7_b7p2_a2");
            fuseMapping.Add("b7p2_bb7_a3", "bb7_b7p2_a3");

            // b7p2 to b7p1
            fuseMapping.Add("b7p2_b7p1_a1", "b7p1_b7p2_a1");
            fuseMapping.Add("b7p2_b7p1_a2", "b7p1_b7p2_a2");

            // b7p2 to b7p4
            fuseMapping.Add("b7p2_b7p4_a1", "b7p4_b7p2_a1");

            // b7p2 to b7p6
            fuseMapping.Add("b7p2_b7p6_a1", "b7p6_b7p2_a1");

            // b7p3 to bb7
            fuseMapping.Add("b7p3_bb7_a1", "bb7_b7p3_a1");
            fuseMapping.Add("b7p3_bb7_a2", "bb7_b7p3_a2");

            // b7p3 to b7p1
            fuseMapping.Add("b7p3_b7p1_a1", "b7p1_b7p3_a1");

            // b7p3 to b7p4
            fuseMapping.Add("b7p3_b7p4_a1", "b7p4_b7p3_a1");

            // b7p4 to bb7
            fuseMapping.Add("b7p4_bb7_a1", "bb7_b7p4_a1");

            // b7p4 to b7p1
            fuseMapping.Add("b7p4_b7p1_a1", "b7p1_b7p4_a1");
            fuseMapping.Add("b7p4_b7p1_a2", "b7p1_b7p4_a2");

            // b7p4 to b7p2
            fuseMapping.Add("b7p4_b7p2_a1", "b7p2_b7p4_a1");

            // b7p4 to b7p3
            fuseMapping.Add("b7p4_b7p3_a1", "b7p3_b7p4_a1");

            // b7p4 to b7p5
            fuseMapping.Add("b7p4_b7p5_a1", "b7p5_b7p4_a1");

            // b7p4 to b7p6
            fuseMapping.Add("b7p4_b7p6_a1", "b7p6_b7p4_a1");

            // b7p5 to bb7
            fuseMapping.Add("b7p5_bb7_a1", "bb7_b7p5_a1");

            // b7p5 to b7p1
            fuseMapping.Add("b7p5_b7p1_a1", "b7p1_b7p5_a1");

            // b7p5 to b7p4
            fuseMapping.Add("b7p5_b7p4_a1", "b7p4_b7p5_a1");

            // b7p5 to b7p6
            fuseMapping.Add("b7p5_b7p6_a1", "b7p6_b7p5_a1");

            // b7p6 to b7p1
            fuseMapping.Add("b7p6_b7p1_a1", "b7p1_b7p6_a1");

            // b7p6 to b7p2
            fuseMapping.Add("b7p6_b7p2_a1", "b7p2_b7p6_a1");

            // b7p6 to b7p4
            fuseMapping.Add("b7p6_b7p4_a1", "b7p4_b7p6_a1");

            // b7p6 to b7p5
            fuseMapping.Add("b7p6_b7p5_a1", "b7p5_b7p6_a1");

        }
        else if (currentScene.Equals("b8"))
        {
            // b8p1 to bb8
            fuseMapping.Add("b8p1_bb8_a1", "bb8_b8p1_a1");

            // b8p1 to b8p2
            fuseMapping.Add("b8p1_b8p2_a1", "b8p2_b8p1_a1");
            fuseMapping.Add("b8p1_b8p2_a2", "b8p2_b8p1_a2");
            fuseMapping.Add("b8p1_b8p2_a3", "b8p2_b8p1_a3");
            fuseMapping.Add("b8p1_b8p2_a4", "b8p2_b8p1_a4");

            // b8p1 to b8p5
            fuseMapping.Add("b8p1_b8p5_a1", "b8p5_b8p1_a1");
            fuseMapping.Add("b8p1_b8p5_a2", "b8p5_b8p1_a2");
            fuseMapping.Add("b8p1_b8p5_a3", "b8p5_b8p1_a3");

            // b8p2 to bb8
            fuseMapping.Add("b8p2_bb8_a1", "bb8_b8p2_a1");
            fuseMapping.Add("b8p2_bb8_a2", "bb8_b8p2_a2");
            fuseMapping.Add("b8p2_bb8_a3", "bb8_b8p2_a3");
            fuseMapping.Add("b8p2_bb8_a4", "bb8_b8p2_a4");
            fuseMapping.Add("b8p2_bb8_a5", "bb8_b8p2_a5");
            fuseMapping.Add("b8p2_bb8_a6", "bb8_b8p2_a6");

            // b8p2 to b8p1
            fuseMapping.Add("b8p2_b8p1_a1", "b8p1_b8p2_a1");
            fuseMapping.Add("b8p2_b8p1_a2", "b8p1_b8p2_a2");
            fuseMapping.Add("b8p2_b8p1_a3", "b8p1_b8p2_a3");
            fuseMapping.Add("b8p2_b8p1_a4", "b8p1_b8p2_a4");

            // b8p3 to bb8
            fuseMapping.Add("b8p3_bb8_a1", "bb8_b8p3_a1");
            fuseMapping.Add("b8p3_bb8_a2", "bb8_b8p3_a2");
            fuseMapping.Add("b8p3_bb8_a3", "bb8_b8p3_a3");

            // b8p3 to b8p4
            fuseMapping.Add("b8p3_b8p4_a1", "b8p4_b8p3_a1");

            // b8p4 to bb8
            fuseMapping.Add("b8p4_bb8_a1", "bb8_b8p4_a1");

            // b8p4 to b8p3
            fuseMapping.Add("b8p4_b8p3_a1", "b8p3_b8p4_a1");

            // b8p4 to b8p6
            fuseMapping.Add("b8p4_b8p6_a1", "b8p6_b8p4_a1");
            fuseMapping.Add("b8p4_b8p6_a2", "b8p6_b8p4_a2");
            fuseMapping.Add("b8p4_b8p6_a3", "b8p6_b8p4_a3");

            // b8p5 to bb8
            fuseMapping.Add("b8p5_bb8_a1", "bb8_b8p5_a1");

            // b8p5 to b8p1
            fuseMapping.Add("b8p5_b8p1_a1", "b8p1_b8p5_a1");
            fuseMapping.Add("b8p5_b8p1_a2", "b8p1_b8p5_a2");
            fuseMapping.Add("b8p5_b8p1_a3", "b8p1_b8p5_a3");

            // b8p6 to b8p4
            fuseMapping.Add("b8p6_b8p4_a1", "b8p4_b8p6_a1");
            fuseMapping.Add("b8p6_b8p4_a2", "b8p4_b8p6_a2");
            fuseMapping.Add("b8p6_b8p4_a3", "b8p4_b8p6_a3");


        }
        else if (currentScene.Equals("sledgehammer"))
        {
            // bottom_point_left to head
            fuseMapping.Add("bottom_point_left_head_attach", "head_bottom_point_left_attach");

            // bottom_point left to bottom_point_right
            fuseMapping.Add("bottom_point_left_right_attach", "bottom_point_right_left_attach");

            // bottom_point_right to head
            fuseMapping.Add("bottom_point_right_head_attach", "head_bottom_point_right_attach");

            // bottom_point_right to bottom_point_left
            fuseMapping.Add("bottom_point_right_left_attach", "bottom_point_left_right_attach");

            // haft to shaft
            fuseMapping.Add("haft_shaft_attach", "shaft_haft_attach");

            // head to trapezoid_harder
            fuseMapping.Add("head_trapezoid_attach", "trapezoid_head_attach");

            // small_tip to small_trapezoid
            fuseMapping.Add("small_tip_small_trapezoid_attach", "small_trapezoid_small_tip_attach");

            // small_trapezoid to shaft
            fuseMapping.Add("small_trapezoid_shaft_attach", "shaft_small_trapezoid_attach");

            // spike to shaft
            fuseMapping.Add("spike_shaft_attach", "shaft_spike_attach");

            // tip to head
            fuseMapping.Add("tip_head_attach", "head_tip_attach");

            // top_point_left to head
            fuseMapping.Add("top_point_left_head_attach", "head_top_point_left_attach");

            // top_point_left to top_point_right
            fuseMapping.Add("top_point_left_right_attach", "top_point_right_left_attach");

            // top_point_right to head
            fuseMapping.Add("top_point_right_head_attach", "head_top_point_right_attach");

            // top_point_right to top_point_left
            fuseMapping.Add("top_point_right_left_attach", "top_point_left_right_attach");

            // trapezoid to head
            fuseMapping.Add("trapezoid_head_attach", "head_trapezoid_attach");

            // trapezoid to shaft
            fuseMapping.Add("trapezoid_shaft_attach", "shaft_trapezoid_attach");

        }
        else if (currentScene.Equals("key1"))
        {
            //TODO: This Construction Mode level hasn't been implemented yet for this version of the game
            /*	HashSet<string> ULDTSet = new HashSet<string>();
                ULDTSet.Add ("dangly_T_upright_L_attach");
                fuseMapping.Add ("upright_L_dangly_T_attach", ULDTSet);
                HashSet<string> ULWSet = new HashSet<string>();
                ULWSet.Add ("upright_L_waluigi_attach");
                fuseMapping.Add ("waluigi_upright_L_attach", ULWSet);

                HashSet<string> UTDTSet = new HashSet<string>();
                UTDTSet.Add ("dangly_T_upright_T_attach");
                fuseMapping.Add ("upright_T_dangly_T_attach", UTDTSet);

                HashSet<string> WPDTSet = new HashSet<string>();
                WPDTSet.Add ("dangly_T_walking_pants_attach");
                fuseMapping.Add ("walking_pants_dangly_T_attach", WPDTSet);

                HashSet<string> URWPSet = new HashSet<string>();
                URWPSet.Add ("walking_pants_upright_rect_attach");
                fuseMapping.Add ("upright_rect_walking_pants_attach", URWPSet);
            */
        }
 
        //else if (currentScene.Equals("ffa"))
        //{
            //TODO: This scene hasn't been implemented yet in this version of the game
            //ring large part
            /*	HashSet<string> fuseToForRingLargePartSide = new HashSet<string>();
                HashSet<string> fuseToForRingLargePartBack = new HashSet<string>();
                HashSet<string> fuseToForRingLargePartLong = new HashSet<string>();
                HashSet<string> fuseToForRingLargePartSmall = new HashSet<string>();
                fuseToForRingLargePartSide.Add ("center_box_ring_large_part_left_attach");
                fuseToForRingLargePartBack.Add ("center_box_ring_large_part_attach");
                fuseToForRingLargePartLong.Add ("ring_long_part_ring_large_part_attach");
                fuseToForRingLargePartSmall.Add ("ring_small_part_ring_large_part_attach");
                fuseMapping.Add ("ring_large_part_center_box_side_attach", fuseToForRingLargePartSide);
                fuseMapping.Add ("ring_large_part_center_box_back_attach", fuseToForRingLargePartBack);
                fuseMapping.Add ("ring_large_part_ring_long_part_attach", fuseToForRingLargePartLong);
                fuseMapping.Add ("ring_large_part_ring_small_part_attach", fuseToForRingLargePartSmall);

                //ring long part
                HashSet<string> fuseToLongPart1 = new HashSet<string>();
                HashSet<string> fuseToLongPart2 = new HashSet<string>();
                HashSet<string> fuseToLongPart3 = new HashSet<string>();
                HashSet<string> fuseToLongPart4 = new HashSet<string>();
                fuseToLongPart1.Add ("center_box_ring_long_part_attach");
                fuseToLongPart2.Add ("center_tri_ring_long_part_attach");
                fuseToLongPart3.Add ("ring_large_part_ring_long_part_attach");
                fuseToLongPart4.Add ("ring_small_part_ring_long_part_attach");
                fuseMapping.Add ("ring_long_part_center_box_attach", fuseToLongPart1);
                fuseMapping.Add ("ring_long_part_center_tri_attach", fuseToLongPart2);
                fuseMapping.Add ("ring_long_part_ring_large_part_attach", fuseToLongPart3);
                fuseMapping.Add ("ring_long_part_ring_small_part_attach", fuseToLongPart4);

                //ring small part
                HashSet<string> fuseToSmallPart1 = new HashSet<string>();
                HashSet<string> fuseToSmallPart2 = new HashSet<string>();
                HashSet<string> fuseToSmallPart3 = new HashSet<string>();
                HashSet<string> fuseToSmallPart4 = new HashSet<string>();
                fuseToSmallPart1.Add ("center_box_ring_small_part_attach");
                fuseToSmallPart2.Add ("right_tri_ring_small_part_attach");
                fuseToSmallPart3.Add ("ring_large_part_ring_small_part_attach");
                fuseToSmallPart4.Add ("ring_long_part_ring_small_part_attach");
                fuseMapping.Add ("ring_small_part_center_box_attach", fuseToSmallPart1);
                fuseMapping.Add ("ring_small_part_right_tri_attach", fuseToSmallPart2);
                fuseMapping.Add ("ring_small_part_ring_large_part_attach", fuseToSmallPart3);
                fuseMapping.Add ("ring_small_part_ring_long_part_attach", fuseToSmallPart4);

                //handle top
                HashSet<string> fuseToForHandleTop1 = new HashSet<string>();
                HashSet<string> fuseToForHandleTop2 = new HashSet<string>();
                fuseToForHandleTop1.Add ("center_box_handle_top_attach");
                fuseToForHandleTop2.Add ("handle_bottom_handle_top_attach");
                fuseMapping.Add ("handle_top_center_box_attach", fuseToForHandleTop1);
                fuseMapping.Add ("handle_top_handle_bottom_attach", fuseToForHandleTop2);

                //handle bottom
                HashSet<string> fuseToForHandleBottom1 = new HashSet<string>();
                HashSet<string> fuseToForHandleBottom2 = new HashSet<string>();
                fuseToForHandleBottom1.Add ("center_box_handle_bottom_attach");
                fuseToForHandleBottom2.Add ("handle_top_handle_bottom_attach");
                fuseMapping.Add ("handle_bottom_center_box_attach", fuseToForHandleBottom1);
                fuseMapping.Add ("handle_bottom_handle_top_attach", fuseToForHandleBottom2);

                //centerTri
                HashSet<string> fuseToForCenterTri1 = new HashSet<string>();
                HashSet<string> fuseToForCenterTri2 = new HashSet<string>();
                HashSet<string> fuseToForCenterTri3 = new HashSet<string>();
                fuseToForCenterTri1.Add ("ring_long_part_center_tri_attach");
                fuseToForCenterTri2.Add ("blue_tri_back_center_tri_attach");
                fuseToForCenterTri3.Add ("blue_tri_side_center_tri_attach");
                fuseMapping.Add ("center_tri_ring_long_part_attach", fuseToForCenterTri1);
                fuseMapping.Add ("center_tri_blue_tri_back_attach", fuseToForCenterTri2);
                fuseMapping.Add ("center_tri_blue_tri_side_attach", fuseToForCenterTri3);

                //leftTri
                HashSet<string> fuseToForLeftTri1 = new HashSet<string>();
                HashSet<string> fuseToForLeftTri2 = new HashSet<string>();
                HashSet<string> fuseToForLeftTri3 = new HashSet<string>();
                fuseToForLeftTri1.Add ("ring_large_part_tri_attach");
                fuseToForLeftTri2.Add ("scalene_left_tri_back_attach");
                fuseToForLeftTri3.Add ("scalene_left_tri_side_attach");
                fuseMapping.Add ("left_tri_ring_large_part_attach", fuseToForLeftTri1);
                fuseMapping.Add ("left_tri_scalene_back_attach", fuseToForLeftTri2);
                fuseMapping.Add ("left_tri_scalene_side_attach", fuseToForLeftTri3);

                //rightTri
                HashSet<string> fuseToForRightTri1 = new HashSet<string>();
                HashSet<string> fuseToForRightTri2 = new HashSet<string>();
                HashSet<string> fuseToForRightTri3 = new HashSet<string>();
                HashSet<string> fuseToForRightTri4 = new HashSet<string>();

                fuseToForRightTri1.Add ("ring_small_part_right_tri_attach");
                fuseToForRightTri2.Add ("right_tri_chunk_right_tri_angle_attach");
                fuseToForRightTri3.Add ("right_tri_chunk_right_tri_back_attach");
                fuseToForRightTri4.Add ("right_tri_chunk_right_tri_side_attach");
                fuseMapping.Add ("right_tri_ring_small_part_attach", fuseToForRightTri1);
                fuseMapping.Add ("right_tri_right_tri_chunk_angle_attach", fuseToForRightTri2);
                fuseMapping.Add ("right_tri_right_tri_chunk_back_attach", fuseToForRightTri3);
                fuseMapping.Add ("right_tri_right_tri_chunk_side_attach", fuseToForRightTri4);

                //blueTri
                HashSet<string> fuseToForBlueTri1 = new HashSet<string>();
                HashSet<string> fuseToForBlueTri2 = new HashSet<string>();
                fuseToForBlueTri1.Add ("center_tri_blue_tri_back_attach");
                fuseToForBlueTri2.Add ("center_tri_blue_tri_side_attach");
                fuseMapping.Add ("blue_tri_center_tri_back_attach", fuseToForBlueTri1);
                fuseMapping.Add ("blue_tri_center_tri_side_attach", fuseToForBlueTri2);

                //rightTriChunk
                HashSet<string> fuseToForRightTriChunk1 = new HashSet<string>();
                HashSet<string> fuseToForRightTriChunk2 = new HashSet<string>();
                HashSet<string> fuseToForRightTriChunk3 = new HashSet<string>();
                fuseToForRightTriChunk1.Add ("right_tri_right_tri_chunk_back_attach");
                fuseToForRightTriChunk2.Add ("right_tri_right_tri_chunk_side_attach");
                fuseToForRightTriChunk3.Add ("right_tri_right_tri_chunk_angle_attach");
                fuseMapping.Add ("right_tri_chunk_right_tri_back_attach", fuseToForRightTriChunk1);
                fuseMapping.Add ("right_tri_chunk_right_tri_side_attach", fuseToForRightTriChunk2);
                fuseMapping.Add ("right_tri_chunk_right_tri_angle_attach", fuseToForRightTriChunk3);

                //Scalene
                HashSet<string> fuseToForScalene1 = new HashSet<string>();
                HashSet<string> fuseToForScalene2 = new HashSet<string>();
                fuseToForScalene1.Add ("left_tri_scalene_back_attach");
                fuseToForScalene2.Add ("left_tri_scalene_side_attach");
                fuseMapping.Add ("scalene_left_tri_back_attach", fuseToForScalene1);
                fuseMapping.Add ("scalene_left_tri_side_attach", fuseToForScalene2);
                */
        //}
 
        //else if (currentScene.Equals("key2"))
        //{
            //TODO: this level hasn't been implemented in this version of the game yet
            //c, hanging l, middle t, ul corner, zigzag
            /*	
                //c
                HashSet<string> fuseToForCBottom = new HashSet<string>();
                HashSet<string> fuseToForCFront = new HashSet<string>();
                HashSet<string> fuseToForCTop = new HashSet<string>();
                HashSet<string> fuseToForCUlCorner = new HashSet<string>();
                fuseToForCBottom.Add ("middle_t_c_bottom_attach");
                fuseToForCFront.Add ("middle_t_c_front_attach");
                fuseToForCTop.Add ("middle_t_c_top_attach");
                fuseToForCUlCorner.Add ("middle_t_ul_corner_attach");
                fuseMapping.Add ("c_middle_t_bottom_attach", fuseToForCBottom);
                fuseMapping.Add ("c_middle_t_front_attach", fuseToForCFront);
                fuseMapping.Add ("c_middle_t_top_attach", fuseToForCTop);
                fuseMapping.Add ("c_middle_ul_corner_attach", fuseToForCUlCorner);

                //hanging l
                HashSet<string> fuseToforHangingL = new HashSet<string>();
                fuseToforHangingL.Add ("post_hanging_l_attach");
                fuseMapping.Add ("hanging_l_post_attach", fuseToforHangingL);

                //middle t
                HashSet<string> fuseToForMiddleTTop = new HashSet<string>();
                HashSet<string> fuseToForMiddleTBottom = new HashSet<string>();
                HashSet<string> fuseToForMiddleTFront = new HashSet<string>();
                HashSet<string> fuseToForMiddleTPost = new HashSet<string>();
                fuseToForMiddleTTop.Add ("c_middle_t_top_attach");
                fuseToForMiddleTBottom.Add ("c_middle_t_bottom_attach");
                fuseToForMiddleTFront.Add ("c_middle_t_front_attach");
                fuseToForMiddleTPost.Add ("post_middle_t_attach");
                fuseMapping.Add ("middle_t_c_top_attach", fuseToForMiddleTTop);
                fuseMapping.Add ("middle_t_c_bottom_attach", fuseToForMiddleTBottom);
                fuseMapping.Add ("middle_t_c_front_attach", fuseToForMiddleTFront);
                fuseMapping.Add ("middle_t_post_attach", fuseToForMiddleTPost);

                //ul corner
                HashSet<string> fuseToForUlCorner = new HashSet<string>();
                fuseToForUlCorner.Add ("c_ul_corner_attach");
                fuseMapping.Add ("ul_corner_c_attach", fuseToForUlCorner);

                //zigzag
                HashSet<string> fuseToForZigzag = new HashSet<string>();
                fuseToForZigzag.Add ("post_zigzag_attach");
                fuseMapping.Add ("zigzag_post_attach", fuseToForZigzag);
            */
        //}
  
        //else if (currentScene.Equals("key3"))
        //{
            //TODO: This level hasn't been implemented in this version of the game yet
            /*
                        HashSet<string> fuseToForLongLBack = new HashSet<string>();
                        HashSet<string> fuseToForLongLSide = new HashSet<string>();
                        HashSet<string> fuseToForLongLTop = new HashSet<string>();
                        HashSet<string> fuseToForLongLCorner = new HashSet<string>();

                        fuseToForLongLBack.Add("block_juts_long_l_back_attach");
                        fuseToForLongLSide.Add("block_juts_long_l_side_attach");
                        fuseToForLongLTop.Add("block_juts_long_l_top_attach");
                        fuseToForLongLCorner.Add ("corner_long_l_attach");

                        fuseMapping.Add("long_l_block_juts_back_attach",fuseToForLongLBack);
                        fuseMapping.Add("long_l_block_juts_side_attach",fuseToForLongLSide);
                        fuseMapping.Add("long_l_block_juts_top_attach",fuseToForLongLTop);
                        fuseMapping.Add("long_l_corner_attach",fuseToForLongLCorner);


                        HashSet<string> fuseToForConnectorCorner = new HashSet<string>();
                        HashSet<string> fuseToForConnectorDiagonalSide = new HashSet<string>();
                        HashSet<string> fuseToForConnectorDiagonalTop = new HashSet<string>();
                        fuseToForConnectorCorner.Add("corner_connector_attach");
                        fuseToForConnectorDiagonalSide.Add("diagonal_connector_side_attach");
                        fuseToForConnectorDiagonalTop.Add("diagonal_connector_top_attach");
                        fuseMapping.Add("connector_corner_attach",fuseToForConnectorCorner);
                        fuseMapping.Add("connector_diagonal_side_attach",fuseToForConnectorDiagonalSide);
                        fuseMapping.Add("connector_diagonal_top_attach",fuseToForConnectorDiagonalTop);

                        HashSet<string> fuseToForBigCornerLongL = new HashSet<string>();
                        fuseToForBigCornerLongL.Add("long_l_big_corner_attach");
                        fuseMapping.Add("big_corner_long_l_attach",fuseToForBigCornerLongL);

                        HashSet<string> fuseToForCornerLongL = new HashSet<string>();
                        HashSet<string> fuseToForCornerBlockJuts = new HashSet<string>();
                        HashSet<string> fuseToForCornerConnector = new HashSet<string>();
                        fuseToForCornerLongL.Add("long_l_corner_attach");
                        fuseToForCornerBlockJuts.Add("block_juts_corner_attach");
                        fuseToForCornerConnector.Add("connector_corner_attach");
                        fuseMapping.Add("corner_long_l_attach",fuseToForCornerLongL);
                        fuseMapping.Add("corner_block_juts_attach",fuseToForCornerBlockJuts);
                        fuseMapping.Add("corner_connector_attach",fuseToForCornerConnector);

                        HashSet<string> fuseToForDiagonalConnectorSide = new HashSet<string>();
                        HashSet<string> fuseToForDiagonalConnectorTop = new HashSet<string>();
                        fuseToForDiagonalConnectorSide.Add("connector_diagonal_side_attach");
                        fuseToForDiagonalConnectorTop.Add("connector_diagonal_top_attach");
                        fuseMapping.Add("diagonal_connector_side_attach",fuseToForDiagonalConnectorSide);
                        fuseMapping.Add("diagonal_connector_top_attach",fuseToForDiagonalConnectorTop);
                        */
        //}
     
		
	}

	public void startLevelTimer() {
		levelTimer = Time.time;
		
	}
	
	public void stopLevelTimer() {
		levelTimer = Time.time - levelTimer;
	}

	public void disableConnectButton() {
		connectButton.interactable = false;
	}

    // if in debug mode, do auto-victory. If not, proceed as usual, checking for fuse errors or successes
    // Uncomment the print statements below to assist with debugging a new Construction Mode level
	public void initiateFuse(bool debugMode) {
		numFuseAttempts++;
		//print ("Fusing: " + GetComponent<SelectPart>().getSelectedObject() + " to " + GetComponent<SelectPart>().getSelectedFuseTo());
		selectedObject = GetComponent<SelectPart>().getSelectedObject();
		selectedFuseTo = GetComponent<SelectPart>().getSelectedFuseTo();
		//print ("fuseMapping.ContainsKey(" + selectedObject.name + ")?");
		//print ("In initiateFuse(): selectedObject = " + selectedObject);
		//print ("SelectedObject: " + selectedObject.name + ", SelectedFuseTo: " + selectedFuseTo.name);
		//print ("SelectedObjectParent: " + selectedObjectParent.name + ", SelectedFuseToParent: " + selectedFuseToParent.name);
		//print ("fuseMapping[" + selectedObject.name + "] = " + fuseMapping[selectedObject.name]);
		//foreach(string s in fuseMapping[selectedObject.name]) {
		//	print (s);
		//}
		if(!debugMode && selectedObject == null) {
			//player tries to connect when there is no active part (only at beginning)
			//print ("Select the black regions you want to join together!");
            //UPDATE: this can't happen if the Fuse button is disabled until two faces are selected
			source.PlayOneShot (failure);

		} else if (!debugMode && !fuseMapping.ContainsKey (selectedObject.name)){
            // this should only happen if the fuseMapping wasn't properly assigned at the beginning of the level
			//print ("Invalid fuse: Cannot fuse " + selectedObject.name + " to " + selectedFuseTo.name);
			//display error on screen for 1 sec
			StartCoroutine(errorWrongFace());

		} else if(debugMode || (!debugMode && fuseMapping[selectedObject.name].Contains(selectedFuseTo.name) && positionMatches (selectedObject, selectedFuseTo))) {
	
			//print ("Successful fuse!");
			fuseStatus="success";
            if(!debugMode && dataManager)
            {
                dataManager.AddPartFused(selectedObject.transform.parent.gameObject.name);
            }
			source.PlayOneShot (success);

            if (!debugMode)
            {
                //Check if FaceSelector is still adjusting part location. If so, abort adjustment and then just fuse
                Coroutine currentlyActiveMovement = selectedObject.GetComponent<FaceSelector>().getCurrentlyActiveCoroutine();
                if (currentlyActiveMovement != null)
                {
                    StopCoroutine(currentlyActiveMovement);
                }
                selectedObject.GetComponent<FuseBehavior>().fuse(selectedFuseTo.name);

                // delete starting part's old BoxCollider, replace with a new one combining the old one's bounds with those of 
                // the part that was just attached
                extendBoxCollider(GetComponent<SelectPart>().startingPart, selectedObject.transform.parent.gameObject);
            }
  

			fuseCleanUp();

            if(!debugMode)
            {
                fuseCount++;
            }

            if (!debugMode && isFirstLevel && !firstFuseComplete)
            {
                firstFuseComplete = true;
                ConversationTrigger.AddToken("finishedFirstFuse");
            }
			if(done()) {
				stopLevelTimer();

                group = Instantiate(victoryPrefab, new Vector3(-100, 30, 100), new Quaternion());
                //to make sure it's created as part of the current construction mode scene
                LoadUtils.InstantiateParenter(group);

                // Make sure every part in each level is tagged "part"
                GameObject[] allParts = GameObject.FindGameObjectsWithTag("part");
                for (int i = 0; i < allParts.Length; i++)
                {
                    allParts[i].transform.SetParent(group.transform, true);
                   // Debug.Log("Setting " + group + " as parent of " + allParts[i]);
                }

                if (dataManager != null)
                {
                    dataManager.SetOutcome("victory");
                }

                if (GameObject.Find("TimeRemainingPanel") != null)
                {
                    GameObject.Find("TimeRemainingPanel").GetComponent<Timer>().stopTimer();
                }
				bottomPanelGroup.alpha = 0;
                congratsPanel.SetActive(true);
				finishedImage.enabled = false;
                if (isFirstLevel)
                {
                    ConversationTrigger.AddToken("finishedB1");
                }

                musicsource.clip = victorymusic;
                mainCam.transform.position = new Vector3(-90, 80, -3.36f);
                mainCam.transform.rotation = Quaternion.Euler(new Vector3(15, 0, 0));
                //Debug.Log("Playing " + musicsource.clip.name + "!");

                musicsource.Play();
                StartCoroutine(FadeAudio(fadeTime, Fade.Out));

                string currentLevel = LoadUtils.currentSceneName;

                //CHANGE this to add new levels
                if (currentLevel.Equals("b1"))
                {
                    ConversationTrigger.AddToken("finished_b1");
                    ConversationTrigger.AddToken("not_finished_collecting_b2");
                    ConversationTrigger.RemoveToken("battery_const_in_progress");
                }
                else if (currentLevel.Equals("b2"))
                {
                    ConversationTrigger.AddToken("finished_b2");
                    ConversationTrigger.AddToken("not_finished_collecting_b3");
                    Debug.Log("Adding finished_b2 token!");
                    ConversationTrigger.RemoveToken("battery_const_in_progress");


                }
                else if (currentLevel.Equals("b3"))
                {
                    ConversationTrigger.AddToken("finished_b3");
                    ConversationTrigger.AddToken("not_finished_collecting_b4");
                    ConversationTrigger.RemoveToken("battery_const_in_progress");

                }
                else if (currentLevel.Equals("b4"))
                {
                    ConversationTrigger.AddToken("finished_b4");
                    ConversationTrigger.AddToken("not_finished_collecting_rocketBoots");
                    ConversationTrigger.AddToken("not_finished_const_map_intro");
                    Debug.Log("Added not_finished_const_map_intro token");

                }
                else if (currentLevel.Equals("rocketBoots"))
                {
                    ConversationTrigger.AddToken("finished_rocketBoots");
                    ConversationTrigger.AddToken("not_finished_collecting_b5");
                    ConversationTrigger.RemoveToken("item_const_in_progress");

                }
                else if (currentLevel.Equals("b5"))
                {
                    ConversationTrigger.AddToken("finished_b5");
                    ConversationTrigger.AddToken("not_finished_collecting_b6");
                    ConversationTrigger.RemoveToken("battery_const_in_progress");

                }
                else if (currentLevel.Equals("b6"))
                {
                    ConversationTrigger.AddToken("finished_b6");
                    ConversationTrigger.AddToken("not_finished_collecting_b7");
                    ConversationTrigger.RemoveToken("battery_const_in_progress");

                }
                else if (currentLevel.Equals("b7"))
                {
                    ConversationTrigger.AddToken("finished_b7");
                    ConversationTrigger.AddToken("not_finished_collecting_b8");
                    ConversationTrigger.RemoveToken("battery_const_in_progress");

                }
                else if (currentLevel.Equals("b8"))
                {
                    ConversationTrigger.AddToken("finished_b8");
                    ConversationTrigger.AddToken("not_finished_collecting_sledgehammer");
                    ConversationTrigger.AddToken("not_finished_const_map_intro");
                    // we don't remove the battery_const_in_progress token here since the map intro
                    // in this level still has to be completed in order for this level to be "finished"
                }
                else if (currentLevel.Equals("sledgehammer"))
                {
                    ConversationTrigger.AddToken("finished_sledgehammer");
                    ConversationTrigger.AddToken("not_finished_collecting_key1");
                    ConversationTrigger.RemoveToken("item_const_in_progress");

                }
                else if (currentLevel.Equals("key1"))
                {
                    ConversationTrigger.AddToken("finished_key1");
                }

                claimItem.gameObject.SetActive(true);

            }



        } else if (!fuseMapping[selectedObject.name].Contains (selectedFuseTo.name)) {
			print ("Invalid fuse: Cannot fuse " + selectedObject.name + " to " + selectedFuseTo.name);
			StartCoroutine(errorWrongFace());
            if (dataManager)
            {
                dataManager.AddFaceError();
            }
		} else if (fuseMapping[selectedObject.name].Contains (selectedFuseTo.name) && !positionMatches (selectedObject, selectedFuseTo)){
			//rotation isn't right - tell player this or let them figure it out themselves?
			StartCoroutine(errorWrongRotation());
			print ("Invalid fuse: Correct fuse selection, but the orientation isn't right!");
            if (dataManager)
            {
                dataManager.AddRotateError();
            }
		} else {
			//this shouldn't happen
			print ("MYSTERIOUS FUSE ERROR");
		}

	}

    // combines the BoxColliders of GameObjects starting and added and replaces
    // starting's BoxCollider with the result
    // This code was used in an older version of the game that auto-aligned parts with selected faces
    // so that they would be moved in front of wherever the selected FuseTo face was to help players understand
    // how the part needed to be rotated to align. It worked ok until some of the newer Construction Mode levels
    // were implemented, broke on these, and I couldn't fix it, so I just disabled auto-align.
    private void extendBoxCollider(GameObject starting, GameObject added)
    {

        BoxCollider startingBoxCollider = starting.GetComponent<BoxCollider>();
        BoxCollider addedBoxCollider = added.GetComponent<BoxCollider>();
        startingBoxCollider.enabled = true;
        addedBoxCollider.enabled = true;
        Bounds oldBounds = starting.GetComponent<BoxCollider>().bounds;
        Bounds addedBounds = added.GetComponent<BoxCollider>().bounds;
        addedBoxCollider.enabled = false;

        Vector3 oldBoundsMax = oldBounds.max;
        Vector3 oldBoundsMin = oldBounds.min;
        Vector3 addedBoundsMax = addedBounds.max;
        Vector3 addedBoundsMin = addedBounds.min;

        float newXMax, newYMax, newZMax;
        float newXMin, newYMin, newZMin;
        newXMax = Mathf.Max(oldBoundsMax.x, addedBoundsMax.x);
        newYMax = Mathf.Max(oldBoundsMax.y, addedBoundsMax.y);
        newZMax = Mathf.Max(oldBoundsMax.z, addedBoundsMax.z);

        newXMin = Mathf.Min(oldBoundsMin.x, addedBoundsMin.x);
        newYMin = Mathf.Min(oldBoundsMin.y, addedBoundsMin.y);
        newZMin = Mathf.Min(oldBoundsMin.z, addedBoundsMin.z);

        Vector3 newCenter = starting.transform.InverseTransformPoint((newXMax + newXMin) / 2, (newYMax + newYMin) / 2, (newZMax + newZMin) / 2);
        Vector3 startingPos = starting.transform.position;

        Vector3 startingScale = starting.transform.lossyScale;
        Vector3 startingSize = startingBoxCollider.size;
        Vector3 addedSize = addedBoxCollider.size;

        //I got this formula from engineeringcorecourses.com
        float startingVolume = startingBoxCollider.size.x * startingBoxCollider.size.y * startingBoxCollider.size.z;
        float addedVolume = addedBoxCollider.size.x * addedBoxCollider.size.y * addedBoxCollider.size.z;
        float totalVolume = startingVolume + addedVolume;

        Vector3 startingColliderWorldCenter = starting.transform.TransformPoint(startingBoxCollider.center);
        Vector3 addedColliderWorldCenter = added.transform.TransformPoint(startingBoxCollider.center);

        float weightedCenterX = startingColliderWorldCenter.x * startingVolume + addedColliderWorldCenter.x * addedVolume;
        float weightedCenterY = startingColliderWorldCenter.y * startingVolume + addedColliderWorldCenter.y * addedVolume;
        float weightedCenterZ = startingColliderWorldCenter.z * startingVolume + addedColliderWorldCenter.z * addedVolume;

        //Vector3 newCenter = new Vector3(weightedCenterX / totalVolume, weightedCenterY / totalVolume, weightedCenterZ / totalVolume);
        Vector3 newCenterLocalCoord = starting.transform.InverseTransformPoint(newCenter);
        // TODO: center formula is slightly off - moves too far in -z direction as shapes are added in -z direction, doesn't move far enough 
        // in +y direction when shapes are added in +y direction

        startingBoxCollider.size = new Vector3((newXMax - newXMin) / startingScale.x, (newYMax - newYMin) / startingScale.y, (newZMax - newZMin) / startingScale.z);
        //Debug.Log("New Size: " + startingBoxCollider.size);

        // sum of the volumes times the centers of starting and added, divided by sum of the volumes
        //startingBoxCollider.center = newCenterLocalCoord;
        startingBoxCollider.center = newCenter;
        //Debug.Log("New Center: " + startingBoxCollider.center);


        startingBoxCollider.enabled = false;

    }

    IEnumerator errorWrongFace()
    {
        fuseStatus = "wrongFace";
        numWrongFacesFails++;
        errorPanelGroup.alpha = 1;
        shapesWrong.enabled = true;
        source.clip = failure;
        source.Play();
        yield return new WaitForSeconds(1f);
        shapesWrong.enabled = false;
        errorPanelGroup.alpha = 0;
    }

    IEnumerator errorWrongRotation()
    {
        fuseStatus = "wrongRotation";
        numWrongRotationFails++;
        errorPanelGroup.alpha = 1;
        rotationWrong.enabled = true;
        source.clip = failure;
        source.Play();
        yield return new WaitForSeconds(1f);
        rotationWrong.enabled = false;
        errorPanelGroup.alpha = 0;
    }

	private void rotateConstruction() {

		group.transform.Rotate (0,50*Time.deltaTime,0);

	}

    IEnumerator FadeAudio(float timer, Fade fadeType)
    {
        float start, end;
        if (fadeType == Fade.In)
        {
            start = 0.0F;
            end = 1.0F;
        }
        else
        {
            start = 1.0F;
            end = 0.0F;
        }
        float i = 0.0F;
        float step = 1.0F / timer;

        while (i <= 1.0F)
        {
            i += step * Time.deltaTime;
            musicsource.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
    }

    //remove old arrows from fused part and unselect fused parts
    public void fuseCleanUp() {
		// Disable rotation gizmo.
		rotateGizmo.Disable();

		//Unselect and unghost the attached fuseTo and active part
		GetComponent<SelectPart>().resetSelectedObject();
		GetComponent<SelectPart>().resetSelectedFuseTo();
		disableConnectButton();
	}
		
	public bool positionMatches(GameObject selectedObj, GameObject fuseTo) {

		string newFuseToName = fuseTo.name;

		Quaternion[] acceptedRotations = new Quaternion[2];
		acceptedRotations = selectedObj.GetComponent<FuseBehavior>().getAcceptableRotations(newFuseToName);

		Quaternion currentRotation = selectedObj.transform.rotation;
		bool acceptable = false;
		for(int i = 0; i < acceptedRotations.Length; i++) {
			float angle = Quaternion.Angle(currentRotation, acceptedRotations[i]);
			if (Mathf.Abs(angle) < 5.0f) {
				acceptable = true;
			}
			print ("Angle: " + angle + ", Current Rotation: " + currentRotation.eulerAngles + " accepted: " + acceptedRotations[i].eulerAngles);
		}
		return acceptable;
	}

	
	
	// Update is called once per frame
	void Update () {

        // continuously rotate the finished constructed item until player clicks the Take/claimItem button.
		if(done())
        {
            
			rotateConstruction ();
 
        }


        // FOR PROCTOR/DEBUG USE ONLY
        // Use to skip this level. Uncomment if you want this feature.
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            if(Input.GetKeyUp(KeyCode.U))
            {
                fuseCount = NUM_FUSES;
                initiateFuse(true); // initiate fuse in debug mode, prompting automatic victory
            }
        }

        // Ensure mouse works...
        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
