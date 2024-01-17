using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController_Game : MonoBehaviour
{
    public TextMeshProUGUI scoreTxt;

    public GameObject effectPrefab;
    public Transform effectsContent;

    public GameObject revivePanel;
    public GameObject revivePanel2;
    public Image adCancelImg;
    public float oriDuration = 6;
    public float duration;

    public static UIController_Game instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        AudioController.instance.PlayMusic("GameTheme");
        duration = oriDuration;
    }

    private void Update()
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

    public void InstEffect(EffectInfo _effectInfo)
    {
        Effect effect = Instantiate(effectPrefab, effectsContent).GetComponent<Effect>();
        GameController.instance.activeEffects.Add(_effectInfo.box);
        effect.Setup(_effectInfo);
    }
}