﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartB4_harder : MonoBehaviour
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
        createLoc = new Vector3(-40, 25, 100);
        offscreenCreateLoc = new Vector3(-40, -60, 100);
        selectionManager = eventSystem.GetComponent<SelectPart>();
        startObject = GameObject.Find("bb4_harderStart");

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
    public FuseAttributes b4p1Fuses()
    {
        GameObject bb4 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb4Pos = bb4.transform.position;
        Vector3 fuseLocation = new Vector3(bb4Pos.x + 5, bb4Pos.y, bb4Pos.z - 27.65f);
        fuseLocations.Add("b4p2_b4p1_a1", fuseLocation);
        fuseLocations.Add("b4p3_b4p1_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b4p2_b4p1_a1", fuseRotation);
        fuseRotations.Add("b4p3_b4p1_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        //Quaternion acceptableRotation2 = Quaternion.Euler(0, 90, 270);
        //Quaternion acceptableRotation3 = Quaternion.Euler(90, 180, 0);
        //Quaternion acceptableRotation4 = Quaternion.Euler(0, 270, 90);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions.Add("b4p2_b4p1_a1", acceptableRotations);
        fusePositions.Add("b4p3_b4p1_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b4p2Fuses()
    {
        GameObject bb4 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb4Pos = bb4.transform.position;
        Vector3 fuseLocation = new Vector3(bb4Pos.x - 5.1f, bb4Pos.y, bb4Pos.z - 27.55f);
        fuseLocations.Add("b4p1_b4p2_a1", fuseLocation);
        fuseLocations.Add("b4p4_b4p2_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b4p1_b4p2_a1", fuseRotation);
        fuseRotations.Add("b4p4_b4p2_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b4p1_b4p2_a1", acceptableRotations);
        fusePositions.Add("b4p4_b4p2_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b4p3Fuses()
    {
        GameObject bb4 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb4Pos = bb4.transform.position;
        Vector3 fuseLocation = new Vector3(bb4Pos.x + 5, bb4Pos.y, bb4Pos.z - 12.5f);
        fuseLocations.Add("bb4_b4p3_a1", fuseLocation);
        fuseLocations.Add("bb4_b4p3_a2", fuseLocation);
        fuseLocations.Add("b4p1_b4p3_a1", fuseLocation);
        fuseLocations.Add("b4p4_b4p3_a1", fuseLocation);
        fuseLocations.Add("b4p4_b4p3_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("bb4_b4p3_a1", fuseRotation);
        fuseRotations.Add("bb4_b4p3_a2", fuseRotation);
        fuseRotations.Add("b4p1_b4p3_a1", fuseRotation);
        fuseRotations.Add("b4p4_b4p3_a1", fuseRotation);
        fuseRotations.Add("b4p4_b4p3_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("bb4_b4p3_a1", acceptableRotations);
        fusePositions.Add("bb4_b4p3_a2", acceptableRotations);
        fusePositions.Add("b4p1_b4p3_a1", acceptableRotations);
        fusePositions.Add("b4p4_b4p3_a1", acceptableRotations);
        fusePositions.Add("b4p4_b4p3_a2", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b4p4Fuses()
    {
        GameObject bb4 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb4Pos = bb4.transform.position;
        Vector3 fuseLocation = new Vector3(bb4Pos.x - 4.9f, bb4Pos.y, bb4Pos.z - 12.5f);
        fuseLocations.Add("b4p2_b4p4_a1", fuseLocation);
        fuseLocations.Add("b4p3_b4p4_a1", fuseLocation);
        fuseLocations.Add("b4p3_b4p4_a2", fuseLocation);
        fuseLocations.Add("b4p5_b4p4_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b4p2_b4p4_a1", fuseRotation);
        fuseRotations.Add("b4p3_b4p4_a1", fuseRotation);
        fuseRotations.Add("b4p3_b4p4_a2", fuseRotation);
        fuseRotations.Add("b4p5_b4p4_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b4p2_b4p4_a1", acceptableRotations);
        fusePositions.Add("b4p3_b4p4_a1", acceptableRotations);
        fusePositions.Add("b4p3_b4p4_a2", acceptableRotations);
        fusePositions.Add("b4p5_b4p4_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b4p5Fuses()
    {
        GameObject bb4 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb4Pos = bb4.transform.position;
        Vector3 fuseLocation = new Vector3(bb4Pos.x - 4.9f, bb4Pos.y, bb4Pos.z + 2.4f);
        fuseLocations.Add("b4p4_b4p5_a1", fuseLocation);
        fuseLocations.Add("bb4_b4p5_a1", fuseLocation);
        fuseLocations.Add("bb4_b4p5_a2", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b4p4_b4p5_a1", fuseRotation);
        fuseRotations.Add("bb4_b4p5_a1", fuseRotation);
        fuseRotations.Add("bb4_b4p5_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b4p4_b4p5_a1", acceptableRotations);
        fusePositions.Add("bb4_b4p5_a1", acceptableRotations);
        fusePositions.Add("bb4_b4p5_a2", acceptableRotations);

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


    public void createB4p1()
    {
        if (!partCreated[0])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(180, 0, 0);
            GameObject newB4p1 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB4p1)); // this creates the zooming up from the ground effect

            Transform b4p1_b4p2_a1 = newB4p1.transform.Find("b4p1_b4p2_a1");
            Transform b4p1_b4p3_a1 = newB4p1.transform.Find("b4p1_b4p3_a1");

            FuseAttributes fuseAtts = b4p1Fuses();

            //b4p1_b4p2_a1
            b4p1_b4p2_a1.gameObject.AddComponent<FuseBehavior>();
            b4p1_b4p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p1_b4p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p1"));

            b4p1_b4p2_a1.gameObject.AddComponent<FaceSelector>();
            b4p1_b4p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b4p1_b4p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p1_b4p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p1_b4p3_a1
            b4p1_b4p3_a1.gameObject.AddComponent<FuseBehavior>();
            b4p1_b4p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p1_b4p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p1"));

            b4p1_b4p3_a1.gameObject.AddComponent<FaceSelector>();
            b4p1_b4p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b4p1_b4p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p1_b4p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newB4p1;
            partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("b4p1_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b4p1");
            }
        }
    }

    public void createB4p2()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(180, 90, 90);
            GameObject newB4p2 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB4p2)); // this creates the zooming up from the ground effect

            Transform b4p2_b4p1_a1 = newB4p2.transform.Find("b4p2_b4p1_a1");
            Transform b4p2_b4p4_a1 = newB4p2.transform.Find("b4p2_b4p4_a1");

            FuseAttributes fuseAtts = b4p2Fuses();


            //b4p2_b4p1_a1
            b4p2_b4p1_a1.gameObject.AddComponent<FuseBehavior>();
            b4p2_b4p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p2_b4p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p2"));

            b4p2_b4p1_a1.gameObject.AddComponent<FaceSelector>();
            b4p2_b4p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b4p2_b4p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p2_b4p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p2_b4p4_a1
            b4p2_b4p4_a1.gameObject.AddComponent<FuseBehavior>();
            b4p2_b4p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p2_b4p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p2"));

            b4p2_b4p4_a1.gameObject.AddComponent<FaceSelector>();
            b4p2_b4p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b4p2_b4p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p2_b4p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[1] = newB4p2;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("b4p2_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b4p2");
            }
        }
    }

    public void createB4p3()
    {
        if (!partCreated[2])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 0, 90);
            GameObject newB4p3 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB4p3)); // this creates the zooming up from the ground effect

            Transform b4p3_bb4_a1 = newB4p3.transform.Find("b4p3_bb4_a1");
            Transform b4p3_bb4_a2 = newB4p3.transform.Find("b4p3_bb4_a2");
            Transform b4p3_b4p1_a1 = newB4p3.transform.Find("b4p3_b4p1_a1");
            Transform b4p3_b4p4_a1 = newB4p3.transform.Find("b4p3_b4p4_a1");
            Transform b4p3_b4p4_a2 = newB4p3.transform.Find("b4p3_b4p4_a2");

            FuseAttributes fuseAtts = b4p3Fuses();

            //b4p3_bb4_a1
            b4p3_bb4_a1.gameObject.AddComponent<FuseBehavior>();
            b4p3_bb4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p3_bb4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p3"));

            b4p3_bb4_a1.gameObject.AddComponent<FaceSelector>();
            b4p3_bb4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b4p3_bb4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p3_bb4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p3_bb4_a2
            b4p3_bb4_a2.gameObject.AddComponent<FuseBehavior>();
            b4p3_bb4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p3_bb4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p3"));

            b4p3_bb4_a2.gameObject.AddComponent<FaceSelector>();
            b4p3_bb4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b4p3_bb4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p3_bb4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p3_b4p1_a1
            b4p3_b4p1_a1.gameObject.AddComponent<FuseBehavior>();
            b4p3_b4p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p3_b4p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p3"));

            b4p3_b4p1_a1.gameObject.AddComponent<FaceSelector>();
            b4p3_b4p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b4p3_b4p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p3_b4p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p3_b4p4_a1
            b4p3_b4p4_a1.gameObject.AddComponent<FuseBehavior>();
            b4p3_b4p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p3_b4p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p3"));

            b4p3_b4p4_a1.gameObject.AddComponent<FaceSelector>();
            b4p3_b4p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b4p3_b4p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p3_b4p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p3_b4p4_a2
            b4p3_b4p4_a2.gameObject.AddComponent<FuseBehavior>();
            b4p3_b4p4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p3_b4p4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p3"));

            b4p3_b4p4_a2.gameObject.AddComponent<FaceSelector>();
            b4p3_b4p4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b4p3_b4p4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p3_b4p4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[2] = newB4p3;
            partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("b4p3_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b4p3");
            }
        }
    }

    public void createB4p4()
    {
        if (!partCreated[3])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 90, 0);
            GameObject newB4p4 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[3], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB4p4)); // this creates the zooming up from the ground effect

            Transform b4p4_b4p2_a1 = newB4p4.transform.Find("b4p4_b4p2_a1");
            Transform b4p4_b4p3_a1 = newB4p4.transform.Find("b4p4_b4p3_a1");
            Transform b4p4_b4p3_a2 = newB4p4.transform.Find("b4p4_b4p3_a2");
            Transform b4p4_b4p5_a1 = newB4p4.transform.Find("b4p4_b4p5_a1");

            FuseAttributes fuseAtts = b4p4Fuses();

            //b4p4_b4p2_a1
            b4p4_b4p2_a1.gameObject.AddComponent<FuseBehavior>();
            b4p4_b4p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p4_b4p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p4"));

            b4p4_b4p2_a1.gameObject.AddComponent<FaceSelector>();
            b4p4_b4p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b4p4_b4p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p4_b4p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p4_b4p3_a1
            b4p4_b4p3_a1.gameObject.AddComponent<FuseBehavior>();
            b4p4_b4p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p4_b4p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p4"));

            b4p4_b4p3_a1.gameObject.AddComponent<FaceSelector>();
            b4p4_b4p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b4p4_b4p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p4_b4p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p4_b4p3_a2
            b4p4_b4p3_a2.gameObject.AddComponent<FuseBehavior>();
            b4p4_b4p3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p4_b4p3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p4"));

            b4p4_b4p3_a2.gameObject.AddComponent<FaceSelector>();
            b4p4_b4p3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b4p4_b4p3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p4_b4p3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p4_b4p5_a1
            b4p4_b4p5_a1.gameObject.AddComponent<FuseBehavior>();
            b4p4_b4p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p4_b4p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p4"));

            b4p4_b4p5_a1.gameObject.AddComponent<FaceSelector>();
            b4p4_b4p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b4p4_b4p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p4_b4p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());


            instantiated[3] = newB4p4;
            partCreated[3] = true;
            partButtons[3].interactable = false;

            selectionManager.newPartCreated("b4p4_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b4p4");
            }
        }
    }

    public void createB4p5()
    {
        if (!partCreated[4])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 180, 0);
            GameObject newB4p5 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[4], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB4p5)); // this creates the zooming up from the ground effect

            Transform b4p5_bb4_a1 = newB4p5.transform.Find("b4p5_bb4_a1");
            Transform b4p5_bb4_a2 = newB4p5.transform.Find("b4p5_bb4_a2");
            Transform b4p5_b4p4_a1 = newB4p5.transform.Find("b4p5_b4p4_a1");

            FuseAttributes fuseAtts = b4p5Fuses();

            //b4p5_bb4_a1
            b4p5_bb4_a1.gameObject.AddComponent<FuseBehavior>();
            b4p5_bb4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p5_bb4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p5"));

            b4p5_bb4_a1.gameObject.AddComponent<FaceSelector>();
            b4p5_bb4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b4p5_bb4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p5_bb4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p5_bb4_a2
            b4p5_bb4_a2.gameObject.AddComponent<FuseBehavior>();
            b4p5_bb4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p5_bb4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p5"));

            b4p5_bb4_a2.gameObject.AddComponent<FaceSelector>();
            b4p5_bb4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b4p5_bb4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p5_bb4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b4p5_b4p4_a1
            b4p5_b4p4_a1.gameObject.AddComponent<FuseBehavior>();
            b4p5_b4p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b4p5_b4p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B4p5"));

            b4p5_b4p4_a1.gameObject.AddComponent<FaceSelector>();
            b4p5_b4p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b4p5_b4p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b4p5_b4p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[4] = newB4p5;
            partCreated[4] = true;
            partButtons[4].interactable = false;

            selectionManager.newPartCreated("b4p5_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b4p5");
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

