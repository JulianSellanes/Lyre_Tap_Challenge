using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float rotSpeed = 60f;
    public Vector3 currentEulerAngles;
    public float z;

    void Update()
    {
        Spin();

        if(Input.GetMouseButtonDown(0))
        {
            z = -z;
        }
    }

    void Spin()
    {
        currentEulerAngles += new Vector3(0, 0, z) * Time.deltaTime * rotSpeed;
        transform.localEulerAngles = currentEulerAngles;
    }
}