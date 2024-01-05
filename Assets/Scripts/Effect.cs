using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public string name;

    public Image effectImg;
    public Image bgImg;

    public BoxType boxType;
    
    public float oriDuration;
    public float duration;
    public bool timer;

    public void Setup(EffectInfo _effectInfo)
    {
        effectImg.sprite = _effectInfo.icon;
        bgImg.sprite = _effectInfo.icon;

        oriDuration = _effectInfo.duration;
        duration = _effectInfo.duration;

        boxType = _effectInfo.box;
        
        switch (boxType)
        {
            case BoxType.Slow:
                effectImg.color = new Color32(28, 68, 255, 255);
                bgImg.color = new Color32(38, 38, 154, 255);
                break;
            case BoxType.Bar:
                effectImg.color = Color.green;
                bgImg.color = new Color32(33, 120, 33, 255);
                break;
            case BoxType.DoubleScore:
                effectImg.color = new Color32(250, 172, 17, 255);
                bgImg.color = new Color32(128, 97, 35, 255);
                break;
            case BoxType.Grow:
                effectImg.color = Color.yellow;
                bgImg.color = new Color32(144, 144, 42, 255);
                break;
            case BoxType.Shield:
                effectImg.color = Color.cyan;
                bgImg.color = new Color32(47, 128, 128, 255);
                break;
            default:
                break;
        }

        timer = true;
    }

    void Update()
    {
        if (GameController.instance.gameOver)
            return;

        if(timer && duration != -1)
        {
            duration -= Time.deltaTime;
            effectImg.fillAmount = duration / oriDuration;

            if (duration <= 0)
            {
                timer = false;
                GameController.instance.RemoveEffect(boxType);
                GameController.instance.activeEffects.Remove(boxType);
                Destroy(this.gameObject);
            }
        }

        if (timer && duration == -1 && !GameController.instance.arrow.hasShield)
        {
            timer = false;
            GameController.instance.RemoveEffect(boxType);
            GameController.instance.activeEffects.Remove(boxType);
            Destroy(this.gameObject);
        }
    }
}