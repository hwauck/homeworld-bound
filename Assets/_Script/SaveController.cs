using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;


public class SaveController : MonoBehaviour
{

    // The old save system: saves all conversation tokens to a file and then loads them back, along with inventory items and battery power previously collected
    // The new system needs to keep track of not just conversation tokens, but also which parts have been picked up (batteries, fuser, item parts, and clues) and 
    // which mode the player ended in (Construction Mode or Exploration Mode). And keep track of telemetry data from previous attempt (start timed and counted things 
    // starting from most recent Attempt rather than starting a new Attempt

    // Can I do this all just with conversation tokens? No, will need to ask DataManager object (DataAggregator, ExplorationDataManager, and/or ConstructionDataManager) for info about
    // which parts have been collected and what Attempt we're currently on

   // Will need to make sure all part pickups have a corresponding and unique prefab associated with them, otherwise the SaveController.Load() method of instantiating a new prefab
   // and dropping it on the player's head to start won't work. Or, we could just grab the existing object in the scene and move it on top of the player, hopefully that works. 

	// Filename for save. Allow loading of any string name.
	public static string filename = "Save";

    // set this to true to prevent SaveController.Load() from executing more than once per game session (see DataAggregator's initializeDataCollection() method)
    public static bool alreadyLoaded = false;

    [DllImport("__Internal")]
    private static extern void saveToDB(string saveData);

    [DllImport("__Internal")]
    private static extern string loadFromDB();



    // Saves a file containing all game options which are in the above. 
    public static void Save()
	{
        // Prepare for data IO. (.NET platforms only, local Windows machine saving)
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(WhereIsData());
        Debug.Log("Starting Save() method in SaveController");

        // Create and write to a new container.
        SaveContainer data = new SaveContainer();

		//! Fields go here.
		// Create and add inventory tokens.
		//InventoryController.ConvertInventoryToTokens();

		// Save all tokens!
		List<string> tokensTemp = new List<string>();
		foreach (string ii in ConversationTrigger.tokens)
		{
			tokensTemp.Add(ii);
		}
		data.tokens = new List<string>(tokensTemp);

        // Save game state to database (online with database only)
        Debug.Log("converting jsonSaveState to JSON and saving to DB");
        string jsonSaveState = JsonUtility.ToJson(data, true);
        saveToDB(jsonSaveState);

        // Save and close safely. (.NET platforms only, local Windows machine saving)
        //bf.Serialize(file, data);
        //file.Close();

        Debug.Log("Saved all options successfully.");
	}

    // Applies all options from the saved file to the locally created variables.
    // The web version implemented here is very messy. First, on load (or reload of page),
    // the get() function in HBView in views.py gets the saved data of the current player
    // and puts it into a div on the page as text. Then, once the game has loaded and calls Load() here,
    // the browserUnityInteraction.jslib file gets that text when loadFromDB() is called 
    // and returns it as a string. Lastly, we convert the string to JSON and then SaveContainer.
	public static void Load()
	{
        Debug.Log("Loading all conversations from files");
		// Make sure all the strings for conversations are loaded.
		ConversationsDB.LoadConversationsFromFiles();

        // online loading from database only
        string loadedData = loadFromDB();

        // Right now, the string itself has quotes around it. Need to get rid of those.
        Debug.Log("Loaded Data with quotes: " + loadedData + "endofLine");

        loadedData = loadedData.Substring(1, loadedData.Length - 2);
        loadedData = loadedData.Replace("'", "\"");
        Debug.Log("Loaded Data with outside quotes removed and single quotes changed to double quotes: " + loadedData + "endofLine");

        SaveContainer data;
        // if the player has no saved game data
        if (loadedData.Equals(""))
        {
            //create new save file
            Debug.Log("No existing save file (empty string). Creating new save file.");
            Save();
        }
        else if (loadedData == null)
        {
            // this is what loadedData is returning right now, regardless of whether there's stuff in the database
            //create new save file
            Debug.Log("No existing save file (null). Creating new save file.");
            Save();
        }
        else
        {
            Debug.Log("Save file exists in database: loading it now...");
            data = (SaveContainer)JsonUtility.FromJson<SaveContainer>(loadedData);
            Debug.Log("Loaded save file data into data variable");

            ConversationTrigger.tokens = new HashSet<string>(data.tokens);
            Debug.Log("Set ConversationTrigger tokens to data.tokens");

        }


        // Read inventory-related tokens.
        InventoryController.ConvertTokensToInventory();
        BatterySystem.TokensToPower();

        Debug.Log("Loaded all options successfully.");


        // (.NET platforms only, local Windows machine saving)
        /*        if (File.Exists(WhereIsData()))
                {
                    // Prepare for data IO. (.NET platforms only, local Windows machine saving)
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(WhereIsData(), FileMode.Open);

                    // Deserialize the data. (.NET platforms only, local Windows machine saving)
                    SaveContainer data = (SaveContainer)bf.Deserialize(file);
                    file.Close();

                    //! Fields go here.
                    ConversationTrigger.tokens = new HashSet<string>(data.tokens);

                    // Read inventory-related tokens.
                    InventoryController.ConvertTokensToInventory();
                    BatterySystem.TokensToPower();

                    Debug.Log("Loaded all options successfully.");
                }
                else
                {
                    // No file exists? We should make the default one!
                    Debug.Log("No save found, creating default file.");
                    Save();
                }
                */
    }

    // Returns the path to the options file. (.NET platforms only, local Windows machine saving)
    public static string WhereIsData()
	{
		return Application.persistentDataPath + "/" + filename + ".dat";
	}

}

[Serializable]
class SaveContainer
{
	//! Fields go here.
	public List<string> tokens;
}