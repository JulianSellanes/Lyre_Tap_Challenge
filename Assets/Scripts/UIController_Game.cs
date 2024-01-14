using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController_Game : MonoBehaviour
{
    public TextMeshProUGUI scoreTxt;

    public GameObject effectPrefab;
    public Transform effectsContent;

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
    }

    public void InstEffect(EffectInfo _effectInfo)
    {
        Effect effect = Instantiate(effectPrefab, effectsContent).GetComponent<Effect>();
        GameController.instance.activeEffects.Add(_effectInfo.box);
        effect.Setup(_effectInfo);
    }
}