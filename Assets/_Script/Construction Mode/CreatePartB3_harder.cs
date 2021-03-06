﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartB3_harder : MonoBehaviour
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
        startObject = GameObject.Find("bb3_harderStart");

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
    public FuseAttributes b3p1Fuses()
    {
        GameObject bb3 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb3Pos = bb3.transform.position;
        Vector3 fuseLocation = new Vector3(bb3Pos.x, bb3Pos.y, bb3Pos.z + 16.1f);
        fuseLocations.Add("bb3_b3p1_a1", fuseLocation);
        fuseLocations.Add("bb3_b3p1_a2", fuseLocation);
        fuseLocations.Add("b3p3_b3p1_a1", fuseLocation);
        fuseLocations.Add("b3p3_b3p1_a2", fuseLocation);
        fuseLocations.Add("b3p2_b3p1_a1", fuseLocation);


        Quaternion fuseRotation = Quaternion.Euler(new Vector3(180, 0, 0));
        fuseRotations.Add("bb3_b3p1_a1", fuseRotation);
        fuseRotations.Add("bb3_b3p1_a2", fuseRotation);
        fuseRotations.Add("b3p3_b3p1_a1", fuseRotation);
        fuseRotations.Add("b3p3_b3p1_a2", fuseRotation);
        fuseRotations.Add("b3p2_b3p1_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        //Quaternion acceptableRotation2 = Quaternion.Euler(0, 90, 270);
        //Quaternion acceptableRotation3 = Quaternion.Euler(90, 180, 0);
        //Quaternion acceptableRotation4 = Quaternion.Euler(0, 270, 90);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions.Add("bb3_b3p1_a1", acceptableRotations);
        fusePositions.Add("bb3_b3p1_a2", acceptableRotations);
        fusePositions.Add("b3p3_b3p1_a1", acceptableRotations);
        fusePositions.Add("b3p3_b3p1_a2", acceptableRotations);
        fusePositions.Add("b3p2_b3p1_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b3p2Fuses()
    {
        GameObject bb3 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb3Pos = bb3.transform.position;
        Vector3 fuseLocation = new Vector3(bb3Pos.x, bb3Pos.y + 7.6f, bb3Pos.z + 5);
        fuseLocations.Add("bb3_b3p2_a1", fuseLocation);
        fuseLocations.Add("bb3_b3p2_a2", fuseLocation);
        fuseLocations.Add("b3p3_b3p2_a1", fuseLocation);
        fuseLocations.Add("b3p3_b3p2_a2", fuseLocation);
        fuseLocations.Add("b3p1_b3p2_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("bb3_b3p2_a1", fuseRotation);
        fuseRotations.Add("bb3_b3p2_a2", fuseRotation);
        fuseRotations.Add("b3p3_b3p2_a1", fuseRotation);
        fuseRotations.Add("b3p3_b3p2_a2", fuseRotation);
        fuseRotations.Add("b3p1_b3p2_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1};
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("bb3_b3p2_a1", acceptableRotations);
        fusePositions.Add("bb3_b3p2_a2", acceptableRotations);
        fusePositions.Add("b3p3_b3p2_a1", acceptableRotations);
        fusePositions.Add("b3p3_b3p2_a2", acceptableRotations);
        fusePositions.Add("b3p1_b3p2_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b3p3Fuses()
    {
        GameObject bb3 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb3Pos = bb3.transform.position;
        Vector3 fuseLocation = new Vector3(bb3Pos.x, bb3Pos.y, bb3Pos.z + 15.7f);
        fuseLocations.Add("b3p1_b3p3_a1", fuseLocation);
        fuseLocations.Add("b3p1_b3p3_a2", fuseLocation);
        fuseLocations.Add("b3p2_b3p3_a1", fuseLocation);
        fuseLocations.Add("b3p2_b3p3_a2", fuseLocation);
        fuseLocations.Add("b3p4_b3p3_a1", fuseLocation);
        fuseLocations.Add("b3p4_b3p3_a2", fuseLocation);
        fuseLocations.Add("b3p4_b3p3_a3", fuseLocation);
        fuseLocations.Add("b3p4_b3p3_a4", fuseLocation);
        fuseLocations.Add("bb3_b3p3_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b3p1_b3p3_a1", fuseRotation);
        fuseRotations.Add("b3p1_b3p3_a2", fuseRotation);
        fuseRotations.Add("b3p2_b3p3_a1", fuseRotation);
        fuseRotations.Add("b3p2_b3p3_a2", fuseRotation);
        fuseRotations.Add("b3p4_b3p3_a1", fuseRotation);
        fuseRotations.Add("b3p4_b3p3_a2", fuseRotation);
        fuseRotations.Add("b3p4_b3p3_a3", fuseRotation);
        fuseRotations.Add("b3p4_b3p3_a4", fuseRotation);
        fuseRotations.Add("bb3_b3p3_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b3p1_b3p3_a1", acceptableRotations);
        fusePositions.Add("b3p1_b3p3_a2", acceptableRotations);
        fusePositions.Add("b3p2_b3p3_a1", acceptableRotations);
        fusePositions.Add("b3p2_b3p3_a2", acceptableRotations);
        fusePositions.Add("b3p4_b3p3_a1", acceptableRotations);
        fusePositions.Add("b3p4_b3p3_a2", acceptableRotations);
        fusePositions.Add("b3p4_b3p3_a3", acceptableRotations);
        fusePositions.Add("b3p4_b3p3_a4", acceptableRotations);
        fusePositions.Add("bb3_b3p3_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b3p4Fuses()
    {
        GameObject bb3 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb3Pos = bb3.transform.position;
        Vector3 fuseLocation = new Vector3(bb3Pos.x, bb3Pos.y, bb3Pos.z + 35);
        fuseLocations.Add("b3p3_b3p4_a1", fuseLocation);
        fuseLocations.Add("b3p3_b3p4_a2", fuseLocation);
        fuseLocations.Add("b3p3_b3p4_a3", fuseLocation);
        fuseLocations.Add("b3p3_b3p4_a4", fuseLocation);
        fuseLocations.Add("b3p5_b3p4_a1", fuseLocation);
        fuseLocations.Add("b3p5_b3p4_a2", fuseLocation);
        fuseLocations.Add("b3p5_b3p4_a3", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b3p3_b3p4_a1", fuseRotation);
        fuseRotations.Add("b3p3_b3p4_a2", fuseRotation);
        fuseRotations.Add("b3p3_b3p4_a3", fuseRotation);
        fuseRotations.Add("b3p3_b3p4_a4", fuseRotation);
        fuseRotations.Add("b3p5_b3p4_a1", fuseRotation);
        fuseRotations.Add("b3p5_b3p4_a2", fuseRotation);
        fuseRotations.Add("b3p5_b3p4_a3", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b3p3_b3p4_a1", acceptableRotations);
        fusePositions.Add("b3p3_b3p4_a2", acceptableRotations);
        fusePositions.Add("b3p3_b3p4_a3", acceptableRotations);
        fusePositions.Add("b3p3_b3p4_a4", acceptableRotations);
        fusePositions.Add("b3p5_b3p4_a1", acceptableRotations);
        fusePositions.Add("b3p5_b3p4_a2", acceptableRotations);
        fusePositions.Add("b3p5_b3p4_a3", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b3p5Fuses()
    {
        GameObject bb3 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb3Pos = bb3.transform.position;
        Vector3 fuseLocation = new Vector3(bb3Pos.x + 4.85f, bb3Pos.y - 4.9f, bb3Pos.z + 39.9f);
        fuseLocations.Add("b3p4_b3p5_a1", fuseLocation);
        fuseLocations.Add("b3p4_b3p5_a2", fuseLocation);
        fuseLocations.Add("b3p4_b3p5_a3", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("b3p4_b3p5_a1", fuseRotation);
        fuseRotations.Add("b3p4_b3p5_a2", fuseRotation);
        fuseRotations.Add("b3p4_b3p5_a3", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 180, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("b3p4_b3p5_a1", acceptableRotations);
        fusePositions.Add("b3p4_b3p5_a2", acceptableRotations);
        fusePositions.Add("b3p4_b3p5_a3", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes bb3_harder_grayFuses()
    {
        GameObject bb3 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb3Pos = bb3.transform.position;
        Vector3 fuseLocation = new Vector3(bb3Pos.x, bb3Pos.y, bb3Pos.z - 7.5f);
        fuseLocations.Add("bb3_bb3_gray_a1", fuseLocation);
  
        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        fuseRotations.Add("bb3_bb3_gray_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(90, 0, 0);
        Quaternion acceptableRotation2 = Quaternion.Euler(0, 270, 270);
        Quaternion acceptableRotation3 = Quaternion.Euler(270, 180, 0);
        Quaternion acceptableRotation4 = Quaternion.Euler(0, 90, 90);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("bb3_bb3_gray_a1", acceptableRotations);

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


    public void createB3p1()
    {
        if (!partCreated[0])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(0, 0, 0);
            GameObject newB3p1 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB3p1)); // this creates the zooming up from the ground effect

            Transform b3p1_bb3_a1 = newB3p1.transform.Find("b3p1_bb3_a1");
            Transform b3p1_b3p2_a1 = newB3p1.transform.Find("b3p1_b3p2_a1");
            Transform b3p1_b3p3_a1 = newB3p1.transform.Find("b3p1_b3p3_a1");
            Transform b3p1_b3p3_a2 = newB3p1.transform.Find("b3p1_b3p3_a2");
            Transform b3p1_bb3_a2 = newB3p1.transform.Find("b3p1_bb3_a2");

            FuseAttributes fuseAtts = b3p1Fuses();

            //b3p1_bb3_a1
            b3p1_bb3_a1.gameObject.AddComponent<FuseBehavior>();
            b3p1_bb3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p1_bb3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p1"));

            b3p1_bb3_a1.gameObject.AddComponent<FaceSelector>();
            b3p1_bb3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b3p1_bb3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p1_bb3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p1_b3p2_a1
            b3p1_b3p2_a1.gameObject.AddComponent<FuseBehavior>();
            b3p1_b3p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p1_b3p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p1"));

            b3p1_b3p2_a1.gameObject.AddComponent<FaceSelector>();
            b3p1_b3p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b3p1_b3p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p1_b3p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p1_b3p3_a1
            b3p1_b3p3_a1.gameObject.AddComponent<FuseBehavior>();
            b3p1_b3p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p1_b3p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p1"));

            b3p1_b3p3_a1.gameObject.AddComponent<FaceSelector>();
            b3p1_b3p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b3p1_b3p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p1_b3p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p1_b3p3_a2
            b3p1_b3p3_a2.gameObject.AddComponent<FuseBehavior>();
            b3p1_b3p3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p1_b3p3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p1"));

            b3p1_b3p3_a2.gameObject.AddComponent<FaceSelector>();
            b3p1_b3p3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b3p1_b3p3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p1_b3p3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p1_bb3_a2
            b3p1_bb3_a2.gameObject.AddComponent<FuseBehavior>();
            b3p1_bb3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p1_bb3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p1"));

            b3p1_bb3_a2.gameObject.AddComponent<FaceSelector>();
            b3p1_bb3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b3p1_bb3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p1_bb3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[0] = newB3p1;
            partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("b3p1_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b3p1");
            }
        }
    }

    public void createB3p2()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(270, 90, 0);
            GameObject newB3p2 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB3p2)); // this creates the zooming up from the ground effect

            Transform b3p2_bb3_a1 = newB3p2.transform.Find("b3p2_bb3_a1");
            Transform b3p2_bb3_a2 = newB3p2.transform.Find("b3p2_bb3_a2");
            Transform b3p2_b3p1_a1 = newB3p2.transform.Find("b3p2_b3p1_a1");
            Transform b3p2_b3p3_a1 = newB3p2.transform.Find("b3p2_b3p3_a1");
            Transform b3p2_b3p3_a2 = newB3p2.transform.Find("b3p2_b3p3_a2");

            FuseAttributes fuseAtts = b3p2Fuses();

            //b3p2_bb3_a1
            b3p2_bb3_a1.gameObject.AddComponent<FuseBehavior>();
            b3p2_bb3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p2_bb3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p2"));

            b3p2_bb3_a1.gameObject.AddComponent<FaceSelector>();
            b3p2_bb3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b3p2_bb3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p2_bb3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p2_bb3_a2
            b3p2_bb3_a2.gameObject.AddComponent<FuseBehavior>();
            b3p2_bb3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p2_bb3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p2"));

            b3p2_bb3_a2.gameObject.AddComponent<FaceSelector>();
            b3p2_bb3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b3p2_bb3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p2_bb3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p2_b3p1_a1
            b3p2_b3p1_a1.gameObject.AddComponent<FuseBehavior>();
            b3p2_b3p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p2_b3p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p2"));

            b3p2_b3p1_a1.gameObject.AddComponent<FaceSelector>();
            b3p2_b3p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b3p2_b3p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p2_b3p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p2_b3p3_a1
            b3p2_b3p3_a1.gameObject.AddComponent<FuseBehavior>();
            b3p2_b3p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p2_b3p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p2"));

            b3p2_b3p3_a1.gameObject.AddComponent<FaceSelector>();
            b3p2_b3p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b3p2_b3p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p2_b3p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p2_b3p3_a2
            b3p2_b3p3_a2.gameObject.AddComponent<FuseBehavior>();
            b3p2_b3p3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p2_b3p3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p2"));

            b3p2_b3p3_a2.gameObject.AddComponent<FaceSelector>();
            b3p2_b3p3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b3p2_b3p3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p2_b3p3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[1] = newB3p2;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("b3p2_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b3p2");
            }
        }
    }

    public void createB3p3()
    {
        if (!partCreated[2])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 90, 90);
            GameObject newB3p3 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB3p3)); // this creates the zooming up from the ground effect

            Transform b3p3_bb3_a1 = newB3p3.transform.Find("b3p3_bb3_a1");

            Transform b3p3_b3p1_a1 = newB3p3.transform.Find("b3p3_b3p1_a1");
            Transform b3p3_b3p1_a2 = newB3p3.transform.Find("b3p3_b3p1_a2");
            Transform b3p3_b3p2_a1 = newB3p3.transform.Find("b3p3_b3p2_a1");
            Transform b3p3_b3p2_a2 = newB3p3.transform.Find("b3p3_b3p2_a2");

            Transform b3p3_b3p4_a1 = newB3p3.transform.Find("b3p3_b3p4_a1");
            Transform b3p3_b3p4_a2 = newB3p3.transform.Find("b3p3_b3p4_a2");
            Transform b3p3_b3p4_a3 = newB3p3.transform.Find("b3p3_b3p4_a3");
            Transform b3p3_b3p4_a4 = newB3p3.transform.Find("b3p3_b3p4_a4");


            FuseAttributes fuseAtts = b3p3Fuses();

            //b3p3_bb3_a1
            b3p3_bb3_a1.gameObject.AddComponent<FuseBehavior>();
            b3p3_bb3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_bb3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_bb3_a1.gameObject.AddComponent<FaceSelector>();
            b3p3_bb3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b3p3_bb3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_bb3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p3_b3p1_a1
            b3p3_b3p1_a1.gameObject.AddComponent<FuseBehavior>();
            b3p3_b3p1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_b3p1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_b3p1_a1.gameObject.AddComponent<FaceSelector>();
            b3p3_b3p1_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b3p3_b3p1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_b3p1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p3_b3p1_a2
            b3p3_b3p1_a2.gameObject.AddComponent<FuseBehavior>();
            b3p3_b3p1_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_b3p1_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_b3p1_a2.gameObject.AddComponent<FaceSelector>();
            b3p3_b3p1_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b3p3_b3p1_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_b3p1_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p3_b3p2_a1
            b3p3_b3p2_a1.gameObject.AddComponent<FuseBehavior>();
            b3p3_b3p2_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_b3p2_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_b3p2_a1.gameObject.AddComponent<FaceSelector>();
            b3p3_b3p2_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b3p3_b3p2_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_b3p2_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p3_b3p2_a2
            b3p3_b3p2_a2.gameObject.AddComponent<FuseBehavior>();
            b3p3_b3p2_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_b3p2_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_b3p2_a2.gameObject.AddComponent<FaceSelector>();
            b3p3_b3p2_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b3p3_b3p2_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_b3p2_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p3_b3p4_a1
            b3p3_b3p4_a1.gameObject.AddComponent<FuseBehavior>();
            b3p3_b3p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_b3p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_b3p4_a1.gameObject.AddComponent<FaceSelector>();
            b3p3_b3p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b3p3_b3p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_b3p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p3_b3p4_a2
            b3p3_b3p4_a2.gameObject.AddComponent<FuseBehavior>();
            b3p3_b3p4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_b3p4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_b3p4_a2.gameObject.AddComponent<FaceSelector>();
            b3p3_b3p4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b3p3_b3p4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_b3p4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p3_b3p4_a3
            b3p3_b3p4_a3.gameObject.AddComponent<FuseBehavior>();
            b3p3_b3p4_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_b3p4_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_b3p4_a3.gameObject.AddComponent<FaceSelector>();
            b3p3_b3p4_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b3p3_b3p4_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_b3p4_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p3_b3p4_a4
            b3p3_b3p4_a4.gameObject.AddComponent<FuseBehavior>();
            b3p3_b3p4_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p3_b3p4_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p3"));

            b3p3_b3p4_a4.gameObject.AddComponent<FaceSelector>();
            b3p3_b3p4_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b3p3_b3p4_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p3_b3p4_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[2] = newB3p3;
            partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("b3p3_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b3p3");
            }
        }
    }

    public void createB3p4()
    {
        if (!partCreated[3])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(90, 0, 90);
            GameObject newB3p4 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[3], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB3p4)); // this creates the zooming up from the ground effect

            Transform b3p4_b3p5_a1 = newB3p4.transform.Find("b3p4_b3p5_a1");
            Transform b3p4_b3p5_a2 = newB3p4.transform.Find("b3p4_b3p5_a2");
            Transform b3p4_b3p5_a3 = newB3p4.transform.Find("b3p4_b3p5_a3");

            Transform b3p4_b3p3_a1 = newB3p4.transform.Find("b3p4_b3p3_a1");
            Transform b3p4_b3p3_a2 = newB3p4.transform.Find("b3p4_b3p3_a2");
            Transform b3p4_b3p3_a3 = newB3p4.transform.Find("b3p4_b3p3_a3");
            Transform b3p4_b3p3_a4 = newB3p4.transform.Find("b3p4_b3p3_a4");


            FuseAttributes fuseAtts = b3p4Fuses();

            //b3p4_b3p5_a1
            b3p4_b3p5_a1.gameObject.AddComponent<FuseBehavior>();
            b3p4_b3p5_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p4_b3p5_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p4"));

            b3p4_b3p5_a1.gameObject.AddComponent<FaceSelector>();
            b3p4_b3p5_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b3p4_b3p5_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p4_b3p5_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p4_b3p5_a2
            b3p4_b3p5_a2.gameObject.AddComponent<FuseBehavior>();
            b3p4_b3p5_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p4_b3p5_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p4"));

            b3p4_b3p5_a2.gameObject.AddComponent<FaceSelector>();
            b3p4_b3p5_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.up;
            b3p4_b3p5_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p4_b3p5_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p4_b3p5_a3
            b3p4_b3p5_a3.gameObject.AddComponent<FuseBehavior>();
            b3p4_b3p5_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p4_b3p5_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p4"));

            b3p4_b3p5_a3.gameObject.AddComponent<FaceSelector>();
            b3p4_b3p5_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.back;
            b3p4_b3p5_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p4_b3p5_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p4_b3p3_a1
            b3p4_b3p3_a1.gameObject.AddComponent<FuseBehavior>();
            b3p4_b3p3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p4_b3p3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p4"));

            b3p4_b3p3_a1.gameObject.AddComponent<FaceSelector>();
            b3p4_b3p3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b3p4_b3p3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p4_b3p3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p4_b3p3_a2
            b3p4_b3p3_a2.gameObject.AddComponent<FuseBehavior>();
            b3p4_b3p3_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p4_b3p3_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p4"));

            b3p4_b3p3_a2.gameObject.AddComponent<FaceSelector>();
            b3p4_b3p3_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.left;
            b3p4_b3p3_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p4_b3p3_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p4_b3p3_a3
            b3p4_b3p3_a3.gameObject.AddComponent<FuseBehavior>();
            b3p4_b3p3_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p4_b3p3_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p4"));

            b3p4_b3p3_a3.gameObject.AddComponent<FaceSelector>();
            b3p4_b3p3_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b3p4_b3p3_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p4_b3p3_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p4_b3p3_a4
            b3p4_b3p3_a4.gameObject.AddComponent<FuseBehavior>();
            b3p4_b3p3_a4.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p4_b3p3_a4.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p4"));

            b3p4_b3p3_a4.gameObject.AddComponent<FaceSelector>();
            b3p4_b3p3_a4.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b3p4_b3p3_a4.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p4_b3p3_a4.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[3] = newB3p4;
            partCreated[3] = true;
            partButtons[3].interactable = false;

            selectionManager.newPartCreated("b3p4_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b3p4");
            }
        }
    }

    public void createB3p5()
    {
        if (!partCreated[4])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 90, 180);
            GameObject newB3p5 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[4], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB3p5)); // this creates the zooming up from the ground effect

            Transform b3p5_b3p4_a1 = newB3p5.transform.Find("b3p5_b3p4_a1");
            Transform b3p5_b3p4_a2 = newB3p5.transform.Find("b3p5_b3p4_a2");
            Transform b3p5_b3p4_a3 = newB3p5.transform.Find("b3p5_b3p4_a3");

            FuseAttributes fuseAtts = b3p5Fuses();

            //b3p5_b3p4_a1
            b3p5_b3p4_a1.gameObject.AddComponent<FuseBehavior>();
            b3p5_b3p4_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p5_b3p4_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p5"));

            b3p5_b3p4_a1.gameObject.AddComponent<FaceSelector>();
            b3p5_b3p4_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.down;
            b3p5_b3p4_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p5_b3p4_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p5_b3p4_a2
            b3p5_b3p4_a2.gameObject.AddComponent<FuseBehavior>();
            b3p5_b3p4_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p5_b3p4_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p5"));

            b3p5_b3p4_a2.gameObject.AddComponent<FaceSelector>();
            b3p5_b3p4_a2.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            b3p5_b3p4_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p5_b3p4_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            //b3p5_b3p4_a3
            b3p5_b3p4_a3.gameObject.AddComponent<FuseBehavior>();
            b3p5_b3p4_a3.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b3p5_b3p4_a3.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3p5"));

            b3p5_b3p4_a3.gameObject.AddComponent<FaceSelector>();
            b3p5_b3p4_a3.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.forward;
            b3p5_b3p4_a3.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b3p5_b3p4_a3.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[4] = newB3p5;
            partCreated[4] = true;
            partButtons[4].interactable = false;

            selectionManager.newPartCreated("b3p5_harderPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("b3p5");
            }
        }
    }

    public void createBb3_harder_gray()
    {
        if (!partCreated[5])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(180, 90, 0);
            GameObject newBb3_harder_gray = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[5], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newBb3_harder_gray)); // this creates the zooming up from the ground effect

            Transform bb3_gray_bb3_a1 = newBb3_harder_gray.transform.Find("bb3_gray_bb3_a1");

            FuseAttributes fuseAtts = bb3_harder_grayFuses();

            //bb3_gray_bb3_a1
            bb3_gray_bb3_a1.gameObject.AddComponent<FuseBehavior>();
            bb3_gray_bb3_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            bb3_gray_bb3_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B3gray"));

            bb3_gray_bb3_a1.gameObject.AddComponent<FaceSelector>();
            bb3_gray_bb3_a1.gameObject.GetComponent<FaceSelector>().selectedNormal = Vector3.right;
            bb3_gray_bb3_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            bb3_gray_bb3_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[5] = newBb3_harder_gray;
            partCreated[5] = true;
            partButtons[5].interactable = false;

            selectionManager.newPartCreated("bb3_harder_grayPrefab(Clone)");
            if (dataManager != null)
            {
                dataManager.AddPartSelected("bb3_harder_gray");
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

