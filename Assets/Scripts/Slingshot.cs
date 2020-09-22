using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    //fields set dynamically
    [Header("set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");

        launchPoint = launchPointTrans.gameObject;

        launchPoint.SetActive(false);

        launchPos = launchPointTrans.position;
    }


    private void OnMouseEnter()
    {
        //print("slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }


    private void OnMouseDown()
    {
        //the player has pressed the mouse button while over the slingshot
        aimingMode = true;

        //instantiate a projectile

        projectile = Instantiate(prefabProjectile) as GameObject;

        //Start it at the launchPoint

        projectile.transform.position = launchPos;

        //set it to isKinematic for now

        projectile.GetComponent<Rigidbody>().isKinematic = true;

        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
    private void Update()
    {
       // if slingshot is not in aiming mode dont run this code
       if (!aimingMode) return;
        
       //get the current mouse position in 2d screen coordinate

       Vector3 mousePos2D = Input.mousePosition;

       mousePos2D.z = -Camera.main.transform.position.z;
       Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);



       //find the delta from the launchPos to the mousePos3D

       Vector3 mouseDelta = mousePos3D - launchPos;
            
       //linit mouse delta to the radius of the slingshot
            
       float maxMagnitude = this.GetComponent<SphereCollider>().radius;
       if(mouseDelta.magnitude > maxMagnitude)
       {
            mouseDelta.Normalize();

            mouseDelta *= maxMagnitude;

       }
       //move the projectile to this new position
       Vector3 projPos = launchPos + mouseDelta;
       projectile.transform.position = projPos;

       if (Input.GetMouseButtonUp(0))
       {
           // the mouse has been released 
          
           aimingMode = false;

           projectileRigidbody.isKinematic = false;

           projectileRigidbody.velocity = -mouseDelta * velocityMult;

           projectile = null;
       }

        
    }

}
