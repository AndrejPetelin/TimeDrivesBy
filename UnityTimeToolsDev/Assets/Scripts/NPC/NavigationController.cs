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
    float baseSpeed = 1;

    public float rotTime = 0.5f;
    ExplosionEffect particles;

    [Tooltip("Does not move before time exceeds this. Unit is seconds.")]
    public float startTime;

    [Tooltip("Endpoint of the path")]
    public Transform target;

    Vector3 startPoint;
    
    [SerializeField]
    protected List<Waypoint> waypoints = new List<Waypoint>();

    protected float fullDistance { get => waypoints[waypoints.Count - 1].distFromStart; }

    public float fullPathTime { get => fullDistance / speed; }

    public float turnDistance = 1f;


    public List<TimeEffect> timeModifiers = new List<TimeEffect>();

    float currentGameTime;

    int slowBombCounter;
    int fastBombCounter;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshPath path = new NavMeshPath();
        NavMeshHit hit;

        if (NavMesh.SamplePosition(transform.position, out hit, 5, NavMesh.AllAreas))
        {
            startPoint = hit.position;
            NavMesh.CalculatePath(startPoint, target.position, NavMesh.AllAreas, path);
            for (int i = 0; i < path.corners.Length; ++i)
            {
                path.corners[i].y = 0f;
            }
            CalculateWaypoints(path);
        }
        particles = GetComponent<ExplosionEffect>();

      /*  TimeEffect te = new TimeEffect();
        te.t1 = 2;
        te.t2 = 5f;
        te.timeModifier = -.9f;
        timeModifiers.Add(te);*/
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
              //  Debug.Log("T: " + t);
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

                float t = 5* (distAtT - distA) / (distB - distA);
                //   float t = 0.5f* (distB - distAtT) / (distB - distA);
              //  float t =  (distAtT - distA) / Mathf.Min( (distB - distA), turnDistance);
                //   Debug.Log("T: " + t);
                //  return waypoints[i - 1].rotation;
                //   Debug.Log("ROT AT T: " + Quaternion.Slerp(waypoints[i - 1].rotation, waypoints[i].rotation, t).eulerAngles);
                //  return Quaternion.Slerp(waypoints[i - 1].rotation, waypoints[i].rotation, t);
                return Quaternion.Slerp(transform.rotation, waypoints[i].rotation, t);
            }
        }
      //  Debug.Log("SHOULD BE HERE: " + waypoints[waypoints.Count - 1].rotation.eulerAngles);
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
            // i think this should be < instead of > ? It works this way. The idea is to get our current index
            // So it should only consider the waypoints behind us. 
            if (waypoints[i].distFromStart < distAtT)
            {
                float distA = waypoints[i - 1].distFromStart;
                float distB = waypoints[i].distFromStart;
               // Debug.Log("DIST A : " + distA + " DISTB : " + distB + "DISTATT: " + distAtT);
               // a bit of a hack, i know, there was sometimes a division by zero, which happens if
               // both waypoints are at the same place. 
                if (Mathf.Approximately((distB - distA), 0)) continue;
                float t = (distAtT - distA) / (distB - distA);
                return i + t;
            }
        }

        //return waypoints.Count;
        // changed the return, considering the case the loop never starts, which is when the count is zero. 
        // Probably the issue i had before was because of the if statement with the wrong > anyway. Too brain fried now!!
        return 0;
    }
    float prevT;
    float prevTModT;
    public float minSpeed = 0.001f;

    public void MoveTo(float t)
    {
       // Debug.Log("NAME: " + transform.gameObject.name);
        currentGameTime = t;
        float tMod = 0;
        //  float modifierMult = 1;

        
      //  Debug.Log("NAME: " + transform.gameObject.name + " TIME MODIFIERS LENGTH: " + timeModifiers.Count);
        foreach (var effect in timeModifiers)
        {
            if (t > effect.t2)
            {
                tMod += (effect.t2 - effect.t1) * effect.timeModifier;
              //  Debug.Log("ID: " + effect.timeEffectID + " T2: " + effect.t2 + " T1: " + effect.t1 + " TMOD: " + tMod );
            }
            else if (t > effect.t1)
            {
                tMod += (t - effect.t1)   * effect.timeModifier ;
               //  Debug.Log("ID: " + effect.timeEffectID + " T1: " + effect.t1 + " TMOD: " + tMod);
              //  modifierMult *= Mathf.Abs(effect.timeModifier);
            }

        }
       
        /*
        transform.position = PositionAtTime(t);
        transform.rotation = RotationAtTime(t);
        */
       // Debug.Log(" T: " + t + " PREVT " + prevT);

       // Debug.Log("PREV: " + prevTModT + " CURR: " + (t + tMod));
        /* t + tMod should give a value that's equal or larger to the one in the previous frame when moving forward
         * equal or smaller to the previous frame when moving backwards. When we stack up two or more bubbles, the values are reversed
         * and the enemies jump backwards. The idea is to, for starters, pass a value that will be equal to the previous one
         * if necessary, to avoid flipping backwards. 
         * prevT : the time at the previous frame
         * prevTModT: the previous t + tMod
         */
        bool forward = t > prevT;


        /*   float ret = forward ? Mathf.Max(prevTModT + minSpeed, t + tMod )  : Mathf.Min(prevTModT - minSpeed, t + tMod ) ;
           if (Mathf.Approximately(t - prevT, 0)) ret = t + tMod;*/
      //  Debug.Log("OBJECT:  " + transform.gameObject.name +  "T+TMOD: " + (t + tMod));
         transform.position = PositionAtTime(t + tMod);
         transform.rotation = RotationAtTime(t + tMod);
     /*       transform.position = PositionAtTime(ret);
        transform.rotation = RotationAtTime(ret);
        Debug.Log("RET: " + ret);
        prevTModT = ret;
        prevT = t;*/
        // Debug.Log("WHO? : " + transform.gameObject.name + "ROT: " + transform.rotation.eulerAngles + " AT TIME: " + t);
    }


 


    protected void CalculateWaypoints(NavMeshPath path, int wptsOffset = 0)
    {
        float currDist = 0;
        
        Waypoint wpt = new Waypoint();
        wpt.point = path.corners[0];
        wpt.distFromStart = currDist;

        // set the rotation of the waypoint to be the initial rotation of the transform
        wpt.rotation = transform.rotation;


        // if there's a waypoint offset there are points already in the waypoints list, grab the distance from the last one
        // otherwise create a new list that we'll then fill
        if (wptsOffset > 0)
        {
            currDist = waypoints[wptsOffset - 1].distFromStart;
            // fix rotation if we have one from the old list
            wpt.rotation = waypoints[wptsOffset - 1].rotation;
           // Debug.Log("WPT ROT: " + wpt.rotation.eulerAngles);
        }
        else
        {
            waypoints = new List<Waypoint>(path.corners.Length);
            waypoints.Add(wpt);
        }



       
        for (int i = 1; i < path.corners.Length; ++i)
        {
            // total distance so far
            currDist += (path.corners[i] - path.corners[i - 1]).magnitude;

            Waypoint pt = new Waypoint();
            pt.point = path.corners[0];
            pt.point = path.corners[i];
          //  Debug.Log("POINT: " + pt.point);
            pt.distFromStart = currDist;

            waypoints.Add(pt);

            // TODO - check if this works correctly. Do we even need it? Could we just determine rotation on the fly
            int index = wptsOffset > 0 ?    i + wptsOffset-1 : i;
            Vector3 pointA = waypoints[index - 1].point;
            Vector3 pointB = waypoints[index].point;
            pointA.y = 0f;
            pointB.y = 0f;
           
            Quaternion rot = Quaternion.FromToRotation(transform.forward, (pointB - pointA).normalized);
            
            // waypoints[i - 1].rotation = rot;
            // since the waypoint[0] already has its rotation set up, we set the current one here. 
            waypoints[index].rotation = rot * transform.rotation;
           


        }
       // Debug.Log("LENGTH: " + waypoints.Count);

      //  waypoints[waypoints.Count - 1].rotation = waypoints[waypoints.Count - 2].rotation;
       // Debug.Log("AT CALC END: " + waypoints.Count);
    }

    /* when we enter collision with a timebomb we check the type (slow / fast). The idea is to prevent accumulation of the effects
     * So we use the counter. When we enter, if it's not 0 we don't create a new modifier and we always increase the counter
     * to keep track of how many bombs we've entered
     */
    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("COLLISION BETWEEN: " + transform.gameObject.name + " AND: " + collision.gameObject.name);
        TimeWarper warper = collision.gameObject.GetComponent<TimeWarper>();
        if (warper != null)
        {
           
            if (warper.speedFactor > 0)
            {
                if (fastBombCounter == 0)
                {
                    CreateNewTimeModifier(warper);
                }
                else
                {
                    AddIDToModifiers(warper);
                }
                ++fastBombCounter;
            }
            else if (warper.speedFactor < 0)
            {
                if (slowBombCounter == 0)
                {
                    CreateNewTimeModifier(warper);
                }
                else
                {
                    AddIDToModifiers(warper);
                }
                ++slowBombCounter;
            }
            
        
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("ENEMY");
            particles.PlayParticlesAt(transform.position);
        }
    }

    void CreateNewTimeModifier(TimeWarper warper)
    {
        TimeEffect ef = new TimeEffect();
        ef.t1 = currentGameTime;
        ef.t2 = Mathf.Infinity;
        ef.timeModifier = warper.speedFactor;
        // ef.timeEffectID = warper.gameObject.GetInstanceID();
        ef.timeEffectIDs = new List<int>();
        ef.timeEffectIDs.Add(warper.gameObject.GetInstanceID());
        timeModifiers.Add(ef);
        
    }

    void AddIDToModifiers(TimeWarper warper)
    {
        for (int i = timeModifiers.Count - 1; i >= 0; --i)
        {
            if (Mathf.Sign(timeModifiers[i].timeModifier) == Mathf.Sign(warper.speedFactor))
            {
                timeModifiers[i].timeEffectIDs.Add(warper.gameObject.GetInstanceID());
                break;
            }
        }
    }

    /* when we exit collision, we also check the type of the bomb. If the count isn't 0, that means we're 
     * within a "nested" bomb, so we lower the counter and continue. When we hit 0, we look for the last bomb in the list of the same type
     * This one must be the one we had entered when the sequence of bombs began, so we set t2 and exit the loop
     */
    private void OnCollisionExit(Collision collision)
    {
       // Debug.Log("COLLISION EXIT BETWEEN: " + transform.gameObject.name + " AND: " + collision.gameObject.name);
        TimeWarper warper = collision.gameObject.GetComponent<TimeWarper>();
        if (warper != null)
        {
           
            if (warper.speedFactor > 0)
            {
                --fastBombCounter;
                if (fastBombCounter == 0)
                {
                    for (int i = timeModifiers.Count - 1; i >=0; --i)
                    {
                        if ( Mathf.Sign(timeModifiers[i].timeModifier) == Mathf.Sign(warper.speedFactor))
                        {
                            //timeModifiers[i].t2 = currentGameTime;
                            if (RemoveTimeModifier(timeModifiers[i]))
                                break;
                            else
                            {
                                timeModifiers[i].t2 = currentGameTime;
                                break;
                            }
                        }
                    }
                }
            }
            else if (warper.speedFactor < 0)
            {
                --slowBombCounter;
                if (slowBombCounter == 0)
                {
                    for (int i = timeModifiers.Count - 1; i >= 0; --i)
                    {
                        if (Mathf.Sign(timeModifiers[i].timeModifier) == Mathf.Sign(warper.speedFactor))
                        {
                            if (RemoveTimeModifier(timeModifiers[i]))
                                break;
                            else
                            {
                                timeModifiers[i].t2 = currentGameTime;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    bool RemoveTimeModifier(TimeEffect modifier)
    {
        foreach (var mod in timeModifiers)
        {
            if (mod == modifier) continue;
            foreach(var id in mod.timeEffectIDs)
            {
                if (modifier.timeEffectIDs.Contains(id))
                {
                    timeModifiers.Remove(mod);
                    timeModifiers.Remove(modifier);
                    return true;
                }
            }
        }
        return false;
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


    public class TimeEffect
    {
       /* static int nextID;

        public TimeEffect()
        {
            timeEffectID = nextID++;
        }*/

        public float t1, t2;
        public float timeModifier;
        public List< int> timeEffectIDs;
    }
    
}
