using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartSledge : MonoBehaviour {

	private GameObject[] instantiated;
	public GameObject[] parts;
    public Button[] partButtons;
    private bool[] partCreated;
	private Vector3 createLoc;
    private Vector3 offscreenCreateLoc;
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
	public FuseAttributes bottomPointLeftFuses() {
		GameObject head = GameObject.Find("head_harderPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		if (head != null) {

			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x + 6, headPos.y - 23.9f, headPos.z);
			fuseLocations.Add ("head_bottom_point_left_attach", fuseLocation);
			fuseRotations.Add ("head_bottom_point_left_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (90, 0, 0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("head_bottom_point_left_attach", acceptableRotations);

			fuseLocations.Add ("bottom_point_right_left_attach", fuseLocation);
			fuseRotations.Add ("bottom_point_right_left_attach", fuseRotation);

			fusePositions.Add ("bottom_point_right_left_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes bottomPointRightFuses() {
		GameObject head = GameObject.Find ("head_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(head != null) {
			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x - 6, headPos.y - 23.9f, headPos.z);
			fuseLocations.Add ("head_bottom_point_right_attach", fuseLocation);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("head_bottom_point_right_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (90,0,0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("head_bottom_point_right_attach", acceptableRotations);

			fuseLocations.Add ("bottom_point_left_right_attach", fuseLocation);
			fuseRotations.Add ("bottom_point_left_right_attach", fuseRotation);

			fusePositions.Add ("bottom_point_left_right_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes haftFuses() {
		GameObject shaft = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3 (shaftPos.x, shaftPos.y - 40, shaftPos.z);
		fuseLocations.Add("shaft_haft_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		fuseRotations.Add ("shaft_haft_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);

		Quaternion[] acceptableRotations = {acceptableRotation1};

		fusePositions.Add ("shaft_haft_attach", acceptableRotations);

		newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes headFuses() {
		GameObject trapezoid = GameObject.Find("trapezoid_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(trapezoid != null) {
			Vector3 trapezoidPos = trapezoid.transform.position;
			Vector3 fuseLocation = new Vector3 (trapezoidPos.x, trapezoidPos.y + 6, trapezoidPos.z - 12);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

			fuseLocations.Add ("trapezoid_head_attach", fuseLocation);
			fuseRotations.Add ("trapezoid_head_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("trapezoid_head_attach", acceptableRotations);
		}


		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes smallTipFuses() {
		GameObject smallTrapezoid = GameObject.Find("small_trapezoidPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(smallTrapezoid != null) {
			Vector3 smallTrapezoidPos = smallTrapezoid.transform.position;
			Vector3 fuseLocation = new Vector3(smallTrapezoidPos.x, smallTrapezoidPos.y - 3.8f, smallTrapezoidPos.z + 9.7f);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

			fuseLocations.Add ("small_trapezoid_small_tip_attach", fuseLocation);
			fuseRotations.Add ("small_trapezoid_small_tip_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("small_trapezoid_small_tip_attach", acceptableRotations);

		}
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes smallTrapezoidFuses() {
		//can be fused to any strut
		GameObject shaft = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3(shaftPos.x, shaftPos.y + 28, shaftPos.z + 9.5f);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("shaft_small_trapezoid_attach", fuseLocation);
		fuseRotations.Add ("shaft_small_trapezoid_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);

		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("shaft_small_trapezoid_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes spikeFuses() {
		//can be fused to any strut
		GameObject shaft = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3(shaftPos.x, shaftPos.y + 40.5f, shaftPos.z);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("shaft_spike_attach", fuseLocation);
		fuseRotations.Add ("shaft_spike_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);

		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("shaft_spike_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes tipFuses() {
		GameObject head = GameObject.Find("head_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if (head != null) {
			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x, headPos.y - 5.9f, headPos.z - 9);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3 (0, 180, 0));

			fuseLocations.Add ("head_tip_attach", fuseLocation);
			fuseRotations.Add ("head_tip_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270, 180, 0);

            Quaternion[] acceptableRotations = { acceptableRotation1 };
			fusePositions.Add ("head_tip_attach", acceptableRotations);

		}
		FuseAttributes newAttributes = new FuseAttributes (fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes topPointLeftFuses() {
		GameObject head = GameObject.Find("head_harderPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		if (head != null) {

			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x + 6, headPos.y + 9.3f, headPos.z + 5f);
			fuseLocations.Add ("head_top_point_left_attach", fuseLocation);
			fuseRotations.Add ("head_top_point_left_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (270, 180, 0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("head_top_point_left_attach", acceptableRotations);

			fuseLocations.Add ("top_point_right_left_attach", fuseLocation);
			fuseRotations.Add ("top_point_right_left_attach", fuseRotation);

			fusePositions.Add ("top_point_right_left_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes topPointRightFuses() {
		GameObject head = GameObject.Find("head_harderPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		if (head != null) {

			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x - 6, headPos.y + 8.7f, headPos.z + 4.3f);
			fuseLocations.Add ("head_top_point_right_attach", fuseLocation);
			fuseRotations.Add ("head_top_point_right_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (270, 180, 0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("head_top_point_right_attach", acceptableRotations);

			fuseLocations.Add ("top_point_left_right_attach", fuseLocation);
			fuseRotations.Add ("top_point_left_right_attach", fuseRotation);

			fusePositions.Add ("top_point_left_right_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes trapezoidFuses() {
		//can be fused to any strut
		GameObject shaft = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3(shaftPos.x, shaftPos.y + 28f, shaftPos.z - 12);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("shaft_trapezoid_attach", fuseLocation);
		fuseRotations.Add ("shaft_trapezoid_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);

		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("shaft_trapezoid_attach", acceptableRotations);

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
	public void createBottomPointRight() {
		if(!partCreated[0]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler(0,90,90);
			GameObject newBottomPointRight = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newBottomPointRight)); // this creates the zooming up from the ground effect

            GameObject bottomPointRightHeadAttach = GameObject.Find("bottom_point_right_head_attach");
			GameObject bottomPointRightLeftAttach = GameObject.Find("bottom_point_right_left_attach");

			FuseAttributes fuseAtts = bottomPointRightFuses ();

            // bottomPointRightHeadAttach
			bottomPointRightHeadAttach.AddComponent<FuseBehavior>();
			bottomPointRightHeadAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bottomPointRightHeadAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BottomPointRight"));

            bottomPointRightHeadAttach.gameObject.AddComponent<FaceSelector>();
            bottomPointRightHeadAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            bottomPointRightHeadAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            bottomPointRightHeadAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            bottomPointRightLeftAttach.AddComponent<FuseBehavior>();
			bottomPointRightLeftAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bottomPointRightLeftAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BottomPointRight"));

            bottomPointRightLeftAttach.gameObject.AddComponent<FaceSelector>();
            bottomPointRightLeftAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            bottomPointRightLeftAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            bottomPointRightLeftAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newBottomPointRight;
			partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("bottom_point_rightPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("bottom_point_right");
            }

        }
	}

    public void createBottomPointLeft()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 0);
            GameObject newBottomPointLeft = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newBottomPointLeft)); // this creates the zooming up from the ground effect

            GameObject bottomPointLeftHeadAttach = GameObject.Find("bottom_point_left_head_attach");
            GameObject bottomPointLeftRightAttach = GameObject.Find("bottom_point_left_right_attach");

            FuseAttributes fuseAtts = bottomPointLeftFuses();

            // bottomPointLeftHeadAttach

            bottomPointLeftHeadAttach.AddComponent<FuseBehavior>();
            bottomPointLeftHeadAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            bottomPointLeftHeadAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("BottomPointLeft"));

            bottomPointLeftHeadAttach.gameObject.AddComponent<FaceSelector>();
            bottomPointLeftHeadAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            bottomPointLeftHeadAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            bottomPointLeftHeadAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // bottomPointLeftRightAttach
            bottomPointLeftRightAttach.AddComponent<FuseBehavior>();
            bottomPointLeftRightAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            bottomPointLeftRightAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("BottomPointLeft"));

            bottomPointLeftRightAttach.gameObject.AddComponent<FaceSelector>();
            bottomPointLeftRightAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            bottomPointLeftRightAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            bottomPointLeftRightAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[1] = newBottomPointLeft;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("bottom_point_leftPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("bottom_point_left");
            }

        }
    }

    public void createHaft() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (270,270,0);
			GameObject newHaft = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newHaft)); // this creates the zooming up from the ground effect

            Transform haftShaftAttach = newHaft.transform.Find("haft_shaft_attach");

			FuseAttributes fuseAtts = haftFuses ();

            // haftShaftAttach
			haftShaftAttach.gameObject.AddComponent<FuseBehavior>();
			haftShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			haftShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Haft"));

            haftShaftAttach.gameObject.AddComponent<FaceSelector>();
            haftShaftAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            haftShaftAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            haftShaftAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[2] = newHaft;	
			partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("haftPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("haft");
            }


        }
	}

	public void createHead() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler(90,180,0);
			GameObject newHead = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newHead)); // this creates the zooming up from the ground effect

            Transform headTrapezoidAttach = newHead.transform.Find("head_trapezoid_attach");
			Transform headTipAttach = newHead.transform.Find("head_tip_attach");
			Transform headBottomPointLeftAttach = newHead.transform.Find("head_bottom_point_left_attach");
			Transform headBottomPointRightAttach = newHead.transform.Find("head_bottom_point_right_attach");
			Transform headTopPointLeftAttach = newHead.transform.Find("head_top_point_left_attach");
			Transform headTopPointRightAttach = newHead.transform.Find("head_top_point_right_attach");

			FuseAttributes fuseAtts = headFuses ();

            // headTrapezoidAttach
			headTrapezoidAttach.gameObject.AddComponent<FuseBehavior>();
			headTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

            headTrapezoidAttach.gameObject.AddComponent<FaceSelector>();
            headTrapezoidAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            headTrapezoidAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            headTrapezoidAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // headTipAttach
            headTipAttach.gameObject.AddComponent<FuseBehavior>();
			headTipAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTipAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

            headTipAttach.gameObject.AddComponent<FaceSelector>();
            headTipAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            headTipAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            headTipAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // headBottomPointLeftAttach
            headBottomPointLeftAttach.gameObject.AddComponent<FuseBehavior>();
			headBottomPointLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headBottomPointLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

            headBottomPointLeftAttach.gameObject.AddComponent<FaceSelector>();
            headBottomPointLeftAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            headBottomPointLeftAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            headBottomPointLeftAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //headBottomPointRightAttach
            headBottomPointRightAttach.gameObject.AddComponent<FuseBehavior>();
			headBottomPointRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headBottomPointRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

            headBottomPointRightAttach.gameObject.AddComponent<FaceSelector>();
            headBottomPointRightAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            headBottomPointRightAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            headBottomPointRightAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // headTopPointLeftAttach
            headTopPointLeftAttach.gameObject.AddComponent<FuseBehavior>();
			headTopPointLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTopPointLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

            headTopPointLeftAttach.gameObject.AddComponent<FaceSelector>();
            headTopPointLeftAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            headTopPointLeftAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            headTopPointLeftAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // headTopPointRightAttach
            headTopPointRightAttach.gameObject.AddComponent<FuseBehavior>();
			headTopPointRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTopPointRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

            headTopPointRightAttach.gameObject.AddComponent<FaceSelector>();
            headTopPointRightAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            headTopPointRightAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            headTopPointRightAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[3] = newHead;
			partCreated[3] = true;
            partButtons[3].interactable = false;

            selectionManager.newPartCreated("head_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("head_harder");
            }


        }
	}

	public void createSmallTip() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,0,270);		
			GameObject newSmallTip = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newSmallTip)); // this creates the zooming up from the ground effect

            Transform smallTipTrapezoidAttach = newSmallTip.transform.Find("small_tip_small_trapezoid_attach");

			FuseAttributes fuseAtts = smallTipFuses ();

            // smallTipTrapezoidAttach
			smallTipTrapezoidAttach.gameObject.AddComponent<FuseBehavior>();
			smallTipTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			smallTipTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("SmallTip"));

            smallTipTrapezoidAttach.gameObject.AddComponent<FaceSelector>();
            smallTipTrapezoidAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            smallTipTrapezoidAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            smallTipTrapezoidAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[4] = newSmallTip;
			partCreated[4] = true;
            partButtons[4].interactable = false;

            selectionManager.newPartCreated("small_tipPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("small_tip");
            }


        }
	}

    public void createTrapezoid()
    {
        if (!partCreated[5])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 270);
            GameObject newTrapezoid = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[5], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newTrapezoid)); // this creates the zooming up from the ground effect

            Transform trapezoidShaftAttach = newTrapezoid.transform.Find("trapezoid_shaft_attach");
            Transform trapezoidHeadAttach = newTrapezoid.transform.Find("trapezoid_head_attach");

            FuseAttributes fuseAtts = trapezoidFuses();

            // trapezoidShaftAttach
            trapezoidShaftAttach.gameObject.AddComponent<FuseBehavior>();
            trapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            trapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("Trapezoid"));

            trapezoidShaftAttach.gameObject.AddComponent<FaceSelector>();
            trapezoidShaftAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            trapezoidShaftAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            trapezoidShaftAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // trapezoidHeadAttach
            trapezoidHeadAttach.gameObject.AddComponent<FuseBehavior>();
            trapezoidHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            trapezoidHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("Trapezoid"));

            trapezoidHeadAttach.gameObject.AddComponent<FaceSelector>();
            trapezoidHeadAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            trapezoidHeadAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            trapezoidHeadAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[5] = newTrapezoid;
            partCreated[5] = true;
            partButtons[5].interactable = false;

            selectionManager.newPartCreated("trapezoid_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("trapezoid_harder");
            }


        }
    }

    public void createTopPointLeft()
    {
        if (!partCreated[6])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 0, 180);
            GameObject newTopPointLeft = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[6], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newTopPointLeft)); // this creates the zooming up from the ground effect

            Transform topPointLeftHeadAttach = newTopPointLeft.transform.Find("top_point_left_head_attach");
            Transform topPointLeftRightAttach = newTopPointLeft.transform.Find("top_point_left_right_attach");

            FuseAttributes fuseAtts = topPointLeftFuses();

            // topPointLeftHeadAttach
            topPointLeftHeadAttach.gameObject.AddComponent<FuseBehavior>();
            topPointLeftHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            topPointLeftHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("TopPointLeft"));

            topPointLeftHeadAttach.gameObject.AddComponent<FaceSelector>();
            topPointLeftHeadAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            topPointLeftHeadAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            topPointLeftHeadAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // topPointLeftRightAttach
            topPointLeftRightAttach.gameObject.AddComponent<FuseBehavior>();
            topPointLeftRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            topPointLeftRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("TopPointLeft"));

            topPointLeftRightAttach.gameObject.AddComponent<FaceSelector>();
            topPointLeftRightAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            topPointLeftRightAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            topPointLeftRightAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[6] = newTopPointLeft;
            partCreated[6] = true;
            partButtons[6].interactable = false;

            selectionManager.newPartCreated("top_point_leftPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("top_point_left");
            }


        }
    }

    public void createTopPointRight()
    {
        if (!partCreated[7])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 90, 0);
            GameObject newTopPointRight = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[7], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newTopPointRight)); // this creates the zooming up from the ground effect

            Transform topPointRightHeadAttach = newTopPointRight.transform.Find("top_point_right_head_attach");
            Transform topPointRightLeftAttach = newTopPointRight.transform.Find("top_point_right_left_attach");

            FuseAttributes fuseAtts = topPointRightFuses();

            // topPointRightHeadAttach
            topPointRightHeadAttach.gameObject.AddComponent<FuseBehavior>();
            topPointRightHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            topPointRightHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("TopPointRight"));

            topPointRightHeadAttach.gameObject.AddComponent<FaceSelector>();
            topPointRightHeadAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            topPointRightHeadAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            topPointRightHeadAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // topPointRightLeftAttach
            topPointRightLeftAttach.gameObject.AddComponent<FuseBehavior>();
            topPointRightLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            topPointRightLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("TopPointRight"));

            topPointRightLeftAttach.gameObject.AddComponent<FaceSelector>();
            topPointRightLeftAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            topPointRightLeftAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            topPointRightLeftAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[7] = newTopPointRight;
            partCreated[7] = true;
            partButtons[7].interactable = false;

            selectionManager.newPartCreated("top_point_rightPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("top_point_right");
            }


        }
    }

    public void createSmallTrapezoid()
    {
        if (!partCreated[8])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(270, 90, 0);
            GameObject newSmallTrapezoid = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[8], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newSmallTrapezoid)); // this creates the zooming up from the ground effect

            Transform smallTrapezoidSmallTipAttach = newSmallTrapezoid.transform.Find("small_trapezoid_small_tip_attach");
            Transform smallTrapezoidShaftAttach = newSmallTrapezoid.transform.Find("small_trapezoid_shaft_attach");

            FuseAttributes fuseAtts = smallTrapezoidFuses();

            // smallTrapezoidSmallTipAttach
            smallTrapezoidSmallTipAttach.gameObject.AddComponent<FuseBehavior>();
            smallTrapezoidSmallTipAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            smallTrapezoidSmallTipAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("SmallTrapezoid"));

            smallTrapezoidSmallTipAttach.gameObject.AddComponent<FaceSelector>();
            smallTrapezoidSmallTipAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            smallTrapezoidSmallTipAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            smallTrapezoidSmallTipAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            // smallTrapezoidShaftAttach
            smallTrapezoidShaftAttach.gameObject.AddComponent<FuseBehavior>();
            smallTrapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            smallTrapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("SmallTrapezoid"));

            smallTrapezoidShaftAttach.gameObject.AddComponent<FaceSelector>();
            smallTrapezoidShaftAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            smallTrapezoidShaftAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            smallTrapezoidShaftAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[8] = newSmallTrapezoid;
            partCreated[8] = true;
            partButtons[8].interactable = false;

            selectionManager.newPartCreated("small_trapezoidPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("small_trapezoid");
            }


        }
    }

    public void createTip()
    {
        if (!partCreated[9])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 0, 90);
            GameObject newTip = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[9], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newTip)); // this creates the zooming up from the ground effect

            Transform tipHeadAttach = newTip.transform.Find("tip_head_attach");

            FuseAttributes fuseAtts = tipFuses();

            // tipHeadAttach
            tipHeadAttach.gameObject.AddComponent<FuseBehavior>();
            tipHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            tipHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("Tip"));

            tipHeadAttach.gameObject.AddComponent<FaceSelector>();
            tipHeadAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            tipHeadAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            tipHeadAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[9] = newTip;
            partCreated[9] = true;
            partButtons[9].interactable = false;

            selectionManager.newPartCreated("tipPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("tip");
            }


        }
    }

    public void createSpike() {
		if(!partCreated[10]){
			clearPartsCreated();
			Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,270);		
			GameObject newSpike = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[10], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newSpike)); // this creates the zooming up from the ground effect

            Transform spikeShaftAttach = newSpike.transform.Find("spike_shaft_attach");

			FuseAttributes fuseAtts = spikeFuses ();

            // spikeShaftAttach
			spikeShaftAttach.gameObject.AddComponent<FuseBehavior>();
			spikeShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			spikeShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Spike"));

            spikeShaftAttach.gameObject.AddComponent<FaceSelector>();
            spikeShaftAttach.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            spikeShaftAttach.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            spikeShaftAttach.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[10] = newSpike;
			partCreated[10] = true;
            partButtons[10].interactable = false;

            selectionManager.newPartCreated("spikePrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("spike");
            }


        }
	}

	//checks to see if an object has been fused already
	public bool alreadyFused(string part) {
		GameObject partInstance = GameObject.Find(part);
		if(partInstance != null && !partInstance.GetComponent<FuseBehavior>().fused ()) {
			return false;
		} else {
			return true;
		}
	}

}
