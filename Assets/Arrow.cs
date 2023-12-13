using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float rotSpeed;
    public float currSpeed;
    public float oriSpeed = 150f;
    Vector3 currentEulerAngles;
    float z = -1;
    Box touchingBox;

    void Start()
    {
        currSpeed = oriSpeed;
        rotSpeed = currSpeed;

        int left = Random.Range(0, 2);

        if(left == 0)
        {
            z = 1;
        }
    }

    void Update()
    {
        Spin();

        if(Input.GetMouseButtonDown(0))
        {
            if (touchingBox != null)
            {
                z = -z;
                GameController.instance.AddTap();
                touchingBox.DestroyBox();
            }
            else
            {
                //z = 0;
                //GameController.instance.GameOver();
            }
        }
    }

    void Spin()
    {
        currentEulerAngles += new Vector3(0, 0, z) * Time.deltaTime * rotSpeed;
        transform.localEulerAngles = currentEulerAngles;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Box")
        {
            touchingBox = other.GetComponent<Box>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        touchingBox = null;
    }
}