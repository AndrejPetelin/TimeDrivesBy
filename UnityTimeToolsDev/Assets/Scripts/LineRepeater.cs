/**
 * LineRepeater.cs -
 * LineRepeater is a script that's meant to be added to a GameObject (Cube?) in order to allow
 * for repeated objects generated in a straight line. Examples: fence, line of street lights,
 * dashed lines in the middle of the street,...
 *
 * Last changed by Andrej, 06.05.2019
 */


using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class LineRepeater : MonoBehaviour
{
    public GameObject[] elementPrefabs;
    public float elementXSize;
    public float elementOffset;
    public bool centerElements = false;
    public bool scaleElements = false;
    public bool rotateElements = false;

    GameObject[] elements = new GameObject[0];

    // old state of class to check for changes
    float oldEltXSize, oldEltOffset;
    bool oldCenterElts, oldScaleElts;


	// Use this for initialization
	void Start()
    {
        CreateObjects();
        InitChangeGuards();
	}
    

	void Update()
    {
		if (WasUpdated())
        {
            if (scaleElements) centerElements = false;
            CreateObjects();         
        }
	}


    void InitChangeGuards()
    {
        oldEltXSize = elementXSize;
        oldEltOffset = elementOffset;
        oldCenterElts = centerElements;
        oldScaleElts = scaleElements;
        transform.hasChanged = false;
    }


    bool WasUpdated()
    {
        bool ret = elementXSize != oldEltXSize || elementOffset != oldEltOffset || centerElements != oldCenterElts || scaleElements != oldScaleElts || transform.hasChanged;
        if (ret)
        {
            InitChangeGuards();
        }

        return ret;
    }


    /**
     * calculates two points in form of Vector3, the beginning and end points of the parent
     * object in global coordinates
     */
    Vector3[] FromToCoords()
    {
        Vector3[] ret = new Vector3[2];

        float fromX = -transform.localScale.x / 2 + elementXSize / 2;
        float toX = transform.localScale.x / 2 + elementXSize / 2;

        ret[0] = new Vector3(fromX, 0.0f, 0.0f);
        ret[1] = new Vector3(toX, 0.0f, 0.0f);

        ret[0] = transform.rotation * ret[0] + transform.position;
        ret[1] = transform.rotation * ret[1] + transform.position;
        return ret;
    }


    /**
     * calculates center coordinate points for all the elements that are to be generated
     * between p1 and p2. Returns an empty array if no points fit between p1 and p2.
     */
    Vector3[] CalculateElementsCoords(Vector3 p1, Vector3 p2)
    {
        Vector3 diff = p2 - p1;
        Vector3 delta = diff.normalized * (elementXSize + elementOffset);

        float distInEltLength = diff.magnitude / (elementXSize + elementOffset);
        int nrOfElts = (int)distInEltLength;

        // if no elements fit between the points return empty array
        if (nrOfElts < 1) return new Vector3[0];

        Vector3[] points = new Vector3[nrOfElts];

        Vector3 startPt;

        if (centerElements)
        {
            float padding = (diff.magnitude - (nrOfElts * (elementXSize + elementOffset)) + elementOffset) / 2.0f;

            startPt = new Vector3(p1.x, p1.y, p1.z) + diff.normalized * padding;
        }
        else
        {
            startPt = p1;
        }

        // Enter first point
        // TODO: not sure if we need to copy the first point yet, perhaps just passing p1 would be enough
        // check after multiple paths implemented
        points[0] = new Vector3(startPt.x, startPt.y, startPt.z);

        for (int i = 1; i < nrOfElts; ++i)
        {
            points[i] = points[i - 1] + delta;
        }

        return points;
    }


    /**
     * generates GameObjects and parents them to parent object. This function destroys and recreates all objects.
     * It also destroys all other objects, parented to this game object in order to prevent duplicating objects
     * when entering and exiting game mode, quitting and returning to project.
     */
    void CreateObjects()
    {
        //if (Application.isPlaying) return;
        // destroy all elements in array (needed so that objects don't persist?)
        /*
        foreach (var elt in elements)
        {
            DestroyImmediate(elt);
        }
        */

        // elements = new GameObject[0];

        // destroy all children of parent object
        List<GameObject> children = new List<GameObject>();
        foreach (Transform elt in transform)
        {
            children.Add(elt.gameObject);
        }

        children.ForEach(child => DestroyImmediate(child));
        Debug.Log("LENGTH: " + elementPrefabs.Length);
        // precondition - if element wasn't added yet just return without creating anything
        //  if (!element) return;
        if (elementPrefabs.Length == 0) return;
        Debug.Log("KAJ?");
        Vector3[] fromTo = FromToCoords();
        Vector3[] points = CalculateElementsCoords(fromTo[0], fromTo[1]);
   
        elements = new GameObject[points.Length];        

        // local x axis scaling factor increase for each element
        float dScaleX = 0.0f;

        // on scaleElements == true reposition points to accommodate for offset due to scaling
        // and calculate local x axis scaling factor increase of individual elements
        if (scaleElements && points.Length > 0)
        {
            // total length of parent object
            float len = (fromTo[1] - fromTo[0]).magnitude;  
            // difference between lengths of parent and sum of all elements' length + offset
            float lenDiff = len - points.Length * (elementXSize + elementOffset);
            float dLenDiff = lenDiff / points.Length;   // difference in length per each element
            Vector3 unit = (fromTo[1] - fromTo[0]).normalized;
            dScaleX =  dLenDiff / elementXSize;

            // adjust center point of each element
            for (int i = 0; i < points.Length; ++i)
            {
                points[i] += unit * dLenDiff * (2.0f * i + 1.0f) / 2.0f;
            }
        }

        // generate and parent objects, applying scaling (dScaleX == 0 if no scaling is needed/scaleElements set to false)
        for (int i = 0; i < points.Length; ++i)
        {
            Debug.Log("HERE");
            elements[i] = Instantiate(elementPrefabs[Random.Range(0, elementPrefabs.Length)], points[i], transform.rotation);
            elements[i].transform.localScale += new Vector3(dScaleX, 0.0f, 0.0f);
           // elements[i].transform.parent = transform;
         /*   if (rotateElements)
            {
                // elements[i].transform.localEulerAngles = new Vector3(transform.rotation.x, Random.Range(0, 360), transform.rotation.z);
                elements[i].transform.Rotate(0f, Random.Range(0, 360), 0f, Space.Self);
            }
            */
        //    Transform intermediary = new GameObject().transform;
        //    intermediary.parent = transform;
        //    intermediary.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
            
            elements[i].transform.parent = transform;
        }
    }
}
