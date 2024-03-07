using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject boxPrefab;
    public List<Spawn> spawners;
    int safe = 0;
    public bool playing;
    public bool revived;

    public Arrow arrow;

    public float timeLeft = 0;

    int score = 0;
    public int scoreMult = 1;

    int taps;
    int tapsToEffect = 1;

    public List<BoxType> boxesInScene;
    public List<EffectInfo> possibleEffects = new List<EffectInfo>();
    public List<BoxType> activeEffects;

    public bool testing;

    public static GameController instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (arrow == null)
            arrow = GameObject.Find("Arrow").GetComponent<Arrow>();

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

        timeLeft = 10;
        playing = true;

        GenerateBox(BoxType.Box);
    }

    void Update()
    {
        if (!playing)
            return;

        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UIController_Game.instance.UpdateTimer();
        }
        else
        {
            timeLeft = 0;
            GameOver();
        }
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

        EffectInfo _effectInfo = possibleEffects.Find(item => item.box == _boxType);
        Box box = Instantiate(boxPrefab, spawners[random].transform.position, Quaternion.identity).GetComponent<Box>();
        box.Setup(spawners[random], _boxType, _effectInfo);
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
        UIController_Game.instance.scoreTxt.text = score.ToString();

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
        EffectInfo _effectInfo = possibleEffects.Find(item => item.box == _box);

        switch (_box)
        {
            case BoxType.Slow:
                arrow.rotSpeed = 100f;
                timeLeft += 5f;
                //arrow.collisionDuration = 0.29f;
                break;
            case BoxType.Bar:
                arrow.greenBar.SetActive(true);
                break;
            case BoxType.DoubleScore:
                scoreMult = 2;
                UIController_Game.instance.scoreTxt.color = _effectInfo.barColor;
                break;
            case BoxType.Grow:
                arrow.transform.localScale = new Vector3(1.6f, 1.6f, 1f);
                //arrow.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -1.14f);
                //arrow.GetComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.6f);

                arrow.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -1.14f);
                arrow.GetComponent<BoxCollider2D>().size = new Vector2(0.18f, 0.6f);
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
                arrow.GetComponent<SpriteRenderer>().color = _effectInfo.barColor;//Color.cyan;
                break;
            default:
                break;
        }

        if (_box == BoxType.Burst)
            return;

        UIController_Game.instance.InstEffect(_effectInfo);
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
                UIController_Game.instance.scoreTxt.color = Color.black;
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
        //Game
        playing = false;
        ChangeSceneToRed(true);

        //UI
        UIController_Game.instance.scoreTxt.text = "Game Over";
        UIController_Game.instance.scoreTxt.color = Color.black;

        //Music
        AudioController.instance.StopMusic();
        AudioController.instance.PlaySFX("GameOver");

        //Game Over
        if (!revived && AdsInitializer.instance.rewardedAdsButton.adReady)
            StartCoroutine(ReviveTimer(2f));
        else
            StartCoroutine(GameOverTimer(2f));
    }

    IEnumerator ReviveTimer(float _time)
    {
        yield return new WaitForSeconds(_time);

        //Game
        UIController_Game.instance.revivePanel.SetActive(true);
        UIController_Game.instance.bgPanel.SetActive(true);
        ChangeSceneToRed(false);

        //UI
        UIController_Game.instance.scoreTxt.text = score.ToString();
        UIController_Game.instance.ReduceAlphaToScoreText(true);
        UIController_Game.instance.ReduceAlphaToEffectsIcons(true);
    }

    IEnumerator GameOverTimer(float _time)
    {
        yield return new WaitForSeconds(_time);

        ReturnToMenu();
    }

    public void ShowAd()
    {
        AdsInitializer.instance.rewardedAdsButton.ShowAd();
    }

    public void ReturnToMenu()
    {
        SceneController.instance.ChangeScene("Menu");
    }

    public void PlayAgain()
    {
        //Arrow
        arrow.Setup();

        //UI
        UIController_Game.instance.ReduceAlphaToScoreText(false);
        UIController_Game.instance.ReduceAlphaToEffectsIcons(false);

        //Music
        AudioController.instance.PlayMusic("GameTheme");

        //Game
        UIController_Game.instance.revivePanel2.SetActive(false);
        UIController_Game.instance.bgPanel.SetActive(false);

        playing = true;
        revived = true;
    }

    public void ChangeSceneToRed(bool _change)
    {
        if(_change)
        {
            //Camera
            Camera.main.GetComponent<Camera>().backgroundColor = Color.red;

            //Arrow
            arrow.GetComponent<SpriteRenderer>().color = Color.red;
            arrow.deathBar.SetActive(true);

            //Boxes
            GameObject[] boxes1 = GameObject.FindGameObjectsWithTag("Box");

            foreach (GameObject box in boxes1)
            {
                box.GetComponent<SpriteRenderer>().color = Color.red;
            }

            return;
        }

        //Camera
        Camera.main.GetComponent<Camera>().backgroundColor = Color.white;

        //Arrow
        if(arrow.hasShield)
            arrow.GetComponent<SpriteRenderer>().color = Color.cyan;
        else
            arrow.GetComponent<SpriteRenderer>().color = Color.black;

        arrow.deathBar.SetActive(false);

        //Boxes
        GameObject[] boxes2 = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject box in boxes2)
        {
            box.GetComponent<SpriteRenderer>().color = box.GetComponent<Box>().boxColor;
        }
    }
}