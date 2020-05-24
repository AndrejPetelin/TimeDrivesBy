using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public float Distance(Vector3 point)
    {
        return Vector3.Distance(transform.position, point);
    }
}
