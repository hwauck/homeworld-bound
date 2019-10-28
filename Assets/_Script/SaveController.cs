﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public class SaveController : MonoBehaviour
{

    // The old save system: saves all conversation tokens to a file and then loads them back, along with inventory items and battery power previously collected
    // The new system needs to keep track of not just conversation tokens, but also which parts have been picked up (batteries, fuser, item parts, and clues) and 
    // which mode the player ended in (Construction Mode or Exploration Mode). And keep track of telemetry data from previous attempt (start timed and counted things 
    // starting from most recent Attempt rather than starting a new Attempt

    // Can I do this all just with conversation tokens? No, will need to ask DataManager object (DataAggregator, ExplorationDataManager, and/or ConstructionDataManager) for info about
    // which parts have been collected and what Attempt we're currently on

   // Will need to make sure all part pickups have a corresponding and unique prefab associated with them, otherwise the SaveController.Load() method of instantiating a new prefab a
   // and dropping it on the player's head to start won't work. Or, we could just grab the existing object in the scene and move it on top of the player, hopefully that works. 

	// Filename for save. Allow loading of any string name.
	public static string filename = "Save";

    // set this to true to prevent SaveController.Load() from executing more than once per game session (see DataAggregator's initializeDataCollection() method)
    public static bool alreadyLoaded = false;

	// Saves a file containing all game options which are in the above. 
	public static void Save()
	{
		// Prepare for data IO.
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(WhereIsData());

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

		// Save and close safely.
		bf.Serialize(file, data);
		file.Close();

		Debug.Log("Saved all options successfully.");
	}

	// Applies all options from the saved file to the locally created variables.
	public static void Load()
	{
		// Make sure all the strings for conversations are loaded.
		ConversationsDB.LoadConversationsFromFiles();

		if (File.Exists(WhereIsData()))
		{
			// Prepare for data IO.
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(WhereIsData(), FileMode.Open);

			// Deserialize the data.
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
	}

	// Returns the path to the options file.
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