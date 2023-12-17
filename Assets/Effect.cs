using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public string name;
    public Image effectImg;
    public Image bgImg;
    public bool timer;
    public float oriDuration;
    public float duration;
    public BoxType box;

    public void Setup(EffectInfo _effectInfo)
    {
        effectImg.sprite = _effectInfo.icon;
        bgImg.sprite = _effectInfo.icon;
        oriDuration = _effectInfo.duration;
        duration = _effectInfo.duration;
        box = _effectInfo.box;
        timer = true;
    }

    void Update()
    {
        if(timer)
        {
            duration -= Time.deltaTime;
            effectImg.fillAmount = duration / oriDuration;

            if (duration <= 0)
            {
                timer = false;
                GameController.instance.RemoveEffect(box);
                GameController.instance.activeEffects.Remove(box);
                Destroy(this.gameObject);
            }
        }
    }
}