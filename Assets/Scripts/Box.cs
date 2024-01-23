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
    public Color boxColor;

    public void Setup(Spawn _spawn, BoxType _boxType, EffectInfo _effectInfo)
    {
        spawn = _spawn;
        spawn.haveBox = true;

        boxType = _boxType;

        if (_effectInfo == null)
            boxColor = Color.black;
        else
            boxColor = _effectInfo.barColor;

        GetComponent<SpriteRenderer>().color = boxColor;

        /*
        switch (boxType)
        {
            case BoxType.Box:
                boxColor = Color.black;
                break;
            case BoxType.Slow:
                boxColor = Color.blue;
                break;
            case BoxType.Bar:
                boxColor = Color.green;
                break;
            case BoxType.DoubleScore:
                boxColor = new Color32(250, 172, 17, 255);
                break;
            case BoxType.Grow:
                boxColor = Color.yellow;
                break;
            case BoxType.Burst:
                boxColor = Color.magenta;
                break;
            case BoxType.Shield:
                boxColor = Color.cyan;
                break;
            default:
                break;
        }*/
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