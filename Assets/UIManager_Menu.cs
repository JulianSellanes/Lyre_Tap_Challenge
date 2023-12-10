using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;

public class UIManager_Menu : MonoBehaviour
{
    public static UIManager_Menu instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public void Play()
    {
        SceneController.instance.ChangeScene("Game");
    }
}