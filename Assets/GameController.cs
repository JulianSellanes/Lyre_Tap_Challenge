using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject boxPrefab;
    public List<Spawn> spawners;
    int safe = 0;

    public Arrow arrow;

    public int score = 0;

    public int taps;
    public int tapsToEffect = 1;

    public List<string> possibleEffects;
    public List<string> currentEffects;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        if(arrow == null)
        {
            arrow = GameObject.Find("Arrow").GetComponent<Arrow>();
        }

        if(spawners.Count == 0)
        {
            Transform spawnersParent = GameObject.Find("Spawners").transform;

            if (spawnersParent == null)
                return;

            foreach (Transform child in spawnersParent)
            {
                spawners.Add(child.GetComponent<Spawn>());
            }
        }

        GenerateBox("box");
    }

    public void GenerateBox(string _effect)
    {
        if (safe >= 60)
            return;

        safe++;

        int random = Random.Range(0, spawners.Count - 1);

        if (spawners[random] != null && spawners[random].haveBox ||
            random > 0 && spawners[random - 1] != null && spawners[random - 1].haveBox ||
            random < spawners.Count - 1 && spawners[random + 1] != null && spawners[random + 1].haveBox)
        {
            GenerateBox(_effect);
            return;
        }

        Box box = Instantiate(boxPrefab, spawners[random].transform.position, Quaternion.identity).GetComponent<Box>();
        spawners[random].haveBox = true;
        box.spawn = spawners[random];
        box.Setup(_effect);

        safe = 0;
    }

    public void AddScore()
    {
        score++;
        UIManager_Game.instance.scoreTxt.text = score.ToString();
    }

    public void AddTap()
    {
        taps++;

        if (taps >= tapsToEffect)
        {
            GenerateEffect();

            taps = 0;

            int random = Random.Range(4, 11);
            tapsToEffect = random;
        }
    }

    void GenerateEffect()
    {
        int random = Random.Range(0, possibleEffects.Count - 1);
        string effect = possibleEffects[random];

        //currentEffects.Add(effect);

        switch (effect)
        {
            //case "twoBoxes":
            //    GenerateBox();
            //    break;
            case "slow":
                GenerateBox("slow");
                break;
            default:
                break;
        }
    }

    public void ApplyEffect(string _effect)
    {
        switch (_effect)
        {
            case "slow":
                arrow.rotSpeed = 100f;
                break;
            default:
                break;
        }

        StartCoroutine(RemoveEffect(_effect));
    }

    IEnumerator RemoveEffect(string _effect)
    {
        yield return new WaitForSeconds(5f);

        switch (_effect)
        {
            case "slow":
                arrow.rotSpeed = 150f;
                break;
            default:
                break;
        }
    }

    public void GameOver()
    {
        Camera.main.GetComponent<Camera>().backgroundColor = Color.red;
        arrow.GetComponent<SpriteRenderer>().color = Color.red;

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject box in boxes)
        {
            box.GetComponent<SpriteRenderer>().color = Color.red;
        }

        UIManager_Game.instance.scoreTxt.text = "Game Over";

        StartCoroutine(GameOverTimer(1f));
    }

    IEnumerator GameOverTimer(float _time)
    {
        yield return new WaitForSeconds(_time);

        SceneController.instance.ChangeScene("Menu");
    }
}