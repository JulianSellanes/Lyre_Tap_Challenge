using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController_Game : MonoBehaviour
{
    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI scoreTxt;

    public GameObject effectPrefab;
    public Transform effectsContent;

    public GameObject revivePanel;
    public GameObject revivePanel2;
    public GameObject bgPanel;
    public Image adCancelImg;
    public float oriDuration = 6;
    public float duration;

    public static UIController_Game instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //DontDestroyOnLoad(this);
    }

    void Start()
    {
        AudioController.instance.PlayMusic("GameTheme");
        duration = oriDuration;
    }

    void Update()
    {
        if(!revivePanel.activeInHierarchy)
            return;

        duration -= Time.deltaTime;
        adCancelImg.fillAmount = duration / oriDuration;

        if (duration <= 0)
        {
            GameController.instance.ReturnToMenu();
        }
    }

    public void UpdateTimer()
    {
        float _time = GameController.instance.timeLeft;
        _time += 1;

        float mins = Mathf.FloorToInt(_time / 60);
        float secs = Mathf.FloorToInt(_time % 60);

        timerTxt.text = string.Format("{0:00} : {1:00}", mins, secs);
    }

    public void InstEffect(EffectInfo _effectInfo)
    {
        Effect effect = Instantiate(effectPrefab, effectsContent).GetComponent<Effect>();
        GameController.instance.activeEffects.Add(_effectInfo.box);
        effect.Setup(_effectInfo);
    }

    public void ReduceAlphaToScoreText(bool _reduce)
    {
        Color32 newColor;

        if (GameController.instance.scoreMult == 1)
            newColor = Color.black;
        else
            newColor = GameController.instance.possibleEffects.Find(item => item.box == BoxType.DoubleScore).barColor;//new Color32(250, 172, 17, 255);

        if (_reduce)
            newColor.a = 215;
        else
            newColor.a = 255;

        scoreTxt.color = newColor;
    }

    public void ReduceAlphaToEffectsIcons(bool _reduce)
    {
        Color32 newColor1;
        Color32 newColor2;
        Color32 newColor3;

        foreach (Transform child in effectsContent)
        {
            newColor1 = child.GetComponent<Image>().color;
            newColor2 = child.GetChild(0).GetComponent<Image>().color;
            newColor3 = child.GetChild(1).GetComponent<Image>().color;

            if(_reduce)
            {
                newColor1.a = 100;
                newColor2.a = 100;
                newColor3.a = 100;
            }
            else
            {
                newColor1.a = 255;
                newColor2.a = 255;
                newColor3.a = 255;
            }

            child.GetComponent<Image>().color = newColor1;
            child.GetChild(0).GetComponent<Image>().color = newColor2;
            child.GetChild(1).GetComponent<Image>().color = newColor3;
        }
    }
}