using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject boxPrefab;
    public List<Spawn> spawners;
    int safe = 0;

    public Arrow arrow;

    public int score = 0;
    public int scoreMult = 1;

    public int taps;
    public int tapsToEffect = 1;

    public List<BoxType> boxesInScene;
    public List<EffectInfo> possibleEffects = new List<EffectInfo>();
    public List<BoxType> activeEffects;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        if (arrow == null)
        {
            arrow = GameObject.Find("Arrow").GetComponent<Arrow>();
        }

        if (spawners.Count == 0)
        {
            Transform spawnersParent = GameObject.Find("Spawners").transform;

            if (spawnersParent == null)
                return;

            foreach (Transform child in spawnersParent)
            {
                spawners.Add(child.GetComponent<Spawn>());
            }
        }

        GenerateBox(BoxType.Box);
    }

    public void GenerateBox(BoxType _boxType)
    {
        if (safe >= 60)
        {
            CheckSafe();
            return;
        }

        if (_boxType != BoxType.Box && boxesInScene.Contains(_boxType))
            return;

        if (_boxType != BoxType.Box && activeEffects.Contains(_boxType))//activeEffects.Find(item => item.box == _boxType) != null)
            return;

        safe++;

        int random = Random.Range(0, spawners.Count);

        if (spawners[random] != null && spawners[random].haveBox ||
            random > 0 && spawners[random - 1] != null && spawners[random - 1].haveBox ||
            random < spawners.Count - 1 && spawners[random + 1] != null && spawners[random + 1].haveBox ||
            random == 0 && spawners[23] != null && spawners[23].haveBox ||
            random == 23 && spawners[0] != null && spawners[0].haveBox)
        {
            GenerateBox(_boxType);
            return;
        }

        Box box = Instantiate(boxPrefab, spawners[random].transform.position, Quaternion.identity).GetComponent<Box>();
        spawners[random].haveBox = true;
        box.spawn = spawners[random];
        box.Setup(_boxType);
        boxesInScene.Add(_boxType);

        safe = 0;
    }

    public void CheckSafe()
    {
        int numBoxes = 0;
        foreach (BoxType box in boxesInScene)
        {
            if (box == BoxType.Box)
                numBoxes++;
        }

        if(numBoxes <= 3)
            safe = 0;
    }

    public void AddScore()
    {
        score += (1 * scoreMult);
        UIManager_Game.instance.scoreTxt.text = score.ToString();
    }

    public void AddTap()
    {
        if (safe >= 60)
            return;

        taps++;

        if (taps >= tapsToEffect)
        {
            int random = Random.Range(0, possibleEffects.Count + 1);

            if(random == possibleEffects.Count)
                GenerateBox(BoxType.Box);
            else
                GenerateBox(possibleEffects[random].box);  

            taps = 0;

            int randomTap = Random.Range(2, 4);
            tapsToEffect = randomTap;
        }
    }

    public void ApplyEffect(BoxType _box)
    {
        switch (_box)
        {
            case BoxType.Slow:
                arrow.rotSpeed = 100f;
                //UIManager_Game.instance.InstEffect(possibleEffects.Find(item => item.effectName == _effect));
                UIManager_Game.instance.InstEffect(possibleEffects.Find(item => item.box == _box));
                break;
            case BoxType.Bar:
                arrow.greenBar.SetActive(true);
                UIManager_Game.instance.InstEffect(possibleEffects.Find(item => item.box == _box));
                break;
            case BoxType.DoubleScore:
                scoreMult = 2;
                UIManager_Game.instance.scoreTxt.color = new Color32(250, 172, 17, 255);
                UIManager_Game.instance.InstEffect(possibleEffects.Find(item => item.box == _box));
                break;
            default:
                break;
        }
    }

    public void RemoveEffect(BoxType _box)
    {
        switch (_box)
        {
            case BoxType.Slow:
                arrow.rotSpeed = arrow.currSpeed;
                break;
            case BoxType.Bar:
                arrow.greenBar.SetActive(false);
                break;
            case BoxType.DoubleScore:
                scoreMult = 1;
                UIManager_Game.instance.scoreTxt.color = Color.black;
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
        UIManager_Game.instance.scoreTxt.color = Color.black;

        StartCoroutine(GameOverTimer(1f));
    }

    IEnumerator GameOverTimer(float _time)
    {
        yield return new WaitForSeconds(_time);

        SceneController.instance.ChangeScene("Menu");
    }
}