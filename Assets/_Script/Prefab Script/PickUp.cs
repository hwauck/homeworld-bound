using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public AudioSource PartsSounds;
    public AudioClip WindChimes;
    public AudioSource Pickup;
	public AudioClip pickupSound;
    public GameObject partCounterObj;
    public ExplorationLevelResetter levelResetter;
	public enum PickupType { Item, Battery, Clue, Fuser };
    private bool fromSave = false; // is this part being picked up from a loaded save file or not? Set by InventoryController.ConvertTokensToInventory()

	[Header("Basic Variables")]
	public PickupType type = PickupType.Item;
	// Pickup-type Settings.
	public string pickupName = "";	// Give it a name, consider this a type. IE should be unique.
	public string pickupDesc = "";  // This is mostly an internal tag. Doesn't really get used.

	[Tooltip("This MUST BE SET for an item to save. It should look like PartPickups/RocketBoots/Pickup_Boots_Body or similar.")]
	public string prefabPath = "";

	// SPECIAL
	// Only necessary for clue:
	[Header("Special Variables")]
	public Sprite clueSprite;

	[Tooltip("If the token specified here exists, this pickup will not spawn (scene load only.)")]
	public string deleteToken = "";
	[Tooltip("If true, this pickup will automatically be unique, meaning it will not spawn again when reloading the scene. Does not work with batteries!")]
	public bool autoDelete = false;


    void Start()

    {
        if (WindChimes != null && PartsSounds != null)
        {
           
            
                PartsSounds.clip = WindChimes;
                PartsSounds.Play();
            

        }
		// check deleteToken.
		if ((deleteToken != "" && ConversationTrigger.GetToken(deleteToken)) ||
			(autoDelete && ConversationTrigger.GetToken("autodelete_" + pickupName)))
		{
            Destroy(gameObject);

        }

        // We need to rename all the bits under the model of the pickup, if there are any.
        // The reason for this is names will conflict with names inside construction mode, and
        // construction mode uses a ton of GameObject.Find... Weird things start to happen!
        //TODO better fix than this maybe? I've been putting this one off for a while.
        foreach (Transform ii in GetComponentsInChildren<Transform>())
		{
			//ii.name = "NAME CHANGED TO PREVENT BUGS";
			//ii.name += "_fix";
		}

    }

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player")
        {
            Pickup.clip = pickupSound;
            Pickup.Play();

            if (WindChimes != null && PartsSounds != null)
            {

                PartsSounds.Stop();

            }
			//SimpleData.WriteDataPoint("Pickup_Item", "", "", "", "", pickupName);
            //if(!levelResetter.hasTaggedFirstPart())
            //{
            //    levelResetter.setTaggedFirstPart(true);

            //    levelResetter.startCountdown();
            //}
            //SimpleData.WriteStringToFile("pickups.txt", Time.time + ",PICKUP," + pickupName);
            //ParticleSystem ps = GetComponent<ParticleSystem>();
            //ParticleSystem.MainModule psMain = GetComponent<ParticleSystem>().main;
            GameObject expDataManagerObj = GameObject.Find("DataCollectionManager");

            switch (type)
			{

				case PickupType.Item:
					// Add the item and update the tokens.
					InventoryController.Add(this, 1);
					InventoryController.ConvertInventoryToTokens();
                    if(expDataManagerObj != null)
                    {
                        expDataManagerObj.GetComponent<ExplorationDataManager>().AddPartCollected(gameObject.name);
                    }

                    PartCounter partCounter = partCounterObj.GetComponent<PartCounter>();

                    // Poke the build button so it can check if it needs to update.
                    //BuildButton.CheckRecipes();

                    if (gameObject.tag.Equals("rocketBoots"))
					{
						ConversationTrigger.AddToken("picked_up_a_boots_piece");
                        partCounter.setObjectToBuild("Rocket Boots");
                        partCounter.setPartsNeeded(7);
                        levelResetter.setWhatToBuild("rocketBoots");
                        gameObject.SetActive(false);

                    }
                    if (gameObject.tag.Equals("sledgehammer"))
					{
						ConversationTrigger.AddToken("picked_up_a_sledge_piece");
                        partCounter.setObjectToBuild("Sledgehammer");
                        partCounter.setPartsNeeded(12);
                        levelResetter.setWhatToBuild("sledgehammer");
                        gameObject.SetActive(false);

                    }
                    if (pickupName.Contains("Key1"))
					{
						ConversationTrigger.AddToken("picked_up_a_key1_piece");
                        partCounter.setObjectToBuild("Ruined City Key");
                        partCounter.setPartsNeeded(6);
                        levelResetter.setWhatToBuild("key1");
                        transform.position = new Vector3(-1000f, -1000f, -1000f); // temporary fix for loading save files

                    }
                    if (pickupName.Contains("FFA"))
					{
						ConversationTrigger.AddToken("picked_up_a_ffa_piece");
					}

                    // Object still needs to exist for the icon to work.
                    // Silly, but let's just shove it into a corner and forget about it.
                    // Also parents to the scene manager object so it rejects deletion as much as possible.
                    //transform.position = new Vector3(-1000f, -1000f, -1000f);

                    //turn aura green and disable pickup trigger if already picked up
                    //this.GetComponent<Collider>().enabled = false;
                    //psMain.startColor = new Color(0f, 255f, 0f, 255f);

					LoadUtils.IconParenter(this.gameObject);

                    // inc count of item parts and check if done collecting
                    partCounterObj.GetComponent<PartCounter>().incParts(fromSave);

                    break;
                
				case PickupType.Battery:


                    ConversationTrigger.AddToken("picked_up_a_battery");

                    InventoryController.Add(this, 1);
                    InventoryController.ConvertInventoryToTokens();
                    if (expDataManagerObj != null)
                    {
                        expDataManagerObj.GetComponent<ExplorationDataManager>().AddPartCollected(gameObject.name);
                    }

                    // inc count of battery parts and check if done collecting
                    // make sure battery parts UI elements are enabled (won't be for every first battery part pickup)

                    partCounterObj.GetComponent<BatteryCounter>().incParts(fromSave);
                    transform.position = new Vector3(-1000f, -1000f, -1000f);

                    //this.GetComponent<Collider>().enabled = false;
                    //psMain.startColor = new Color(0f, 255f, 0f, 255f);
                    break;

				case PickupType.Clue:
                    //save clue 
                    InventoryController.Add(this, 1);
                    InventoryController.ConvertInventoryToTokens();

                    CluePopulator.AddClue(pickupName, clueSprite);
					ConversationTrigger.AddToken("clue_" + pickupName);
					ConversationTrigger.AddToken("picked_up_a_clue");
                    Debug.Log("Picked up a clue!");
                    transform.position = new Vector3(-1000f, -1000f, -1000f);
                    levelResetter.inventorySystem.gameObject.SetActive(true);
                    break;
                case PickupType.Fuser:
                    transform.position = new Vector3(-1000f, -1000f, -1000f);
                    ConversationTrigger.AddToken("pickedUpFuser");
                    InventoryController.Add(this, 1);
                    InventoryController.ConvertInventoryToTokens();
                    // gross might have to fix this at some point
                    GameObject.FindWithTag("Player").GetComponent<Fuser>().ActivateFuserFirstLook();

                    break;
			}
			if (autoDelete)
			{
				ConversationTrigger.AddToken("autodelete_" + pickupName);
			}
        }
    }

    // indicate that this Pickup is being loaded into player's inventory from a loaded save file
    public void setFromSave(bool fromSave)
    {
        this.fromSave = fromSave;
    }




}
