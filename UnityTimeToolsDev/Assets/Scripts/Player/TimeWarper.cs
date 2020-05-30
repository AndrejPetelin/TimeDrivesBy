using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarper : MonoBehaviour
{

    public float speedFactor = 1;

    public void DestroyOnClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }
}
