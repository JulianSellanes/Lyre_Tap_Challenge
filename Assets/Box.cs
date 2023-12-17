using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
    Box,
    Slow,
    Bar,
}

public class Box : MonoBehaviour
{
    public Spawn spawn;

    public BoxType boxType = BoxType.Box;

    public void Setup(BoxType _boxType)
    {
        boxType = _boxType;

        switch (boxType)
        {
            case BoxType.Slow:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case BoxType.Bar:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            default:
                break;
        }
    }

    public void DestroyBox()
    {
        GameController.instance.boxesInScene.Remove(boxType);

        if (boxType == BoxType.Box)
        {
            GameController.instance.GenerateBox(boxType);
        }
        else
        {
            GameController.instance.ApplyEffect(boxType);
        }

        Destroy(this.gameObject);
        spawn.haveBox = false;
        
        GameController.instance.AddScore();
    }
}