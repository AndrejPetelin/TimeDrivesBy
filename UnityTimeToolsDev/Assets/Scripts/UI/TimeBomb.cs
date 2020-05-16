﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBomb : MonoBehaviour
{
	private bool isCarrying = false;
	Camera mCamera;
	public GameObject prefab;
	public GameObject shadowPrefab;
    public LayerMask layerMask;
	GameObject timeBomb;
	GameObject shadow;


	// Start is called before the first frame update
	void Start()
	{
		mCamera = Camera.main;
	}


	void Update()
	{
		if (timeBomb != null && isCarrying)
		{
			ObjectFollowCursor(timeBomb, shadow);
		}

	}


	public void CreateTimeBomb()
	{
		Vector3 position = Input.mousePosition;
		timeBomb = Instantiate(prefab, position, Quaternion.identity);
		shadow = Instantiate(shadowPrefab, position, Quaternion.identity);
		//Debug.Log("Object is created");
		isCarrying = true;
	}

	
	void ObjectFollowCursor(GameObject timeBomb, GameObject shadow)
	{
		Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
       
		Vector3 point = ray.origin + ray.direction * 60;
		//Debug.Log("Point " + point);
		//timeBomb.transform.position = point;
		shadow.transform.position = new Vector3(point.x, 0f, point.z);
        RaycastHit hit;

        // here we cast a ray and it will give us the point at which it hits a collider. 
        // we also use a LayerMask so that the gems don't fall on top of buildings (or end up hidden under them)
        // This last part can probably be improved
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) &&(1 << hit.collider.gameObject.layer) == layerMask)
        {
            //Debug.Log("HIT LAYER: " + hit.collider.gameObject.layer);
            timeBomb.transform.position = hit.point;
        }
            
            


        if (Input.GetMouseButtonDown(0)  )
		{
			isCarrying = false;
           
            // we enable the gem's collider here so that it doesn't collide agains the ray 
            timeBomb.GetComponent<Collider>().enabled = true;
			Destroy(shadow);
		}
		else if (Input.GetMouseButtonDown(1))
		{
			Destroy(timeBomb);
			Destroy(shadow);
		}

	}
}
