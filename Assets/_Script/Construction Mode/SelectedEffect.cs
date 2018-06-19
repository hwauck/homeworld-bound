using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectedEffect : MonoBehaviour
{

	float delay = 0.15f;
	float timer = 0f;
	public RaycastResult hitInfo;      // Set by SelectPart when this script is applied.
    public RaycastHit hitUpdate;
	GameObject instance;
	GameObject hitCaster;

	void Start()
	{

		// Also add a hitcaster object to keep our normal updated.
        // if you make this hitCaster a child of the object this SelectedEffect is attached to, raycasts often
        // won't work. Dunno why.
		hitCaster = new GameObject(this.gameObject.name + "_hitCaster");
        LoadUtils.InstantiateParenter(hitCaster);
        hitCaster.transform.localScale = transform.parent.localScale; //3f * transform.localScale;
        hitCaster.transform.position = transform.position;

		hitCaster.transform.rotation = transform.rotation;

        // sometimes changing the constant here makes weird Ghost display problems go away
        hitCaster.transform.position += (50f * hitInfo.worldNormal); 

    }

	void FixedUpdate()
	{

		timer += Time.deltaTime;
		if (timer > delay)
		{
			timer = 0f;
			SpawnGhost();
		}

		// Update the normal with the hitCaster.
		Physics.Raycast(hitCaster.transform.position, transform.position - hitCaster.transform.position, out hitUpdate);
        //Debug.Log("hitUpdate normal for " + gameObject + ": " + hitUpdate.normal);
        //Debug.Log("Object hit: " + hitUpdate.collider.gameObject);
        //Debug.Log("Ray position: " + hitCaster.transform.position + ", Ray direction and length: " + 10 * (transform.position - hitCaster.transform.position));
		

	}

	public void SpawnGhost()
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
        //Vector3[] normals = GetComponent<MeshFilter>().mesh.normals;
        //ghost.setNormal(normals[0]);
        //ghost.setNormal(hitUpdate.normal);
        ghost.setNormal(gameObject.GetComponent<FaceSelector>().selectedNormal);
	}

	void OnDestroy()
	{
		Destroy(instance);
	}
}
