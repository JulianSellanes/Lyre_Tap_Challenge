using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public string scene = "Menu";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
        this.scene = _sceneName;
    }
}