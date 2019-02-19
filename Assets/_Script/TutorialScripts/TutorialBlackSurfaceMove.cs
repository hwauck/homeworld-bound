using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Similar to TutorialSelect.
/// However, this class moves another part and makes tutorial go to next step after the movement is finished.
/// </summary>
public class TutorialBlackSurfaceMove : MonoBehaviour, IPointerClickHandler
{

    public bool isSelected = false;
    private bool isMoving = false;
    private bool hasMoved = false;
    private float delay = 0.15f;
    private float timer = 0f;
    public Vector3 normal;
    GameObject instance;
    // The part to be moved
    public GameObject partToMove1;
    public GameObject partToMove2;
    // The old position of that part
    private Vector3 origPos1;
    private Vector3 origPos2;
    public Vector3 destination;
    private Vector3 distSurfanceObj;

    public AudioSource audioSource;
    private AudioClip audioClip;

    void Start()
    {
        // Set selected effect normal's default value
        if (normal.Equals(new Vector3(0, 0, 0)))
        {
            normal = new Vector3(1f, 0, 0);
        }
        //destination = partToMove1.transform.TransformPoint(destination);

        audioClip = Resources.Load<AudioClip>("Audio/ConstModeMusic/SelectSurface");
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (!isSelected && !hasMoved)
        {
            audioSource.PlayOneShot(audioClip);
            origPos1 = partToMove1.transform.position;
            origPos2 = partToMove2.transform.position;
            distSurfanceObj = origPos1 - origPos2;
            //StartCoroutine(Move(destination));
            TutorialManager.step++;
            TutorialManager.triggerStep = true;
            isSelected = true;
        }
    }

    void Update()
    {
        if (isSelected)
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                timer = 0f;
                spawnGhost(normal);
            }
        }
    }

    private void spawnGhost(Vector3 normal)
    {
        // Transforms.
        // will adding the name parameter help unity distinguish fuseTo ghosts from AC ghosts? We'll see!
        instance = new GameObject(this.name + "_ghost");

        instance.transform.position = transform.position;
        instance.transform.localScale = /*10 */ transform.parent.localScale.x * transform.localScale;
        instance.transform.rotation = transform.rotation;
        LoadUtils.InstantiateParenter(instance);
        instance.layer = 2;

        // Add mesh filter and renderer
        MeshFilter meshf = instance.AddComponent<MeshFilter>();
        meshf.mesh = GetComponent<MeshFilter>().mesh;
        MeshRenderer meshr = instance.AddComponent<MeshRenderer>();
        meshr.material = Resources.Load("Opacity") as Material;

        // Add ghost script.
        SelectedGhost ghost = instance.AddComponent<SelectedGhost>();
        ghost.setNormal(normal);
    }

    public IEnumerator Move(Vector3 destination)
    {
        Vector3 movePath = destination - origPos1;
        Debug.Log(movePath);
        if (!isMoving)
        {
            isMoving = true;
            // How many frames the movement animation takes
            int moveFrame = 30;
            for (int i = 0; i < moveFrame; i++)
            {
                partToMove1.transform.position += movePath / ((float)moveFrame);
                partToMove2.transform.position = partToMove1.transform.position - distSurfanceObj;
                Debug.Log(partToMove1.transform.position);
                yield return null;
            }
            hasMoved = true;
            TutorialManager.step++;
        }
    }

}
