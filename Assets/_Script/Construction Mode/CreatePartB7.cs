using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartB7 : MonoBehaviour
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
        createLoc = new Vector3(-60, 25, 80);
        offscreenCreateLoc = new Vector3(-60, -60, 80);
        selectionManager = eventSystem.GetComponent<SelectPart>();
        startObject = GameObject.Find("bb7Start");

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
    public FuseAttributes b7p1Fuses()
    {
        GameObject bb7 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb7Pos = bb7.transform.position;
        Vector3 fuseLocation = new Vector3(bb7Pos.x, bb7Pos.y, bb7Pos.z);
        fuseLocations.Add("b7p2_b7p1_a1", fuseLocation);
        fuseLocations.Add("b7p2_b7p1_a2", fuseLocation);
        fuseLocations.Add("b7p3_b7p1_a1", fuseLocation);
        fuseLocations.Add("b7p4_b7p1_a1", fuseLocation);
        fuseLocations.Add("b7p4_b7p1_a2", fuseLocation);
        fuseLocations.Add("b7p5_b7p1_a1", fuseLocation);
        fuseLocations.Add("b7p6_b7p1_a1", fuseLocation);
        fuseLocations.Add("bb7_b7p1_a1", fuseLocation);
        fuseLocations.Add("bb7_b7p1_a2", fuseLocation);
        fuseLocations.Add("bb7_b7p1_a3", fuseLocation);
        fuseLocations.Add("bb7_b7p1_a4", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b7p2_b7p1_a1", fuseRotation);
        fuseRotations.Add("b7p2_b7p1_a2", fuseRotation);
        fuseRotations.Add("b7p3_b7p1_a1", fuseRotation);
        fuseRotations.Add("b7p4_b7p1_a1", fuseRotation);
        fuseRotations.Add("b7p4_b7p1_a2", fuseRotation);
        fuseRotations.Add("b7p5_b7p1_a1", fuseRotation);
        fuseRotations.Add("b7p6_b7p1_a1", fuseRotation);
        fuseRotations.Add("bb7_b7p1_a1", fuseRotation);
        fuseRotations.Add("bb7_b7p1_a2", fuseRotation);
        fuseRotations.Add("bb7_b7p1_a3", fuseRotation);
        fuseRotations.Add("bb7_b7p1_a4", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        //Quaternion acceptableRotation2 = Quaternion.Euler(0, 90, 270);
        //Quaternion acceptableRotation3 = Quaternion.Euler(90, 180, 0);
        //Quaternion acceptableRotation4 = Quaternion.Euler(0, 270, 90);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions.Add("b7p2_b7p1_a1", acceptableRotations);
        fusePositions.Add("b7p2_b7p1_a2", acceptableRotations);
        fusePositions.Add("b7p3_b7p1_a1", acceptableRotations);
        fusePositions.Add("b7p4_b7p1_a1", acceptableRotations);
        fusePositions.Add("b7p4_b7p1_a2", acceptableRotations);
        fusePositions.Add("b7p5_b7p1_a1", acceptableRotations);
        fusePositions.Add("b7p6_b7p1_a1", acceptableRotations);
        fusePositions.Add("bb7_b7p1_a1", acceptableRotations);
        fusePositions.Add("bb7_b7p1_a2", acceptableRotations);
        fusePositions.Add("bb7_b7p1_a3", acceptableRotations);
        fusePositions.Add("bb7_b7p1_a4", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b7p2Fuses()
    {
        GameObject bb7 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb6Pos = bb7.transform.position;
        Vector3 fuseLocation = new Vector3(bb6Pos.x + 5.3f, bb6Pos.y + 2.5f, bb6Pos.z - 15);
        fuseLocations.Add("b7p1_b7p2_a1", fuseLocation);
        fuseLocations.Add("b7p1_b7p2_a2", fuseLocation);
        fuseLocations.Add("b7p4_b7p2_a1", fuseLocation);
        fuseLocations.Add("b7p6_b7p2_a1", fuseLocation);
        fuseLocations.Add("bb7_b7p2_a1", fuseLocation);
        fuseLocations.Add("bb7_b7p2_a2", fuseLocation);
        fuseLocations.Add("bb7_b7p2_a3", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b7p1_b7p2_a1", fuseRotation);
        fuseRotations.Add("b7p1_b7p2_a2", fuseRotation);
        fuseRotations.Add("b7p4_b7p2_a1", fuseRotation);
        fuseRotations.Add("b7p6_b7p2_a1", fuseRotation);
        fuseRotations.Add("bb7_b7p2_a1", fuseRotation);
        fuseRotations.Add("bb7_b7p2_a2", fuseRotation);
        fuseRotations.Add("bb7_b7p2_a3", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1};
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b7p1_b7p2_a1", acceptableRotations);
        fusePositions.Add("b7p1_b7p2_a2", acceptableRotations);
        fusePositions.Add("b7p4_b7p2_a1", acceptableRotations);
        fusePositions.Add("b7p6_b7p2_a1", acceptableRotations);
        fusePositions.Add("bb7_b7p2_a1", acceptableRotations);
        fusePositions.Add("bb7_b7p2_a2", acceptableRotations);
        fusePositions.Add("bb7_b7p2_a3", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b7p3Fuses()
    {
        GameObject bb7 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb7Pos = bb7.transform.position;
        Vector3 fuseLocation = new Vector3(bb7Pos.x + 0.2f, bb7Pos.y - 6.5f, bb7Pos.z);
        fuseLocations.Add("b7p1_b7p3_a1", fuseLocation);
        fuseLocations.Add("b7p4_b7p3_a1", fuseLocation);
        fuseLocations.Add("bb7_b7p3_a1", fuseLocation);
        fuseLocations.Add("bb7_b7p3_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b7p1_b7p3_a1", fuseRotation);
        fuseRotations.Add("b7p4_b7p3_a1", fuseRotation);
        fuseRotations.Add("bb7_b7p3_a1", fuseRotation);
        fuseRotations.Add("bb7_b7p3_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b7p1_b7p3_a1", acceptableRotations);
        fusePositions.Add("b7p4_b7p3_a1", acceptableRotations);
        fusePositions.Add("bb7_b7p3_a1", acceptableRotations);
        fusePositions.Add("bb7_b7p3_a2", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b7p4Fuses()
    {
        GameObject bb7 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb7Pos = bb7.transform.position;
        Vector3 fuseLocation = new Vector3(bb7Pos.x - 5, bb7Pos.y, bb7Pos.z);
        fuseLocations.Add("b7p1_b7p4_a1", fuseLocation);
        fuseLocations.Add("b7p1_b7p4_a2", fuseLocation);
        fuseLocations.Add("b7p2_b7p4_a1", fuseLocation);
        fuseLocations.Add("b7p3_b7p4_a1", fuseLocation);
        fuseLocations.Add("b7p5_b7p4_a1", fuseLocation);
        fuseLocations.Add("b7p6_b7p4_a1", fuseLocation);
        fuseLocations.Add("bb7_b7p4_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b7p1_b7p4_a1", fuseRotation);
        fuseRotations.Add("b7p1_b7p4_a2", fuseRotation);
        fuseRotations.Add("b7p2_b7p4_a1", fuseRotation);
        fuseRotations.Add("b7p3_b7p4_a1", fuseRotation);
        fuseRotations.Add("b7p5_b7p4_a1", fuseRotation);
        fuseRotations.Add("b7p6_b7p4_a1", fuseRotation);
        fuseRotations.Add("bb7_b7p4_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b7p1_b7p4_a1", acceptableRotations);
        fusePositions.Add("b7p1_b7p4_a2", acceptableRotations);
        fusePositions.Add("b7p2_b7p4_a1", acceptableRotations);
        fusePositions.Add("b7p3_b7p4_a1", acceptableRotations);
        fusePositions.Add("b7p5_b7p4_a1", acceptableRotations);
        fusePositions.Add("b7p6_b7p4_a1", acceptableRotations);
        fusePositions.Add("bb7_b7p4_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b7p5Fuses()
    {
        GameObject bb7 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb7Pos = bb7.transform.position;
        Vector3 fuseLocation = new Vector3(bb7Pos.x - 0.4f, bb7Pos.y + 5.5f, bb7Pos.z + 10);
        fuseLocations.Add("b7p1_b7p5_a1", fuseLocation);
        fuseLocations.Add("b7p4_b7p5_a1", fuseLocation);
        fuseLocations.Add("b7p6_b7p5_a1", fuseLocation);
        fuseLocations.Add("bb7_b7p5_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b7p1_b7p5_a1", fuseRotation);
        fuseRotations.Add("b7p4_b7p5_a1", fuseRotation);
        fuseRotations.Add("b7p6_b7p5_a1", fuseRotation);
        fuseRotations.Add("bb7_b7p5_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b7p1_b7p5_a1", acceptableRotations);
        fusePositions.Add("b7p4_b7p5_a1", acceptableRotations);
        fusePositions.Add("b7p6_b7p5_a1", acceptableRotations);
        fusePositions.Add("bb7_b7p5_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b7p6Fuses()
    {
        GameObject bb7 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb7Pos = bb7.transform.position;
        Vector3 fuseLocation = new Vector3(bb7Pos.x + 0.2f, bb7Pos.y + 8.5f, bb7Pos.z - 6.3f);
        fuseLocations.Add("b7p1_b7p6_a1", fuseLocation);
        fuseLocations.Add("b7p2_b7p6_a1", fuseLocation);
        fuseLocations.Add("b7p4_b7p6_a1", fuseLocation);
        fuseLocations.Add("b7p5_b7p6_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b7p1_b7p6_a1", fuseRotation);
        fuseRotations.Add("b7p2_b7p6_a1", fuseRotation);
        fuseRotations.Add("b7p4_b7p6_a1", fuseRotation);
        fuseRotations.Add("b7p5_b7p6_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b7p1_b7p6_a1", acceptableRotations);
        fusePositions.Add("b7p2_b7p6_a1", acceptableRotations);
        fusePositions.Add("b7p4_b7p6_a1", acceptableRotations);
        fusePositions.Add("b7p5_b7p6_a1", acceptableRotations);

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


    public void createB7p1()
    {
        if (!partCreated[0])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(0, 270, 180);
            GameObject newB7p1 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB7p1)); // this creates the zooming up from the ground effect

            Transform b7p1_b7p2_a1 = newB7p1.transform.Find("b7p1_b7p2_a1");
            Transform b7p1_b7p2_a2 = newB7p1.transform.Find("b7p1_b7p2_a2");
            Transform b7p1_b7p3_a1 = newB7p1.transform.Find("b7p1_b7p3_a1");
            Transform b7p1_b7p4_a1 = newB7p1.transform.Find("b7p1_b7p4_a1");
            Transform b7p1_b7p4_a2 = newB7p1.transform.Find("b7p1_b7p4_a2");
            Transform b7p1_b7p5_a1 = newB7p1.transform.Find("b7p1_b7p5_a1");
            Transform b7p1_b7p6_a1 = newB7p1.transform.Find("b7p1_b7p6_a1");
            Transform b7p1_bb7_a1 = newB7p1.transform.Find("b7p1_bb7_a1");
            Transform b7p1_bb7_a2 = newB7p1.transform.Find("b7p1_bb7_a2");
            Transform b7p1_bb7_a3 = newB7p1.transform.Find("b7p1_bb7_a3");
            Transform b7p1_bb7_a4 = newB7p1.transform.Find("b7p1_bb7_a4");

            FuseAttributes fuseAtts = b7p1Fuses();

            //b7p1_b7p2_a1
            b7p1_b7p2_a1.gameObject.AddComponent<FuseBehavior>();
            b7p1_b7p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_b7p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_b7p2_a1.gameObject.AddComponent<FaceSelector>();
            b7p1_b7p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b7p1_b7p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_b7p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_b7p2_a2
            b7p1_b7p2_a2.gameObject.AddComponent<FuseBehavior>();
            b7p1_b7p2_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_b7p2_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_b7p2_a2.gameObject.AddComponent<FaceSelector>();
            b7p1_b7p2_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b7p1_b7p2_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_b7p2_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_b7p3_a1
            b7p1_b7p3_a1.gameObject.AddComponent<FuseBehavior>();
            b7p1_b7p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_b7p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_b7p3_a1.gameObject.AddComponent<FaceSelector>();
            b7p1_b7p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p1_b7p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_b7p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_b7p4_a1
            b7p1_b7p4_a1.gameObject.AddComponent<FuseBehavior>();
            b7p1_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_b7p4_a1.gameObject.AddComponent<FaceSelector>();
            b7p1_b7p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p1_b7p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_b7p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_b7p4_a2
            b7p1_b7p4_a2.gameObject.AddComponent<FuseBehavior>();
            b7p1_b7p4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_b7p4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_b7p4_a2.gameObject.AddComponent<FaceSelector>();
            b7p1_b7p4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p1_b7p4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_b7p4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_b7p5_a1
            b7p1_b7p5_a1.gameObject.AddComponent<FuseBehavior>();
            b7p1_b7p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_b7p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_b7p5_a1.gameObject.AddComponent<FaceSelector>();
            b7p1_b7p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b7p1_b7p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_b7p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_b7p6_a1
            b7p1_b7p6_a1.gameObject.AddComponent<FuseBehavior>();
            b7p1_b7p6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_b7p6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_b7p6_a1.gameObject.AddComponent<FaceSelector>();
            b7p1_b7p6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b7p1_b7p6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_b7p6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_bb7_a1
            b7p1_bb7_a1.gameObject.AddComponent<FuseBehavior>();
            b7p1_bb7_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_bb7_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_bb7_a1.gameObject.AddComponent<FaceSelector>();
            b7p1_bb7_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b7p1_bb7_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_bb7_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_bb7_a2
            b7p1_bb7_a2.gameObject.AddComponent<FuseBehavior>();
            b7p1_bb7_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_bb7_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_bb7_a2.gameObject.AddComponent<FaceSelector>();
            b7p1_bb7_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b7p1_bb7_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_bb7_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_bb7_a3
            b7p1_bb7_a3.gameObject.AddComponent<FuseBehavior>();
            b7p1_bb7_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_bb7_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_bb7_a3.gameObject.AddComponent<FaceSelector>();
            b7p1_bb7_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b7p1_bb7_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_bb7_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p1_bb7_a4
            b7p1_bb7_a4.gameObject.AddComponent<FuseBehavior>();
            b7p1_bb7_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p1_bb7_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p1"));

            b7p1_bb7_a4.gameObject.AddComponent<FaceSelector>();
            b7p1_bb7_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p1_bb7_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p1_bb7_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newB7p1;
            partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("b7p1Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b7p1");
            }
        }
    }

    public void createB7p2()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 90);
            GameObject newB7p2 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB7p2)); // this creates the zooming up from the ground effect

            Transform b7p2_b7p1_a1 = newB7p2.transform.Find("b7p2_b7p1_a1");
            Transform b7p2_b7p1_a2 = newB7p2.transform.Find("b7p2_b7p1_a2");
            Transform b7p2_b7p4_a1 = newB7p2.transform.Find("b7p2_b7p4_a1");
            Transform b7p2_b7p6_a1 = newB7p2.transform.Find("b7p2_b7p6_a1");
            Transform b7p2_bb7_a1 = newB7p2.transform.Find("b7p2_bb7_a1");
            Transform b7p2_bb7_a2 = newB7p2.transform.Find("b7p2_bb7_a2");
            Transform b7p2_bb7_a3 = newB7p2.transform.Find("b7p2_bb7_a3");

            FuseAttributes fuseAtts = b7p2Fuses();

            //b7p2_b7p1_a1
            b7p2_b7p1_a1.gameObject.AddComponent<FuseBehavior>();
            b7p2_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p2_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p2"));

            b7p2_b7p1_a1.gameObject.AddComponent<FaceSelector>();
            b7p2_b7p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p2_b7p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p2_b7p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p2_b7p1_a2
            b7p2_b7p1_a2.gameObject.AddComponent<FuseBehavior>();
            b7p2_b7p1_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p2_b7p1_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p2"));

            b7p2_b7p1_a2.gameObject.AddComponent<FaceSelector>();
            b7p2_b7p1_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p2_b7p1_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p2_b7p1_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p2_b7p4_a1
            b7p2_b7p4_a1.gameObject.AddComponent<FuseBehavior>();
            b7p2_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p2_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p2"));

            b7p2_b7p4_a1.gameObject.AddComponent<FaceSelector>();
            b7p2_b7p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p2_b7p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p2_b7p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p2_b7p6_a1
            b7p2_b7p6_a1.gameObject.AddComponent<FuseBehavior>();
            b7p2_b7p6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p2_b7p6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p2"));

            b7p2_b7p6_a1.gameObject.AddComponent<FaceSelector>();
            b7p2_b7p6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b7p2_b7p6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p2_b7p6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p2_bb7_a1
            b7p2_bb7_a1.gameObject.AddComponent<FuseBehavior>();
            b7p2_bb7_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p2_bb7_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p2"));

            b7p2_bb7_a1.gameObject.AddComponent<FaceSelector>();
            b7p2_bb7_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p2_bb7_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p2_bb7_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p2_bb7_a2
            b7p2_bb7_a2.gameObject.AddComponent<FuseBehavior>();
            b7p2_bb7_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p2_bb7_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p2"));

            b7p2_bb7_a2.gameObject.AddComponent<FaceSelector>();
            b7p2_bb7_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p2_bb7_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p2_bb7_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p2_bb7_a3
            b7p2_bb7_a3.gameObject.AddComponent<FuseBehavior>();
            b7p2_bb7_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p2_bb7_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p2"));

            b7p2_bb7_a3.gameObject.AddComponent<FaceSelector>();
            b7p2_bb7_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b7p2_bb7_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p2_bb7_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[1] = newB7p2;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("b7p2Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b7p2");
            }
        }
    }

    public void createB7p3()
    {
        if (!partCreated[2])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 0);
            GameObject newB7p3 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB7p3)); // this creates the zooming up from the ground effect

            Transform b7p3_b7p1_a1 = newB7p3.transform.Find("b7p3_b7p1_a1");
            Transform b7p3_b7p4_a1 = newB7p3.transform.Find("b7p3_b7p4_a1");
            Transform b7p3_bb7_a1 = newB7p3.transform.Find("b7p3_bb7_a1");
            Transform b7p3_bb7_a2 = newB7p3.transform.Find("b7p3_bb7_a2");

            FuseAttributes fuseAtts = b7p3Fuses();

            //b7p3_b7p1_a1
            b7p3_b7p1_a1.gameObject.AddComponent<FuseBehavior>();
            b7p3_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p3_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p3"));

            b7p3_b7p1_a1.gameObject.AddComponent<FaceSelector>();
            b7p3_b7p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p3_b7p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p3_b7p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p3_b7p4_a1
            b7p3_b7p4_a1.gameObject.AddComponent<FuseBehavior>();
            b7p3_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p3_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p3"));

            b7p3_b7p4_a1.gameObject.AddComponent<FaceSelector>();
            b7p3_b7p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p3_b7p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p3_b7p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p3_bb7_a1
            b7p3_bb7_a1.gameObject.AddComponent<FuseBehavior>();
            b7p3_bb7_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p3_bb7_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p3"));

            b7p3_bb7_a1.gameObject.AddComponent<FaceSelector>();
            b7p3_bb7_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p3_bb7_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p3_bb7_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p3_bb7_a2
            b7p3_bb7_a2.gameObject.AddComponent<FuseBehavior>();
            b7p3_bb7_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p3_bb7_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p3"));

            b7p3_bb7_a2.gameObject.AddComponent<FaceSelector>();
            b7p3_bb7_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p3_bb7_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p3_bb7_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

   
            instantiated[2] = newB7p3;
            partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("b7p3Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b7p3");
            }
        }
    }

    public void createB7p4()
    {
        if (!partCreated[3])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 0, 90);
            GameObject newB7p4 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[3], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB7p4)); // this creates the zooming up from the ground effect

            Transform b7p4_b7p1_a1 = newB7p4.transform.Find("b7p4_b7p1_a1");
            Transform b7p4_b7p1_a2 = newB7p4.transform.Find("b7p4_b7p1_a2");
            Transform b7p4_b7p2_a1 = newB7p4.transform.Find("b7p4_b7p2_a1");
            Transform b7p4_b7p3_a1 = newB7p4.transform.Find("b7p4_b7p3_a1");
            Transform b7p4_b7p5_a1 = newB7p4.transform.Find("b7p4_b7p5_a1");
            Transform b7p4_b7p6_a1 = newB7p4.transform.Find("b7p4_b7p6_a1");
            Transform b7p4_bb7_a1 = newB7p4.transform.Find("b7p4_bb7_a1");

            FuseAttributes fuseAtts = b7p4Fuses();

            //b7p4_b7p1_a1
            b7p4_b7p1_a1.gameObject.AddComponent<FuseBehavior>();
            b7p4_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p4_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p4"));

            b7p4_b7p1_a1.gameObject.AddComponent<FaceSelector>();
            b7p4_b7p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p4_b7p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p4_b7p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p4_b7p1_a2
            b7p4_b7p1_a2.gameObject.AddComponent<FuseBehavior>();
            b7p4_b7p1_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p4_b7p1_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p4"));

            b7p4_b7p1_a2.gameObject.AddComponent<FaceSelector>();
            b7p4_b7p1_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p4_b7p1_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p4_b7p1_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p4_b7p2_a1
            b7p4_b7p2_a1.gameObject.AddComponent<FuseBehavior>();
            b7p4_b7p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p4_b7p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p4"));

            b7p4_b7p2_a1.gameObject.AddComponent<FaceSelector>();
            b7p4_b7p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p4_b7p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p4_b7p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p4_b7p3_a1
            b7p4_b7p3_a1.gameObject.AddComponent<FuseBehavior>();
            b7p4_b7p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p4_b7p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p4"));

            b7p4_b7p3_a1.gameObject.AddComponent<FaceSelector>();
            b7p4_b7p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p4_b7p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p4_b7p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p4_b7p5_a1
            b7p4_b7p5_a1.gameObject.AddComponent<FuseBehavior>();
            b7p4_b7p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p4_b7p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p4"));

            b7p4_b7p5_a1.gameObject.AddComponent<FaceSelector>();
            b7p4_b7p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p4_b7p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p4_b7p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p4_b7p6_a1
            b7p4_b7p6_a1.gameObject.AddComponent<FuseBehavior>();
            b7p4_b7p6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p4_b7p6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p4"));

            b7p4_b7p6_a1.gameObject.AddComponent<FaceSelector>();
            b7p4_b7p6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p4_b7p6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p4_b7p6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p4_bb7_a1
            b7p4_bb7_a1.gameObject.AddComponent<FuseBehavior>();
            b7p4_bb7_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p4_bb7_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p4"));

            b7p4_bb7_a1.gameObject.AddComponent<FaceSelector>();
            b7p4_bb7_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b7p4_bb7_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p4_bb7_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[3] = newB7p4;
            partCreated[3] = true;
            partButtons[3].interactable = false;

            selectionManager.newPartCreated("b7p4Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b7p4");
            }
        }
    }

    public void createB7p5()
    {
        if (!partCreated[4])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(180, 90, 0);
            GameObject newB7p5 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[4], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB7p5)); // this creates the zooming up from the ground effect

            Transform b7p5_b7p1_a1 = newB7p5.transform.Find("b7p5_b7p1_a1");
            Transform b7p5_b7p4_a1 = newB7p5.transform.Find("b7p5_b7p4_a1");
            Transform b7p5_b7p6_a1 = newB7p5.transform.Find("b7p5_b7p6_a1");
            Transform b7p5_bb7_a1 = newB7p5.transform.Find("b7p5_bb7_a1");

            FuseAttributes fuseAtts = b7p5Fuses();

            //b7p5_b7p1_a1
            b7p5_b7p1_a1.gameObject.AddComponent<FuseBehavior>();
            b7p5_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p5_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p5"));

            b7p5_b7p1_a1.gameObject.AddComponent<FaceSelector>();
            b7p5_b7p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p5_b7p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p5_b7p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p5_b7p4_a1
            b7p5_b7p4_a1.gameObject.AddComponent<FuseBehavior>();
            b7p5_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p5_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p5"));

            b7p5_b7p4_a1.gameObject.AddComponent<FaceSelector>();
            b7p5_b7p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p5_b7p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p5_b7p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p5_b7p6_a1
            b7p5_b7p6_a1.gameObject.AddComponent<FuseBehavior>();
            b7p5_b7p6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p5_b7p6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p5"));

            b7p5_b7p6_a1.gameObject.AddComponent<FaceSelector>();
            b7p5_b7p6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b7p5_b7p6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p5_b7p6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p5_bb7_a1
            b7p5_bb7_a1.gameObject.AddComponent<FuseBehavior>();
            b7p5_bb7_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p5_bb7_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p5"));

            b7p5_bb7_a1.gameObject.AddComponent<FaceSelector>();
            b7p5_bb7_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p5_bb7_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p5_bb7_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[4] = newB7p5;
            partCreated[4] = true;
            partButtons[4].interactable = false;

            selectionManager.newPartCreated("b7p5Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b7p5");
            }
        }
    }

    public void createB7p6()
    {
        if (!partCreated[5])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 180, 0);
            GameObject newB7p6 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[5], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB7p6)); // this creates the zooming up from the ground effect

            Transform b7p6_b7p1_a1 = newB7p6.transform.Find("b7p6_b7p1_a1");
            Transform b7p6_b7p2_a1 = newB7p6.transform.Find("b7p6_b7p2_a1");
            Transform b7p6_b7p4_a1 = newB7p6.transform.Find("b7p6_b7p4_a1");
            Transform b7p6_b7p5_a1 = newB7p6.transform.Find("b7p6_b7p5_a1");

            FuseAttributes fuseAtts = b7p6Fuses();

            //b7p6_b7p1_a1
            b7p6_b7p1_a1.gameObject.AddComponent<FuseBehavior>();
            b7p6_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p6_b7p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p6"));

            b7p6_b7p1_a1.gameObject.AddComponent<FaceSelector>();
            b7p6_b7p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b7p6_b7p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p6_b7p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p6_b7p2_a1
            b7p6_b7p2_a1.gameObject.AddComponent<FuseBehavior>();
            b7p6_b7p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p6_b7p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p6"));

            b7p6_b7p2_a1.gameObject.AddComponent<FaceSelector>();
            b7p6_b7p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b7p6_b7p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p6_b7p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p6_b7p4_a1
            b7p6_b7p4_a1.gameObject.AddComponent<FuseBehavior>();
            b7p6_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p6_b7p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p6"));

            b7p6_b7p4_a1.gameObject.AddComponent<FaceSelector>();
            b7p6_b7p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b7p6_b7p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p6_b7p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b7p6_b7p5_a1
            b7p6_b7p5_a1.gameObject.AddComponent<FuseBehavior>();
            b7p6_b7p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b7p6_b7p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B7p6"));

            b7p6_b7p5_a1.gameObject.AddComponent<FaceSelector>();
            b7p6_b7p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b7p6_b7p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b7p6_b7p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[5] = newB7p6;
            partCreated[5] = true;
            partButtons[5].interactable = false;

            selectionManager.newPartCreated("b7p6Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b7p6");
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

