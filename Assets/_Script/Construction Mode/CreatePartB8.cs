using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartB8 : MonoBehaviour
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
        startObject = GameObject.Find("bb8Start");

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
    public FuseAttributes b8p1Fuses()
    {
        GameObject bb8 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb8Pos = bb8.transform.position;
        Vector3 fuseLocation = new Vector3(bb8Pos.x, bb8Pos.y, bb8Pos.z);
        fuseLocations.Add("b8p2_b8p1_a1", fuseLocation);
        fuseLocations.Add("b8p2_b8p1_a2", fuseLocation);
        fuseLocations.Add("b8p2_b8p1_a3", fuseLocation);
        fuseLocations.Add("b8p2_b8p1_a4", fuseLocation);
        fuseLocations.Add("b8p5_b8p1_a1", fuseLocation);
        fuseLocations.Add("b8p5_b8p1_a2", fuseLocation);
        fuseLocations.Add("bb8_b8p1_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b8p2_b8p1_a1", fuseRotation);
        fuseRotations.Add("b8p2_b8p1_a2", fuseRotation);
        fuseRotations.Add("b8p2_b8p1_a3", fuseRotation);
        fuseRotations.Add("b8p2_b8p1_a4", fuseRotation);
        fuseRotations.Add("b8p5_b8p1_a1", fuseRotation);
        fuseRotations.Add("b8p5_b8p1_a2", fuseRotation);
        fuseRotations.Add("bb8_b8p1_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        //Quaternion acceptableRotation2 = Quaternion.Euler(0, 90, 270);
        //Quaternion acceptableRotation3 = Quaternion.Euler(90, 180, 0);
        //Quaternion acceptableRotation4 = Quaternion.Euler(0, 270, 90);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions.Add("b8p2_b8p1_a1", acceptableRotations);
        fusePositions.Add("b8p2_b8p1_a2", acceptableRotations);
        fusePositions.Add("b8p2_b8p1_a3", acceptableRotations);
        fusePositions.Add("b8p2_b8p1_a4", acceptableRotations);
        fusePositions.Add("b8p5_b8p1_a1", acceptableRotations);
        fusePositions.Add("b8p5_b8p1_a2", acceptableRotations);
        fusePositions.Add("bb8_b8p1_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b8p2Fuses()
    {
        GameObject bb8 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb8Pos = bb8.transform.position;
        Vector3 fuseLocation = new Vector3(bb8Pos.x, bb8Pos.y, bb8Pos.z);
        fuseLocations.Add("b8p1_b8p2_a1", fuseLocation);
        fuseLocations.Add("b8p1_b8p2_a2", fuseLocation);
        fuseLocations.Add("b8p1_b8p2_a3", fuseLocation);
        fuseLocations.Add("b8p1_b8p2_a4", fuseLocation);
        fuseLocations.Add("bb8_b8p2_a1", fuseLocation);
        fuseLocations.Add("bb8_b8p2_a2", fuseLocation);
        fuseLocations.Add("bb8_b8p2_a3", fuseLocation);
        fuseLocations.Add("bb8_b8p2_a4", fuseLocation);
        fuseLocations.Add("bb8_b8p2_a5", fuseLocation);
        fuseLocations.Add("bb8_b8p2_a6", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b8p1_b8p2_a1", fuseRotation);
        fuseRotations.Add("b8p1_b8p2_a2", fuseRotation);
        fuseRotations.Add("b8p1_b8p2_a3", fuseRotation);
        fuseRotations.Add("b8p1_b8p2_a4", fuseRotation);
        fuseRotations.Add("bb8_b8p2_a1", fuseRotation);
        fuseRotations.Add("bb8_b8p2_a2", fuseRotation);
        fuseRotations.Add("bb8_b8p2_a3", fuseRotation);
        fuseRotations.Add("bb8_b8p2_a4", fuseRotation);
        fuseRotations.Add("bb8_b8p2_a5", fuseRotation);
        fuseRotations.Add("bb8_b8p2_a6", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1};
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b8p1_b8p2_a1", acceptableRotations);
        fusePositions.Add("b8p1_b8p2_a2", acceptableRotations);
        fusePositions.Add("b8p1_b8p2_a3", acceptableRotations);
        fusePositions.Add("b8p1_b8p2_a4", acceptableRotations);
        fusePositions.Add("bb8_b8p2_a1", acceptableRotations);
        fusePositions.Add("bb8_b8p2_a2", acceptableRotations);
        fusePositions.Add("bb8_b8p2_a3", acceptableRotations);
        fusePositions.Add("bb8_b8p2_a4", acceptableRotations);
        fusePositions.Add("bb8_b8p2_a5", acceptableRotations);
        fusePositions.Add("bb8_b8p2_a6", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b8p3Fuses()
    {
        GameObject bb8 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb8Pos = bb8.transform.position;
        Vector3 fuseLocation = new Vector3(bb8Pos.x, bb8Pos.y - 0.01f, bb8Pos.z + 8.6f);
        fuseLocations.Add("b8p4_b8p3_a1", fuseLocation);
        fuseLocations.Add("bb8_b8p3_a1", fuseLocation);
        fuseLocations.Add("bb8_b8p3_a2", fuseLocation);
        fuseLocations.Add("bb8_b8p3_a3", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b8p4_b8p3_a1", fuseRotation);
        fuseRotations.Add("bb8_b8p3_a1", fuseRotation);
        fuseRotations.Add("bb8_b8p3_a2", fuseRotation);
        fuseRotations.Add("bb8_b8p3_a3", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b8p4_b8p3_a1", acceptableRotations);
        fusePositions.Add("bb8_b8p3_a1", acceptableRotations);
        fusePositions.Add("bb8_b8p3_a2", acceptableRotations);
        fusePositions.Add("bb8_b8p3_a3", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b8p4Fuses()
    {
        GameObject bb8 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb8Pos = bb8.transform.position;
        Vector3 fuseLocation = new Vector3(bb8Pos.x, bb8Pos.y - 0.01f, bb8Pos.z + 16);
        fuseLocations.Add("b8p3_b8p4_a1", fuseLocation);
        fuseLocations.Add("b8p6_b8p4_a1", fuseLocation);
        fuseLocations.Add("b8p6_b8p4_a2", fuseLocation);
        fuseLocations.Add("b8p6_b8p4_a3", fuseLocation);
        fuseLocations.Add("bb8_b8p4_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b8p3_b8p4_a1", fuseRotation);
        fuseRotations.Add("b8p6_b8p4_a1", fuseRotation);
        fuseRotations.Add("b8p6_b8p4_a2", fuseRotation);
        fuseRotations.Add("b8p6_b8p4_a3", fuseRotation);
        fuseRotations.Add("bb8_b8p4_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b8p3_b8p4_a1", acceptableRotations);
        fusePositions.Add("b8p6_b8p4_a1", acceptableRotations);
        fusePositions.Add("b8p6_b8p4_a2", acceptableRotations);
        fusePositions.Add("b8p6_b8p4_a3", acceptableRotations);
        fusePositions.Add("bb8_b8p4_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b8p5Fuses()
    {
        GameObject bb8 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb8Pos = bb8.transform.position;
        Vector3 fuseLocation = new Vector3(bb8Pos.x, bb8Pos.y, bb8Pos.z);
        fuseLocations.Add("b8p1_b8p5_a1", fuseLocation);
        fuseLocations.Add("b8p1_b8p5_a2", fuseLocation);
        fuseLocations.Add("bb8_b8p5_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b8p1_b8p5_a1", fuseRotation);
        fuseRotations.Add("b8p1_b8p5_a2", fuseRotation);
        fuseRotations.Add("bb8_b8p5_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b8p1_b8p5_a1", acceptableRotations);
        fusePositions.Add("b8p1_b8p5_a2", acceptableRotations);
        fusePositions.Add("bb8_b8p5_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b8p6Fuses()
    {
        GameObject bb8 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb8Pos = bb8.transform.position;
        Vector3 fuseLocation = new Vector3(bb8Pos.x + 1.5f, bb8Pos.y - 0.01f, bb8Pos.z + 17.5f);
        fuseLocations.Add("b8p4_b8p6_a1", fuseLocation);
        fuseLocations.Add("b8p4_b8p6_a2", fuseLocation);
        fuseLocations.Add("b8p4_b8p6_a3", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b8p4_b8p6_a1", fuseRotation);
        fuseRotations.Add("b8p4_b8p6_a2", fuseRotation);
        fuseRotations.Add("b8p4_b8p6_a3", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(0, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1};
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b8p4_b8p6_a1", acceptableRotations);
        fusePositions.Add("b8p4_b8p6_a2", acceptableRotations);
        fusePositions.Add("b8p4_b8p6_a3", acceptableRotations);

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


    public void createB8p1()
    {
        if (!partCreated[0])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(0, 0, 180);
            GameObject newB8p1 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB8p1)); // this creates the zooming up from the ground effect

            Transform b8p1_b8p2_a1 = newB8p1.transform.Find("b8p1_b8p2_a1");
            Transform b8p1_b8p2_a2 = newB8p1.transform.Find("b8p1_b8p2_a2");
            Transform b8p1_b8p2_a3 = newB8p1.transform.Find("b8p1_b8p2_a3");
            Transform b8p1_b8p2_a4 = newB8p1.transform.Find("b8p1_b8p2_a4");
            Transform b8p1_b8p5_a1 = newB8p1.transform.Find("b8p1_b8p5_a1");
            Transform b8p1_b8p5_a2 = newB8p1.transform.Find("b8p1_b8p5_a2");
            Transform b8p1_bb8_a1 = newB8p1.transform.Find("b8p1_bb8_a1");

            FuseAttributes fuseAtts = b8p1Fuses();

            //b8p1_b8p2_a1
            b8p1_b8p2_a1.gameObject.AddComponent<FuseBehavior>();
            b8p1_b8p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p1_b8p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p1"));

            b8p1_b8p2_a1.gameObject.AddComponent<FaceSelector>();
            b8p1_b8p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b8p1_b8p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p1_b8p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p1_b8p2_a2
            b8p1_b8p2_a2.gameObject.AddComponent<FuseBehavior>();
            b8p1_b8p2_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p1_b8p2_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p1"));

            b8p1_b8p2_a2.gameObject.AddComponent<FaceSelector>();
            b8p1_b8p2_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b8p1_b8p2_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p1_b8p2_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p1_b8p2_a3
            b8p1_b8p2_a3.gameObject.AddComponent<FuseBehavior>();
            b8p1_b8p2_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p1_b8p2_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p1"));

            b8p1_b8p2_a3.gameObject.AddComponent<FaceSelector>();
            b8p1_b8p2_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p1_b8p2_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p1_b8p2_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p1_b8p2_a4
            b8p1_b8p2_a4.gameObject.AddComponent<FuseBehavior>();
            b8p1_b8p2_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p1_b8p2_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p1"));

            b8p1_b8p2_a4.gameObject.AddComponent<FaceSelector>();
            b8p1_b8p2_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p1_b8p2_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p1_b8p2_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p1_b8p5_a1
            b8p1_b8p5_a1.gameObject.AddComponent<FuseBehavior>();
            b8p1_b8p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p1_b8p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p1"));

            b8p1_b8p5_a1.gameObject.AddComponent<FaceSelector>();
            b8p1_b8p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p1_b8p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p1_b8p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p1_b8p5_a2
            b8p1_b8p5_a2.gameObject.AddComponent<FuseBehavior>();
            b8p1_b8p5_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p1_b8p5_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p1"));

            b8p1_b8p5_a2.gameObject.AddComponent<FaceSelector>();
            b8p1_b8p5_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b8p1_b8p5_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p1_b8p5_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p1_bb8_a1
            b8p1_bb8_a1.gameObject.AddComponent<FuseBehavior>();
            b8p1_bb8_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p1_bb8_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p1"));

            b8p1_bb8_a1.gameObject.AddComponent<FaceSelector>();
            b8p1_bb8_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b8p1_bb8_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p1_bb8_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newB8p1;
            partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("b8p1Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b8p1");
            }
        }
    }

    public void createB8p2()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 90, 0);
            GameObject newB8p2 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB8p2)); // this creates the zooming up from the ground effect

            Transform b8p2_b8p1_a1 = newB8p2.transform.Find("b8p2_b8p1_a1");
            Transform b8p2_b8p1_a2 = newB8p2.transform.Find("b8p2_b8p1_a2");
            Transform b8p2_b8p1_a3 = newB8p2.transform.Find("b8p2_b8p1_a3");
            Transform b8p2_b8p1_a4 = newB8p2.transform.Find("b8p2_b8p1_a4");
            Transform b8p2_bb8_a1 = newB8p2.transform.Find("b8p2_bb8_a1");
            Transform b8p2_bb8_a2 = newB8p2.transform.Find("b8p2_bb8_a2");
            Transform b8p2_bb8_a3 = newB8p2.transform.Find("b8p2_bb8_a3");
            Transform b8p2_bb8_a4 = newB8p2.transform.Find("b8p2_bb8_a4");
            Transform b8p2_bb8_a5 = newB8p2.transform.Find("b8p2_bb8_a5");
            Transform b8p2_bb8_a6 = newB8p2.transform.Find("b8p2_bb8_a6");

            FuseAttributes fuseAtts = b8p2Fuses();

            //b8p2_b8p1_a1
            b8p2_b8p1_a1.gameObject.AddComponent<FuseBehavior>();
            b8p2_b8p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_b8p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_b8p1_a1.gameObject.AddComponent<FaceSelector>();
            b8p2_b8p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p2_b8p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_b8p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_b8p1_a2
            b8p2_b8p1_a2.gameObject.AddComponent<FuseBehavior>();
            b8p2_b8p1_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_b8p1_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_b8p1_a2.gameObject.AddComponent<FaceSelector>();
            b8p2_b8p1_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p2_b8p1_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_b8p1_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_b8p1_a3
            b8p2_b8p1_a3.gameObject.AddComponent<FuseBehavior>();
            b8p2_b8p1_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_b8p1_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_b8p1_a3.gameObject.AddComponent<FaceSelector>();
            b8p2_b8p1_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p2_b8p1_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_b8p1_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_b8p1_a4
            b8p2_b8p1_a4.gameObject.AddComponent<FuseBehavior>();
            b8p2_b8p1_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_b8p1_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_b8p1_a4.gameObject.AddComponent<FaceSelector>();
            b8p2_b8p1_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b8p2_b8p1_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_b8p1_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_bb8_a1
            b8p2_bb8_a1.gameObject.AddComponent<FuseBehavior>();
            b8p2_bb8_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_bb8_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_bb8_a1.gameObject.AddComponent<FaceSelector>();
            b8p2_bb8_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p2_bb8_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_bb8_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_bb8_a2
            b8p2_bb8_a2.gameObject.AddComponent<FuseBehavior>();
            b8p2_bb8_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_bb8_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_bb8_a2.gameObject.AddComponent<FaceSelector>();
            b8p2_bb8_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p2_bb8_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_bb8_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_bb8_a3
            b8p2_bb8_a3.gameObject.AddComponent<FuseBehavior>();
            b8p2_bb8_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_bb8_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_bb8_a3.gameObject.AddComponent<FaceSelector>();
            b8p2_bb8_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b8p2_bb8_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_bb8_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_bb8_a4
            b8p2_bb8_a4.gameObject.AddComponent<FuseBehavior>();
            b8p2_bb8_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_bb8_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_bb8_a4.gameObject.AddComponent<FaceSelector>();
            b8p2_bb8_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b8p2_bb8_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_bb8_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_bb8_a5
            b8p2_bb8_a5.gameObject.AddComponent<FuseBehavior>();
            b8p2_bb8_a5.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_bb8_a5.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_bb8_a5.gameObject.AddComponent<FaceSelector>();
            b8p2_bb8_a5.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b8p2_bb8_a5.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_bb8_a5.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p2_bb8_a6
            b8p2_bb8_a6.gameObject.AddComponent<FuseBehavior>();
            b8p2_bb8_a6.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p2_bb8_a6.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p2"));

            b8p2_bb8_a6.gameObject.AddComponent<FaceSelector>();
            b8p2_bb8_a6.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b8p2_bb8_a6.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p2_bb8_a6.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[1] = newB8p2;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("b8p2Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b8p2");
            }
        }
    }

    public void createB8p3()
    {
        if (!partCreated[2])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(270, 0, 90);
            GameObject newB8p3 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB8p3)); // this creates the zooming up from the ground effect

            Transform b8p3_b8p4_a1 = newB8p3.transform.Find("b8p3_b8p4_a1");
            Transform b8p3_bb8_a1 = newB8p3.transform.Find("b8p3_bb8_a1");
            Transform b8p3_bb8_a2 = newB8p3.transform.Find("b8p3_bb8_a2");
            Transform b8p3_bb8_a3 = newB8p3.transform.Find("b8p3_bb8_a3");

            FuseAttributes fuseAtts = b8p3Fuses();

            //b8p3_b8p4_a1
            b8p3_b8p4_a1.gameObject.AddComponent<FuseBehavior>();
            b8p3_b8p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p3_b8p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p3"));

            b8p3_b8p4_a1.gameObject.AddComponent<FaceSelector>();
            b8p3_b8p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p3_b8p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p3_b8p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p3_bb8_a1
            b8p3_bb8_a1.gameObject.AddComponent<FuseBehavior>();
            b8p3_bb8_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p3_bb8_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p3"));

            b8p3_bb8_a1.gameObject.AddComponent<FaceSelector>();
            b8p3_bb8_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p3_bb8_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p3_bb8_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p3_bb8_a2
            b8p3_bb8_a2.gameObject.AddComponent<FuseBehavior>();
            b8p3_bb8_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p3_bb8_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p3"));

            b8p3_bb8_a2.gameObject.AddComponent<FaceSelector>();
            b8p3_bb8_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p3_bb8_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p3_bb8_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p3_bb8_a3
            b8p3_bb8_a3.gameObject.AddComponent<FuseBehavior>();
            b8p3_bb8_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p3_bb8_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p3"));

            b8p3_bb8_a3.gameObject.AddComponent<FaceSelector>();
            b8p3_bb8_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p3_bb8_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p3_bb8_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

   
            instantiated[2] = newB8p3;
            partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("b8p3Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b8p3");
            }
        }
    }

    public void createB8p4()
    {
        if (!partCreated[3])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 0, 90);
            GameObject newB8p4 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[3], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB8p4)); // this creates the zooming up from the ground effect

            Transform b8p4_b8p3_a1 = newB8p4.transform.Find("b8p4_b8p3_a1");
            Transform b8p4_b8p6_a1 = newB8p4.transform.Find("b8p4_b8p6_a1");
            Transform b8p4_b8p6_a2 = newB8p4.transform.Find("b8p4_b8p6_a2");
            Transform b8p4_b8p6_a3 = newB8p4.transform.Find("b8p4_b8p6_a3");
            Transform b8p4_bb8_a1 = newB8p4.transform.Find("b8p4_bb8_a1");

            FuseAttributes fuseAtts = b8p4Fuses();

            //b8p4_b8p3_a1
            b8p4_b8p3_a1.gameObject.AddComponent<FuseBehavior>();
            b8p4_b8p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p4_b8p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p4"));

            b8p4_b8p3_a1.gameObject.AddComponent<FaceSelector>();
            b8p4_b8p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p4_b8p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p4_b8p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p4_b8p6_a1
            b8p4_b8p6_a1.gameObject.AddComponent<FuseBehavior>();
            b8p4_b8p6_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p4_b8p6_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p4"));

            b8p4_b8p6_a1.gameObject.AddComponent<FaceSelector>();
            b8p4_b8p6_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p4_b8p6_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p4_b8p6_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p4_b8p6_a2
            b8p4_b8p6_a2.gameObject.AddComponent<FuseBehavior>();
            b8p4_b8p6_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p4_b8p6_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p4"));

            b8p4_b8p6_a2.gameObject.AddComponent<FaceSelector>();
            b8p4_b8p6_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p4_b8p6_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p4_b8p6_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p4_b8p6_a3
            b8p4_b8p6_a3.gameObject.AddComponent<FuseBehavior>();
            b8p4_b8p6_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p4_b8p6_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p4"));

            b8p4_b8p6_a3.gameObject.AddComponent<FaceSelector>();
            b8p4_b8p6_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p4_b8p6_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p4_b8p6_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p4_bb8_a1
            b8p4_bb8_a1.gameObject.AddComponent<FuseBehavior>();
            b8p4_bb8_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p4_bb8_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p4"));

            b8p4_bb8_a1.gameObject.AddComponent<FaceSelector>();
            b8p4_bb8_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b8p4_bb8_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p4_bb8_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[3] = newB8p4;
            partCreated[3] = true;
            partButtons[3].interactable = false;

            selectionManager.newPartCreated("b8p4Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b8p4");
            }
        }
    }

    public void createB8p5()
    {
        if (!partCreated[4])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(180, 90, 0);
            GameObject newB8p5 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[4], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB8p5)); // this creates the zooming up from the ground effect

            Transform b8p5_b8p1_a1 = newB8p5.transform.Find("b8p5_b8p1_a1");
            Transform b8p5_b8p1_a2 = newB8p5.transform.Find("b8p5_b8p1_a2");
            Transform b8p5_bb8_a1 = newB8p5.transform.Find("b8p5_bb8_a1");

            FuseAttributes fuseAtts = b8p5Fuses();

            //b8p5_b8p1_a1
            b8p5_b8p1_a1.gameObject.AddComponent<FuseBehavior>();
            b8p5_b8p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p5_b8p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p5"));

            b8p5_b8p1_a1.gameObject.AddComponent<FaceSelector>();
            b8p5_b8p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p5_b8p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p5_b8p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p5_b8p1_a2
            b8p5_b8p1_a2.gameObject.AddComponent<FuseBehavior>();
            b8p5_b8p1_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p5_b8p1_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p5"));

            b8p5_b8p1_a2.gameObject.AddComponent<FaceSelector>();
            b8p5_b8p1_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b8p5_b8p1_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p5_b8p1_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p5_bb8_a1
            b8p5_bb8_a1.gameObject.AddComponent<FuseBehavior>();
            b8p5_bb8_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p5_bb8_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p5"));

            b8p5_bb8_a1.gameObject.AddComponent<FaceSelector>();
            b8p5_bb8_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b8p5_bb8_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p5_bb8_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[4] = newB8p5;
            partCreated[4] = true;
            partButtons[4].interactable = false;

            selectionManager.newPartCreated("b8p5Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b8p5");
            }
        }
    }

    public void createB8p6()
    {
        if (!partCreated[5])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 0, 270);
            GameObject newB8p6 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[5], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB8p6)); // this creates the zooming up from the ground effect

            Transform b8p6_b8p4_a1 = newB8p6.transform.Find("b8p6_b8p4_a1");
            Transform b8p6_b8p4_a2 = newB8p6.transform.Find("b8p6_b8p4_a2");
            Transform b8p6_b8p4_a3 = newB8p6.transform.Find("b8p6_b8p4_a3");

            FuseAttributes fuseAtts = b8p6Fuses();

            //b8p6_b8p4_a1
            b8p6_b8p4_a1.gameObject.AddComponent<FuseBehavior>();
            b8p6_b8p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p6_b8p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p6"));

            b8p6_b8p4_a1.gameObject.AddComponent<FaceSelector>();
            b8p6_b8p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b8p6_b8p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p6_b8p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p6_b8p4_a2
            b8p6_b8p4_a2.gameObject.AddComponent<FuseBehavior>();
            b8p6_b8p4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p6_b8p4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p6"));

            b8p6_b8p4_a2.gameObject.AddComponent<FaceSelector>();
            b8p6_b8p4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b8p6_b8p4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p6_b8p4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b8p6_b8p4_a3
            b8p6_b8p4_a3.gameObject.AddComponent<FuseBehavior>();
            b8p6_b8p4_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b8p6_b8p4_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B8p6"));

            b8p6_b8p4_a3.gameObject.AddComponent<FaceSelector>();
            b8p6_b8p4_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b8p6_b8p4_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b8p6_b8p4_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[5] = newB8p6;
            partCreated[5] = true;
            partButtons[5].interactable = false;

            selectionManager.newPartCreated("b8p6Prefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b8p6");
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

