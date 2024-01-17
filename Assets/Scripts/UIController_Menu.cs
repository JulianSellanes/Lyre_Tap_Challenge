using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController_Menu : MonoBehaviour
{
    public TextMeshProUGUI highScoreTxt;

    public static UIController_Menu instance;

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
        AudioController.instance.PlayMusic("MenuTheme");
        highScoreTxt.text = $"High Score: {PlayerPrefs.GetInt("HighScore")}";
    }

    public void Play()
    {
        AudioController.instance.PlaySFX("PressButton");
        SceneController.instance.ChangeScene("Game");
    }

    public void Test()
    {
        PlayerPrefs.DeleteKey("Lifes");
    }
}