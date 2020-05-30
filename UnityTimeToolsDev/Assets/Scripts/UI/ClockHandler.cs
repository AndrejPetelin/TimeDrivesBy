using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClockHandler : MonoBehaviour
{

   // public GameObject targetHandPrefab;

    public GameObject targetHand;
    public GameObject displayObject;

    public TimeManager timeManager;

    [SerializeField] float _time;
    public float time { get { return _time; } private set { } }

    Vector2 pivot;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
       /* targetHand = Instantiate(targetHandPrefab);
        targetHand.transform.position = transform.position;
        targetHand.transform.rotation = transform.rotation;
        
        targetHand.transform.SetParent(transform.parent);*/
        displayObject.SetActive(false);

        cam = Camera.main;

        pivot = cam.WorldToScreenPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTargetHand(bool show)
    {
        displayObject.SetActive(show);
      //  Debug.Log("OVER CLOCK");
    }

    public void Test(BaseEventData data)
    {
        Debug.Log("OVER CLOCK");
        Debug.Log(data.currentInputModule.input.mousePosition );
    }



    public void DragHand()
    {
        Vector2 direction = (Vector2)Input.mousePosition - pivot;

        float angle = Mathf.Acos(direction.y / direction.magnitude) * Mathf.Rad2Deg;

        if (Mathf.Sign(direction.x) < 0)
        {
            angle = 360f - angle;
        }

        _time = angle / 6f;
        targetHand.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    

    public void SetTargetTime()
    {
        timeManager.gameTime = time;
    }
}
