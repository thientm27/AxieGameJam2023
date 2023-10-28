using DG.Tweening;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameModel model;
    [SerializeField] private GameView view;
    [SerializeField] private Image pressImg;
    // Monster
    [SerializeField] private GameObject[] monsters;
    [SerializeField] private Transform lava;
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
    private void Awake()
    {
        var axie = model.AxieCharacters[5];
        maxHeart = axie.MaxHP;
        player.Initialized(axie);
        player.OnHit = PlayerGotHit;
        player.OnMiss = PlayerMissHit;
        player.OnDeath = () =>
        {
            isEnd = true;
            StopAllCoroutines();
            DOTween.KillAll();
        };
        pressImg.DOFillAmount(1, 0.5f).SetEase(Ease.OutCirc).SetLoops(-1, LoopType.Yoyo);
        view.SetCombo(combo);
        currentHeart = maxHeart;
        view.SetHeart(maxHeart, currentHeart);
        lavaGO = lava.gameObject;
        colliderLava = lava.GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (isEnd == true) return;
        if (isStart == true)
        {
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
                isEnd = true;
                Win();
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
            isEnd = true;
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
        combo += 1;
        if(combo >= 10)
        {
            speed += 10;
            combo = 0;
            speedLava += 5;
        }
        view.SetCombo(combo);
        speed += sp;
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
            ms.Init(player.transform);
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
            ms.Init(player.transform);
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
            ms.Init(player.transform);
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
            ms.Init(player.transform);
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
            ms.Init(player.transform);
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
            ms.Init(player.transform);
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
            ms.Init(player.transform);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnLaze(timeSpawn, numberSpawnPerTime, monster));
    }
}
