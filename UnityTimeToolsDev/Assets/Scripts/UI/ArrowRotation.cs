using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowRotation : MonoBehaviour
{
    public Transform player;
    public Transform target;
    RectTransform rect;
    
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        rect =GetComponent<RectTransform>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
       /* Vector3 dir = transform.InverseTransformDirection(target.transform.position - player.transform.position);
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        rect.eulerAngles = new Vector3(0, 0, angle);*/

        var targetPosLocal = cam.transform.InverseTransformPoint(target.position);
        var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg ;
        rect.eulerAngles = new Vector3(0, 0, targetAngle);
    }
}
