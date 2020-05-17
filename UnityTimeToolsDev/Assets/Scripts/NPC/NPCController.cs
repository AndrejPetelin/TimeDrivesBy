using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [Tooltip("Speed in m/s")]
    public float speed = 1;

    public float rotTime = 0.5f;

    [Tooltip("Does not move before time exceeds this. Unit is seconds.")]
    public float startTime;

    [Tooltip("Endpoint of the path")]
    public Transform goal;

    Vector3 startPoint;
    NavMeshPath path;
    
    [SerializeField]
    Waypoint[] waypoints;

    float fullDistance { get => waypoints[waypoints.Length - 1].distFromStart; }
    

    

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        NavMeshHit hit;

        if (NavMesh.SamplePosition(transform.position, out hit, 5, NavMesh.AllAreas))
        {
            startPoint = hit.position;
            NavMesh.CalculatePath(startPoint, goal.position, NavMesh.AllAreas, path);
            CalculateWaypoints();
        }



    }


    Vector3 PositionAtTime(float time)
    {
        // if we're not yet at start time don't start driving yet
        if (time < startTime) return waypoints[0].point;

        // distance at time
        float distAtT = (time - startTime) * speed;

        for (int i = 1; i < waypoints.Length; ++i)
        {
            if (waypoints[i].distFromStart > distAtT)
            {
                float distA = waypoints[i - 1].distFromStart;
                float distB = waypoints[i].distFromStart;

                float t = (distAtT - distA) / (distB - distA);
                Debug.Log("T: " + t);
                return Vector3.Lerp(waypoints[i - 1].point, waypoints[i].point, t);
            }
        }

        return waypoints[waypoints.Length - 1].point;
    }





    public void MoveTo(float t)
    {
        transform.position = PositionAtTime(t);
    }


    void CalculateWaypoints()
    {
        float currDist = 0;
        waypoints = new Waypoint[path.corners.Length];
        waypoints[0].point = path.corners[0];
        waypoints[0].distFromStart = 0;

        for (int i = 1; i < path.corners.Length; ++i)
        {
            // total distance so far
            currDist += (path.corners[i] - path.corners[i - 1]).magnitude;

            waypoints[i].point = path.corners[i];
            waypoints[i].distFromStart = currDist;

            // TODO - check if this works correctly. Do we even need it? Could we just determine rotation on the fly?
            waypoints[i - 1].direction = Quaternion.FromToRotation(Vector3.forward, (waypoints[i - 1].point - waypoints[i].point));
        }
    }

    [Serializable]
    public struct Waypoint
    {
        public Vector3 point;
        public Quaternion direction;
        public float distFromStart;
    }
    
}
