using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject boxPrefab;
    public List<Spawn> spawners;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Generate();
        }
    }

    void Generate()
    {
        int random = Random.Range(0, spawners.Count - 1);

        if (spawners[random - 1] != null && spawners[random - 1].haveBox == true)
        {
            Generate();
            return;
        }

        if (spawners[random + 1] != null && spawners[random + 1].haveBox == true)
        {
            Generate();
            return;
        }

        if (spawners[random] != null && spawners[random].haveBox == true)
        {
            Generate();
            return;
        }

        Instantiate(boxPrefab, spawners[random].transform.position, Quaternion.identity);
        spawners[random].haveBox = true;
    }
}