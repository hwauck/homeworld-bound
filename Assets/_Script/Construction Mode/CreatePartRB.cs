using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartRB : MonoBehaviour {


	private GameObject[] instantiated;
	public GameObject[] parts;
	private bool[] partCreated;
	private Vector3 createLoc;
    private Vector3 offscreenCreateLoc;
    public Button[] partButtons;
    public GameObject eventSystem;
	private SelectPart selectionManager;
	public int NUM_PARTS;
	private GameObject startObject;

	public RotationGizmo rotateGizmo;
    private ConstructionDataManager dataManager;

    private const float MOVEMENT_SPEED = 100;
    private float step;
    private const float WAIT_TIME = 0.01f;

    // Use this for initialization
    void Awake () {

		//number of parts to fuse
		partCreated = new bool[NUM_PARTS];
		instantiated = new GameObject[NUM_PARTS];
		for(int i = 0; i < NUM_PARTS; i++) {
			partCreated[i] = false;
		}
		for(int i = 0; i < NUM_PARTS; i++) {
			instantiated[i] = null;
		}
		createLoc = new Vector3(-40, 25, 100);
		selectionManager = eventSystem.GetComponent<SelectPart>();

		//CHANGE this string to the name of your starting part
		startObject = GameObject.Find ("startObject");
        offscreenCreateLoc = new Vector3(-40, -60, 100);

        rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();

        // For data collection.
        GameObject dataManagerObject = GameObject.Find("DataCollectionManager");
        if (dataManagerObject != null)
        {
            dataManager = dataManagerObject.GetComponent<ConstructionDataManager>();
        }
    }

	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at boot from the front)


	//CHANGE these next 5 methods so that they refer to the 5 prefabs you made. This requires you to 
	// change most of the variables and strings in each method. For now, set the fuseLocation to the 
	// location of whatever part you're going to attach it to, set the fuseRotation to the location 
	// (0,0,0), and make acceptableRotations contain only one rotation: Quaternion.Euler (0,0,0). Later,
	// you will come back and change fuseLocation, fuseRotation, and acceptableRotations after testing.

	//returns list of objects body can fuse to
	public FuseAttributes ballfootFuses() {
		GameObject midfoot = GameObject.Find("midfootPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		if (midfoot != null) {
				
			Vector3 midfootPos = midfoot.transform.position;
			Vector3 fuseLocation = new Vector3 (midfootPos.x, midfootPos.y - 2.75f, midfootPos.z - 20);
			fuseLocations.Add ("midfoot_ballfoot_attach", fuseLocation);
			fuseRotations.Add ("midfoot_ballfoot_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (270, 180, 0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("midfoot_ballfoot_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes calfFuses() {
		GameObject widening = GameObject.Find ("wideningPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(widening != null) {
			Vector3 wideningPos = widening.transform.position;
			Vector3 fuseLocation = new Vector3 (wideningPos.x, wideningPos.y + 14.5f, wideningPos.z + 0.25f);
			fuseLocations.Add ("widening_calf_attach", fuseLocation);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("widening_calf_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (270,180,0);

			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
			fusePositions = new Dictionary<string, Quaternion[]>();
			fusePositions.Add ("widening_calf_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes midfootFuses() {
		GameObject heel = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		Vector3 heelPos = heel.transform.position;
		Vector3 fuseLocation = new Vector3 (heelPos.x, heelPos.y, heelPos.z - 24);
		fuseLocations.Add("heel_midfoot_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		fuseRotations.Add ("heel_midfoot_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
	
		Quaternion[] acceptableRotations = {acceptableRotation1};

		fusePositions.Add ("heel_midfoot_attach", acceptableRotations);

		newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes toeFuses() {
		GameObject ballfoot = GameObject.Find("ballfootPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(ballfoot != null) {
			Vector3 ballfootPos = ballfoot.transform.position;
			Vector3 fuseLocation = new Vector3 (ballfootPos.x, ballfootPos.y + 1.4f, ballfootPos.z - 14.5f);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

			fuseLocations.Add ("ballfoot_toe_attach", fuseLocation);
			fuseRotations.Add ("ballfoot_toe_attach", fuseRotation);
			Quaternion acceptableRotation = Quaternion.Euler (270,180,0);
			Quaternion[] acceptableRotations = {acceptableRotation};
			fusePositions.Add ("ballfoot_toe_attach", acceptableRotations);
		}


		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes trimFuses() {
		//can be fused to any strut
		GameObject calf = GameObject.Find("calf_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(calf != null) {
			Vector3 calfPos = calf.transform.position;
			Vector3 fuseLocation = new Vector3(calfPos.x, calfPos.y + 14.5f, calfPos.z);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

			fuseLocations.Add ("calf_trim_attach", fuseLocation);
			fuseRotations.Add ("calf_trim_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (270,0,0);

			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
			fusePositions.Add ("calf_trim_attach", acceptableRotations);

		}
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes wideningFuses() {
		//can be fused to any strut
		GameObject heel = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 heelPos = heel.transform.position;
		Vector3 fuseLocation = new Vector3(heelPos.x, heelPos.y + 11.5f, heelPos.z + 1.5f);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("heel_widening_attach", fuseLocation);
		fuseRotations.Add ("heel_widening_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);

		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("heel_widening_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}


	//when a new part is created, clear partsCreated
	public void clearPartsCreated() {
		for(int i = 0; i < partCreated.Length; i++) {
			partCreated[i] = false;
		}
		for(int i = 0; i < instantiated.Length; i++) {
			if(instantiated[i] != null && !instantiated[i].GetComponent<IsFused>().isFused) {
				Destroy(instantiated[i]);
                partButtons[i].interactable = true;
            }
        }
	}

    //when power failure occurs, delete all but starting part.
    // Called by LevelResetter
    public void destroyAllCreatedParts()
    {
        for (int i = 0; i < partCreated.Length; i++)
        {
            partCreated[i] = false;
        }
        for (int i = 0; i < instantiated.Length; i++)
        {
            if (instantiated[i] != null)
            {
                Destroy(instantiated[i]);
                partButtons[i].interactable = true;
            }
        }
    }

    // Makes the newly created part zip up from a lower point as it's created, making it seem like it was pulled up from the ground
    IEnumerator moveToStartingPosition(GameObject part)
    {
        // while the part hasn't reached its destination and while it hasn't been destroyed by choosing another part
        while (part != null && !part.transform.position.Equals(createLoc))
        {
            step = MOVEMENT_SPEED * Time.deltaTime;
            part.transform.position = Vector3.MoveTowards(part.transform.position, createLoc, step);

            yield return new WaitForSeconds(WAIT_TIME);
        }
    }

    //CHANGE these next 5 methods so that they refer to the 5 prefabs you made. This requires you to 
    // change most of the variables and strings in each method.	
    public void createBallfoot() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);
			GameObject newBallfoot = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newBallfoot)); // this creates the zooming up from the ground effect

            Transform ballfootMidfootAttach = newBallfoot.transform.Find("ballfoot_midfoot_attach");
			Transform ballfootToeAttach = newBallfoot.transform.Find("ballfoot_toe_attach");

			FuseAttributes fuseAtts = ballfootFuses ();

            // ballfoot_Midfoot_Attach
			ballfootMidfootAttach.gameObject.AddComponent<FuseBehavior>();
			ballfootMidfootAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ballfootMidfootAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Ballfoot"));

            ballfootMidfootAttach.gameObject.AddComponent<FaceSelector>();
            ballfootMidfootAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            ballfootMidfootAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            ballfootMidfootAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // ballfoot_Toe_Attach
            ballfootToeAttach.gameObject.AddComponent<FuseBehavior>();
			ballfootToeAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ballfootToeAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Ballfoot"));

            ballfootToeAttach.gameObject.AddComponent<FaceSelector>();
            ballfootToeAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            ballfootToeAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            ballfootToeAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newBallfoot;
			partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("ballfootPrefab(Clone)");
            dataManager.AddPartSelected("ballfoot");

		}
	}

	public void createCalf() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler(0,90,90);
			GameObject newCalf = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newCalf)); // this creates the zooming up from the ground effect

            Transform calfWideningAttach = newCalf.transform.Find("calf_widening_attach");
			Transform calfTrimAttach = newCalf.transform.Find("calf_trim_attach");

			FuseAttributes fuseAtts = calfFuses ();

            // calf_widening_attach
			calfWideningAttach.gameObject.AddComponent<FuseBehavior>();
			calfWideningAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			calfWideningAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Calf"));

            calfWideningAttach.gameObject.AddComponent<FaceSelector>();
            calfWideningAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            calfWideningAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            calfWideningAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // calf_trim_attach
            calfTrimAttach.gameObject.AddComponent<FuseBehavior>();
			calfTrimAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			calfTrimAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Calf"));

            calfTrimAttach.gameObject.AddComponent<FaceSelector>();
            calfTrimAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            calfTrimAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            calfTrimAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[1] = newCalf;
			partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("calf_harderPrefab(Clone)");
            dataManager.AddPartSelected("calf");


        }
    }

	public void createMidfoot() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (270,0,90);
			GameObject newMidfoot = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newMidfoot)); // this creates the zooming up from the ground effect

            Transform midfootHeelAttach = newMidfoot.transform.Find("midfoot_heel_attach");
			Transform midfootBallfootAttach = newMidfoot.transform.Find("midfoot_ballfoot_attach");

			FuseAttributes fuseAtts = midfootFuses ();

            // midfoot_heel_attach
			midfootHeelAttach.gameObject.AddComponent<FuseBehavior>();
			midfootHeelAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			midfootHeelAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Midfoot"));

            midfootHeelAttach.gameObject.AddComponent<FaceSelector>();
            midfootHeelAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            midfootHeelAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            midfootHeelAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // midfoot_ballfoot_attach
            midfootBallfootAttach.gameObject.AddComponent<FuseBehavior>();
			midfootBallfootAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			midfootBallfootAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Midfoot"));

            midfootBallfootAttach.gameObject.AddComponent<FaceSelector>();
            midfootBallfootAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            midfootBallfootAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            midfootBallfootAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[2] = newMidfoot;	
			partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("midfootPrefab(Clone)");
            dataManager.AddPartSelected("midfoot");


        }
    }

	public void createToe() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newToe = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newToe)); // this creates the zooming up from the ground effect

            Transform toeBallfootAttach = newToe.transform.Find("toe_ballfoot_attach");

			FuseAttributes fuseAtts = toeFuses ();

            // toe_ballfoot_attach
			toeBallfootAttach.gameObject.AddComponent<FuseBehavior>();
			toeBallfootAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			toeBallfootAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Toe"));

            toeBallfootAttach.gameObject.AddComponent<FaceSelector>();
            toeBallfootAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            toeBallfootAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            toeBallfootAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());


            instantiated[3] = newToe;
			partCreated[3] = true;
            partButtons[3].interactable = false;

            selectionManager.newPartCreated("toe_harderPrefab(Clone)");
            dataManager.AddPartSelected("toe");


        }
    }

	public void createTrim() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (270,270,0);		
			GameObject newTrim = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newTrim)); // this creates the zooming up from the ground effect

            Transform trimCalfAttach = newTrim.transform.Find("trim_calf_attach");

			FuseAttributes fuseAtts = trimFuses ();

            // trim_calf_attach
			trimCalfAttach.gameObject.AddComponent<FuseBehavior>();
			trimCalfAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			trimCalfAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Trim"));

            trimCalfAttach.gameObject.AddComponent<FaceSelector>();
            trimCalfAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            trimCalfAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            trimCalfAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[4] = newTrim;
			partCreated[4] = true;
            partButtons[4].interactable = false;

            selectionManager.newPartCreated("trim_harderPrefab(Clone)");
            dataManager.AddPartSelected("trim");


        }
    }

	public void createWidening() {
		if(!partCreated[5]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,270);		
			GameObject newWidening = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[5], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newWidening)); // this creates the zooming up from the ground effect

            Transform wideningHeelAttach = newWidening.transform.Find("widening_heel_attach");
			Transform wideningCalfAttach = newWidening.transform.Find("widening_calf_attach");

			FuseAttributes fuseAtts = wideningFuses ();

            // widening_heel_attach
			wideningHeelAttach.gameObject.AddComponent<FuseBehavior>();
			wideningHeelAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			wideningHeelAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Widening"));

            wideningHeelAttach.gameObject.AddComponent<FaceSelector>();
            wideningHeelAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            wideningHeelAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            wideningHeelAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // widening_calf_attach
            wideningCalfAttach.gameObject.AddComponent<FuseBehavior>();
			wideningCalfAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			wideningCalfAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Widening"));

            wideningCalfAttach.gameObject.AddComponent<FaceSelector>();
            wideningCalfAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            wideningCalfAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            wideningCalfAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[5] = newWidening;
			partCreated[5] = true;
            partButtons[5].interactable = false;

            selectionManager.newPartCreated("wideningPrefab(Clone)");
            dataManager.AddPartSelected("widening");


        }
    }

	//checks to see if an object has been fused already
	public bool alreadyFused(string part) {
		GameObject partInstance = GameObject.Find(part);
		if(partInstance != null && !partInstance.GetComponent<FuseBehavior>().fused()) {
			return false;
		} else {
			return true;
		}
	}
	// Update is called once per frame
	void Update () {

	}
}
