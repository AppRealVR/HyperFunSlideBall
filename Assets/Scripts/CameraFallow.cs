using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{

    public GameObject PlayerObj;

    float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;
    public int yOffset = 5; 

    // Update is called once per frame
    void Update()
    {
        FallowPlayer();

    }

    void FallowPlayer()
    {
        Vector3 targetPosition = PlayerObj.transform.TransformPoint(new Vector3(0,yOffset, -10));
        targetPosition = new Vector3(0, targetPosition.y, -10); // on z axis should be targetPosition.z for 2D invaroment

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

	
}
