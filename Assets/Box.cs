using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
    Box,
    Slow,
    Bar,
    DoubleScore,
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
            case BoxType.DoubleScore:
                GetComponent<SpriteRenderer>().color = new Color32(250, 172, 17, 255);
                break;
            default:
                break;
        }
    }

    public void DestroyBox()
    {
        GameController.instance.AddScore();

        if (boxType == BoxType.Box)
        {
            GameController.instance.GenerateBox(boxType);
        }
        else
        {
            GameController.instance.ApplyEffect(boxType);
        }

        GameController.instance.boxesInScene.Remove(boxType);
        Destroy(this.gameObject);
        spawn.haveBox = false;
    }
}