using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public string name;

    public Image circleImg;
    public Image effectBarImg;
    public Image effectIconImg;

    public BoxType boxType;
    
    public float oriDuration;
    public float duration;
    public bool timer;

    public void Setup(EffectInfo _effectInfo)
    {
        effectIconImg.sprite = _effectInfo.icon;
        //bgImg.sprite = _effectInfo.icon;

        oriDuration = _effectInfo.duration;
        duration = _effectInfo.duration;

        boxType = _effectInfo.box;
        
        switch (boxType)
        {
            case BoxType.Slow:
                effectBarImg.color = new Color32(28, 68, 255, 255);
                circleImg.color = new Color32(38, 38, 154, 255);
                break;
            case BoxType.Bar:
                effectBarImg.color = Color.green;
                circleImg.color = new Color32(33, 120, 33, 255);
                break;
            case BoxType.DoubleScore:
                effectBarImg.color = new Color32(250, 172, 17, 255);
                circleImg.color = new Color32(128, 97, 35, 255);
                break;
            case BoxType.Grow:
                effectBarImg.color = Color.yellow;
                circleImg.color = new Color32(144, 144, 42, 255);
                break;
            case BoxType.Shield:
                effectBarImg.color = Color.cyan;
                circleImg.color = new Color32(47, 128, 128, 255);
                break;
            default:
                break;
        }

        timer = true;
    }

    private void Update()
    {
        if (!GameController.instance.playing)
            return;

        if(timer && duration != -1)
        {
            duration -= Time.deltaTime;
            effectBarImg.fillAmount = duration / oriDuration;

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