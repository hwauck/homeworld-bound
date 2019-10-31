// Nick Olenz

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class InventoryController : MonoBehaviour
{
	// Child references, set in inspector.
	public GameObject tabs; // The tabs parent.
	public Transform[] tabButtons;  // Every tab.
	public GameObject[] menus;	// Every menu.
	public GameObject inv;	// The inventory parent.
	public GameObject rec;  // The recipes parent.
	public GameObject clue; // The clues parent.
	public InventoryPopulator invPop;   // Where items are populated.
	public RecipePopulator recPop;      // Where recipes are populated.
	public CluePopulator cluePop;		// Where clues are populated.

    private static int batteryPartCount;
    private static int itemPartCount;

    // Other references, grabbed on Start.
    UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController player;
	
	float tabButtonY;	// Default Y position of tab buttons, for tab popping.

	// Internal variables.
	bool menuOpen = false;  // Whether menu is open. Used for releasing mouse.
	float sensitivityXTemp = 0;	// Used to store sensitivity settings while menu is open.
	float sensitivityYTemp = 0; // "

	// Storage for all items / parts currently held by the player.
	//public static List<InvItem> items = new List<InvItem>();
	public static Dictionary<string, InvItem> items = new Dictionary<string, InvItem>();

	// Static references to the current player state, set when pressing the button to enter construction mode.
	// These are set in the script "RecipeButtonBridge", on the AddListener for the construction button.
	// Necessary so construction mode knows which scene to dump you back into.
	public static string levelName = "";

	void Awake ()
	{
		//! Initial recipes unlocked should be placed here.
		//RecipesDB.unlockedRecipes.Add(RecipesDB.TestRecipe);
		//RecipesDB.unlockedRecipes.Add(RecipesDB.TestRecipe2);
		if (!RecipesDB.unlockedRecipes.Contains(RecipesDB.RocketBoots))
			RecipesDB.unlockedRecipes.Add(RecipesDB.RocketBoots);
		if (!RecipesDB.unlockedRecipes.Contains(RecipesDB.Sledgehammer))
			RecipesDB.unlockedRecipes.Add(RecipesDB.Sledgehammer);
		if (!RecipesDB.unlockedRecipes.Contains(RecipesDB.Key1))
			RecipesDB.unlockedRecipes.Add(RecipesDB.Key1);
		if (!RecipesDB.unlockedRecipes.Contains(RecipesDB.FFA))
			RecipesDB.unlockedRecipes.Add(RecipesDB.FFA);

        //batteryPartCount = 0;
        //itemPartCount = 0;

	}


	void Start ()
	{
		// Grabbing references.
		//player = Reference.Instance().player;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
		//tabButtonY = tabButtons[1].transform.localPosition.y;

		// Will be closed on run, so disable all menus.
		// Grab initial sensitivity settings first, though, or they will zero.
		sensitivityXTemp = player.mouseLook.XSensitivity;
		sensitivityYTemp = player.mouseLook.YSensitivity;

		// Also ensure everything is populated when initialized.
		// We do this literally everywhere because script run order is not
		// defined in Unity... Keeps things interesting, I guess.
		//invPop.Repopulate();
		//recPop.Repopulate();

		CloseInventory();


	}

	void OnEnable()
	{
		CloseInventory();
	}

	// Need this somewhere for the restart stuff.
	// DEMO MODE ONLY
    // TODO: Disable for full game version
	public static void RestartGame()
	{
		Debug.Log("Restarting!");
		//System.IO.File.Delete(Application.persistentDataPath + "/Save.dat");	// Delete save.
		LoadUtils.loadedScenes.Clear();							// Clear loaded scenes to account for all scene conditions.
		ConversationTrigger.tokens.Clear();						// Clear tokens for sanity.
		items.Clear();                                          // Clear items for safety.
		BatterySystem.SetPower(10);								// Reset power.
		levelName = "";
		Destroy(GameObject.FindGameObjectWithTag("FullPlayer"));	// Must make sure the old player is properly deleted...
		UnityEngine.SceneManagement.SceneManager.LoadScene("SimpleMenu");	// Reload, finally.
	}

    public static int getSavedBatteryPartCount()
    {
        return batteryPartCount;
    }

	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (menuOpen)
				CloseInventory();
			else
				OpenInventory();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
			CloseInventory();

		// Restarting game
		// DEMO MODE ONLY.
        // TODO: delete for full version
		//if (Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.R))
		//{
		//	RestartGame();
		//}
	}

	// Defaults to "Inventory" tab.
	public void OpenInventory()
	{
		// Check to see if conversation is currently going. Conversations currently take priority over inventory.
		if (ConversationController.currentlyEnabled)
			return;

		// Internal Settings.
		menuOpen = true;
		UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController.allowMovement = false;

		// Player Settings.
		player.mouseLook.SetCursorLock(false);
		sensitivityXTemp = player.mouseLook.XSensitivity;
		sensitivityYTemp = player.mouseLook.YSensitivity;
		player.mouseLook.XSensitivity = 0;
		player.mouseLook.YSensitivity = 0;

		// Menus Active.
		FakeActive(tabs, true);
		SwitchToClue();
	}

	public void CloseInventory()
	{
		if (!ConversationController.currentlyEnabled)
		{
			// Internal Settings.
			menuOpen = false;
			UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController.allowMovement = true;

			// Player Settings.
			if (player != null)
			{
				player.mouseLook.SetCursorLock(true);
				player.mouseLook.XSensitivity = sensitivityXTemp;
				player.mouseLook.YSensitivity = sensitivityYTemp;
			}

			// Menus Inactive.
			FakeActive(tabs, false);
			PickMenu(-1);
		}
	}

	// Tab switching functions.
	public void SwitchToInv()
	{
		PickMenu(0);

		PopUpTab(0);

		// Ensure inventory pane is populated.
		invPop.Repopulate();
	}
	public void SwitchToRec()
	{
		PickMenu(1);

		PopUpTab(1);

		// Ensure recipes pane is populated.
		recPop.Repopulate();
	}

	public void SwitchToClue()
	{
		PickMenu(2);

		PopUpTab(2);

		// Ensure clues pane is populated.
		cluePop.Repopulate();
	}

	// Enables menu at index. -1 disables all menus.
	public void PickMenu(int idx)
	{
		foreach (GameObject ii in menus)
		{
			FakeActive(ii, false);
		}
		if (idx != -1)
			FakeActive(menus[idx], true);
	}

	public void PopUpTab(int idx)
	{
		
		// Reset all heights to normal.
		foreach (Transform ii in tabButtons)
		{
			ii.localPosition = new Vector3(ii.localPosition.x, 0, ii.localPosition.z);
		}
		
		// Pop up the one at idx.
		Vector3 pos = tabButtons[idx].localPosition;
		pos.y += 4;
		tabButtons[idx].localPosition = pos;
		
	}

	// Unity's SetActive function is more trouble than it's worth in situations
	// like menus where a lot can be going on. SetActive doesn't seem to happen
	// until the next frame.
	// This solves that problem by moving "inactive" objects behind you... 
	public void FakeActive(GameObject go, bool active)
	{
		Vector3 pos = go.transform.localPosition;
		if (!active)
		{
			pos.z = 80000;
			go.transform.localPosition = pos;
		}
		else
		{
			pos.z = 0;
			go.transform.localPosition = pos;
		}
	}

////////////////////////////////////////////
// Here starts functions related to items //
////////////////////////////////////////////

	// Definitely wasting time with these functions, but
	// the simplicity may be worth it, considering there aren't supposed
	// to be many different types of parts.
	public static int GetQuantity(string itemName)
	{
		if (items.ContainsKey(itemName))
		{
			return items[itemName].quantity;
		}
		Debug.Log("Item not found: " + itemName);
		return 0;
	}

	// Add an amount of an item type to the list.
	// Assuming that items will only be added by pickups.
	public static int Add(PickUp pickup, int quantity)
	{
		// If item is already in list.
		if (items.ContainsKey(pickup.pickupName))
			return items[pickup.pickupName].Add(quantity);

		// If item is not already in list.
		InvItem added = new InvItem(pickup.pickupName, pickup.pickupDesc, quantity, pickup.gameObject);
		items.Add(pickup.pickupName, added);
		return quantity;
	}

	public static int Consume(string itemName, int quantity)
	{
		if (items.ContainsKey(itemName))
			return items[itemName].Consume(quantity);

		Debug.Log("Item not found: " + itemName);
		return 0;
	}

	// Called when an item is picked up. Prepares the tokens for saving.
	public static void ConvertInventoryToTokens()
	{
		// Remove all current inventory tokens. (anything containing "item|")
	/*	List<string> toRemove = new List<string>();	// Need a list because you cannot modify a hashset while iterating over it.
		foreach (string ii in ConversationTrigger.tokens)
		{
			if (ii.Contains("item|"))
				toRemove.Add(ii);
		}
		foreach (string ii in toRemove)
		{
			ConversationTrigger.RemoveToken(ii, false);
		}
    */
		// Create and add the tokens to model the inventory.
		// "item|PATH|COUNT"
		List<string> toAdd = new List<string>();	// Need a list because you cannot modify a hashset while iterating over it.
		foreach (KeyValuePair<string, InvItem> ii in items)
		{
			string newToken = "item|";
            //newToken += ii.Value.pickup.GetComponent<PickUp>().prefabPath;
            newToken += ii.Value.pickup.name;
			newToken += "|";
			newToken += ii.Value.quantity;
			toAdd.Add(newToken);
		}
		foreach (string ii in toAdd)
		{
			ConversationTrigger.AddToken(ii, false);
		}
		SaveController.Save();
	}

	// Only called by SaveController.Load();
	public static void ConvertTokensToInventory()
	{
        

		// Get the references we will need.
		Transform player = GameObject.FindGameObjectWithTag("Player").transform;

		// Read all tokens with "item|" in them, split into path and count.
		List<string> itemStrings = new List<string>();
		foreach (string ii in ConversationTrigger.tokens)
		{
            Debug.Log("string in ConversationTrigger.tokens: " + ii);
			if (ii.Contains("item|"))
				itemStrings.Add(ii);
		}

        batteryPartCount = 0;
        itemPartCount = 0;

        foreach (string ii in itemStrings)
		{
            Debug.Log("string in itemStrings: " + ii);
			string[] separated = ii.Split(new char[]{ '|' });
			string path = separated[1];
			int count = int.Parse(separated[2]);

            // Spawn prefab found at PATH, COUNT times on top of the player.
            //GameObject instantiated = Resources.Load<GameObject>(path);
            //for (int i = 0; i < count; i++)
            //{
            //	if (instantiated == null)
            //	{
            //		Debug.LogError("Path for item was not found! Path was: " + path);
            //	}
            //	else
            //	{
            //		GameObject instance = Instantiate(instantiated);
            //		instance.GetComponent<PickUp>().autoDelete = false;
            //		instance.transform.position = player.position;
            //	}
            //}

            // NEW method - just move the in-game object on top of the player rather than instantiating a new prefab
            GameObject instance = GameObject.Find(path);
            if(instance == null)
            {
                // gross, this needs to be fixed eventually - just a temporary way of avoiding errors when calling GameObject.Find()
                // on RocketBoots parts that are currently deactivated
                if(!path.Contains("Pickup_Boots_"))
                {
                    Debug.LogError("Error: No GameObject with name " + path + " found in scene!");
                }
            } else
            {
                PickUp pickup = instance.GetComponent<PickUp>();
                pickup.setFromSave(true);
                instance.transform.position = new Vector3(-1000f, -1000f, -1000f);
                if (pickup.type == PickUp.PickupType.Battery)
                {
                    batteryPartCount++;
                    Debug.Log("Incremented battery part count! Is now " + batteryPartCount);
                } else if (pickup.type == PickUp.PickupType.Item)
                {
                    itemPartCount++;
                    Debug.Log("Incremented item part count! Is now " + itemPartCount);

                }
                // instead of dropping them on the player's head, compute the total amount first, 
                // then use that to set batteriesBuilt and batteryParts. WhatToBuild will already be correct.
                // otherwise it's a mess and partsNeeded gets messed up, and possibly other things as well
            }
		}

        calculateBatteryCounts();
        calculateItemCounts();


    }


    private static void calculateBatteryCounts()
    {
        GameObject batteryCounterObj = GameObject.Find("BatteryPartsFound");
        if (batteryCounterObj != null && batteryCounterObj.GetComponent<BatteryCounter>() != null)
        {
            BatteryCounter batteryCounter = batteryCounterObj.GetComponent<BatteryCounter>();
            int[] partsNeeded = batteryCounter.PARTS_NEEDED;
            int batteriesBuilt = 0;
            int i = 0;
            if (batteryPartCount == 0)
            {
                batteryCounter.setBatteryParts(batteryPartCount);
                batteryCounter.setBatteriesBuilt(0);
                Debug.Log("Setting batteries and battery parts to 0!");

            }
            else
            {
                batteryPartCount -= partsNeeded[i];
                while (batteryPartCount > 0)
                {
                    i++;
                    batteriesBuilt++;
                    batteryCounter.newBatteryBuilt.Invoke();
                    batteryPartCount -= partsNeeded[i];
                    Debug.Log("Subtracting partsNeeded " + partsNeeded[i] + " from batteryPartCount. BatteryPartCount is now " + batteryPartCount);
                }

                if (batteryPartCount == 0)
                {
                    batteriesBuilt++;
                    batteryCounter.newBatteryBuilt.Invoke();
                    batteryCounter.setBatteryParts(0);
                } else if (batteryPartCount < 0)
                {
                    batteryCounter.setBatteryParts(partsNeeded[i] - Mathf.Abs(batteryPartCount));
                }

                int batteriesNeeded = batteryCounter.getBatteriesNeeded();

                batteriesBuilt = batteriesBuilt % batteriesNeeded;
                batteryCounter.setBatteriesBuilt(batteriesBuilt);

                Debug.Log("Setting battery part count to " + (partsNeeded[i] - Mathf.Abs(batteryPartCount)) + "!");
                Debug.Log("Setting batteries count to " + batteriesBuilt + "!");
                if(batteryPartCount > 0 || batteriesBuilt > 0)
                {
                    Debug.Log("showing battery parts collected!");
                    batteryCounter.showBatteryParts();
                }
                if(batteriesBuilt > 0)
                {
                    batteryCounter.showBatteriesBuilt();
                }
                
            }
    

        }
        else
        {
            Debug.LogError("Either no BatteryPartsFound object exists in the scene or it has no associated BatteryCounter script");
        }
    }

    private static void calculateItemCounts()
    {
        GameObject itemCounterObj = GameObject.Find("PartsFound");
        if (itemCounterObj != null && itemCounterObj.GetComponent<PartCounter>() != null)
        {
            PartCounter itemPartCounter = itemCounterObj.GetComponent<PartCounter>();
            int[] partsNeeded = itemPartCounter.PARTS_NEEDED;
            int i = 0;
            if (itemPartCount == 0)
            {
                itemPartCounter.setPartsFound(itemPartCount);
                Debug.Log("Setting item parts to 0!");

            }
            else
            {
                itemPartCount -= partsNeeded[i];
                while (itemPartCount > 0)
                {
                    i++;
                    itemPartCount -= partsNeeded[i];
                    Debug.Log("Subtracting partsNeeded " + partsNeeded[i] + " from itemPartCount. ItemPartCount is now " + itemPartCount);

                }

                itemPartCounter.setPartsFound(partsNeeded[i] - Mathf.Abs(itemPartCount));
                Debug.Log("Setting item part count to " + Mathf.Abs(itemPartCount) + "!");

                itemPartCounter.showParts();

                if (itemPartCount == 0 && !ConversationTrigger.GetToken("finished_" + itemPartCounter.getObjectToBuild()))
                {
                    Debug.Log("Current itemPartCounter.getObjectToBuild(): " + itemPartCounter.getObjectToBuild());
                    itemPartCounter.readyForNextLevel.Invoke();
                }

            }

        }
        else
        {
            Debug.LogError("Either no PartsFound object exists in the scene or it has no associated PartCounter script");
        }
    }




}

////////////////////////////////
// Here starts the item class //
////////////////////////////////

[System.Serializable]
public class InvItem
{
	// Fields
	public string itemName;
	public string description;
	public int quantity;            // Number of this item currently held
	public GameObject pickup;		// The object of the pickup itself is stored, may be used as an icon.

	// Ctor
	public InvItem(string newName, string newDesc, int newQuantity, GameObject newPickup)
	{
		itemName = newName;
		description = newDesc;
		quantity = newQuantity;
		pickup = newPickup;
	}

	// Remove "amount" of this item type.
	// Returns what is left.
	public int Consume(int amount)
	{
		if (quantity > amount)
		{
			quantity -= amount;
		}
		else
		{
			Debug.LogError("Attempting to consume more items than player has. Consume() does not account for this! Quantity will be set to 0");
			quantity = 0;
		}
		return quantity;
	}

	// Add "amount" of this item type.
	// Returns new quantity.
	public int Add(int amount)
	{
		if (amount < 0)
		{
			Debug.LogError("A negative amount of an item is being added. Use Consume() instead.");
			return Consume(-1 * amount);
		}
		quantity += amount;
		return quantity;
	}
}


//////////////////////////////////
// Here starts the recipe class //
//////////////////////////////////

public class Recipe
{
	public string recipeName;       // Name of recipe.
	public string recipeDesc;		// Description of recipe. Currently used as scene name to load.
	public List<InvItem> components = new List<InvItem>();	// List of InvItems which make up the recipe.

	public Recipe(string newName, string newDesc, string[] itemNames, int[] itemQuantities)
	{
		recipeName = newName;
		recipeDesc = newDesc;
		if (itemNames.Length != itemQuantities.Length)
			Debug.LogError("Names and Quantities of items in recipe do not match in length. Recipe name: " + newName);

		for (int ii = 0; ii < itemNames.Length; ii++)
		{
			components.Add(new InvItem(itemNames[ii], "", itemQuantities[ii], null));
		}
	}
}
