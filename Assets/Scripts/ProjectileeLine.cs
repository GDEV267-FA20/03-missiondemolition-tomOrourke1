using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileeLine : MonoBehaviour
{

    static public ProjectileeLine S;

    [Header("Set in Nspector")]
    public float minDist = 0.1f;


    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this; // Set the singleton
        //get a reference to the linerenderer
        line = GetComponent<LineRenderer>();
        //Disable the lineRenderer unitl it's needed
        line.enabled = false;
        //Initialize the points list
        points = new List<Vector3>();

    }

    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                // When _poi is set to something new, it resets everyhitng
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }


    private void AddPoint()
    {
        //this is called to add a point to the line
        Vector3 pt = _poi.transform.position;

        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            // if the point isn't far enouch from the last point
            return;
        }
        if (points.Count == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // Enables line renderer
            line.enabled = true;
        }
        else
        {
            // Normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //returns the location of the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                // If there are no points, reurn Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            // if there is no poi, search for one
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;
                }


            }
            else
            {
                return;
            }
        }

        // if there is a poi, its loc is added ever fixed update

        AddPoint();
        if (FollowCam.POI == null)
        {
            poi = null;
        }


    }

}
