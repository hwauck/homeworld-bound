using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FaceSelector : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    private RaycastResult globalHitInfo;
    private Vector3 activePartNormal;
    public SelectPart selectPart;
    private IsFused isFusedComponent;
    public Button fuseButton;
    private const float OFFSET = 15f;
    private Coroutine currentlyActiveCoroutine;
    public Vector3 selectedNormal;

    // Use this for initialization
    void Start () {
        isFusedComponent = this.transform.parent.gameObject.GetComponent<IsFused>();
        currentlyActiveCoroutine = null;

    }
	
	// Update is called once per frame
	void Update () {
	}

    public void setSelectPartScript(SelectPart selectPart)
    {
        this.selectPart = selectPart;
    }

    public void setFuseButton(Button fuseButton)
    {
        this.fuseButton = fuseButton;
    }

    // Do NOT call this method with a fused object. It's only for active/non-fused parts
    public void adjustPartAlignment()
    {
        GameObject currentlySelectedFuseTo = selectPart.getSelectedFuseTo();

        // Move it to the position of the fused object, offset by a multiple of the normal, 
        // offset again by the scaled local positional difference of the connection face and the parent object.

        //! DUE TO THE CRAZINESS: All parts with non-boxy attachment points will require box colliders roughly positioned at their center.'
        if (this.gameObject.GetComponent<BoxCollider>() == null)
        {
            BoxCollider boxy = this.gameObject.AddComponent<BoxCollider>();
            boxy.size = Vector3.zero;
        }
        if (currentlySelectedFuseTo.GetComponent<BoxCollider>() == null)
        {
            BoxCollider boxy = currentlySelectedFuseTo.AddComponent<BoxCollider>();
            boxy.size = Vector3.zero;
        }

        // The actual location of the selected fuse marker... Wow.
        Vector3 properFuseToPos = currentlySelectedFuseTo.transform.position
            + (Quaternion.Euler(currentlySelectedFuseTo.transform.eulerAngles)
            * (currentlySelectedFuseTo.transform.parent.localScale.x
            * (currentlySelectedFuseTo.GetComponent<BoxCollider>().center)));
        // The actual offset of the object face from the object parent... Also wow.
        Vector3 properOffset = Quaternion.Euler(this.gameObject.transform.parent.localEulerAngles)
            * (this.gameObject.transform.parent.localScale.x
            * (this.gameObject.transform.localPosition
            + Quaternion.Euler(this.gameObject.transform.localEulerAngles)
            * (this.gameObject.GetComponent<BoxCollider>().center)));

        //Debug.DrawLine(selectedObject.transform.parent.position, selectedObject.transform.parent.position + properOffset, Color.red, 25f, false);
        //Debug.DrawLine(selectedFuseTo.transform.parent.position, properFuseToPos, Color.red, 25f, false);

       // Vector3 targetPosition = properFuseToPos - properOffset + (OFFSET * selectPart.getFuseToNormal());
        Vector3 targetPosition = properFuseToPos + (OFFSET * selectPart.getFuseToNormal());
        //Debug.Log("properFuseToPos: " + properFuseToPos + ", OFFSET: " + OFFSET + ", fuseToNormal: " + selectPart.getFuseToNormal());
        //Debug.Log("targetPosition: " + targetPosition);

        //Set currently active coroutine variable so FuseEvent can check it and stop it if it needs to perform a fuse
        currentlyActiveCoroutine = StartCoroutine(SweepPosition(this.gameObject.transform.parent.gameObject, targetPosition, 20));
    }

    IEnumerator SweepPosition(GameObject toSweep, Vector3 targetPos, int frames)
    {
        // Interpolate.
        Vector3 initialPos = toSweep.transform.position;
        float iteration = 1 / (float)frames;
        for (float i = 0.0f; i < 1; i += iteration)
        {
            toSweep.transform.position = Vector3.Lerp(initialPos, targetPos, i);
            yield return null;
        }

        // Ensure it ends in the right place no matter what.
        yield return null;
        toSweep.transform.position = targetPos;

        // Now, get starting part's bounding box and the active part's bounding box so
        // we can check if they're intersecting and prevent that
        Collider activePartCollider;

        activePartCollider = selectPart.getSelectedObject().transform.parent.GetComponent<BoxCollider>();
        Debug.Log("Parent of FaceSelector obj: " + this.gameObject.transform.parent.gameObject);

        GameObject startingPart = selectPart.startingPart;
        GameObject currentlySelectedFuseTo = selectPart.getSelectedFuseTo();
        Vector3 fuseToNormal = currentlySelectedFuseTo.GetComponent<FaceSelector>().selectedNormal;
        Collider fusedCollider;

        // enable the bounding box - only enable it temporarily since doing so prevents FaceSelectors from detecting selection
        fusedCollider = startingPart.GetComponent<BoxCollider>();
        Debug.Log("ENABLING COLLIDERS");
        fusedCollider.enabled = true;
        activePartCollider.enabled = true;
        Debug.Log("ENABLED COLLIDERS!");

        // Keep moving in direction of FuseTo's normal until the active part no longer intersects with the construction
        while (activePartCollider.bounds.Intersects(fusedCollider.bounds))
        {
            toSweep.transform.Translate(iteration * 10 * fuseToNormal, Space.World);
            Debug.Log("INTERSECT - ADJUSTING POSITION - fuseToNormal: " + fuseToNormal);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("DISABLING COLLIDERS");
        fusedCollider.enabled = false;
        activePartCollider.enabled = false;
        Debug.Log("DISABLED COLLIDERS");

        currentlyActiveCoroutine = null;
    }

    public Coroutine getCurrentlyActiveCoroutine()
    {
        return currentlyActiveCoroutine;
    }

    public void OnPointerDown(PointerEventData data)
    {
    }

    public void OnPointerUp(PointerEventData data)
    {

    }

    // only executes if this gameobject was the first one hit by mouse's raycast
    // so it won't fire if UI element is clicked and object is behind it, yay
    public void OnPointerClick(PointerEventData data)
    {
        if (!selectPart.controlsDisabled)
        {
            //print("OnPointerClick on " + gameObject + "!");
            //	print ("Active part: " + activePart);
            bool fusedPart = isFusedComponent.isFused;
            globalHitInfo = data.pointerCurrentRaycast;
            GameObject lastSelectedFuseTo = selectPart.getSelectedFuseTo();
            GameObject lastSelectedObject = selectPart.getSelectedObject();

            if (fusedPart && lastSelectedFuseTo != this.gameObject)
            {
                selectPart.setFuseToNormal(globalHitInfo.worldNormal);
                //Debug.Log("Normal of globalFuseToHitInfo: " + globalFuseToHitInfo.worldNormal);

                selectPart.setSelectedFuseTo(this.gameObject);
                //print("Currently Selected FuseTo: " + selectPart.getSelectedFuseTo());
                //print("Previously selected FuseTo: " + selectPart.getPrevSelectedFuseTo());
                // unhighlight previously selected fused part
                if (selectPart.getPrevSelectedFuseTo() != null)
                {
                    //! CODE FOR REMOVING Marker FROM PREVIOUS PART. prevSelectedFuseTo
                    Destroy(selectPart.getPrevSelectedFuseTo().GetComponent<SelectedEffect>());

                }

                //! CODE FOR ADDING MARKER TO SELECTED PART. selectedFuseTo
                if (GetComponent<SelectedEffect>() == null)
                {
                    SelectedEffect sel = this.gameObject.AddComponent<SelectedEffect>();
                    sel.hitInfo = globalHitInfo;
                    //Debug.Log("Normals of hitInfo for " + this.gameObject + ": " + globalHitInfo.worldNormal);
                }

            }
            else if (!fusedPart && lastSelectedObject != this.gameObject)
            {
                //active part
                //Debug.Log("Normal of globalActivePartHitinfo: " + globalActivePartHitInfo.worldNormal);
                selectPart.setSelectedObject(this.gameObject);
                //print("Currently Selected Object: " + selectPart.getSelectedObject());

                // unhighlight previously selected active part
                if (selectPart.getPrevSelectedObject() != null)
                {
                    //! CODE FOR REMOVING MARKER FROM PREVIOUS PART. prevSelectedObject
                    Destroy(selectPart.getPrevSelectedObject().GetComponent<SelectedEffect>());

                }

                //highlight selected object
                //! CODE FOR ADDING MARKER TO SELECTED PART. selectedObject
                if (GetComponent<SelectedEffect>() == null)
                {
                    SelectedEffect sel = gameObject.AddComponent<SelectedEffect>();
                    sel.hitInfo = globalHitInfo;
                    //Debug.Log("Normals of hitInfo for " + this.gameObject + ": " + globalHitInfo.worldNormal);
                }

            }

            GameObject currentlySelectedObject = selectPart.getSelectedObject();
            GameObject currentlySelectedFuseTo = selectPart.getSelectedFuseTo();

            if (currentlySelectedObject != null && currentlySelectedFuseTo != null)
            {
                // both faces selected, player is now allowed to attempt fuses between the two objects
                fuseButton.interactable = true;

                //! PART MOVEMENT.
                // We ALWAYS need the hitinfo of selecting a part on the static object.
                // To do this, globalHitInfo is set whenever we click on a fused part.
                // TODO: what if it could be set every time we rotate? Would that eliminate SelectedEffect and
                // part alignment problems?

                // Move it to the position of the fused object, offset by a multiple of the normal, 
                // offset again by the scaled local positional difference of the connection face and the parent object.

                //! DUE TO THE CRAZINESS: All parts with non-boxy attachment points will require box colliders roughly 
                // positioned at their center.'
                if (currentlySelectedObject.GetComponent<BoxCollider>() == null)
                {
                    BoxCollider boxy = currentlySelectedObject.AddComponent<BoxCollider>();
                    boxy.size = Vector3.zero;
                }

                if (currentlySelectedFuseTo.GetComponent<BoxCollider>() == null)
                {
                    BoxCollider boxy = currentlySelectedFuseTo.AddComponent<BoxCollider>();
                    boxy.size = Vector3.zero;
                }

                // The actual location of the selected fuse marker... Wow.
                Vector3 properFuseToPos = currentlySelectedFuseTo.transform.position
                    + (Quaternion.Euler(currentlySelectedFuseTo.transform.eulerAngles)
                    * (currentlySelectedFuseTo.transform.parent.localScale.x
                    * (currentlySelectedFuseTo.GetComponent<BoxCollider>().center)));
                // The actual offset of the object face from the object parent... Also wow.
                Vector3 properOffset = Quaternion.Euler(this.gameObject.transform.parent.localEulerAngles)
                    * (currentlySelectedObject.transform.parent.localScale.x
                    * (currentlySelectedObject.transform.localPosition
                    + Quaternion.Euler(this.gameObject.transform.localEulerAngles)
                    * (currentlySelectedObject.GetComponent<BoxCollider>().center)));

                //Debug.DrawLine(selectedObject.transform.parent.position, selectedObject.transform.parent.position + properOffset, Color.red, 25f, false);
                //Debug.DrawLine(selectedFuseTo.transform.parent.position, properFuseToPos, Color.red, 25f, false);

                //Vector3 targetPosition = properFuseToPos + (OFFSET * selectPart.getFuseToNormal());
                Vector3 targetPosition = properFuseToPos - properOffset + (OFFSET * selectPart.getFuseToNormal());

                //Set currently active coroutine variable so FuseEvent can check it and stop it if it needs to perform a fuse
                currentlyActiveCoroutine = StartCoroutine(SweepPosition(currentlySelectedObject.transform.parent.gameObject, targetPosition, 20));

            }
        }

    }

}
