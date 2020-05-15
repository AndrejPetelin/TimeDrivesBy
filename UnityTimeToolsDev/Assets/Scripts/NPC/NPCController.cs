using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public float speed;

    public Transform target;

    Vector3 startPoint;
    Vector3 endPoint;
    NavMeshPath path;
    
    [SerializeField]
    Waypoint[] waypoints;

    float fullDistance { get => waypoints[waypoints.Length - 1].distFromStart; }
    

    

    // Start is called before the first frame update
    void Start()
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(endPoint, out hit, 5, NavMesh.AllAreas))
        {
            endPoint = hit.position;
        }

        if (NavMesh.SamplePosition(transform.position, out hit, 5, NavMesh.AllAreas))
        {
            startPoint = hit.position;
            NavMesh.CalculatePath(startPoint, target.position, NavMesh.AllAreas, path);
            CalculateWaypoints();
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }


    Vector3 PositionAtTime(float time)
    {
        float distAtT = time * speed;

        /*
        // if we've exceded the full distance travel time
        if (distAtT > fullDistance)
        {
            return waypoints[waypoints.Length - 1].point;
        }
        */

        for (int i = 1; i < waypoints.Length; ++i)
        {
            if (waypoints[i].distFromStart > distAtT)
            {
                float distA = waypoints[i - 1].distFromStart;
                float distB = waypoints[i].distFromStart;

                float t = distAtT - distA / distB - distA;

                return Vector3.Lerp(waypoints[i - 1].point, waypoints[i].point, t);
            }
        }

        return waypoints[waypoints.Length - 1].point;
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
        }
    }

    [Serializable]
    public struct Waypoint
    {
        public Vector3 point;
        public float distFromStart;
    }
    
}
