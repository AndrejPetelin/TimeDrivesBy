using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClockHandler : MonoBehaviour
{

   // public GameObject targetHandPrefab;

    public GameObject targetHand;
    public GameObject targetHandDisplayObject;
    public GameObject hand;
    public PlayerManager playerManager;

    public PlayerController playerController;

    public TimeManager timeManager;

    [SerializeField] float _timeTarget;
    public float timeTarget { get { return _timeTarget; } private set { } }

    Vector2 pivot;
    Camera cam;
    float previousAngle;

    // Start is called before the first frame update
    void Start()
    {
       /* targetHand = Instantiate(targetHandPrefab);
        targetHand.transform.position = transform.position;
        targetHand.transform.rotation = transform.rotation;
        
        targetHand.transform.SetParent(transform.parent);*/
        targetHandDisplayObject.SetActive(false);

        cam = Camera.main;

        pivot = cam.WorldToScreenPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeManager.targetTimeReached)
        {
            hand.transform.localRotation = Quaternion.Euler(0, 0, timeManager.gameTime * 6);
           // timeManager.timeRate = 1;
        }
        else if (timeManager.targetTimeReached && timeManager.gameTime >= playerController.fullPathTime)
        {
            targetHand.transform.localRotation = Quaternion.Euler(0, 0, playerController.fullPathTime * 6);
            timeManager.gameTime = playerController.fullPathTime;
           // timeManager.timeRate = 1;

        }
       // else timeManager.timeRate = 2;
    }

    public void ShowTargetHand(bool show)
    {
        targetHandDisplayObject.SetActive(show);
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
        Debug.Log("PREV: " + previousAngle + " CURR: " + angle);
        if (previousAngle < 20 && angle > 320) return;
        if (previousAngle > 340 && angle < 20) return;
        _timeTarget = angle / 6f;
        Debug.Log("ANGLE: " + angle);
        targetHand.transform.localRotation = Quaternion.Euler(0, 0, angle);
        previousAngle = angle;
    }
    

    public void SetTargetTime()
    {
        timeManager.gameTime = timeTarget;
    }

    public void Overlapping()
    {
        Debug.Log("OVERLAPPING");
        playerManager.changingClock = true;
    }

    public void NotOverlapping()
    {
        Debug.Log("NOT OVERLAPPING");
        playerManager.changingClock = false;
    }
}
