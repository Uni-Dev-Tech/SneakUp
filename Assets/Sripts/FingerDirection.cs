using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerDirection : MonoBehaviour
{
    public Camera currentCamera;
    public Image directionCircle;
    public Image directionCircleZone;

    private Vector3 motionDirection;

    [HideInInspector]
    public bool useFingerDirection = true;

    static public FingerDirection instance;

    private void Start()
    {
        if(FingerDirection.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        FingerDirection.instance = this;
    }
    private void Update()
    {
        if(useFingerDirection)
        {
            if (Input.GetMouseButtonDown(0))
            {
                directionCircleZone.gameObject.SetActive(true);
                directionCircleZone.gameObject.transform.position = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 direction = -(directionCircleZone.gameObject.transform.position - Input.mousePosition);

                if (direction.x > 100)
                    direction -= new Vector3(direction.x - 100, 0, 0);
                if (direction.x < -100)
                    direction -= new Vector3(direction.x + 100, 0, 0);

                if (direction.y > 100)
                    direction -= new Vector3(0, direction.y - 100, 0);
                if (direction.y < -100)
                    direction -= new Vector3(0, direction.y + 100, 0);

                directionCircle.transform.localPosition = direction;
            }

            if (Input.GetMouseButtonUp(0))
            {
                directionCircleZone.gameObject.SetActive(false);
                directionCircle.transform.localPosition = Vector3.zero;
            }
        }
    }
}
