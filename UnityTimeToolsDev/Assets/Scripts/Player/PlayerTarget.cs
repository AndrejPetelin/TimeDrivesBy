using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public float offset;
    float currentY;
    float nextY;
    float from;
    float dest;
    float timer;
    public float effectTime;
    Material mat;
    public Transform playerTransform;
    public float distanceOffset = 0.5f;
    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public float Distance(Vector3 point)
    {
        return Vector3.Distance(transform.position, point);
    }

    private void Start()
    {
        //currentY = transform.position.y;
        
        nextY = transform.position.y + offset;
        mat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        
        if (Mathf.Approximately(currentY, transform.position.y))
        {
           
            from = currentY;
            dest = nextY;
            timer = 0;
        }
        else if (Mathf.Approximately(nextY, transform.position.y))
        {
            from = nextY;
            dest = currentY;
            timer = 0;
        }
        timer += Time.deltaTime ;

        float newY = Mathf.Lerp(from, dest, timer * effectTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

       // Debug.Log("DISTANCE: " + (Distance(playerTransform.position) - offset));
        mat.SetFloat("_TransparencyMult", Mathf.Clamp(Distance(playerTransform.position) - distanceOffset, 0, 1));
    }
}
