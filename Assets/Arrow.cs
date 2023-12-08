using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float rotSpeed = 60f;
    public Vector3 currentEulerAngles;
    public float z;

    public Transform customPivot;

    void Update()
    {
        //transform.RotateAround(customPivot.position, Vector3.up, 20 * Time.deltaTime);
        currentEulerAngles += new Vector3(0, 0, z) * Time.deltaTime * rotSpeed;
        transform.localEulerAngles = currentEulerAngles;
    }
}