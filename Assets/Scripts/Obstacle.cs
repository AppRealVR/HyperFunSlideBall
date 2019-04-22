using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public int RotationSpeed; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RotationSpeed != 0)
        { 
            Rotation(); 
        }
    }
    void Rotation()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * RotationSpeed);
    }

}
