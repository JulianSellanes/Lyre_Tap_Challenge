using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager_Game : MonoBehaviour
{
    public static UIManager_Game instance;

    public TextMeshProUGUI scoreTxt;

    public GameObject effectPrefab;
    public Transform effectsContent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public void InstEffect(EffectInfo _effectInfo)
    {
        Effect effect = Instantiate(effectPrefab, effectsContent).GetComponent<Effect>();
        GameController.instance.activeEffects.Add(_effectInfo.box);
        effect.Setup(_effectInfo);
    }
}