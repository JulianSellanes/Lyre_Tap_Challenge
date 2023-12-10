using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager_Game : MonoBehaviour
{
    public static UIManager_Game instance;

    public TextMeshProUGUI scoreTxt;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }
}