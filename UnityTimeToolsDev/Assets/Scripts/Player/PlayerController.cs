using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : NavigationController
{

    public PostProcessHandler postProc;
    void Start()
    {
        Waypoint wp0 = new Waypoint(transform.position, transform.rotation, 0f);
        waypoints.Add(wp0);

     //   ExtendPath();
    }


    /**
     * Adds a new waypoint to the existing path at the current time, discarding everything after 
     * our new location.
     * this should work even if point is at the beginning or end of path (I think!)
     */
    public void AddWaypointAtCurrentPos(float time)
    {
        // we're currently here on the path
        float pathTravelled = IndexAtTime(time);
        Debug.Log("TIME: " + time + " PT: " + pathTravelled);
        int index = (int)pathTravelled;
        Debug.Log("INDEX: " + index);
        // remove all waypoints on the timeline after our current location
        for (int i = waypoints.Count - 1; i > index; --i)
        {
            waypoints.RemoveAt(i);
        }

        // create new waypoint at our current location
        float currentDist = waypoints[waypoints.Count - 1].distFromStart + (transform.position - waypoints[waypoints.Count - 1].point).magnitude;
        //  Waypoint newPoint = new Waypoint(transform.position, waypoints[waypoints.Count - 1].rotation, currentDist);
        Waypoint newPoint = new Waypoint(transform.position, transform.rotation, currentDist);

        // commenting this out fixes the issue of weird points at time 0 in the middle of the thing (causes some teleporting)
      //  waypoints.Add(newPoint);
        // InsertWaypoint(time);
        // recalculate rotation of the point before last (this is probably not needed, should be moving in the same direction anyway)
        // waypoints[waypoints.Count - 2].rotation = Quaternion.FromToRotation(Vector3.left, (waypoints[waypoints.Count - 1].point - waypoints[waypoints.Count - 2].point));
    }



    /**
     * Adds points to the current target location from the last point on, preserving previous points
     */
    public void ExtendPath()
    {
        NavMeshPath addedPath = new NavMeshPath();
        NavMeshHit hit;

        if (NavMesh.SamplePosition(target.position, out hit, 5, NavMesh.AllAreas))
        {
            Vector3 targetPosition = hit.position;
            NavMesh.CalculatePath(waypoints[waypoints.Count - 1].point, targetPosition, NavMesh.AllAreas, addedPath);
            CalculateWaypoints(addedPath, waypoints.Count);
        }
    }

    public void InsertWaypoint(float time)
    {
        // find the waypoint index before where we're currently at
        int i = 0;
        while (i < waypoints.Count && waypoints[i].distFromStart / speed < time)
        {
            ++i;
        }

        if (i >= waypoints.Count && waypoints[i - 1].point != transform.position)
        {
            waypoints.Insert(i, new Waypoint(transform.position, transform.rotation, time * speed));
        }
        else if (i < waypoints.Count && waypoints[i].point != transform.position)
        {
            waypoints.Insert(i, new Waypoint(transform.position, transform.rotation, time * speed));
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
      //  Debug.Log("CAR COLLIDES WITH: " + collision.gameObject.name + " TAG: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("COLL WITH ENEMY");
            postProc.FlipToDying();
        }
    }
}
