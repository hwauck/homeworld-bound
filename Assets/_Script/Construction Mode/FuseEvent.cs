using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class FuseEvent : MonoBehaviour {

    //tutorial variables
    public static bool runningJustConstructionMode = false;
    private bool activatedTakeButton;
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
    public Button claimItem;    // NEW ADDITION. A button which appears upon completion of an item to claim it in exploration mode.

    // Non-Button UI elements
	public Text shapesWrong;
	public Text rotationWrong;
	public RotationGizmo rotateGizmo;	// NEW ADDITION. when completing a fusion, disable the rotation gizmo.
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
	private string filename;
	//private StreamWriter sr;
	private int numFuseAttempts;
	private int numWrongRotationFails;
	private int numWrongFacesFails;

    public UnityEvent levelComplete;

    // Data collection
    public ConstructionDataManager dataManager;

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

		// Setup the back button.
		//backButton.onClick.AddListener(() => 
		//{
		//	SimpleData.WriteDataPoint("Left_Scene", "Incomplete_Construction", "", "", "", "");
		//	//SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO," + InventoryController.levelName);
		//	stopLevelTimer();
		//	printLevelDataFail();
		//	LoadUtils.LoadScene(InventoryController.levelName);
		//});

        // is this a tutorial level? If so, there will be a conversation after victory that must be clicked through before
        // Take button appears. So we need to know what level it is so that the script can listen for the correct ConversationTrigger
        activatedTakeButton = false;


  //      // New addition for claim item button.
  //      if (claimItem != null)
		//{
		//	claimItem.onClick.AddListener(() => {

		//		SimpleData.WriteDataPoint("Left_Scene", "Complete_Construction", "", "", "", "");
		//		//SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO," + InventoryController.levelName);
		//		switch (SceneManager.GetActiveScene().name)
		//		{
  //                  // TODO: Need to think about how to handle it when the player might need an unspecified number of 
  //                  // batteries. Procedurally generate battery models/Construction Mode levels so they stay different?
  //                  // Or just make a lot of batteries in advance, loop through them again, and hope the player doesn't
  //                  // notice?
  //                  // Should have a generic "battery" mode for all non-tutorial batteries
		//			case "b1":
  //                      BatterySystem.AddPower(2);
  //                      BatterySystem.PowerToTokens();
  //                      LoadUtils.LoadScene("b2");
		//				LoadUtils.UnloadScene("b1");
		//				break;
  //                  case "b2":
  //                      BatterySystem.AddPower(2);
  //                      BatterySystem.PowerToTokens();
  //                      LoadUtils.LoadScene("b3");
  //                      LoadUtils.UnloadScene("b2");
  //                      break;
  //                  case "b3":
  //                      BatterySystem.AddPower(2);
  //                      BatterySystem.PowerToTokens();
  //                      LoadUtils.LoadScene("b4");
  //                      LoadUtils.UnloadScene("b3");
  //                      break;
  //                  case "b4":
  //                      BatterySystem.AddPower(2);
  //                      BatterySystem.PowerToTokens();
		//				LoadUtils.LoadScene("rocketBoots");
		//				LoadUtils.UnloadScene("b4");
		//				break;
		//			case "boot":
		//				RocketBoots.ActivateBoots();
		//				InventoryController.items.Remove("Rocket Boots Body");
		//				InventoryController.items.Remove("Rocket Boots Calf");
		//				InventoryController.items.Remove("Rocket Boots Sole");
		//				InventoryController.items.Remove("Rocket Boots Toe");
		//				InventoryController.items.Remove("Rocket Boots Toe Sole");
		//				InventoryController.items.Remove("Rocket Boots Trim");
		//				InventoryController.items.Remove("Rocket Boots Widening");
		//				InventoryController.ConvertInventoryToTokens();
		//				//RecipesDB.unlockedRecipes.Remove(RecipesDB.RocketBoots);
		//				LoadUtils.LoadScene(InventoryController.levelName);
		//				LoadUtils.UnloadScene("rocketBoots");
		//				break;
		//			case "axe":
		//				Sledgehammer.ActivateSledgehammer();
		//				InventoryController.items.Remove("Sledgehammer Trapezoid");
		//				InventoryController.items.Remove("Sledgehammer Top Point");
		//				InventoryController.items.Remove("Sledgehammer Shaft");
		//				InventoryController.items.Remove("Sledgehammer Head");
		//				InventoryController.items.Remove("Sledgehammer Haft");
		//				InventoryController.items.Remove("Sledgehammer Bottom Point");
		//				InventoryController.items.Remove("Sledgehammer Bottom Point Right");
		//				InventoryController.items.Remove("Sledgehammer Trapezoid");
		//				InventoryController.items.Remove("Sledgehammer Top Point Right");
		//				InventoryController.items.Remove("Sledgehammer Small Tip");
		//				InventoryController.items.Remove("Sledgehammer Small Trap");
		//				InventoryController.items.Remove("Sledgehammer Tip");

		//				InventoryController.ConvertInventoryToTokens();
		//				LoadUtils.LoadScene(InventoryController.levelName);
		//				LoadUtils.UnloadScene("sledgehammer");
		//				break;
		//			case "key1":
		//				ConversationTrigger.AddToken("player_has_key1");
		//				InventoryController.items.Remove("Key 1 Dangly T");
		//				InventoryController.items.Remove("Key 1 Upright L");
		//				InventoryController.items.Remove("Key 1 Upright Rect");
		//				InventoryController.items.Remove("Key 1 Upright T");
		//				InventoryController.items.Remove("Key 1 Walking Pants");
		//				InventoryController.items.Remove("Key 1 Waluigi");
		//				InventoryController.ConvertInventoryToTokens();
		//				LoadUtils.LoadScene(InventoryController.levelName);
		//				LoadUtils.UnloadScene("key1");
		//				break;
		//			case "ffa":
		//				ConversationTrigger.AddToken("player_has_ffa");
		//				InventoryController.items.Remove("FFA Blue Tri");
		//				InventoryController.items.Remove("FFA Center Box");
		//				InventoryController.items.Remove("FFA Center Tri");
		//				InventoryController.items.Remove("FFA Handle Bottom");
		//				InventoryController.items.Remove("FFA Handle Top");
		//				InventoryController.items.Remove("FFA Left Tri");
		//				InventoryController.items.Remove("FFA Right Tri");
		//				InventoryController.items.Remove("FFA Right Tri Chunk");
		//				InventoryController.items.Remove("FFA Ring Large");
		//				InventoryController.items.Remove("FFA Ring Long");
		//				InventoryController.items.Remove("FFA Ring Small");
		//				InventoryController.items.Remove("FFA Scalene");
		//				InventoryController.ConvertInventoryToTokens();
		//				LoadUtils.LoadScene(InventoryController.levelName);
		//				LoadUtils.UnloadScene("ffaHarder");
		//				break;

		//			default:
		//				Debug.Log("Not Yet Implemented: " + SceneManager.GetActiveScene().name);
		//				break;
		//		}
		//		// Update the build button based on the now-removed parts.
		//		BuildButton.CheckRecipes();

		//	});
		//	Debug.Log("Made it to this point");
		//	Debug.Log("Disabling Take button!");

       // }

        firstFuseComplete = false;


        // Infinite energy if running construction mode separately.
        if (InventoryController.levelName == "")
        {
            // Special stuff happens because we are just running construction mode without exploration mode.
            runningJustConstructionMode = true;
            SimpleData.CreateInitialFiles();

            //! Is this a really bad idea?
            SaveController.filename += "_CONSTRUCTION-ONLY";

            // This works because levelName will be "" when we aren't coming from any specific level.

            // Add a ton of power and hide the battery indicator.
            // Disabling is generally a bad idea.
            //BatterySystem.AddPower(999999999);
            //GameObject.Find("BatteryIndicator").transform.localScale = Vector3.zero;

            // Change back button functionality.
            //backButton.onClick.RemoveAllListeners();
            //backButton.onClick.AddListener(() =>
            //{
            //    stopLevelTimer();
            //    printLevelDataFail();
            //    SimpleData.WriteDataPoint("Left_Scene", "Construction_Only", "", "", "", "");
            //    //SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO,SimpleMenu");
            //    SceneManager.LoadScene("SimpleMenu");
            //});


            // Change Claim Item functionality.
            //if (claimItem != null)
            //{
            //	//claimItem.transform.localScale = Vector3.zero;
            //	claimItem.onClick.RemoveAllListeners();
            //	claimItem.onClick.AddListener(() =>
            //	{
            //		SimpleData.WriteDataPoint("Left_Scene", "Construction_Only", "", "", "", "");
            //		//SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO,SimpleMenu");
            //		//stopLevelTimer();
            //		//printLevelDataFail();
            //		SceneManager.LoadScene("SimpleMenu");
            //	});
            //}
        }
        else
        {
            runningJustConstructionMode = false;
        }

        // TODO: make sure all this is reset in data collection as needed whenever level is reset
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
        Debug.Log("Instantiating group in Awake() for FuseEvent in scene " + LoadUtils.currentSceneName);
		group = Instantiate(victoryPrefab, new Vector3(-100, 30, 100), new Quaternion());
        //to make sure it's created as part of the current construction mode scene
        LoadUtils.InstantiateParenter(group);

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

    public void setIsFirstLevel(bool isFirstLevel)
    {
        this.isFirstLevel = isFirstLevel;
    }

    // called by Claim/Take button after level is finished for all levels except the last battery level
    // before timed Exploration Mode
    public void goToNextScene()
    {

        SimpleData.WriteDataPoint("Left_Scene", "Complete_Construction", "", "", "", "");
        //SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO," + InventoryController.levelName);

        string currentLevel;  
        if(loadedLevels < 2)
        {
            currentLevel = SceneManager.GetActiveScene().name;
        } else
        {
            currentLevel = LoadUtils.currentSceneName;
        }
        Debug.Log("currentLevel: " + currentLevel);
        switch (currentLevel)
        {
            case "b1":
                BatterySystem.AddPower(2);
                BatterySystem.PowerToTokens();
                LoadUtils.LoadScene(InventoryController.levelName);
                LoadUtils.UnloadScene("b1");
                break;
            case "b2":
                BatterySystem.AddPower(2);
                BatterySystem.PowerToTokens();
                LoadUtils.LoadScene(InventoryController.levelName);
                LoadUtils.UnloadScene("b2");
                break;
            case "b3":
                BatterySystem.AddPower(2);
                BatterySystem.PowerToTokens();
                LoadUtils.LoadScene(InventoryController.levelName);
                LoadUtils.UnloadScene("b3");
                break;
            case "b4":
                BatterySystem.AddPower(2);
                BatterySystem.PowerToTokens();
                LoadUtils.LoadScene(InventoryController.levelName);
                LoadUtils.UnloadScene("b4");
                break;
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
                //RecipesDB.unlockedRecipes.Remove(RecipesDB.RocketBoots);
                LoadUtils.LoadScene(InventoryController.levelName);
                LoadUtils.UnloadScene("rocketBoots");
                break;
            case "axe":
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
                LoadUtils.LoadScene(InventoryController.levelName);
                LoadUtils.UnloadScene("sledgehammer");
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
                LoadUtils.LoadScene(InventoryController.levelName);
                LoadUtils.UnloadScene("key1");
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
                LoadUtils.LoadScene(InventoryController.levelName);
                LoadUtils.UnloadScene("ffaHarder");
                break;

            default:
                Debug.Log("Not Yet Implemented: " + SceneManager.GetActiveScene().name);
                break;
        }

        if (!runningJustConstructionMode)
        {
            // Update the build button based on the now-removed parts.
            //BuildButton.CheckRecipes();
        }
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
        else if (currentScene.Equals("b2"))
        {
            //b2p1 to b2p2
            fuseMapping.Add("b2p1_b2p2_a1", "b2p2_b2p1_a1");
            fuseMapping.Add("b2p1_b2p2_a2", "b2p2_b2p1_a2");
            fuseMapping.Add("b2p2_b2p1_a1", "b2p1_b2p2_a1");
            fuseMapping.Add("b2p2_b2p1_a2", "b2p1_b2p2_a2");

            //b2p1 to bb2
            fuseMapping.Add("b2p1_bb2_a1", "bb2_b2p1_a1");
            fuseMapping.Add("b2p1_bb2_a2", "bb2_b2p1_a2");
            fuseMapping.Add("b2p1_bb2_a3", "bb2_b2p1_a3");
            fuseMapping.Add("b2p1_bb2_a4", "bb2_b2p1_a4");
            fuseMapping.Add("b2p1_bb2_a5", "bb2_b2p1_a5");

            //b2p2 to bb2
            fuseMapping.Add("b2p2_bb2_a1", "bb2_b2p2_a1");
            fuseMapping.Add("b2p2_bb2_a2", "bb2_b2p2_a2");
            fuseMapping.Add("b2p2_bb2_a3", "bb2_b2p2_a3");

        }
        else if(currentScene.Equals("b3"))
        {
            //b3p1 to bb3
            fuseMapping.Add("b3p1_bb3_a1", "bb3_b3p1_a1");
            fuseMapping.Add("b3p1_bb3_a2", "bb3_b3p1_a2");
            fuseMapping.Add("b3p1_bb3_a3", "bb3_b3p1_a3");
            fuseMapping.Add("b3p1_bb3_a4", "bb3_b3p1_a4");
            fuseMapping.Add("b3p1_bb3_a5", "bb3_b3p1_a5");
            fuseMapping.Add("b3p1_bb3_a6", "bb3_b3p1_a6");

            //b3p2 to bb3
            fuseMapping.Add("b3p2_bb3_a1", "bb3_b3p2_a1");
            fuseMapping.Add("b3p2_bb3_a2", "bb3_b3p2_a2");
            fuseMapping.Add("b3p2_bb3_a3", "bb3_b3p2_a3");
            fuseMapping.Add("b3p2_bb3_a4", "bb3_b3p2_a4");
            fuseMapping.Add("b3p2_bb3_a5", "bb3_b3p2_a5");
            fuseMapping.Add("b3p2_bb3_a6", "bb3_b3p2_a6");

        } 
        else if (currentScene.Equals("b4"))
        {
            //b4p1 to bb4
            fuseMapping.Add("b4p1_bb4_a1", "bb4_b4p1_a1");
            fuseMapping.Add("b4p1_bb4_a2", "bb4_b4p1_a2");

            //b4p1 to b4p2
            fuseMapping.Add("b4p1_b4p2_a1", "b4p2_b4p1_a1");

            //b4p1 to b4p3
            fuseMapping.Add("b4p1_b4p3_a1", "b4p3_b4p1_a1");
            fuseMapping.Add("b4p1_b4p3_a2", "b4p3_b4p1_a2");

            //b4p2 to bb4
            fuseMapping.Add("b4p2_bb4_a1", "bb4_b4p2_a1");
            fuseMapping.Add("b4p2_bb4_a2", "bb4_b4p2_a2");

            //b4p2 to b4p1
            fuseMapping.Add("b4p2_b4p1_a1", "b4p1_b4p2_a1");

            //b4p2 to b4p3
            fuseMapping.Add("b4p2_b4p3_a1", "b4p3_b4p2_a1");
            fuseMapping.Add("b4p2_b4p3_a2", "b4p3_b4p2_a2");

            //b4p3 to bb4
            fuseMapping.Add("b4p3_bb4_a1", "bb4_b4p3_a1");
            fuseMapping.Add("b4p3_bb4_a2", "bb4_b4p3_a2");
            fuseMapping.Add("b4p3_bb4_a3", "bb4_b4p3_a3");

            //b4p3 to b4p1
            fuseMapping.Add("b4p3_b4p1_a1", "b4p1_b4p3_a1");
            fuseMapping.Add("b4p3_b4p1_a2", "b4p1_b4p3_a2");

            //b4p3 to b4p2
            fuseMapping.Add("b4p3_b4p2_a1", "b4p2_b4p3_a1");
            fuseMapping.Add("b4p3_b4p2_a2", "b4p2_b4p3_a2");

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
        else if (currentScene.Equals("key1"))
        {
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
        else if (currentScene.Equals("axe"))
        {
            /*	HashSet<string> fuseToForHaft = new HashSet<string>();
                fuseToForHaft.Add ("shaft_haft_attach");
                fuseMapping.Add ("haft_shaft_attach", fuseToForHaft);

                HashSet<string> fuseToForTrapezoid = new HashSet<string>();
                fuseToForTrapezoid.Add ("shaft_trapezoid_attach");
                fuseMapping.Add ("trapezoid_shaft_attach", fuseToForTrapezoid);

                HashSet<string> fuseToForHead = new HashSet<string>();
                fuseToForHead.Add ("trapezoid_head_attach");
                fuseMapping.Add ("head_trapezoid_attach", fuseToForHead);
                fuseMapping.Add ("head_tip_attach", fuseToForHead);

                HashSet<string> fuseToForBottomPointLeft1 = new HashSet<string>();
                HashSet<string> fuseToForBottomPointLeft2 = new HashSet<string>();
                fuseToForBottomPointLeft1.Add ("head_bottom_point_left_attach");
                fuseToForBottomPointLeft2.Add ("bottom_point_right_left_attach");
                fuseMapping.Add ("bottom_point_left_head_attach", fuseToForBottomPointLeft1);
                fuseMapping.Add ("bottom_point_left_right_attach", fuseToForBottomPointLeft2);

                HashSet<string> fuseToForBottomPointRight1 = new HashSet<string>();
                HashSet<string> fuseToForBottomPointRight2 = new HashSet<string>();
                fuseToForBottomPointRight1.Add ("head_bottom_point_right_attach");
                fuseToForBottomPointRight2.Add ("bottom_point_left_right_attach");
                fuseMapping.Add ("bottom_point_right_head_attach", fuseToForBottomPointRight1);
                fuseMapping.Add ("bottom_point_right_left_attach", fuseToForBottomPointRight2);

                HashSet<string> fuseToForTopPointLeft1 = new HashSet<string>();
                HashSet<string> fuseToForTopPointLeft2 = new HashSet<string>();
                fuseToForTopPointLeft1.Add ("head_top_point_left_attach");
                fuseToForTopPointLeft2.Add ("top_point_right_left_attach");
                fuseMapping.Add ("top_point_left_head_attach", fuseToForTopPointLeft1);
                fuseMapping.Add ("top_point_left_right_attach", fuseToForTopPointLeft2);

                HashSet<string> fuseToForTopPointRight1 = new HashSet<string>();
                HashSet<string> fuseToForTopPointRight2 = new HashSet<string>();
                fuseToForTopPointRight1.Add ("head_top_point_right_attach");
                fuseToForTopPointRight2.Add ("top_point_left_right_attach");
                fuseMapping.Add ("top_point_right_head_attach", fuseToForTopPointRight1);
                fuseMapping.Add ("top_point_right_left_attach", fuseToForTopPointRight2);

                HashSet<string> fuseToForSmallTip = new HashSet<string>();
                fuseToForSmallTip.Add ("small_trapezoid_small_tip_attach");
                fuseMapping.Add ("small_tip_small_trapezoid_attach", fuseToForSmallTip);

                HashSet<string> fuseToForSmallTrapezoid = new HashSet<string>();
                fuseToForSmallTrapezoid.Add ("shaft_small_trapezoid_attach");
                fuseMapping.Add ("small_trapezoid_shaft_attach", fuseToForSmallTrapezoid);

                HashSet<string> fuseToForSpike = new HashSet<string>();
                fuseToForSpike.Add ("shaft_spike_attach");
                fuseMapping.Add ("spike_shaft_attach", fuseToForSpike);

                HashSet<string> fuseToForTip = new HashSet<string>();
                fuseToForTip.Add ("head_tip_attach");
                fuseMapping.Add ("tip_head_attach", fuseToForTip);
                */
        }
        else if (currentScene.Equals("hull"))
        {
            /*	HashSet<string> fuseToForBridgeCoverLeft = new HashSet<string>();
                HashSet<string> fuseToForBridgeCoverRight = new HashSet<string>();
                fuseToForBridgeCoverLeft.Add ("bridge_bridge_cover_left_attach");
                fuseToForBridgeCoverRight.Add ("bridge_bridge_cover_right_attach");
                fuseMapping.Add ("bridge_cover_bridge_left_attach", fuseToForBridgeCoverLeft);
                fuseMapping.Add ("bridge_cover_bridge_right_attach", fuseToForBridgeCoverRight);

                HashSet<string> fuseToForBackBridge = new HashSet<string>();
                HashSet<string> fuseToForBackLeftCover = new HashSet<string>();
                HashSet<string> fuseToForBackRightCover = new HashSet<string>();
                fuseToForBackBridge.Add ("bridge_back_attach");
                fuseToForBackLeftCover.Add ("left_cover_back_attach");
                fuseToForBackRightCover.Add ("right_cover_back_attach");
                fuseMapping.Add ("back_bridge_attach", fuseToForBackBridge);
                fuseMapping.Add ("back_left_cover_attach", fuseToForBackBridge);
                fuseMapping.Add ("back_right_cover_attach", fuseToForBackBridge);

                HashSet<string> fuseToForBackSlopeBridgeCover = new HashSet<string>();
                HashSet<string> fuseToForBackSlopeRightCover = new HashSet<string>();
                HashSet<string> fuseToForBackSlopeLeftCover = new HashSet<string>();
                fuseToForBackSlopeBridgeCover.Add ("bridge_cover_back_slope_attach");
                fuseToForBackSlopeRightCover.Add ("left_cover_back_slope_attach");
                fuseToForBackSlopeLeftCover.Add ("right_cover_back_slope_attach");
                fuseMapping.Add ("back_slope_bridge_cover_attach", fuseToForBackSlopeBridgeCover);
                fuseMapping.Add ("back_slope_right_cover_attach", fuseToForBackSlopeRightCover);
                fuseMapping.Add ("back_slope_left_cover_attach", fuseToForBackSlopeLeftCover);

                HashSet<string> fuseToForLeftCoverBack = new HashSet<string>();
                HashSet<string> fuseToForLeftCoverSlope = new HashSet<string>();
                fuseToForLeftCoverBack.Add ("back_left_cover_attach");
                fuseToForLeftCoverSlope.Add ("back_slope_left_cover_attach");
                fuseMapping.Add ("left_cover_back_attach", fuseToForLeftCoverBack);
                fuseMapping.Add ("left_cover_back_slope_attach", fuseToForLeftCoverSlope);

                HashSet<string> fuseToForRightCoverBack = new HashSet<string>();
                HashSet<string> fuseToForRightCoverSlope = new HashSet<string>();
                fuseToForRightCoverBack.Add ("back_right_cover_attach");
                fuseToForRightCoverSlope.Add ("back_slope_right_cover_attach");
                fuseMapping.Add ("right_cover_back_attach", fuseToForRightCoverBack);
                fuseMapping.Add ("right_cover_back_slope_attach", fuseToForRightCoverSlope);
            */
        }
        else if (currentScene.Equals("ffa"))
        {

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
        }
        else if (currentScene.Equals("gloves"))
        {
            //palm, fingers, thumb, armDec, palmDec
            /*	
                //palm
                HashSet<string> fuseToForPalm = new HashSet<string>();
                fuseToForPalm.Add ("arm_palm_attach");
                fuseMapping.Add ("palm_arm_attach", fuseToForPalm);

                //fingers
                HashSet<string> fuseToForFingers = new HashSet<string>();
                fuseToForFingers.Add ("palm_fingers_attach");
                fuseMapping.Add ("fingers_palm_attach", fuseToForFingers);

                //thumb
                HashSet<string> fuseToForThumb = new HashSet<string>();
                fuseToForThumb.Add ("palm_thumb_attach");
                fuseMapping.Add ("thumb_palm_attach", fuseToForThumb);

                //armDec
                HashSet<string> fuseToForArmDec = new HashSet<string>();
                fuseToForArmDec.Add ("arm_arm_dec_attach");
                fuseMapping.Add ("arm_dec_arm_attach", fuseToForArmDec);

                //palmDec
                HashSet<string> fuseToForPalmDec = new HashSet<string>();
                fuseToForPalmDec.Add ("palm_palm_dec_attach");
                fuseMapping.Add ("palm_dec_palm_attach", fuseToForPalmDec);
            */
        }
        else if (currentScene.Equals("key2"))
        {
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
        }
        else if (currentScene.Equals("catapult"))
        {
            //fuse to Platform
            /*	HashSet<string> fuseToForBackAxleBottom = new HashSet<string>();
                HashSet<string> fuseToForBackAxleLeft = new HashSet<string>();
                HashSet<string> fuseToForBackAxleRight = new HashSet<string>();
                HashSet<string> fuseToForFrontAxleRight = new HashSet<string>();
                HashSet<string> fuseToForFrontAxleLeft = new HashSet<string>();
                HashSet<string> fuseToForFrontAxleBottom = new HashSet<string>();
                HashSet<string> fuseToForLeftSupportBottom = new HashSet<string>();
                HashSet<string> fuseToForRightSupportBottom = new HashSet<string>();

                fuseToForBackAxleBottom.Add ("platform_back_axle_bottom_attach");
                fuseToForBackAxleLeft.Add ("platform_back_axle_left_attach");
                fuseToForBackAxleRight.Add ("platform_back_axle_right_attach");
                fuseToForFrontAxleRight.Add ("platform_front_axle_right_attach");
                fuseToForFrontAxleLeft.Add ("platform_front_axle_left_attach");
                fuseToForFrontAxleBottom.Add ("platform_front_axle_bottom_attach");
                fuseToForLeftSupportBottom.Add ("platform_left_support_attach");
                fuseToForRightSupportBottom.Add ("platform_right_support_attach");

                fuseMapping.Add ("back_axle_platform_bottom_attach", fuseToForBackAxleBottom);
                fuseMapping.Add ("back_axle_platform_left_attach", fuseToForBackAxleLeft);
                fuseMapping.Add ("back_axle_platform_right_attach", fuseToForBackAxleRight);
                fuseMapping.Add ("front_axle_platform_right_attach", fuseToForFrontAxleRight);
                fuseMapping.Add ("front_axle_platform_left_attach", fuseToForFrontAxleLeft);
                fuseMapping.Add ("front_axle_platform_bottom_attach", fuseToForFrontAxleBottom);
                fuseMapping.Add ("left_support_platform_attach", fuseToForLeftSupportBottom);
                fuseMapping.Add ("right_support_platform_attach", fuseToForRightSupportBottom);

                //fuse to back axle
                HashSet<string> fuseToForRightWheel = new HashSet<string>();
                HashSet<string> fuseToForFrontAxleBackAxle = new HashSet<string>();

                fuseToForRightWheel.Add ("back_axle_back_right_wheel_attach");
                fuseToForFrontAxleBackAxle.Add ("back_axle_front_axle_attach");

                fuseMapping.Add ("back_right_wheel_back_axle_attach", fuseToForRightWheel);
                fuseMapping.Add ("front_axle_back_axle_attach", fuseToForFrontAxleBackAxle);

                //fuse to front axle
                //back axle, front left wheel
                HashSet<string> fuseToForLeftWheel = new HashSet<string>();
                HashSet<string> fuseToForBackAxleFrontAxle = new HashSet<string>();

                fuseToForLeftWheel.Add ("front_axle_front_left_wheel_attach");
                fuseToForBackAxleFrontAxle.Add ("front_axle_back_axle_attach");

                fuseMapping.Add ("front_left_wheel_front_axle_attach", fuseToForLeftWheel);
                fuseMapping.Add ("back_axle_front_axle_attach", fuseToForBackAxleFrontAxle);

                //fuse to left support
                //axle
                HashSet<string> fuseToForAxleLeftSupport = new HashSet<string>();
                fuseToForAxleLeftSupport.Add ("left_support_axle_attach");
                fuseMapping.Add ("axle_left_support_attach", fuseToForAxleLeftSupport);

                //fuse to right support
                //axle
                HashSet<string> fuseToForAxleRightSupport = new HashSet<string>();
                fuseToForAxleRightSupport.Add ("right_support_axle_attach");
                fuseMapping.Add ("axle_right_support_attach", fuseToForAxleRightSupport);

                //fuse to axle
                //throwing arm, right support, left support
                HashSet<string> fuseToForThrowingArmBottom = new HashSet<string>();
                HashSet<string> fuseToForThrowingArmLeft = new HashSet<string>();
                HashSet<string> fuseToForThrowingArmRight = new HashSet<string>();
                HashSet<string> fuseToForRightSupportSide = new HashSet<string>();
                HashSet<string> fuseToForLeftSupportSide = new HashSet<string>();

                fuseToForThrowingArmBottom.Add ("axle_throwing_arm_bottom_attach");
                fuseToForThrowingArmLeft.Add ("axle_throwing_arm_left_attach");
                fuseToForThrowingArmRight.Add ("axle_throwing_arm_right_attach");
                fuseToForRightSupportSide.Add ("axle_right_support_attach");
                fuseToForLeftSupportSide.Add ("axle_left_support_attach");

                fuseMapping.Add ("throwing_arm_axle_bottom_attach", fuseToForThrowingArmBottom);
                fuseMapping.Add ("throwing_arm_axle_left_attach", fuseToForThrowingArmLeft);
                fuseMapping.Add ("throwing_arm_axle_right_attach", fuseToForThrowingArmRight);
                fuseMapping.Add ("right_support_axle_attach", fuseToForRightSupportSide);
                fuseMapping.Add ("left_support_axle_attach", fuseToForLeftSupportSide);
            */
        }
        else if (currentScene.Equals("key3"))
        {
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
        }
        else if (currentScene.Equals("vest"))
        {
            /*	
                HashSet<string> fuseToForBackStrapRight = new HashSet<string>();
                HashSet<string> fuseToForBackStrapSide = new HashSet<string>();
                HashSet<string> fuseToForLeftStrapBottom = new HashSet<string>();
                HashSet<string> fuseToForLeftStrapSide = new HashSet<string>();
                HashSet<string> fuseToForLeftStrapTop = new HashSet<string>();			
                HashSet<string> fuseToForRightStrapBottom = new HashSet<string>();
                HashSet<string> fuseToForRightStrapTop = new HashSet<string>();
                HashSet<string> fuseToForVestDiamond = new HashSet<string>();

                fuseToForBackStrapRight.Add("right_strap_back_strap_attach");
                fuseToForBackStrapSide.Add ("back_strap_long_back_strap_short_attach");
                fuseToForLeftStrapBottom.Add("vest_base_left_strap_bottom_attach");
                fuseToForLeftStrapTop.Add("vest_base_left_strap_top_attach");
                fuseToForRightStrapBottom.Add("vest_base_right_strap_bottom_attach");
                fuseToForRightStrapTop.Add("vest_base_right_strap_top_attach");
                fuseToForVestDiamond.Add("vest_base_vest_diamond_attach");

                fuseMapping.Add("back_strap_right_strap_attach",fuseToForBackStrapRight);
                fuseMapping.Add("back_strap_short_back_strap_long_attach",fuseToForBackStrapSide);
                fuseMapping.Add("left_strap_vest_base_bottom_attach",fuseToForLeftStrapBottom);
                fuseMapping.Add("left_strap_vest_base_top_attach",fuseToForLeftStrapTop);
                fuseMapping.Add("right_strap_vest_base_bottom_attach",fuseToForRightStrapBottom);
                fuseMapping.Add("right_strap_vest_base_top_attach",fuseToForRightStrapTop);
                fuseMapping.Add("vest_diamond_vest_base_attach",fuseToForVestDiamond);

                HashSet<string> fuseToForRightStrap = new HashSet<string>();

                fuseToForLeftStrapSide.Add("back_strap_short_back_strap_long_attach");
                fuseToForRightStrap.Add("back_strap_right_strap_attach");

                fuseMapping.Add("back_strap_long_back_strap_short_attach",fuseToForLeftStrapSide);
                fuseMapping.Add("right_strap_back_strap_attach",fuseToForRightStrap);

                HashSet<string> fuseToForVestBaseBottom = new HashSet<string>();
                HashSet<string> fuseToForVestBaseTop = new HashSet<string>();

                fuseToForVestBaseBottom.Add("left_strap_vest_base_bottom_attach");
                fuseToForVestBaseTop.Add("left_strap_vest_base_top_attach");

                fuseMapping.Add("vest_base_left_strap_bottom_attach",fuseToForVestBaseBottom);
                fuseMapping.Add("vest_base_left_strap_top_attach",fuseToForVestBaseTop);

                HashSet<string> fuseToForVestDiamond2 = new HashSet<string>();

                fuseToForVestDiamond2.Add("left_vest_oval_vest_diamond_attach");

                fuseMapping.Add("vest_diamond_left_vest_oval_attach",fuseToForVestDiamond2);

                HashSet<string> fuseToForLeftVestOval = new HashSet<string>();
                HashSet<string> fuseToForRightVestOval = new HashSet<string>();
                HashSet<string> fuseToForVestBase = new HashSet<string>();
                HashSet<string> fuseToForVestOval = new HashSet<string>();

                fuseToForLeftVestOval.Add("vest_diamond_left_vest_oval_attach");
                fuseToForRightVestOval.Add("vest_diamond_right_vest_oval_attach");
                fuseToForVestBase.Add("vest_diamond_vest_base_attach");
                fuseToForVestOval.Add("vest_diamond_vest_oval_attach");

                fuseMapping.Add("left_vest_oval_vest_diamond_attach",fuseToForLeftVestOval);
                fuseMapping.Add("right_vest_oval_vest_diamond_attach",fuseToForRightVestOval);
                fuseMapping.Add("vest_base_vest_diamond_attach",fuseToForVestBase);
                fuseMapping.Add("vest_oval_vest_diamond_attach",fuseToForVestOval);

                HashSet<string> fuseToForVestDiamond3 = new HashSet<string>();

                fuseToForVestDiamond3.Add("vest_oval_vest_diamond_attach");

                fuseMapping.Add("vest_diamond_vest_oval_attach",fuseToForVestDiamond3);
    */
        }
        else if (currentScene.Equals("engine"))
        {
            /*	HashSet<string> fuseToForEngineFront = new HashSet<string>();
                fuseToForEngineFront.Add ("engine_base_engine_front_attach");

                HashSet<string> fuseToForEngineTop = new HashSet<string>();
                fuseToForEngineTop.Add ("engine_base_engine_top_attach");

                HashSet<string> fuseToForEngineLeft = new HashSet<string>();
                fuseToForEngineLeft.Add ("engine_base_engine_left_attach");

                HashSet<string> fuseToForEngineTopRight = new HashSet<string>();
                fuseToForEngineTopRight.Add ("engine_base_engine_top_right_attach");

                HashSet<string> fuseToForEngineRight = new HashSet<string>();
                fuseToForEngineRight.Add ("engine_base_engine_right_attach");

                fuseMapping.Add("engine_front_engine_base_attach",fuseToForEngineFront);
                fuseMapping.Add("engine_top_engine_base_attach",fuseToForEngineTop);
                fuseMapping.Add("engine_left_engine_base_attach",fuseToForEngineLeft);
                fuseMapping.Add("engine_top_right_engine_base_attach",fuseToForEngineTopRight);
                fuseMapping.Add("engine_right_engine_base_attach",fuseToForEngineRight);
    */
        }
     
		
	}

	public void startLevelTimer() {
		levelTimer = Time.time;
		
	}
	
	public void stopLevelTimer() {
		levelTimer = Time.time - levelTimer;
	}
	
	public void printLevelData() {
		//SimpleData.WriteStringToFile("ConstructionData.txt", Time.time + ",CONSTRUCTION,FINISHED," + mode + "," + levelTimer);
		//int xRotations = rotateGizmo.xRots;
		//int yRotations = rotateGizmo.yRots;
		//int zRotations = rotateGizmo.zRots;
		//int totalRotations = xRotations + yRotations + zRotations;
		//SimpleData.WriteStringToFile("ConstructionData.txt", Time.time + ",CONSTRUCTION,X_ROTATIONS," + xRotations);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,Y_ROTATIONS," + yRotations);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,Z_ROTATIONS," + zRotations);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_ROTATIONS," + totalRotations);	ABOVE LINES HANDLED BY RotationGizmo.cs NOW.
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_FUSE_ATTEMPTS," + numFuseAttempts);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_FUSE_FAILS," + numFuseFails);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_WRONG_FACE_FAILS," + numWrongFacesFails);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_WRONG_ROTATION_FAILS," + numWrongRotationFails);
		//if (numFuseAttempts != 0)
		//	SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,AVG_ROTATIONS_PER_FUSE_ATTEMPT," + totalRotations / numFuseAttempts);
		//else
		//	SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,AVG_ROTATIONS_PER_FUSE_ATTEMPT,0");
		
		//sr.Close();
	}

	public void printLevelDataFail() {
		/*SimpleData.WriteStringToFile("ConstructionData.txt", Time.time + ",CONSTRUCTION,ABORTED," + mode + "," + levelTimer);
		int xRotations = rotateGizmo.xRots;
		int yRotations = rotateGizmo.yRots;
		int zRotations = rotateGizmo.zRots;
		int totalRotations = xRotations + yRotations + zRotations;
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,X_ROTATIONS," + xRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,Y_ROTATIONS," + yRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,Z_ROTATIONS," + zRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_ROTATIONS," + totalRotations);*/ // ABOVE LINES HANDLED BY RotationGizmo.cs NOW.
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_FUSE_ATTEMPTS," + numFuseAttempts);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_FUSE_FAILS," + numFuseFails);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_WRONG_FACE_FAILS," + numWrongFacesFails);
		//SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_WRONG_ROTATION_FAILS," + numWrongRotationFails);
		//if (numFuseAttempts != 0)
		//	SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,AVG_ROTATIONS_PER_FUSE_ATTEMPT," + totalRotations / numFuseAttempts);
		//else
		//	SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,AVG_ROTATIONS_PER_FUSE_ATTEMPT,0");

		//sr.Close();
	}

	public void disableConnectButton() {
		connectButton.interactable = false;
	}

	public void initiateFuse() {
        //TODO: if tutorial is on, don't increment fuse attempts
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
		if(selectedObject == null) {
			//player tries to connect when there is no active part (only at beginning)
			//print ("Select the black regions you want to join together!");
            //UPDATE: this can't happen if the Fuse button is disabled until two faces are selected
			source.PlayOneShot (failure);

		} else if (!fuseMapping.ContainsKey (selectedObject.name)){
            // this should only happen if the fuseMapping wasn't properly assigned at the beginning of the level
			print ("Invalid fuse: Cannot fuse " + selectedObject.name + " to " + selectedFuseTo.name);
			//display error on screen for 1 sec
			StartCoroutine(errorWrongFace());

		} else if(fuseMapping[selectedObject.name].Contains(selectedFuseTo.name) && positionMatches (selectedObject, selectedFuseTo)) {
	
			print ("Successful fuse!");
			fuseStatus="success";
            if(dataManager)
            {
                dataManager.AddPartFused(selectedObject.transform.parent.gameObject.name);
            }
			source.PlayOneShot (success);

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

			fuseCleanUp();
			fuseCount++;

            if(isFirstLevel && !firstFuseComplete)
            {
                firstFuseComplete = true;
                ConversationTrigger.AddToken("finishedFirstFuse");
            }
			if(done ()) {
				stopLevelTimer();

                if(dataManager != null)
                {
                    dataManager.SetOutcome("victory");
                }

                if (GameObject.Find("TimeRemainingPanel") != null)
                {
                    GameObject.Find("TimeRemainingPanel").GetComponent<Timer>().stopTimer();
                }
                printLevelData();
				bottomPanelGroup.alpha = 0;
                congratsPanel.SetActive(true);
				finishedImage.enabled = false;
                if (isFirstLevel)
                {
                    ConversationTrigger.AddToken("finishedB1");
                    levelComplete.Invoke(); // tells Tutorial1 that level is complete, so tooltips should be disabled
                }

                musicsource.clip = victorymusic;
                mainCam.transform.position = new Vector3(-90, 80, -3.36f);
                mainCam.transform.rotation = Quaternion.Euler(new Vector3(15, 0, 0));
                musicsource.Play();
                StartCoroutine(FadeAudio(fadeTime, Fade.Out));

                string currentLevel = LoadUtils.currentSceneName;

                //CHANGE this to add new levels
                if (currentLevel.Equals("b1"))
                {
                    ConversationTrigger.AddToken("finished_b1");
                }
                else if (currentLevel.Equals("b2"))
                {
                    ConversationTrigger.AddToken("finished_b2");
                } else if (currentLevel.Equals("b3"))
                {
                    ConversationTrigger.AddToken("finished_b3");
                } else if (currentLevel.Equals("b4"))
                {
                    ConversationTrigger.AddToken("finished_b4");
                } else if (currentLevel.Equals("rocketBoots"))
                {
                    ConversationTrigger.AddToken("finished_RB");
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

        //SimpleData.WriteDataPoint("Fuse_Attempt", selectedObject.transform.parent.name, data_failureType, "", "", data_fuseStatus);
	}

    // combines the BoxColliders of GameObjects starting and added and replaces
    // starting's BoxCollider with the result
    private void extendBoxCollider(GameObject starting, GameObject added)
    {
        Debug.Log("Starting: " + starting);
        Debug.Log("added: " + added);
        BoxCollider startingBoxCollider = starting.GetComponent<BoxCollider>();
        BoxCollider addedBoxCollider = added.GetComponent<BoxCollider>();
        startingBoxCollider.enabled = true;
        addedBoxCollider.enabled = true;
        Bounds oldBounds = starting.GetComponent<BoxCollider>().bounds;
        Bounds addedBounds = added.GetComponent<BoxCollider>().bounds;
        addedBoxCollider.enabled = false;
        Debug.Log("oldBounds: " + oldBounds);
        Debug.Log("addedBounds: " + addedBounds);

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
        Debug.Log("New Size: " + startingBoxCollider.size);

        // sum of the volumes times the centers of starting and added, divided by sum of the volumes
        //startingBoxCollider.center = newCenterLocalCoord;
        startingBoxCollider.center = newCenter;
        Debug.Log("New Center: " + startingBoxCollider.center);


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
        // Make sure every part in each level is tagged "part"
        GameObject[] allParts = GameObject.FindGameObjectsWithTag("part");
        for(int i = 0; i < allParts.Length; i++)
        {
           allParts[i].transform.SetParent(group.transform, true);
        }

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

		if(done ())
        {
			rotateConstruction ();
 
        }

        // Ensure mouse works...
        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
