using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TimeBomb : MonoBehaviour
{
	private bool isCarrying = false;
	Camera mCamera;
	public GameObject prefab;
	//public LayerMask layerMask;
	int areaMask;
	GameObject timeBomb;


	// Start is called before the first frame update
	void Start()
	{
		mCamera = Camera.main;
		areaMask = 1 << NavMesh.GetAreaFromName("Walkable");
	}


	void Update()
	{
		if (timeBomb != null && isCarrying)
		{
			ObjectFollowCursor(timeBomb);
		}
	}


	public void CreateTimeBomb()
	{
		Vector3 position = Input.mousePosition;
		timeBomb = Instantiate(prefab, position, Quaternion.identity);
		isCarrying = true;
	}

	
	void ObjectFollowCursor(GameObject timeBomb)
	{
		Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
		Vector3 point = ray.origin + ray.direction * 60;
        RaycastHit hit;
		// here we cast a ray and it will give us the point at which it hits a collider. 
		// we also use a LayerMask so that the gems don't fall on top of buildings (or end up hidden under them)
		// This last part can probably be improved
		/*if (Physics.Raycast(ray, out hit, Mathf.Infinity) &&(1 << hit.collider.gameObject.layer) == layerMask)
        {
            //Debug.Log("HIT LAYER: " + hit.collider.gameObject.layer);
            timeBomb.transform.position = hit.point;
        }*/

		// Find nearest point on NaveMash Walkable area
		if (Physics.Raycast(ray, out hit, Mathf.Infinity) && 
		   (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 0.1f, areaMask)))
		{
			{
				Debug.Log("Navmesh HIT position: " + navMeshHit.position);
				timeBomb.transform.position = navMeshHit.position;
			}
		}

		if (Input.GetMouseButtonDown(0)  )
		{
			isCarrying = false; 
            // we enable the gem's collider here so that it doesn't collide agains the ray 
            //timeBomb.GetComponent<Collider>().enabled = true;
		}
		else if (Input.GetMouseButtonDown(1))
		{
			Destroy(timeBomb);
		}
	}
}
