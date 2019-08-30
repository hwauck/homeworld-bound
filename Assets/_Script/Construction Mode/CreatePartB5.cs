using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartB5 : MonoBehaviour
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
        createLoc = new Vector3(-60, 30, 100);
        offscreenCreateLoc = new Vector3(-60, -60, 100);
        selectionManager = eventSystem.GetComponent<SelectPart>();
        startObject = GameObject.Find("bb5Start");

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
    public FuseAttributes b5p2Fuses()
    {
        GameObject bb5 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb5Pos = bb5.transform.position;
        Vector3 fuseLocation = new Vector3(bb5Pos.x, bb5Pos.y + 5.4f, bb5Pos.z - 23);
        fuseLocations.Add("bb5_b5p2_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("bb5_b5p2_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        //Quaternion acceptableRotation2 = Quaternion.Euler(0, 90, 270);
        //Quaternion acceptableRotation3 = Quaternion.Euler(90, 180, 0);
        //Quaternion acceptableRotation4 = Quaternion.Euler(0, 270, 90);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions.Add("bb5_b5p2_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b5p3Fuses()
    {
        GameObject bb5 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb5Pos = bb5.transform.position;
        Vector3 fuseLocation = new Vector3(bb5Pos.x, bb5Pos.y + 9.7f, bb5Pos.z - 15);
        fuseLocations.Add("b5p4_b5p3_a1", fuseLocation);
        fuseLocations.Add("b5p5_b5p3_a1", fuseLocation);
        fuseLocations.Add("bb5_b5p3_a1", fuseLocation);
        fuseLocations.Add("bb5_b5p3_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b5p4_b5p3_a1", fuseRotation);
        fuseRotations.Add("b5p5_b5p3_a1", fuseRotation);
        fuseRotations.Add("bb5_b5p3_a1", fuseRotation);
        fuseRotations.Add("bb5_b5p3_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1};
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b5p4_b5p3_a1", acceptableRotations);
        fusePositions.Add("b5p5_b5p3_a1", acceptableRotations);
        fusePositions.Add("bb5_b5p3_a1", acceptableRotations);
        fusePositions.Add("bb5_b5p3_a2", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b5p4Fuses()
    {
        GameObject bb5 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb5Pos = bb5.transform.position;
        Vector3 fuseLocation = new Vector3(bb5Pos.x - 5, bb5Pos.y + 4.9f, bb5Pos.z - 0.4f);
        fuseLocations.Add("b5p3_b5p4_a1", fuseLocation);
        fuseLocations.Add("b5p5_b5p4_a1", fuseLocation);
        fuseLocations.Add("b5p5_b5p4_a2", fuseLocation);
        fuseLocations.Add("bb5_b5p4_a1", fuseLocation);
        fuseLocations.Add("bb5_b5p4_a2", fuseLocation);
        fuseLocations.Add("bb5_b5p4_a3", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b5p3_b5p4_a1", fuseRotation);
        fuseRotations.Add("b5p5_b5p4_a1", fuseRotation);
        fuseRotations.Add("b5p5_b5p4_a2", fuseRotation);
        fuseRotations.Add("bb5_b5p4_a1", fuseRotation);
        fuseRotations.Add("bb5_b5p4_a2", fuseRotation);
        fuseRotations.Add("bb5_b5p4_a3", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b5p3_b5p4_a1", acceptableRotations);
        fusePositions.Add("b5p5_b5p4_a1", acceptableRotations);
        fusePositions.Add("b5p5_b5p4_a2", acceptableRotations);
        fusePositions.Add("bb5_b5p3_a1", acceptableRotations);
        fusePositions.Add("bb5_b5p3_a2", acceptableRotations);
        fusePositions.Add("bb5_b5p4_a3", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b5p5Fuses()
    {
        GameObject bb5 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb5Pos = bb5.transform.position;
        Vector3 fuseLocation = new Vector3(bb5Pos.x, bb5Pos.y + 4.9f, bb5Pos.z + 9.8f);
        fuseLocations.Add("b5p3_b5p5_a1", fuseLocation);
        fuseLocations.Add("b5p4_b5p5_a1", fuseLocation);
        fuseLocations.Add("b5p4_b5p5_a2", fuseLocation);
        fuseLocations.Add("bb5_b5p5_a1", fuseLocation);
        fuseLocations.Add("bb5_b5p5_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b5p3_b5p5_a1", fuseRotation);
        fuseRotations.Add("b5p4_b5p5_a1", fuseRotation);
        fuseRotations.Add("b5p4_b5p5_a2", fuseRotation);
        fuseRotations.Add("bb5_b5p5_a1", fuseRotation);
        fuseRotations.Add("bb5_b5p5_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b5p3_b5p5_a1", acceptableRotations);
        fusePositions.Add("b5p4_b5p5_a1", acceptableRotations);
        fusePositions.Add("b5p4_b5p5_a2", acceptableRotations);
        fusePositions.Add("bb5_b5p5_a1", acceptableRotations);
        fusePositions.Add("bb5_b5p5_a2", acceptableRotations);

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
    }


    public void createB5p2()
    {
        if (!partCreated[0])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(0, 270, 180);
            GameObject newB3p2 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB3p2)); // this creates the zooming up from the ground effect

            Transform b5p2_bb5_a1 = newB3p2.transform.Find("b5p2_bb5_a1");

            FuseAttributes fuseAtts = b5p2Fuses();

            //b5p2_bb5_a1
            b5p2_bb5_a1.gameObject.AddComponent<FuseBehavior>();
            b5p2_bb5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p2_bb5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p2"));

            b5p2_bb5_a1.gameObject.AddComponent<FaceSelector>();
            b5p2_bb5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b5p2_bb5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p2_bb5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newB3p2;
            partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("b5p2Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b5p2");
            }
        }
    }

    public void createB5p3()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(270, 90, 0);
            GameObject newB5p3 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB5p3)); // this creates the zooming up from the ground effect

            Transform b5p3_b5p4_a1 = newB5p3.transform.Find("b5p3_b5p4_a1");
            Transform b5p3_b5p5_a1 = newB5p3.transform.Find("b5p3_b5p5_a1");
            Transform b5p3_bb5_a1 = newB5p3.transform.Find("b5p3_bb5_a1");
            Transform b5p3_bb5_a2 = newB5p3.transform.Find("b5p3_bb5_a2");

            FuseAttributes fuseAtts = b5p3Fuses();

            //b5p3_b5p4_a1
            b5p3_b5p4_a1.gameObject.AddComponent<FuseBehavior>();
            b5p3_b5p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p3_b5p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p3"));

            b5p3_b5p4_a1.gameObject.AddComponent<FaceSelector>();
            b5p3_b5p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b5p3_b5p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p3_b5p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p3_b5p5_a1
            b5p3_b5p5_a1.gameObject.AddComponent<FuseBehavior>();
            b5p3_b5p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p3_b5p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p3"));

            b5p3_b5p5_a1.gameObject.AddComponent<FaceSelector>();
            b5p3_b5p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b5p3_b5p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p3_b5p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p3_bb5_a1
            b5p3_bb5_a1.gameObject.AddComponent<FuseBehavior>();
            b5p3_bb5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p3_bb5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p3"));

            b5p3_bb5_a1.gameObject.AddComponent<FaceSelector>();
            b5p3_bb5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b5p3_bb5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p3_bb5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p3_bb5_a2
            b5p3_bb5_a2.gameObject.AddComponent<FuseBehavior>();
            b5p3_bb5_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p3_bb5_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p3"));

            b5p3_bb5_a2.gameObject.AddComponent<FaceSelector>();
            b5p3_bb5_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b5p3_bb5_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p3_bb5_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[1] = newB5p3;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("b5p3Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b5p3");
            }
        }
    }

    public void createB5p4()
    {
        if (!partCreated[2])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 0);
            GameObject newB5p4 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB5p4)); // this creates the zooming up from the ground effect

            Transform b5p4_b5p3_a1 = newB5p4.transform.Find("b5p4_b5p3_a1");
            Transform b5p4_b5p5_a1 = newB5p4.transform.Find("b5p4_b5p5_a1");
            Transform b5p4_b5p5_a2 = newB5p4.transform.Find("b5p4_b5p5_a2");
            Transform b5p4_bb5_a1 = newB5p4.transform.Find("b5p4_bb5_a1");
            Transform b5p4_bb5_a2 = newB5p4.transform.Find("b5p4_bb5_a2");
            Transform b5p4_bb5_a3 = newB5p4.transform.Find("b5p4_bb5_a3");

            FuseAttributes fuseAtts = b5p4Fuses();

            //b5p4_b5p3_a1
            b5p4_b5p3_a1.gameObject.AddComponent<FuseBehavior>();
            b5p4_b5p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p4_b5p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p4"));

            b5p4_b5p3_a1.gameObject.AddComponent<FaceSelector>();
            b5p4_b5p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b5p4_b5p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p4_b5p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p4_b5p5_a1
            b5p4_b5p5_a1.gameObject.AddComponent<FuseBehavior>();
            b5p4_b5p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p4_b5p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p4"));

            b5p4_b5p5_a1.gameObject.AddComponent<FaceSelector>();
            b5p4_b5p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b5p4_b5p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p4_b5p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p4_b5p5_a2
            b5p4_b5p5_a2.gameObject.AddComponent<FuseBehavior>();
            b5p4_b5p5_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p4_b5p5_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p4"));

            b5p4_b5p5_a2.gameObject.AddComponent<FaceSelector>();
            b5p4_b5p5_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b5p4_b5p5_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p4_b5p5_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p4_bb5_a1
            b5p4_bb5_a1.gameObject.AddComponent<FuseBehavior>();
            b5p4_bb5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p4_bb5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p4"));

            b5p4_bb5_a1.gameObject.AddComponent<FaceSelector>();
            b5p4_bb5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b5p4_bb5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p4_bb5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p4_bb5_a2
            b5p4_bb5_a2.gameObject.AddComponent<FuseBehavior>();
            b5p4_bb5_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p4_bb5_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p4"));

            b5p4_bb5_a2.gameObject.AddComponent<FaceSelector>();
            b5p4_bb5_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b5p4_bb5_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p4_bb5_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p4_bb5_a3
            b5p4_bb5_a3.gameObject.AddComponent<FuseBehavior>();
            b5p4_bb5_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p4_bb5_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p4"));

            b5p4_bb5_a3.gameObject.AddComponent<FaceSelector>();
            b5p4_bb5_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b5p4_bb5_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p4_bb5_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[2] = newB5p4;
            partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("b5p4Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b5p4");
            }
        }
    }

    public void createB5p5()
    {
        if (!partCreated[3])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 0, 90);
            GameObject newB5p5 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[3], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB5p5)); // this creates the zooming up from the ground effect

            Transform b5p5_b5p3_a1 = newB5p5.transform.Find("b5p5_b5p3_a1");
            Transform b5p5_b5p4_a1 = newB5p5.transform.Find("b5p5_b5p4_a1");
            Transform b5p5_b5p4_a2 = newB5p5.transform.Find("b5p5_b5p4_a2");
            Transform b5p5_bb5_a1 = newB5p5.transform.Find("b5p5_bb5_a1");
            Transform b5p5_bb5_a2 = newB5p5.transform.Find("b5p5_bb5_a2");

            FuseAttributes fuseAtts = b5p5Fuses();

            //b5p5_b5p3_a1
            b5p5_b5p3_a1.gameObject.AddComponent<FuseBehavior>();
            b5p5_b5p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p5_b5p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p5"));

            b5p5_b5p3_a1.gameObject.AddComponent<FaceSelector>();
            b5p5_b5p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b5p5_b5p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p5_b5p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p5_b5p4_a1
            b5p5_b5p4_a1.gameObject.AddComponent<FuseBehavior>();
            b5p5_b5p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p5_b5p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p5"));

            b5p5_b5p4_a1.gameObject.AddComponent<FaceSelector>();
            b5p5_b5p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b5p5_b5p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p5_b5p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p5_b5p4_a2
            b5p5_b5p4_a2.gameObject.AddComponent<FuseBehavior>();
            b5p5_b5p4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p5_b5p4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p5"));

            b5p5_b5p4_a2.gameObject.AddComponent<FaceSelector>();
            b5p5_b5p4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b5p5_b5p4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p5_b5p4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p5_bb5_a1
            b5p5_bb5_a1.gameObject.AddComponent<FuseBehavior>();
            b5p5_bb5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p5_bb5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p5"));

            b5p5_bb5_a1.gameObject.AddComponent<FaceSelector>();
            b5p5_bb5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b5p5_bb5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p5_bb5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b5p5_bb5_a2
            b5p5_bb5_a2.gameObject.AddComponent<FuseBehavior>();
            b5p5_bb5_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b5p5_bb5_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B5p5"));

            b5p5_bb5_a2.gameObject.AddComponent<FaceSelector>();
            b5p5_bb5_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b5p5_bb5_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b5p5_bb5_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[3] = newB5p5;
            partCreated[3] = true;
            partButtons[3].interactable = false;

            selectionManager.newPartCreated("b5p5Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b5p5");
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

}

