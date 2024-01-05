using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject boxPrefab;
    public List<Spawn> spawners;
    int safe = 0;
    public bool gameOver;

    public Arrow arrow;

    int score = 0;
    int scoreMult = 1;

    int taps;
    int tapsToEffect = 1;

    public List<BoxType> boxesInScene;
    public List<EffectInfo> possibleEffects = new List<EffectInfo>();
    public List<BoxType> activeEffects;

    public bool testing;

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
            Transform spawnersParent = GameObject.Find("Spawners2").transform;

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
        /*
        int startNum = 10;
        for (int i = 0; i < 2; i++)
        {
            Box box = Instantiate(boxPrefab, spawners[startNum + (i * 2)].transform.position, Quaternion.identity).GetComponent<Box>();
            box.Setup(spawners[startNum + (i * 2)], _boxType);
            boxesInScene.Add(_boxType);
        }
        */

        
        if (safe >= 60)
        {
            CheckSafe();
            return;
        }

        if (_boxType != BoxType.Box && boxesInScene.Contains(_boxType))
            return;

        if (_boxType != BoxType.Box && activeEffects.Contains(_boxType))
            return;

        safe++;

        int random = Random.Range(0, spawners.Count);
        int max = spawners.Count - 1;

        if (spawners[random] != null && spawners[random].haveBox ||
            random > 0 && spawners[random - 1] != null && spawners[random - 1].haveBox ||
            random < spawners.Count - 1 && spawners[random + 1] != null && spawners[random + 1].haveBox ||
            random == 0 && spawners[max] != null && spawners[max].haveBox ||
            random == max && spawners[0] != null && spawners[0].haveBox)
        {
            GenerateBox(_boxType);
            return;
        }

        Box box = Instantiate(boxPrefab, spawners[random].transform.position, Quaternion.identity).GetComponent<Box>();
        box.Setup(spawners[random], _boxType);
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

        if(numBoxes <= 2)
            safe = 0;
    }

    public void AddScore()
    {
        score += (1 * scoreMult);
        UIManager_Game.instance.scoreTxt.text = score.ToString();

        if (score > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", score);
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

            int randomTap;

            if (testing)
                randomTap = Random.Range(0, 1);
            else
                randomTap = Random.Range(3, 6);

            tapsToEffect = randomTap;
        }
    }

    public void ApplyEffect(BoxType _box)
    {
        switch (_box)
        {
            //150 => 0.2
            //100 => 0.29
            case BoxType.Slow:
                arrow.rotSpeed = 100f;
                //arrow.collisionDuration = 0.29f;
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
            case BoxType.Grow:
                arrow.transform.localScale = new Vector3(1.6f, 1.6f, 1f);
                //arrow.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -1.14f);
                //arrow.GetComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.6f);

                arrow.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -1.14f);
                arrow.GetComponent<BoxCollider2D>().size = new Vector2(0.18f, 0.6f);
                UIManager_Game.instance.InstEffect(possibleEffects.Find(item => item.box == _box));
                break;
            case BoxType.Burst:
                CameraShake.instance.StartShake();

                GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

                foreach (GameObject box in boxes)
                {
                    if (box.GetComponent<Box>().boxType != BoxType.Burst)
                        box.GetComponent<Box>().DestroyBox(true);
                }

                GenerateBox(BoxType.Box);
                break;
            case BoxType.Shield:
                arrow.hasShield = true;
                arrow.GetComponent<SpriteRenderer>().color = Color.cyan;
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
                //arrow.collisionDuration = 0.2f;
                break;
            case BoxType.Bar:
                arrow.greenBar.SetActive(false);
                break;
            case BoxType.DoubleScore:
                scoreMult = 1;
                UIManager_Game.instance.scoreTxt.color = Color.black;
                break;
            case BoxType.Grow:
                arrow.transform.localScale = new Vector3(1f, 1f, 1f);
                //arrow.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -1.6f);
                //arrow.GetComponent<BoxCollider2D>().size = new Vector2(0.4f, 1.4f);

                arrow.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -1.6f);
                arrow.GetComponent<BoxCollider2D>().size = new Vector2(0.3f, 1.4f);
                break;
            case BoxType.Shield:
                arrow.GetComponent<SpriteRenderer>().color = Color.black;
                break;
            default:
                break;
        }
    }

    public void GameOver()
    {
        gameOver = true;

        Camera.main.GetComponent<Camera>().backgroundColor = Color.red;
        arrow.GetComponent<SpriteRenderer>().color = Color.red;
        arrow.greenBar.SetActive(false);
        arrow.deathBar.SetActive(true);

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