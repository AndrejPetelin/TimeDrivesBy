using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBomb : MonoBehaviour
{
	private bool isCarrying = false;
	Camera mCamera;
	public GameObject prefab;
	public GameObject shadowPrefab;
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
		Debug.Log("Point " + point);
		timeBomb.transform.position = point;
		shadow.transform.position = new Vector3(point.x, 0f, point.z);
		if (Input.GetMouseButtonDown(0))
		{
			isCarrying = false;
			timeBomb.transform.position = new Vector3(point.x, 0f, point.z);
			Destroy(shadow);
		}
		else if (Input.GetMouseButtonDown(1))
		{
			Destroy(timeBomb);
			Destroy(shadow);
		}

	}
}
