using DG.Tweening;
using System.Collections;
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
    private int combo = 0;
    private int maxHeart = 3;
    private int currentHeart = 0;
    private int speed = 0;
    private bool isStart = false;
    private int speedMax = 10;

    private float heightPlayer = 0;
    private float heightLava = 0;
    private int speedDecrease = 1;
    private float speedLava = 2;
    private bool startLava = false;
    private void Awake()
    {
        player.Initialized(model.AxieCharacters[0]);
        player.OnHit = PlayerGotHit;
        player.OnMiss = PlayerMissHit;
        pressImg.DOFillAmount(1, 0.5f).SetEase(Ease.OutCirc).SetLoops(-1, LoopType.Yoyo);
        view.SetCombo(combo);
        currentHeart = maxHeart;
        view.SetHeart(maxHeart, currentHeart);
    }
    private void Update()
    {
        if (isStart == true)
        {
            heightPlayer += Time.deltaTime * speed;
            if(startLava == true)
            {
                heightLava += Time.deltaTime * speedLava;
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
            StartCoroutine(SpawnNormal());
            StartCoroutine(DecreaseSpeedOverTime());
            StartCoroutine(StartLava());
        }
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
        StartCoroutine(DecreaseSpeedOverTime());
    }
    private void PlayerGotHit()
    {
        currentHeart -= 1;
        view.SetHeart(maxHeart, currentHeart);
        if(currentHeart < 0)
        {
            player.Death();
        }
        combo -= 2;
        combo = combo < 0 ? 0 : combo;
    }
    private void PlayerMissHit()
    {
        speed -= 2;
    }
    private void PlayerAttack(int sp)
    {
        combo += 1;
        if(combo >= 10)
        {
            speed += 10;
            combo = 0;
        }
        view.SetCombo(combo);
        speed += sp;
        view.SetSpeed(speed);
    }
    private IEnumerator SpawnNormal()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
        int rd = UnityEngine.Random.Range(1, 3);
        for(int i = 0; i < rd; i++)
        {
            GameObject go = SimplePool.Spawn(monsters[0], Vector2.zero, Quaternion.identity);
            Monster ms = go.GetComponent<Monster>();
            ms.Init(player.transform);
            ms.OnDeath = PlayerAttack;
        }
        StartCoroutine(SpawnNormal());
    }
}
