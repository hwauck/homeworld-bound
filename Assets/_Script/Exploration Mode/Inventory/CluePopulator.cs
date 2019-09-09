﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CluePopulator : MonoBehaviour
{
	// References.
	public Image bigImage;

	// Internal Variables.
	List<GameObject> cluesInList = new List<GameObject>();
	GameObject clueBase;

	// Clue storage
	public static Dictionary<string, Sprite> clues = new Dictionary<string, Sprite>();

	void Awake()
	{
		clueBase = Resources.Load<GameObject>("Prefabs/ClueTile");
		Repopulate();
	}

	// Scans the list of items, adds icons for each one.
	public void Repopulate()
	{
		// Clear out the big image.
		bigImage.color = new Color(1f, 1f, 1f, 0f);

		// Destroy everything currently in the list.
		foreach (GameObject ii in cluesInList)
		{
			Destroy(ii);
		}

		// Convert special tokens to clues.
		// Clue sprites must be under Resources/Clues for this to work.
		//! Clue sprites must also have the same name as the item name.
		foreach (string ii in ConversationTrigger.tokens)
		{
			if (ii.Contains("clue_"))
			{
				string clueName = ii.Substring(5);
				AddClue(clueName, Resources.Load<Sprite>("Clues/" + clueName));
			}
		}

		// Actually repopulate.
		foreach (KeyValuePair<string, Sprite> ii in clues)
		//foreach (InvItem ii in InventoryController.items)
		{
			// Create the object.
			GameObject instance = Instantiate(clueBase);
			ClueButtonBridge cbb = instance.GetComponent<ClueButtonBridge>();
			cbb.bigImage = bigImage;
			cbb.clueSprite = ii.Value;

            // Set parent and internals.
            instance.transform.SetParent(this.transform, false);
			instance.GetComponent<Image>().sprite = ii.Value;

			// Check if the clue has already been solved.
			// Hardcoding this for the sake of sanity.
			if (ConversationTrigger.GetToken("autodelete_Key 1 Dangly T") && ii.Key == "CityPart1" ||
				ConversationTrigger.GetToken("autodelete_Key 1 Upright L") && ii.Key == "CityPart2" ||
				ConversationTrigger.GetToken("autodelete_Key 1 Upright Rect") && ii.Key == "CityPart3" ||
				ConversationTrigger.GetToken("autodelete_Key 1 Upright T") && ii.Key == "CityPart4" ||
				ConversationTrigger.GetToken("autodelete_Key 1 Walking Pants") && ii.Key == "CityPart5" ||
				ConversationTrigger.GetToken("autodelete_Key 1 Waluigi") && ii.Key == "CityPart6")
			{
				instance.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f);
			}

			// Add to object list.
			cluesInList.Add(instance);
		}
	}

	public static void AddClue(string clueName, Sprite clueSprite)
	{
		if (!clues.ContainsKey(clueName))
			clues.Add(clueName, clueSprite);
	}
}
