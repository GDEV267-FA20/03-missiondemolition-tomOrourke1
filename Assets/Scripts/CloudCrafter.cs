using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{

    [Header("Set in Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

    private void Awake()
    {
        //make an array large enough to hold all the oculd_ instances
        cloudInstances = new GameObject[numClouds];
        //find the couldanchorParent
        GameObject anchor = GameObject.Find("CloudAnchor");
        //iterate though anf make coud_s
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            //make an instance of cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);

            //position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            //scale cloud
            float scaleU = Random.value;

            float scalVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            //smaller clouds wil smaller scale U should be nearer the ground

            cPos.z = 100 - 90 * scaleU;
            //apply these transforms to the cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scalVal;

            //make cloud a child of anchor
            cloud.transform.SetParent(anchor.transform);

            //add the cloud to cloudINstances
            cloudInstances[i] = cloud;

        }
    }


    private void Update()
    {
        
        // Iterate over each cloud that was created

        foreach(GameObject cloud in cloudInstances)
        {
            // get the cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            //Move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            // if cloud has moved too far to the left

            if (cPos.x <= cloudPosMin.x)
            {
                //move it to the far right
                cPos.x = cloudPosMax.x;
            }
            // apply to new position to cloud
            cloud.transform.position = cPos;

        }

    }


}
