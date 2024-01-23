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
    public bool canSpin = true;

    public Box touchingBox;

    //public float collisionDuration = 0.2f;
    //public float currCollisionDuration;
    //public bool timer;

    public GameObject greenBar;
    public GameObject deathBar;
    public bool hasShield;

    void Start()
    {
        currSpeed = oriSpeed;
        rotSpeed = currSpeed;

        Setup();
    }

    void Update()
    {
        if (!GameController.instance.playing)
            return;

        Spin();

        /*
        if(timer)
        {
            currCollisionDuration -= 1f;//Time.deltaTime; //0.165
            //Debug.Log(currCollisionDuration);

            if (currCollisionDuration <= 0)
            {
                Debug.Log($"Time out");
                z = 0;
                touchingBox = null;

                timer = false;
                Debug.Log($"Stop timer");
                currCollisionDuration = collisionDuration;
            }
        }
        */

        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentEulerAngles = new Vector3(0, 0, 0);

            int left = Random.Range(0, 2);

            if (left == 0)
                z = 1;
            else
                z = -1;

            canSpin = true;
        }
        */

        if(Input.GetMouseButtonDown(0))
        {
            if (touchingBox != null)
            {
                z = -z;

                GameController.instance.AddTap();
                touchingBox.DestroyBox(false);
                touchingBox = null;
            }
            else
            {
                if (hasShield)
                {
                    CameraShake.instance.StartShake();
                    hasShield = false;
                }
                else
                {
                    if(!GameController.instance.testing)
                    {
                        z = 0;
                        GameController.instance.GameOver();
                    }
                }
            }
        }
    }

    public void Setup()
    {
        //currentEulerAngles = new Vector3(0, 0, 0);
        //transform.localEulerAngles = currentEulerAngles;

        int left = Random.Range(0, 2);

        if (left == 0)
            z = 1;
        else
            z = -1;
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

            /*
            Debug.Log("================================");
            Debug.Log($"Toco la box {other.GetComponent<Box>().spawn.name}");
            Debug.Log("================================");
            */
        }
        /*
        if (other.tag == "Box")
        {
            timer = false;
            //Debug.Log("Stop timer");

            currCollisionDuration = collisionDuration;
            touchingBox = other.GetComponent<Box>();

            Debug.Log("================================");
            Debug.Log($"Toco la box {other.GetComponent<Box>().spawn.name}");
            Debug.Log($"currCollisionDuration es: {currCollisionDuration}");
            Debug.Log("================================");

            timer = true;
            Debug.Log("Empiezo timer");
        }
        */
    }

    void OnTriggerExit2D(Collider2D other)
    {
        touchingBox = null;

        //canSpin = false;
        //z = 0;

        /*
        Debug.Log("================================");
        Debug.Log($"Dejo de tocar la box {other.GetComponent<Box>().spawn.name}");
        Debug.Log("================================");
        */

        /*
        if (other.GetComponent<Box>().spawn.name == "Spawn17")
        {
            z = 0;
            timer = false;
        }
        */
    }
}