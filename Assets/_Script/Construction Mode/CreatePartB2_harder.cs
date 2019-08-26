using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartB2_harder : MonoBehaviour
{

    private GameObject[] instantiated;
    public GameObject[] parts;
    private bool[] partCreated;
    public Button[] partButtons;
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
    void Awake()
    {
        //number of parts to fuse
        partCreated = new bool[NUM_PARTS];
        instantiated = new GameObject[NUM_PARTS];
        for (int i = 0; i < NUM_PARTS; i++)
        {
            partCreated[i] = false;
        }
        for (int i = 0; i < NUM_PARTS; i++)
        {
            instantiated[i] = null;
        }
        createLoc = new Vector3(-60, 30, 120);
        offscreenCreateLoc = new Vector3(-60, -60, 120);
        selectionManager = eventSystem.GetComponent<SelectPart>();
        startObject = GameObject.Find("bb2_harderStart");

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

    //returns list of objects body can fuse to
    public FuseAttributes b2p1Fuses()
    {
        GameObject bb2 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb2Pos = bb2.transform.position;
        Vector3 fuseLocation = new Vector3(bb2Pos.x, bb2Pos.y, bb2Pos.z+10);
        fuseLocations.Add("bb2_b2p1_a1", fuseLocation);
        fuseLocations.Add("bb2_b2p1_a2", fuseLocation);
        fuseLocations.Add("bb2_b2p1_a3", fuseLocation);
        fuseLocations.Add("bb2_b2p1_a4", fuseLocation);
        fuseLocations.Add("b2p2_b2p1_a1", fuseLocation);
        fuseLocations.Add("b2p2_b2p1_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("bb2_b2p1_a1", fuseRotation);
        fuseRotations.Add("bb2_b2p1_a2", fuseRotation);
        fuseRotations.Add("bb2_b2p1_a3", fuseRotation);
        fuseRotations.Add("bb2_b2p1_a4", fuseRotation);
        fuseRotations.Add("b2p2_b2p1_a1", fuseRotation);
        fuseRotations.Add("b2p2_b2p1_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        //Quaternion acceptableRotation2 = Quaternion.Euler(0, 90, 270);
        //Quaternion acceptableRotation3 = Quaternion.Euler(90, 180, 0);
        //Quaternion acceptableRotation4 = Quaternion.Euler(0, 270, 90);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions.Add("bb2_b2p1_a1", acceptableRotations);
        fusePositions.Add("bb2_b2p1_a2", acceptableRotations);
        fusePositions.Add("bb2_b2p1_a3", acceptableRotations);
        fusePositions.Add("bb2_b2p1_a4", acceptableRotations);
        fusePositions.Add("b2p2_b2p1_a1", acceptableRotations);
        fusePositions.Add("b2p2_b2p1_a2", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b2p2Fuses()
    {
        GameObject bb2 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb2Pos = bb2.transform.position;
        Vector3 fuseLocation = new Vector3(bb2Pos.x - 5, bb2Pos.y, bb2Pos.z + 25);
        fuseLocations.Add("b2p1_b2p2_a1", fuseLocation);
        fuseLocations.Add("b2p1_b2p2_a2", fuseLocation);
        fuseLocations.Add("b2p3_b2p2_a1", fuseLocation);
        fuseLocations.Add("b2p3_b2p2_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b2p1_b2p2_a1", fuseRotation);
        fuseRotations.Add("b2p1_b2p2_a2", fuseRotation);
        fuseRotations.Add("b2p3_b2p2_a1", fuseRotation);
        fuseRotations.Add("b2p3_b2p2_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b2p1_b2p2_a1", acceptableRotations);
        fusePositions.Add("b2p1_b2p2_a2", acceptableRotations);
        fusePositions.Add("b2p3_b2p2_a1", acceptableRotations);
        fusePositions.Add("b2p3_b2p2_a2", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b2p3Fuses()
    {
        GameObject bb2 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb2Pos = bb2.transform.position;
        Vector3 fuseLocation = new Vector3(bb2Pos.x + 5.2f, bb2Pos.y, bb2Pos.z + 25);
        fuseLocations.Add("b2p2_b2p3_a1", fuseLocation);
        fuseLocations.Add("b2p2_b2p3_a2", fuseLocation);
        fuseLocations.Add("b2p1_b2p3_a1", fuseLocation);


        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b2p2_b2p3_a1", fuseRotation);
        fuseRotations.Add("b2p2_b2p3_a2", fuseRotation);
        fuseRotations.Add("b2p1_b2p3_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b2p2_b2p3_a1", acceptableRotations);
        fusePositions.Add("b2p2_b2p3_a2", acceptableRotations);
        fusePositions.Add("b2p1_b2p3_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    //when a new part is created, clear partsCreated
    public void clearPartsCreated()
    {
        for (int i = 0; i < partCreated.Length; i++)
        {
            partCreated[i] = false;
        }
        for (int i = 0; i < instantiated.Length; i++)
        {
            if (instantiated[i] != null && !instantiated[i].GetComponent<IsFused>().isFused)
            {
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
        //Debug.Log("finished moving " + part.name + "!");
    }


    public void createB2p1()
    {
        if (!partCreated[0])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(-90, 0, 0);
            GameObject newB2p1 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB2p1)); // this creates the zooming up from the ground effect

            Transform b2p1_bb2_a1 = newB2p1.transform.Find("b2p1_bb2_a1");
            Transform b2p1_bb2_a2 = newB2p1.transform.Find("b2p1_bb2_a2");
            Transform b2p1_bb2_a3 = newB2p1.transform.Find("b2p1_bb2_a3");
            Transform b2p1_bb2_a4 = newB2p1.transform.Find("b2p1_bb2_a4");
            Transform b2p1_b2p2_a1 = newB2p1.transform.Find("b2p1_b2p2_a1");
            Transform b2p1_b2p2_a2 = newB2p1.transform.Find("b2p1_b2p2_a2");

            FuseAttributes fuseAtts = b2p1Fuses();

            //b2p1_bb2_a1
            b2p1_bb2_a1.gameObject.AddComponent<FuseBehavior>();
            b2p1_bb2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p1_bb2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p1"));

            b2p1_bb2_a1.gameObject.AddComponent<FaceSelector>();
            b2p1_bb2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b2p1_bb2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p1_bb2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p1_bb2_a2
            b2p1_bb2_a2.gameObject.AddComponent<FuseBehavior>();
            b2p1_bb2_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p1_bb2_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p1"));

            b2p1_bb2_a2.gameObject.AddComponent<FaceSelector>();
            b2p1_bb2_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b2p1_bb2_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p1_bb2_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p1_bb2_a3
            b2p1_bb2_a3.gameObject.AddComponent<FuseBehavior>();
            b2p1_bb2_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p1_bb2_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p1"));

            b2p1_bb2_a3.gameObject.AddComponent<FaceSelector>();
            b2p1_bb2_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b2p1_bb2_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p1_bb2_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p1_bb2_a4
            b2p1_bb2_a4.gameObject.AddComponent<FuseBehavior>();
            b2p1_bb2_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p1_bb2_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p1"));

            b2p1_bb2_a4.gameObject.AddComponent<FaceSelector>();
            b2p1_bb2_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b2p1_bb2_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p1_bb2_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p1_b2p2_a1
            b2p1_b2p2_a1.gameObject.AddComponent<FuseBehavior>();
            b2p1_b2p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p1_b2p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p1"));

            b2p1_b2p2_a1.gameObject.AddComponent<FaceSelector>();
            b2p1_b2p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = new Vector3(1, -1, -1);
            b2p1_b2p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p1_b2p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p1_b2p2_a2
            b2p1_b2p2_a2.gameObject.AddComponent<FuseBehavior>();
            b2p1_b2p2_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p1_b2p2_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p1"));

            b2p1_b2p2_a2.gameObject.AddComponent<FaceSelector>();
            b2p1_b2p2_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b2p1_b2p2_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p1_b2p2_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newB2p1;
            partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("b2p1_harderPrefab(Clone)");
            if(dataManager != null)
            {
                dataManager.AddPartSelected("b2p1");
            }

        }
    }

    public void createB2p2()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(180, 0, 90);
            GameObject newB2p2 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB2p2)); // this creates the zooming up from the ground effect

            Transform b2p2_b2p1_a1 = newB2p2.transform.Find("b2p2_b2p1_a1");
            Transform b2p2_b2p1_a2 = newB2p2.transform.Find("b2p2_b2p1_a2");
            Transform b2p2_b2p3_a1 = newB2p2.transform.Find("b2p2_b2p3_a1");
            Transform b2p2_b2p3_a2 = newB2p2.transform.Find("b2p2_b2p3_a2");

            FuseAttributes fuseAtts = b2p2Fuses();

            //b2p2_b2p1_a1
            b2p2_b2p1_a1.gameObject.AddComponent<FuseBehavior>();
            b2p2_b2p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p2_b2p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p2"));

            b2p2_b2p1_a1.gameObject.AddComponent<FaceSelector>();
            b2p2_b2p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = new Vector3(1,1,-1);
            b2p2_b2p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p2_b2p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p2_b2p1_a2
            b2p2_b2p1_a2.gameObject.AddComponent<FuseBehavior>();
            b2p2_b2p1_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p2_b2p1_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p2"));

            b2p2_b2p1_a2.gameObject.AddComponent<FaceSelector>();
            b2p2_b2p1_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b2p2_b2p1_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p2_b2p1_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p2_b2p3_a1
            b2p2_b2p3_a1.gameObject.AddComponent<FuseBehavior>();
            b2p2_b2p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p2_b2p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p2"));

            b2p2_b2p3_a1.gameObject.AddComponent<FaceSelector>();
            b2p2_b2p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b2p2_b2p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p2_b2p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p2_b2p3_a2
            b2p2_b2p3_a2.gameObject.AddComponent<FuseBehavior>();
            b2p2_b2p3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p2_b2p3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p2"));

            b2p2_b2p3_a2.gameObject.AddComponent<FaceSelector>();
            b2p2_b2p3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = new Vector3(1, 1, 0);
            b2p2_b2p3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p2_b2p3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

 

            instantiated[1] = newB2p2;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("b2p2_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b2p2");
            }
        }
    }

    public void createB2p3 ()
    {
        if (!partCreated[2])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 180);
            GameObject newB2p3 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB2p3)); // this creates the zooming up from the ground effect

            Transform b2p3_b2p2_a1 = newB2p3.transform.Find("b2p3_b2p2_a1");
            Transform b2p3_b2p2_a2 = newB2p3.transform.Find("b2p3_b2p2_a2");

            FuseAttributes fuseAtts = b2p3Fuses();

            //b2p2_b2p1_a1
            b2p3_b2p2_a1.gameObject.AddComponent<FuseBehavior>();
            b2p3_b2p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p3_b2p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p3"));

            b2p3_b2p2_a1.gameObject.AddComponent<FaceSelector>();
            b2p3_b2p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b2p3_b2p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p3_b2p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b2p2_b2p1_a2
            b2p3_b2p2_a2.gameObject.AddComponent<FuseBehavior>();
            b2p3_b2p2_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b2p3_b2p2_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B2p3"));

            b2p3_b2p2_a2.gameObject.AddComponent<FaceSelector>();
            b2p3_b2p2_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = new Vector3(-1, -1, 0);
            b2p3_b2p2_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b2p3_b2p2_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[2] = newB2p3;
            partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("b2p3_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b2p3");
            }
        }
    }

    //checks to see if an object has been fused already
    public bool alreadyFused(string part)
    {
        GameObject partInstance = GameObject.Find(part);
        if (partInstance != null && !partInstance.GetComponent<FuseBehavior>().fused())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}

