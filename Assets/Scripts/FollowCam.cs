using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    static public GameObject POI; //the static point of interest


    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // the desired z pos of the camera

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        if (POI == null) return;

        //get tehe position of poi
        Vector3 destination = POI.transform.position;

        //limit the Z & Y to minimum valuies
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //interpolate from the current camera position toward desitinaion
        destination = Vector3.Lerp(transform.position, destination, easing);

        // force destination.z to be camZ to keep the camera for enough away

        destination.z = camZ;

        // set the camera to the destination

        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;

    }
}
