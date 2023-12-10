using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Spawn spawn;

    public string effect = "box";

    public void Setup(string _effect)
    {
        effect = _effect;

        switch (_effect)
        {
            case "slow":
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            default:
                break;
        }
    }

    public void DestroyBox()
    {
        if(effect == "box")
        {
            GameController.instance.GenerateBox("box");
        }
        else
        {
            GameController.instance.ApplyEffect(effect);
        }

        Destroy(this.gameObject);
        spawn.haveBox = false;
        GameController.instance.AddScore();
    }
}