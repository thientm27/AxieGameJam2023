using DG.Tweening;
using Services;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameModel model;
    [SerializeField] private GameView view;
    [SerializeField] private Image pressImg;
    // Monster
    [SerializeField] private GameObject[] monsters;
    [SerializeField] private Transform lava;
    [SerializeField] private GameObject layer;
    [SerializeField] private Transform bgParent;
    [SerializeField] private ChangeSceneController changeScene;
    private float offetTileBG = 4.7f;
    private float startOffset = 9f;
    private float currentOffset = 9.8f;
    private float nextOffset = 9.8f;

    private int combo = 0;
    private int maxHeart = 3;
    private int currentHeart = 0;
    private int speed = 0;
    private bool isStart = false;
    private int speedMax = 10;

    private float heightPlayer = 0;
    private float heightLava = 0;
    private int speedDecrease = 1;
    private float speedLava = 3;
    private bool startLava = false;
    private GameObject lavaGO;
    private Collider2D colliderLava;
    private bool isEnd = false;

    private float maxHeigh = 1000f;
    // Services
    private GameServices gameServices;
    private PlayerService playerService;
    private AudioService audioService;

    private int gold = 0;
    private bool outGame = false;
    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag(Constants.ServicesTag) == null)
        {
            SceneManager.LoadScene(Constants.EntryScene);
            return;
        }
        else
        {
            GameObject gameServiceObject = GameObject.FindGameObjectWithTag(Constants.ServicesTag);
            gameServices = gameServiceObject.GetComponent<GameServices>();
            playerService = gameServices.GetService<PlayerService>();
            audioService = gameServices.GetService<AudioService>();
        }

        var axie = model.AxieCharacters[UnityEngine.Random.Range(0, Mathf.Min(playerService.GetLevel() + 1, model.AxieCharacters.Count))];
        maxHeart = axie.MaxHP;
        // From shop
        maxHeart += playerService.ArmoryLevel[0] - 1;
        player.Speed *= (playerService.ArmoryLevel[2] * 0.2f + 1) - 0.2f;
        speedMax += playerService.ArmoryLevel[3] * 2 - 2;

        player.DownSpeed -= playerService.AccessoryLevel[2] * 0.2f + 0.2f;

        player.Initialized(axie, audioService);
        player.OnHit = PlayerGotHit;
        player.OnMiss = PlayerMissHit;
        player.OnDeath = Lose;
        pressImg.DOFillAmount(1, 0.5f).SetEase(Ease.OutCirc).SetLoops(-1, LoopType.Yoyo);
        view.SetCombo(combo);
        currentHeart = maxHeart;
        view.SetHeart(maxHeart, currentHeart);
        lavaGO = lava.gameObject;
        colliderLava = lava.GetComponent<Collider2D>();

        maxHeigh += maxHeigh * 0.3f * playerService.GetLevel();
        speedLava *= (playerService.GetLevel() * 0.5f + 1);
        audioService.PlayMusicGame();
    }
    private void Start()
    {
        changeScene.Open();
    }
    private void Lose()
    {
        if(isEnd == false)
        {
            audioService.EndGame();
            isEnd = true;
            StopAllCoroutines();
            DOTween.KillAll();

            playerService.UserCoin += gold;
            playerService.SavePlayerData();

            view.OpenLose(true, playerService.GetLevel(), playerService.UserCoin, gold, playerService.GetRecordGold(), (int)heightPlayer, playerService.GetRecordDistance());
            playerService.SetRecordGold(gold);
            playerService.SetReCordDistance((int)heightPlayer);
        }
    }
    private void Update()
    {
        if (outGame == true) return;
        if (isEnd == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                changeScene.Close(() =>
                {
                    SceneManager.LoadScene(Constants.GamePlay);
                });
                audioService.Button();
                outGame = true;
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                changeScene.Close(() =>
                {
                    SceneManager.LoadScene(Constants.ShopScene);
                });
                audioService.Button();
                outGame = true;
            }
            return;
        }
        if (isStart == true)
        {
            var p = bgParent.position;
            p.y -= Time.deltaTime * speed;
            currentOffset += Time.deltaTime * speed;
            if(currentOffset >= nextOffset)
            {
                nextOffset += offetTileBG;
                GameObject go = SimplePool.Spawn(layer, new Vector3(0, nextOffset), Quaternion.identity);
                Transform tileTf = go.transform;
                tileTf.localScale = Vector3.one * 2;
                tileTf.SetParent(bgParent);
                tileTf.localPosition = new Vector3(0, nextOffset);
            }
            bgParent.position = p;

            heightPlayer += Time.deltaTime * speed;
            if(startLava == true)
            {
                heightLava += Time.deltaTime * speedLava;
                lava.position = new Vector2(0, heightLava - heightPlayer - 10f);
            }
            lavaGO.SetActive(lava.localPosition.y > -10f);
            colliderLava.enabled = lava.localPosition.y > -7f;
            if (speedLava > speed + 2)
            {
                speed += 1;
            }
            view.SetProgress(heightPlayer / maxHeigh);
            if(heightPlayer>= maxHeigh)
            {
                if(isEnd == false)
                {
                    isEnd = true;
                    Win();
                }
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isStart = true;
            pressImg.DOKill();
            float fillAmount = pressImg.fillAmount;
            speed = (int)(fillAmount * speedMax);
            view.ViewUIIngame();
            player.IsStart = true;
            view.SetSpeed(speed);
            StartCoroutine(SpawnNormal(1, 3));
            StartCoroutine(SpawnThorn(3, 1));
            StartCoroutine(SpawnHigh(6, 1));
            StartCoroutine(SpawnStraight(6, 1));
            StartCoroutine(SpawnDragon(10, 1));
            StartCoroutine(SpawnBat(10, 1));
            StartCoroutine(SpawnLaze(15, 1));
            StartCoroutine(DecreaseSpeedOverTime());
            StartCoroutine(StartLava());
        }
    }
    private void Win()
    {
        audioService.EndGame();
        playerService.UserCoin += gold;
        playerService.SavePlayerData();
        playerService.SetLevel(playerService.GetLevel() + 1);
        view.OpenLose(false, playerService.GetLevel(), playerService.UserCoin, gold, playerService.GetRecordGold(), (int)heightPlayer, playerService.GetRecordDistance());
    }
    private IEnumerator StartLava()
    {
        yield return new WaitForSeconds(3.0f);
        startLava = true;
    }
    private IEnumerator DecreaseSpeedOverTime()
    {
        yield return new WaitForSeconds(3);
        speed -= speedDecrease;
        speed = speed < 0 ? 0 : speed;
        view.SetSpeed(speed);
        speedLava += 1.5f;
        StartCoroutine(DecreaseSpeedOverTime());
    }
    private void PlayerGotHit()
    {
        currentHeart -= 1;
        view.SetHeart(maxHeart, currentHeart);
        if(currentHeart <= 0)
        {
            player.Death();
            Lose();
        }
        combo -= 2;
        combo = combo < 0 ? 0 : combo;
    }
    private void PlayerMissHit(int miss)
    {
        speed -= miss;
        heightLava += miss;
    }
    private void PlayerAttack(int sp)
    {
        gold += 1;
        combo += 1;
        if(combo >= 10)
        {
            speed += 10;
            combo = 0;
            speedLava += 5;
            speed += playerService.AccessoryLevel[3] * 2 - 2;
            audioService.Buff();
        }
        view.SetCombo(combo);
        speed += sp;
        speed += playerService.AccessoryLevel[1];
        view.SetSpeed(speed);
    }
    private IEnumerator SpawnNormal(float timeSpawn, int numberSpawnPerTime, int monster = 0)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(timeSpawn, timeSpawn + 2f));
        int rd = UnityEngine.Random.Range(1, numberSpawnPerTime + 1);
        for(int i = 0; i < rd; i++)
        {
            GameObject go = SimplePool.Spawn(monsters[monster], Vector2.zero, Quaternion.identity);
            Monster ms = go.GetComponent<Monster>();
            ms.Init(player.transform, audioService);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnNormal(timeSpawn, numberSpawnPerTime, monster));
    }
    private IEnumerator SpawnThorn(float timeSpawn, int numberSpawnPerTime, int monster = 1)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(timeSpawn, timeSpawn + 2f));
        int rd = UnityEngine.Random.Range(1, numberSpawnPerTime + 1);
        for (int i = 0; i < rd; i++)
        {
            GameObject go = SimplePool.Spawn(monsters[monster], Vector2.zero, Quaternion.identity);
            Monster ms = go.GetComponent<Monster>();
            ms.Init(player.transform, audioService);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnThorn(timeSpawn, numberSpawnPerTime, monster));
    }
    private IEnumerator SpawnHigh(float timeSpawn, int numberSpawnPerTime, int monster = 2)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(timeSpawn, timeSpawn + 2f));
        int rd = UnityEngine.Random.Range(1, numberSpawnPerTime + 1);
        for (int i = 0; i < rd; i++)
        {
            GameObject go = SimplePool.Spawn(monsters[monster], Vector2.zero, Quaternion.identity);
            Monster ms = go.GetComponent<Monster>();
            ms.Init(player.transform, audioService);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnHigh(timeSpawn, numberSpawnPerTime, monster));
    }
    private IEnumerator SpawnStraight(float timeSpawn, int numberSpawnPerTime, int monster = 3)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(timeSpawn, timeSpawn + 2f));
        int rd = UnityEngine.Random.Range(1, numberSpawnPerTime + 1);
        for (int i = 0; i < rd; i++)
        {
            GameObject go = SimplePool.Spawn(monsters[monster], Vector2.zero, Quaternion.identity);
            Monster ms = go.GetComponent<Monster>();
            ms.Init(player.transform, audioService);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnStraight(timeSpawn, numberSpawnPerTime, monster));
    }
    private IEnumerator SpawnDragon(float timeSpawn, int numberSpawnPerTime, int monster = 4)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(timeSpawn, timeSpawn + 2f));
        int rd = UnityEngine.Random.Range(1, numberSpawnPerTime + 1);
        for (int i = 0; i < rd; i++)
        {
            GameObject go = SimplePool.Spawn(monsters[monster], Vector2.zero, Quaternion.identity);
            Monster ms = go.GetComponent<Monster>();
            ms.Init(player.transform, audioService);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnDragon(timeSpawn, numberSpawnPerTime, monster));
    }
    private IEnumerator SpawnBat(float timeSpawn, int numberSpawnPerTime, int monster = 5)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(timeSpawn, timeSpawn + 2f));
        int rd = UnityEngine.Random.Range(1, numberSpawnPerTime + 1);
        for (int i = 0; i < rd; i++)
        {
            GameObject go = SimplePool.Spawn(monsters[monster], Vector2.zero, Quaternion.identity);
            Monster ms = go.GetComponent<Monster>();
            ms.Init(player.transform, audioService);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnBat(timeSpawn, numberSpawnPerTime, monster));
    }
    private IEnumerator SpawnLaze(float timeSpawn, int numberSpawnPerTime, int monster = 6)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(timeSpawn, timeSpawn + 2f));
        int rd = UnityEngine.Random.Range(1, numberSpawnPerTime + 1);
        for (int i = 0; i < rd; i++)
        {
            GameObject go = SimplePool.Spawn(monsters[monster], Vector2.zero, Quaternion.identity);
            Monster ms = go.GetComponent<Monster>();
            ms.Init(player.transform, audioService);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnLaze(timeSpawn, numberSpawnPerTime, monster));
    }
}
