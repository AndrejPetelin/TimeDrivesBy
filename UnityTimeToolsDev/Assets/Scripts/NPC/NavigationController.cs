using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
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
    protected List<Waypoint> waypoints = new List<Waypoint>();

    protected float fullDistance { get => waypoints[waypoints.Count - 1].distFromStart; }
    

    

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

        for (int i = 1; i < waypoints.Count; ++i)
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

        return waypoints[waypoints.Count - 1].point;
    }



    Quaternion RotationAtTime(float time)
    {
        // if we're not yet at start time don't start driving yet
        if (time < startTime) return waypoints[0].rotation;

        // distance at time
        float distAtT = (time - startTime) * speed;

        for (int i = 1; i < waypoints.Count; ++i)
        {
            if (waypoints[i].distFromStart > distAtT)
            {
                float distA = waypoints[i - 1].distFromStart;
                float distB = waypoints[i].distFromStart;

                float t = (distAtT - distA) / (distB - distA);
                Debug.Log("T: " + t);
                //  return waypoints[i - 1].rotation;
                return Quaternion.Slerp(waypoints[i - 1].rotation, waypoints[i].rotation, t);
            }
        }

        return waypoints[waypoints.Count - 1].rotation;
    }


    public float IndexAtTime(float time)
    {
        // if we're not yet at start time don't start driving yet
        if (time < startTime) return 0f;

        // distance at time
        float distAtT = (time - startTime) * speed;


        for (int i = 1; i < waypoints.Count; ++i)
        {
            if (waypoints[i].distFromStart > distAtT)
            {
                float distA = waypoints[i - 1].distFromStart;
                float distB = waypoints[i].distFromStart;

                float t = (distAtT - distA) / (distB - distA);
                return i + t;
            }
        }

        return waypoints.Count;
    }


    public void MoveTo(float t)
    {
        transform.position = PositionAtTime(t);
        transform.rotation = RotationAtTime(t);
    }


    void CalculateWaypoints()
    {
        float currDist = 0;
        waypoints = new List<Waypoint>(path.corners.Length);
        Waypoint wpt = new Waypoint();
        wpt.point = path.corners[0];
        wpt.distFromStart = 0;
        // set the rotation of the waypoint to be the initial rotation of the transform
        wpt.rotation = transform.rotation;
        waypoints.Add(wpt);

        for (int i = 1; i < path.corners.Length; ++i)
        {
            // total distance so far
            currDist += (path.corners[i] - path.corners[i - 1]).magnitude;

            Waypoint pt = new Waypoint();
            pt.point = path.corners[0];
            pt.point = path.corners[i];
            pt.distFromStart = currDist;

            waypoints.Add(pt);

            // TODO - check if this works correctly. Do we even need it? Could we just determine rotation on the fly
            Quaternion rot = Quaternion.FromToRotation(Vector3.left, (waypoints[i - 1].point - waypoints[i].point));

           // waypoints[i - 1].rotation = rot;
           // since the waypoint[0] already has its rotation set up, we set the current one here. 
            waypoints[i ].rotation = rot;


        }

      //  waypoints[waypoints.Count - 1].rotation = waypoints[waypoints.Count - 2].rotation;
    }



    [Serializable]
    public class Waypoint
    {
        public Waypoint() { }

        public Waypoint(Vector3 pt, Quaternion rot, float fromStart)
        {
            point = pt;
            rotation = rot;
            distFromStart = fromStart;
        }


        public Vector3 point;
        public Quaternion rotation;
        public float distFromStart;
    }
    
}
