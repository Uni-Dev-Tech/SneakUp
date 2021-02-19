using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private Vector3 positionDifference;

    [HideInInspector]
    public bool cameraFollowActive = true;

    [HideInInspector]
    public bool cameraLookAtPlayer = false;

    static public CameraFollow instance;
    private void Start()
    {
        if (CameraFollow.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        CameraFollow.instance = this;

        positionDifference = target.position - transform.position;
    }

    void Update()
    {
        if(cameraFollowActive)
            transform.position = target.position - positionDifference;

        if (cameraLookAtPlayer)
            transform.LookAt(target);
    }
}
