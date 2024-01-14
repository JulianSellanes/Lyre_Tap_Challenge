using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
    Box,
    Slow,
    Bar,
    DoubleScore,
    Grow,
    Burst,
    Shield,
}

public class Box : MonoBehaviour
{
    public Spawn spawn;

    public BoxType boxType = BoxType.Box;

    public void Setup(Spawn _spawn, BoxType _boxType)
    {
        spawn = _spawn;
        spawn.haveBox = true;

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
            case BoxType.Grow:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case BoxType.Burst:
                GetComponent<SpriteRenderer>().color = Color.magenta;
                break;
            case BoxType.Shield:
                GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            default:
                break;
        }
    }

    public void DestroyBox(bool _burst)
    {
        GameController.instance.AddScore();

        if (boxType == BoxType.Box && !_burst)
            GameController.instance.GenerateBox(boxType);

        GameController.instance.boxesInScene.Remove(boxType);
        Destroy(this.gameObject);
        spawn.haveBox = false;

        if (boxType != BoxType.Box)
        {
            AudioController.instance.PlaySFX("GetItem");
            GameController.instance.ApplyEffect(boxType);
        }
    }
}