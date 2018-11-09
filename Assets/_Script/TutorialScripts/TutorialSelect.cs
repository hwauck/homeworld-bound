using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Defines selection behaviors of black surfaces in tutorial mode.
/// </summary>
public class TutorialSelect : MonoBehaviour, IPointerClickHandler
{
    public bool isSelected = false;
    private float delay = 0.15f;
    private float timer = 0f;
    public Vector3 normal;
    GameObject instance;

    void Start()
    {
        // Set selected effect normal's default value
        if (normal.Equals(new Vector3(0, 0, 0)))
        {
            normal = new Vector3(1f, 0, 0);
        }
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (!isSelected)
        {
            TutorialManager.step++;
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
}
