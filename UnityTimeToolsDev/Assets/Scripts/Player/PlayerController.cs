using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : NavigationController
{
    void Start()
    {
        Waypoint wp0 = new Waypoint(transform.position, transform.rotation, 0f);
        waypoints.Add(wp0);
    }


    /**
     * Adds a new point at current time, should work (even if point is at the beginning or end of path?)
     */
    public void AddPointAtCurrentPos(float time)
    {
        // we're currently here on the path
        float pathTravelled = IndexAtTime(time);
        int index = (int)pathTravelled;

        // remove all waypoints on the timeline after our current location
        for (int i = waypoints.Count - 1; i > index; --i)
        {
            waypoints.RemoveAt(i);
        }

        // create new waypoint at our current location
        float currentDist = waypoints[waypoints.Count - 2].distFromStart + (transform.position - waypoints[waypoints.Count - 2].point).magnitude;
        Waypoint newPoint = new Waypoint(transform.position, waypoints[waypoints.Count - 2].rotation, currentDist);

        waypoints.Add(newPoint);

        // recalculate rotation of the point before last (this is probably not needed, should be moving in the same direction anyway)
        waypoints[waypoints.Count - 2].rotation = Quaternion.FromToRotation(Vector3.left, (waypoints[waypoints.Count - 1].point - waypoints[waypoints.Count - 2].point));
    }


    public void ExtendPath()
    {
        
    }
}
