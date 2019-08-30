using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartB6 : MonoBehaviour
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
        startObject = GameObject.Find("bb6Start");

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
    public FuseAttributes b6p1Fuses()
    {
        GameObject bb6 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb6Pos = bb6.transform.position;
        Vector3 fuseLocation = new Vector3(bb6Pos.x + 4.9f, bb6Pos.y, bb6Pos.z - 33.3f);
        fuseLocations.Add("b6p2_b6p1_a1", fuseLocation);
        fuseLocations.Add("b6p6_b6p1_a1", fuseLocation);
        fuseLocations.Add("b6p4_b6p1_a1", fuseLocation);
        fuseLocations.Add("b6p4_b6p1_a2", fuseLocation);
        fuseLocations.Add("b6p4_b6p1_a3", fuseLocation);
        fuseLocations.Add("b6p4_b6p1_a4", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b6p2_b6p1_a1", fuseRotation);
        fuseRotations.Add("b6p6_b6p1_a1", fuseRotation);
        fuseRotations.Add("b6p4_b6p1_a1", fuseRotation);
        fuseRotations.Add("b6p4_b6p1_a2", fuseRotation);
        fuseRotations.Add("b6p4_b6p1_a3", fuseRotation);
        fuseRotations.Add("b6p4_b6p1_a4", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        //Quaternion acceptableRotation2 = Quaternion.Euler(0, 90, 270);
        //Quaternion acceptableRotation3 = Quaternion.Euler(90, 180, 0);
        //Quaternion acceptableRotation4 = Quaternion.Euler(0, 270, 90);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions.Add("b6p2_b6p1_a1", acceptableRotations);
        fusePositions.Add("b6p6_b6p1_a1", acceptableRotations);
        fusePositions.Add("b6p4_b6p1_a1", acceptableRotations);
        fusePositions.Add("b6p4_b6p1_a2", acceptableRotations);
        fusePositions.Add("b6p4_b6p1_a3", acceptableRotations);
        fusePositions.Add("b6p4_b6p1_a4", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b6p2Fuses()
    {
        GameObject bb6 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb6Pos = bb6.transform.position;
        Vector3 fuseLocation = new Vector3(bb6Pos.x - 5.2f, bb6Pos.y, bb6Pos.z - 32.8f);
        fuseLocations.Add("b6p1_b6p2_a1", fuseLocation);
        fuseLocations.Add("b6p5_b6p2_a1", fuseLocation);
        fuseLocations.Add("b6p3_b6p2_a1", fuseLocation);
        fuseLocations.Add("b6p3_b6p2_a2", fuseLocation);
        fuseLocations.Add("b6p3_b6p2_a3", fuseLocation);
        fuseLocations.Add("b6p3_b6p2_a4", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b6p1_b6p2_a1", fuseRotation);
        fuseRotations.Add("b6p5_b6p2_a1", fuseRotation);
        fuseRotations.Add("b6p3_b6p2_a1", fuseRotation);
        fuseRotations.Add("b6p3_b6p2_a2", fuseRotation);
        fuseRotations.Add("b6p3_b6p2_a3", fuseRotation);
        fuseRotations.Add("b6p3_b6p2_a4", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1};
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b6p1_b6p2_a1", acceptableRotations);
        fusePositions.Add("b6p5_b6p2_a1", acceptableRotations);
        fusePositions.Add("b6p3_b6p2_a1", acceptableRotations);
        fusePositions.Add("b6p3_b6p2_a2", acceptableRotations);
        fusePositions.Add("b6p3_b6p2_a3", acceptableRotations);
        fusePositions.Add("b6p3_b6p2_a4", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b6p3Fuses()
    {
        GameObject bb6 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb6Pos = bb6.transform.position;
        Vector3 fuseLocation = new Vector3(bb6Pos.x - 5, bb6Pos.y, bb6Pos.z - 20);
        fuseLocations.Add("b6p2_b6p3_a1", fuseLocation);
        fuseLocations.Add("b6p2_b6p3_a2", fuseLocation);
        fuseLocations.Add("b6p2_b6p3_a3", fuseLocation);
        fuseLocations.Add("b6p2_b6p3_a4", fuseLocation);
        fuseLocations.Add("b6p4_b6p3_a1", fuseLocation);
        fuseLocations.Add("b6p5_b6p3_a1", fuseLocation);
        fuseLocations.Add("b6p5_b6p3_a2", fuseLocation);
        fuseLocations.Add("b6p5_b6p3_a3", fuseLocation);
        fuseLocations.Add("b6p5_b6p3_a4", fuseLocation);
        fuseLocations.Add("b6p5_b6p3_a5", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b6p2_b6p3_a1", fuseRotation);
        fuseRotations.Add("b6p2_b6p3_a2", fuseRotation);
        fuseRotations.Add("b6p2_b6p3_a3", fuseRotation);
        fuseRotations.Add("b6p2_b6p3_a4", fuseRotation);
        fuseRotations.Add("b6p4_b6p3_a1", fuseRotation);
        fuseRotations.Add("b6p5_b6p3_a1", fuseRotation);
        fuseRotations.Add("b6p5_b6p3_a2", fuseRotation);
        fuseRotations.Add("b6p5_b6p3_a3", fuseRotation);
        fuseRotations.Add("b6p5_b6p3_a4", fuseRotation);
        fuseRotations.Add("b6p5_b6p3_a5", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b6p2_b6p3_a1", acceptableRotations);
        fusePositions.Add("b6p2_b6p3_a2", acceptableRotations);
        fusePositions.Add("b6p2_b6p3_a3", acceptableRotations);
        fusePositions.Add("b6p2_b6p3_a4", acceptableRotations);
        fusePositions.Add("b6p4_b6p3_a1", acceptableRotations);
        fusePositions.Add("b6p5_b6p3_a1", acceptableRotations);
        fusePositions.Add("b6p5_b6p3_a2", acceptableRotations);
        fusePositions.Add("b6p5_b6p3_a3", acceptableRotations);
        fusePositions.Add("b6p5_b6p3_a4", acceptableRotations);
        fusePositions.Add("b6p5_b6p3_a5", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b6p4Fuses()
    {
        GameObject bb6 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb6Pos = bb6.transform.position;
        Vector3 fuseLocation = new Vector3(bb6Pos.x + 5, bb6Pos.y, bb6Pos.z - 20);
        fuseLocations.Add("b6p1_b6p4_a1", fuseLocation);
        fuseLocations.Add("b6p1_b6p4_a2", fuseLocation);
        fuseLocations.Add("b6p1_b6p4_a3", fuseLocation);
        fuseLocations.Add("b6p1_b6p4_a4", fuseLocation);
        fuseLocations.Add("b6p3_b6p4_a1", fuseLocation);
        fuseLocations.Add("b6p6_b6p4_a1", fuseLocation);
        fuseLocations.Add("b6p6_b6p4_a2", fuseLocation);
        fuseLocations.Add("b6p6_b6p4_a3", fuseLocation);
        fuseLocations.Add("b6p6_b6p4_a4", fuseLocation);
        fuseLocations.Add("b6p6_b6p4_a5", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b6p1_b6p4_a1", fuseRotation);
        fuseRotations.Add("b6p1_b6p4_a2", fuseRotation);
        fuseRotations.Add("b6p1_b6p4_a3", fuseRotation);
        fuseRotations.Add("b6p1_b6p4_a4", fuseRotation);
        fuseRotations.Add("b6p3_b6p4_a1", fuseRotation);
        fuseRotations.Add("b6p6_b6p4_a1", fuseRotation);
        fuseRotations.Add("b6p6_b6p4_a2", fuseRotation);
        fuseRotations.Add("b6p6_b6p4_a3", fuseRotation);
        fuseRotations.Add("b6p6_b6p4_a4", fuseRotation);
        fuseRotations.Add("b6p6_b6p4_a5", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b6p1_b6p4_a1", acceptableRotations);
        fusePositions.Add("b6p1_b6p4_a2", acceptableRotations);
        fusePositions.Add("b6p1_b6p4_a3", acceptableRotations);
        fusePositions.Add("b6p1_b6p4_a4", acceptableRotations);
        fusePositions.Add("b6p3_b6p4_a1", acceptableRotations);
        fusePositions.Add("b6p6_b6p4_a1", acceptableRotations);
        fusePositions.Add("b6p6_b6p4_a2", acceptableRotations);
        fusePositions.Add("b6p6_b6p4_a3", acceptableRotations);
        fusePositions.Add("b6p6_b6p4_a4", acceptableRotations);
        fusePositions.Add("b6p6_b6p4_a5", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b6p5Fuses()
    {
        GameObject bb6 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb6Pos = bb6.transform.position;
        Vector3 fuseLocation = new Vector3(bb6Pos.x - 5, bb6Pos.y, bb6Pos.z - 7.6f);
        fuseLocations.Add("b6p2_b6p5_a1", fuseLocation);
        fuseLocations.Add("b6p3_b6p5_a1", fuseLocation);
        fuseLocations.Add("b6p3_b6p5_a2", fuseLocation);
        fuseLocations.Add("b6p3_b6p5_a3", fuseLocation);
        fuseLocations.Add("b6p3_b6p5_a4", fuseLocation);
        fuseLocations.Add("b6p3_b6p5_a5", fuseLocation);
        fuseLocations.Add("b6p6_b6p5_a1", fuseLocation);
        fuseLocations.Add("b6p6_b6p5_a2", fuseLocation);
        fuseLocations.Add("bb6_b6p5_a1", fuseLocation);
        fuseLocations.Add("bb6_b6p5_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b6p2_b6p5_a1", fuseRotation);
        fuseRotations.Add("b6p3_b6p5_a1", fuseRotation);
        fuseRotations.Add("b6p3_b6p5_a2", fuseRotation);
        fuseRotations.Add("b6p3_b6p5_a3", fuseRotation);
        fuseRotations.Add("b6p3_b6p5_a4", fuseRotation);
        fuseRotations.Add("b6p3_b6p5_a5", fuseRotation);
        fuseRotations.Add("b6p6_b6p5_a1", fuseRotation);
        fuseRotations.Add("b6p6_b6p5_a2", fuseRotation);
        fuseRotations.Add("bb6_b6p5_a1", fuseRotation);
        fuseRotations.Add("bb6_b6p5_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b6p2_b6p5_a1", acceptableRotations);
        fusePositions.Add("b6p3_b6p5_a1", acceptableRotations);
        fusePositions.Add("b6p3_b6p5_a2", acceptableRotations);
        fusePositions.Add("b6p3_b6p5_a3", acceptableRotations);
        fusePositions.Add("b6p3_b6p5_a4", acceptableRotations);
        fusePositions.Add("b6p3_b6p5_a5", acceptableRotations);
        fusePositions.Add("b6p6_b6p5_a1", acceptableRotations);
        fusePositions.Add("b6p6_b6p5_a2", acceptableRotations);
        fusePositions.Add("bb6_b6p5_a1", acceptableRotations);
        fusePositions.Add("bb6_b6p5_a2", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b6p6Fuses()
    {
        GameObject bb6 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb6Pos = bb6.transform.position;
        Vector3 fuseLocation = new Vector3(bb6Pos.x + 5, bb6Pos.y, bb6Pos.z - 7.6f);
        fuseLocations.Add("b6p1_b6p6_a1", fuseLocation);
        fuseLocations.Add("b6p4_b6p6_a1", fuseLocation);
        fuseLocations.Add("b6p4_b6p6_a2", fuseLocation);
        fuseLocations.Add("b6p4_b6p6_a3", fuseLocation);
        fuseLocations.Add("b6p4_b6p6_a4", fuseLocation);
        fuseLocations.Add("b6p4_b6p6_a5", fuseLocation);
        fuseLocations.Add("b6p5_b6p6_a1", fuseLocation);
        fuseLocations.Add("b6p5_b6p6_a2", fuseLocation);
        fuseLocations.Add("bb6_b6p6_a1", fuseLocation);
        fuseLocations.Add("bb6_b6p6_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b6p1_b6p6_a1", fuseRotation);
        fuseRotations.Add("b6p4_b6p6_a1", fuseRotation);
        fuseRotations.Add("b6p4_b6p6_a2", fuseRotation);
        fuseRotations.Add("b6p4_b6p6_a3", fuseRotation);
        fuseRotations.Add("b6p4_b6p6_a4", fuseRotation);
        fuseRotations.Add("b6p4_b6p6_a5", fuseRotation);
        fuseRotations.Add("b6p5_b6p6_a1", fuseRotation);
        fuseRotations.Add("b6p5_b6p6_a2", fuseRotation);
        fuseRotations.Add("bb6_b6p6_a1", fuseRotation);
        fuseRotations.Add("bb6_b6p6_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b6p2_b6p6_a1", acceptableRotations);
        fusePositions.Add("b6p4_b6p6_a1", acceptableRotations);
        fusePositions.Add("b6p4_b6p6_a2", acceptableRotations);
        fusePositions.Add("b6p4_b6p6_a3", acceptableRotations);
        fusePositions.Add("b6p4_b6p6_a4", acceptableRotations);
        fusePositions.Add("b6p4_b6p6_a5", acceptableRotations);
        fusePositions.Add("b6p5_b6p6_a1", acceptableRotations);
        fusePositions.Add("b6p5_b6p6_a2", acceptableRotations);
        fusePositions.Add("bb6_b6p6_a1", acceptableRotations);
        fusePositions.Add("bb6_b6p6_a2", acceptableRotations);

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


    public void createB6p1()
    {
        if (!partCreated[0])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(0, 270, 180);
            GameObject newB6p1 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB6p1)); // this creates the zooming up from the ground effect

            Transform b6p1_b6p2_a1 = newB6p1.transform.Find("b6p1_b6p2_a1");
            Transform b6p1_b6p4_a1 = newB6p1.transform.Find("b6p1_b6p4_a1");
            Transform b6p1_b6p4_a2 = newB6p1.transform.Find("b6p1_b6p4_a2");
            Transform b6p1_b6p4_a3 = newB6p1.transform.Find("b6p1_b6p4_a3");
            Transform b6p1_b6p4_a4 = newB6p1.transform.Find("b6p1_b6p4_a4");
            Transform b6p1_b6p6_a1 = newB6p1.transform.Find("b6p1_b6p6_a1");

            FuseAttributes fuseAtts = b6p1Fuses();

            //b6p1_b6p2_a1
            b6p1_b6p2_a1.gameObject.AddComponent<FuseBehavior>();
            b6p1_b6p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p1_b6p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p1"));

            b6p1_b6p2_a1.gameObject.AddComponent<FaceSelector>();
            b6p1_b6p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p1_b6p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p1_b6p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p1_b6p4_a1
            b6p1_b6p4_a1.gameObject.AddComponent<FuseBehavior>();
            b6p1_b6p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p1_b6p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p1"));

            b6p1_b6p4_a1.gameObject.AddComponent<FaceSelector>();
            b6p1_b6p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p1_b6p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p1_b6p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p1_b6p4_a2
            b6p1_b6p4_a2.gameObject.AddComponent<FuseBehavior>();
            b6p1_b6p4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p1_b6p4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p1"));

            b6p1_b6p4_a2.gameObject.AddComponent<FaceSelector>();
            b6p1_b6p4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p1_b6p4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p1_b6p4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p1_b6p4_a3
            b6p1_b6p4_a3.gameObject.AddComponent<FuseBehavior>();
            b6p1_b6p4_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p1_b6p4_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p1"));

            b6p1_b6p4_a3.gameObject.AddComponent<FaceSelector>();
            b6p1_b6p4_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b6p1_b6p4_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p1_b6p4_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p1_b6p4_a4
            b6p1_b6p4_a4.gameObject.AddComponent<FuseBehavior>();
            b6p1_b6p4_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p1_b6p4_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p1"));

            b6p1_b6p4_a4.gameObject.AddComponent<FaceSelector>();
            b6p1_b6p4_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p1_b6p4_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p1_b6p4_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p1_b6p6_a1
            b6p1_b6p6_a1.gameObject.AddComponent<FuseBehavior>();
            b6p1_b6p6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p1_b6p6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p1"));

            b6p1_b6p6_a1.gameObject.AddComponent<FaceSelector>();
            b6p1_b6p6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b6p1_b6p6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p1_b6p6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newB6p1;
            partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("b6p1Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b6p1");
            }
        }
    }

    public void createB6p2()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 90);
            GameObject newB6p2 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB6p2)); // this creates the zooming up from the ground effect

            Transform b6p2_b6p1_a1 = newB6p2.transform.Find("b6p2_b6p1_a1");
            Transform b6p2_b6p3_a1 = newB6p2.transform.Find("b6p2_b6p3_a1");
            Transform b6p2_b6p3_a2 = newB6p2.transform.Find("b6p2_b6p3_a2");
            Transform b6p2_b6p3_a3 = newB6p2.transform.Find("b6p2_b6p3_a3");
            Transform b6p2_b6p3_a4 = newB6p2.transform.Find("b6p2_b6p3_a4");
            Transform b6p2_b6p5_a1 = newB6p2.transform.Find("b6p2_b6p5_a1");

            FuseAttributes fuseAtts = b6p2Fuses();

            //b6p2_b6p1_a1
            b6p2_b6p1_a1.gameObject.AddComponent<FuseBehavior>();
            b6p2_b6p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p2_b6p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p2"));

            b6p2_b6p1_a1.gameObject.AddComponent<FaceSelector>();
            b6p2_b6p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p2_b6p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p2_b6p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p2_b6p3_a1
            b6p2_b6p3_a1.gameObject.AddComponent<FuseBehavior>();
            b6p2_b6p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p2_b6p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p2"));

            b6p2_b6p3_a1.gameObject.AddComponent<FaceSelector>();
            b6p2_b6p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p2_b6p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p2_b6p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p2_b6p3_a2
            b6p2_b6p3_a2.gameObject.AddComponent<FuseBehavior>();
            b6p2_b6p3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p2_b6p3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p2"));

            b6p2_b6p3_a2.gameObject.AddComponent<FaceSelector>();
            b6p2_b6p3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p2_b6p3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p2_b6p3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p2_b6p3_a3
            b6p2_b6p3_a3.gameObject.AddComponent<FuseBehavior>();
            b6p2_b6p3_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p2_b6p3_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p2"));

            b6p2_b6p3_a3.gameObject.AddComponent<FaceSelector>();
            b6p2_b6p3_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p2_b6p3_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p2_b6p3_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p2_b6p3_a4
            b6p2_b6p3_a4.gameObject.AddComponent<FuseBehavior>();
            b6p2_b6p3_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p2_b6p3_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p2"));

            b6p2_b6p3_a4.gameObject.AddComponent<FaceSelector>();
            b6p2_b6p3_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p2_b6p3_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p2_b6p3_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p2_b6p5_a1
            b6p2_b6p5_a1.gameObject.AddComponent<FuseBehavior>();
            b6p2_b6p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p2_b6p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p2"));

            b6p2_b6p5_a1.gameObject.AddComponent<FaceSelector>();
            b6p2_b6p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p2_b6p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p2_b6p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[1] = newB6p2;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("b6p2Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b6p2");
            }
        }
    }

    public void createB6p3()
    {
        if (!partCreated[2])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 0);
            GameObject newB6p3 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB6p3)); // this creates the zooming up from the ground effect

            Transform b6p3_b6p2_a1 = newB6p3.transform.Find("b6p3_b6p2_a1");
            Transform b6p3_b6p2_a2 = newB6p3.transform.Find("b6p3_b6p2_a2");
            Transform b6p3_b6p2_a3 = newB6p3.transform.Find("b6p3_b6p2_a3");
            Transform b6p3_b6p2_a4 = newB6p3.transform.Find("b6p3_b6p2_a4");
            Transform b6p3_b6p4_a1 = newB6p3.transform.Find("b6p3_b6p4_a1");
            Transform b6p3_b6p5_a1 = newB6p3.transform.Find("b6p3_b6p5_a1");
            Transform b6p3_b6p5_a2 = newB6p3.transform.Find("b6p3_b6p5_a2");
            Transform b6p3_b6p5_a3 = newB6p3.transform.Find("b6p3_b6p5_a3");
            Transform b6p3_b6p5_a4 = newB6p3.transform.Find("b6p3_b6p5_a4");
            Transform b6p3_b6p5_a5 = newB6p3.transform.Find("b6p3_b6p5_a5");

            FuseAttributes fuseAtts = b6p3Fuses();

            //b6p3_b6p2_a1
            b6p3_b6p2_a1.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p2_a1.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p3_b6p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p2_a2
            b6p3_b6p2_a2.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p2_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p2_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p2_a2.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p2_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p3_b6p2_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p2_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p2_a3
            b6p3_b6p2_a3.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p2_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p2_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p2_a3.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p2_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b6p3_b6p2_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p2_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p2_a4
            b6p3_b6p2_a4.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p2_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p2_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p2_a4.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p2_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p3_b6p2_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p2_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p4_a1
            b6p3_b6p4_a1.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p4_a1.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p3_b6p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p5_a1
            b6p3_b6p5_a1.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p5_a1.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p3_b6p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p5_a2
            b6p3_b6p5_a2.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p5_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p5_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p5_a2.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p5_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p3_b6p5_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p5_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p5_a3
            b6p3_b6p5_a3.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p5_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p5_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p5_a3.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p5_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p3_b6p5_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p5_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p5_a4
            b6p3_b6p5_a4.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p5_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p5_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p5_a4.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p5_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p3_b6p5_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p5_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p3_b6p5_a5
            b6p3_b6p5_a5.gameObject.AddComponent<FuseBehavior>();
            b6p3_b6p5_a5.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p3_b6p5_a5.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p3"));

            b6p3_b6p5_a5.gameObject.AddComponent<FaceSelector>();
            b6p3_b6p5_a5.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p3_b6p5_a5.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p3_b6p5_a5.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[2] = newB6p3;
            partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("b6p3Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b6p3");
            }
        }
    }

    public void createB6p4()
    {
        if (!partCreated[3])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 0, 90);
            GameObject newB6p4 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[3], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB6p4)); // this creates the zooming up from the ground effect

            Transform b6p4_b6p1_a1 = newB6p4.transform.Find("b6p4_b6p1_a1");
            Transform b6p4_b6p1_a2 = newB6p4.transform.Find("b6p4_b6p1_a2");
            Transform b6p4_b6p1_a3 = newB6p4.transform.Find("b6p4_b6p1_a3");
            Transform b6p4_b6p1_a4 = newB6p4.transform.Find("b6p4_b6p1_a4");
            Transform b6p4_b6p3_a1 = newB6p4.transform.Find("b6p4_b6p3_a1");
            Transform b6p4_b6p6_a1 = newB6p4.transform.Find("b6p4_b6p6_a1");
            Transform b6p4_b6p6_a2 = newB6p4.transform.Find("b6p4_b6p6_a2");
            Transform b6p4_b6p6_a3 = newB6p4.transform.Find("b6p4_b6p6_a3");
            Transform b6p4_b6p6_a4 = newB6p4.transform.Find("b6p4_b6p6_a4");
            Transform b6p4_b6p6_a5 = newB6p4.transform.Find("b6p4_b6p6_a5");

            FuseAttributes fuseAtts = b6p4Fuses();

            //b6p4_b6p1_a1
            b6p4_b6p1_a1.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p1_a1.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p4_b6p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p1_a2
            b6p4_b6p1_a2.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p1_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p1_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p1_a2.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p1_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b6p4_b6p1_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p1_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p1_a3
            b6p4_b6p1_a3.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p1_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p1_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p1_a3.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p1_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p4_b6p1_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p1_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p1_a4
            b6p4_b6p1_a4.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p1_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p1_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p1_a4.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p1_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p4_b6p1_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p1_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p3_a1
            b6p4_b6p3_a1.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p3_a1.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p4_b6p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p6_a1
            b6p4_b6p6_a1.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p6_a1.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b6p4_b6p6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p6_a2
            b6p4_b6p6_a2.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p6_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p6_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p6_a2.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p6_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p4_b6p6_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p6_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p6_a3
            b6p4_b6p6_a3.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p6_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p6_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p6_a3.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p6_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b6p4_b6p6_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p6_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p6_a4
            b6p4_b6p6_a4.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p6_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p6_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p6_a4.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p6_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b6p4_b6p6_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p6_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p4_b6p6_a5
            b6p4_b6p6_a5.gameObject.AddComponent<FuseBehavior>();
            b6p4_b6p6_a5.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p4_b6p6_a5.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p4"));

            b6p4_b6p6_a5.gameObject.AddComponent<FaceSelector>();
            b6p4_b6p6_a5.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b6p4_b6p6_a5.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p4_b6p6_a5.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[3] = newB6p4;
            partCreated[3] = true;
            partButtons[3].interactable = false;

            selectionManager.newPartCreated("b6p4Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b6p4");
            }
        }
    }

    public void createB6p5()
    {
        if (!partCreated[4])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(180, 90, 0);
            GameObject newB6p5 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[4], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB6p5)); // this creates the zooming up from the ground effect

            Transform b6p5_b6p3_a1 = newB6p5.transform.Find("b6p5_b6p3_a1");
            Transform b6p5_b6p3_a2 = newB6p5.transform.Find("b6p5_b6p3_a2");
            Transform b6p5_b6p3_a3 = newB6p5.transform.Find("b6p5_b6p3_a3");
            Transform b6p5_b6p3_a4 = newB6p5.transform.Find("b6p5_b6p3_a4");
            Transform b6p5_b6p3_a5 = newB6p5.transform.Find("b6p5_b6p3_a5");
            Transform b6p5_b6p2_a1 = newB6p5.transform.Find("b6p5_b6p2_a1");
            Transform b6p5_b6p6_a1 = newB6p5.transform.Find("b6p5_b6p6_a1");
            Transform b6p5_b6p6_a2 = newB6p5.transform.Find("b6p5_b6p6_a2");
            Transform b6p5_bb6_a1 = newB6p5.transform.Find("b6p5_bb6_a1");
            Transform b6p5_bb6_a2 = newB6p5.transform.Find("b6p5_bb6_a2");

            FuseAttributes fuseAtts = b6p5Fuses();

            //b6p5_b6p3_a1
            b6p5_b6p3_a1.gameObject.AddComponent<FuseBehavior>();
            b6p5_b6p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_b6p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_b6p3_a1.gameObject.AddComponent<FaceSelector>();
            b6p5_b6p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p5_b6p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_b6p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_b6p3_a2
            b6p5_b6p3_a2.gameObject.AddComponent<FuseBehavior>();
            b6p5_b6p3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_b6p3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_b6p3_a2.gameObject.AddComponent<FaceSelector>();
            b6p5_b6p3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p5_b6p3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_b6p3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_b6p3_a3
            b6p5_b6p3_a3.gameObject.AddComponent<FuseBehavior>();
            b6p5_b6p3_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_b6p3_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_b6p3_a3.gameObject.AddComponent<FaceSelector>();
            b6p5_b6p3_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p5_b6p3_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_b6p3_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_b6p3_a4
            b6p5_b6p3_a4.gameObject.AddComponent<FuseBehavior>();
            b6p5_b6p3_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_b6p3_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_b6p3_a4.gameObject.AddComponent<FaceSelector>();
            b6p5_b6p3_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p5_b6p3_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_b6p3_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_b6p3_a5
            b6p5_b6p3_a5.gameObject.AddComponent<FuseBehavior>();
            b6p5_b6p3_a5.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_b6p3_a5.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_b6p3_a5.gameObject.AddComponent<FaceSelector>();
            b6p5_b6p3_a5.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p5_b6p3_a5.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_b6p3_a5.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_b6p2_a1
            b6p5_b6p2_a1.gameObject.AddComponent<FuseBehavior>();
            b6p5_b6p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_b6p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_b6p2_a1.gameObject.AddComponent<FaceSelector>();
            b6p5_b6p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p5_b6p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_b6p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_b6p6_a1
            b6p5_b6p6_a1.gameObject.AddComponent<FuseBehavior>();
            b6p5_b6p6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_b6p6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_b6p6_a1.gameObject.AddComponent<FaceSelector>();
            b6p5_b6p6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p5_b6p6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_b6p6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_b6p6_a2
            b6p5_b6p6_a2.gameObject.AddComponent<FuseBehavior>();
            b6p5_b6p6_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_b6p6_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_b6p6_a2.gameObject.AddComponent<FaceSelector>();
            b6p5_b6p6_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p5_b6p6_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_b6p6_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_bb6_a1
            b6p5_bb6_a1.gameObject.AddComponent<FuseBehavior>();
            b6p5_bb6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_bb6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_bb6_a1.gameObject.AddComponent<FaceSelector>();
            b6p5_bb6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p5_bb6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_bb6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p5_bb6_a2
            b6p5_bb6_a2.gameObject.AddComponent<FuseBehavior>();
            b6p5_bb6_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p5_bb6_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p5"));

            b6p5_bb6_a2.gameObject.AddComponent<FaceSelector>();
            b6p5_bb6_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b6p5_bb6_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p5_bb6_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[4] = newB6p5;
            partCreated[4] = true;
            partButtons[4].interactable = false;

            selectionManager.newPartCreated("b6p5Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b6p5");
            }
        }
    }

    public void createB6p6()
    {
        if (!partCreated[5])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 180, 0);
            GameObject newB6p6 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[5], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB6p6)); // this creates the zooming up from the ground effect

            Transform b6p6_b6p4_a1 = newB6p6.transform.Find("b6p6_b6p4_a1");
            Transform b6p6_b6p4_a2 = newB6p6.transform.Find("b6p6_b6p4_a2");
            Transform b6p6_b6p4_a3 = newB6p6.transform.Find("b6p6_b6p4_a3");
            Transform b6p6_b6p4_a4 = newB6p6.transform.Find("b6p6_b6p4_a4");
            Transform b6p6_b6p4_a5 = newB6p6.transform.Find("b6p6_b6p4_a5");
            Transform b6p6_b6p1_a1 = newB6p6.transform.Find("b6p6_b6p1_a1");
            Transform b6p6_b6p5_a1 = newB6p6.transform.Find("b6p6_b6p5_a1");
            Transform b6p6_b6p5_a2 = newB6p6.transform.Find("b6p6_b6p5_a2");
            Transform b6p6_bb6_a1 = newB6p6.transform.Find("b6p6_bb6_a1");
            Transform b6p6_bb6_a2 = newB6p6.transform.Find("b6p6_bb6_a2");

            FuseAttributes fuseAtts = b6p6Fuses();

            //b6p6_b6p4_a1
            b6p6_b6p4_a1.gameObject.AddComponent<FuseBehavior>();
            b6p6_b6p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_b6p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_b6p4_a1.gameObject.AddComponent<FaceSelector>();
            b6p6_b6p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p6_b6p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_b6p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_b6p4_a2
            b6p6_b6p4_a2.gameObject.AddComponent<FuseBehavior>();
            b6p6_b6p4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_b6p4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_b6p4_a2.gameObject.AddComponent<FaceSelector>();
            b6p6_b6p4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p6_b6p4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_b6p4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_b6p4_a3
            b6p6_b6p4_a3.gameObject.AddComponent<FuseBehavior>();
            b6p6_b6p4_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_b6p4_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_b6p4_a3.gameObject.AddComponent<FaceSelector>();
            b6p6_b6p4_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b6p6_b6p4_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_b6p4_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_b6p4_a4
            b6p6_b6p4_a4.gameObject.AddComponent<FuseBehavior>();
            b6p6_b6p4_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_b6p4_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_b6p4_a4.gameObject.AddComponent<FaceSelector>();
            b6p6_b6p4_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p6_b6p4_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_b6p4_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_b6p4_a5
            b6p6_b6p4_a5.gameObject.AddComponent<FuseBehavior>();
            b6p6_b6p4_a5.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_b6p4_a5.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_b6p4_a5.gameObject.AddComponent<FaceSelector>();
            b6p6_b6p4_a5.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b6p6_b6p4_a5.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_b6p4_a5.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_b6p1_a1
            b6p6_b6p1_a1.gameObject.AddComponent<FuseBehavior>();
            b6p6_b6p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_b6p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_b6p1_a1.gameObject.AddComponent<FaceSelector>();
            b6p6_b6p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b6p6_b6p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_b6p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_b6p5_a1
            b6p6_b6p5_a1.gameObject.AddComponent<FuseBehavior>();
            b6p6_b6p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_b6p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_b6p5_a1.gameObject.AddComponent<FaceSelector>();
            b6p6_b6p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p6_b6p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_b6p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_b6p5_a2
            b6p6_b6p5_a2.gameObject.AddComponent<FuseBehavior>();
            b6p6_b6p5_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_b6p5_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_b6p5_a2.gameObject.AddComponent<FaceSelector>();
            b6p6_b6p5_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b6p6_b6p5_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_b6p5_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_bb6_a1
            b6p6_bb6_a1.gameObject.AddComponent<FuseBehavior>();
            b6p6_bb6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_bb6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_bb6_a1.gameObject.AddComponent<FaceSelector>();
            b6p6_bb6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b6p6_bb6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_bb6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b6p6_bb6_a2
            b6p6_bb6_a2.gameObject.AddComponent<FuseBehavior>();
            b6p6_bb6_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b6p6_bb6_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B6p6"));

            b6p6_bb6_a2.gameObject.AddComponent<FaceSelector>();
            b6p6_bb6_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b6p6_bb6_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b6p6_bb6_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[5] = newB6p6;
            partCreated[5] = true;
            partButtons[5].interactable = false;

            selectionManager.newPartCreated("b6p6Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b6p6");
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

