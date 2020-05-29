using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    [SerializeField]
    Transform target;

    TimeManager timeManager;

    Camera cam;

    public bool placingTimeBomb;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PLAYER MAN");
        timeManager = GetComponent<TimeManager>();
        cam = Camera.main;

        target.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // handling input here
        if (Input.GetMouseButtonDown(0) )
        {
            Debug.Log("PTB: " + placingTimeBomb);
            if (! placingTimeBomb)
            {
                Vector3 worldPos;

                if (MouseToWorld(out worldPos))
                {
                    Vector3 newpos = new Vector3(worldPos.x, 0, worldPos.z);
                    target.position = newpos;
                   
                    player.AddWaypointAtCurrentPos(timeManager.gameTime);
                    player.ExtendPath();
                    Debug.Log("HERE");
                }
            }
        
        }


        player.MoveTo(timeManager.gameTime);

    }


    bool MouseToWorld(out Vector3 worldCoord)
    {
        Vector3 mouseCoords = Input.mousePosition;
        mouseCoords.z = 1f;
        Ray ray = cam.ScreenPointToRay(mouseCoords);
        //  print(mouseCoords);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 5f, NavMesh.AllAreas))
            {
                worldCoord = navHit.position;
                return true;
            }
            else
            {
                worldCoord = Vector3.zero;
                return false;
            }

        }
        else
        {
            worldCoord = Vector3.zero;
            return false;
        }
    }


}
