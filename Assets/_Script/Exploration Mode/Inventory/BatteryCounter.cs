using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BatteryCounter : MonoBehaviour {

    private int partsFound;
    private int partsNeeded;
    public Text partsFoundText;
    public PartCounter partCounter;
    private string whatToBuild;
    private bool partsDone = false; // has the player collected all the parts they need yet? 

    // Use this for initialization
    void Start () {
        partsFound = 13;
	}

    public void incParts()
    {
        partsFound++;
        partsFoundText.text = "Battery Parts: " + partsFound + "/" + partsNeeded;
        if (partsFound == partsNeeded)
        {
            partsDone = true;
        }
        if (partsDone && partCounter.allPartsCollected()) {
            InventoryController.levelName = SceneManager.GetActiveScene().name;
            LoadUtils.LoadScene(whatToBuild);
        }
    }

    public bool allPartsCollected()
    {
        return partsDone;
    }

    public void setWhatToBuild(string whatToBuild)
    {
        this.whatToBuild = whatToBuild;

    }

    public void setPartsNeeded(int partsNeeded)
    {
        this.partsNeeded = partsNeeded;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
