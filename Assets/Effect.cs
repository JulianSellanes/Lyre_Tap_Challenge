using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public string effectName;
    public Image effectImg;
    public Image bgImg;
    public bool timer;
    public float oriDuration;
    public float duration;

    public void Setup(EffectInfo _effectInfo)
    {
        effectName = _effectInfo.effectName;
        effectImg.sprite = _effectInfo.effectIcon;
        bgImg.sprite = _effectInfo.effectIcon;
        oriDuration = _effectInfo.effectDuration;
        duration = _effectInfo.effectDuration;
        timer = true;

        //StartCoroutine(Timer());
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
                GameController.instance.RemoveEffect(effectName);
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator Timer()
    {
        while(duration > 0)
        {
            yield return new WaitForSeconds(1f);
            duration--;

            //update ui
            effectImg.fillAmount = duration / oriDuration;

            if (duration <= 0)
            {
                //restart stats
                GameController.instance.RemoveEffect(effectName);

                //destroy ui icon
                Destroy(this.gameObject);
            }
        }
    }
}