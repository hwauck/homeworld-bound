using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartCounter : MonoBehaviour {

    private int partsFound;
    private int partsNeeded;
    public Text partsFoundText;
    private string whatToBuild;
    private string objectToBuild;
    public BatteryCounter batteryCounter;
    private bool partsDone = false; // has the player collected all the parts they need yet? 

	// Use this for initialization
	void Start () {
        partsFound = 5;
	}

    public void incParts()
    {
        partsFound++;
        partsFoundText.text = objectToBuild + " Parts: " + partsFound + "/" + partsNeeded;
        if(partsFound==partsNeeded)
        {
            partsDone = true;
        }
        if(partsDone && batteryCounter.allPartsCollected()) {
            InventoryController.levelName = SceneManager.GetActiveScene().name;
            LoadUtils.LoadScene(whatToBuild);
        }
    }

    public bool allPartsCollected()
    {
        return partsDone;
    }


    public void setWhatToBuild(string whatToBuild, string objectToBuild)
    {
        this.whatToBuild = whatToBuild;
        batteryCounter.setWhatToBuild(whatToBuild);
        this.objectToBuild = objectToBuild;
    }

    public void setPartsNeeded(int partsNeeded)
    {
        this.partsNeeded = partsNeeded;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
