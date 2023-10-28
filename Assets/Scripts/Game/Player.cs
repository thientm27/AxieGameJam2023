using UnityEngine;
using Spine;
using Spine.Unity;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    private const float timeMoveUp = 0.5f;
    private const float defaultDownSpeed = 2.0f;
    private const float attackDownSpeed = 100f;
    [SerializeField] private SkeletonAnimation model;
    [SerializeField] private Transform goTransform;
    [SerializeField] private float maxHeight = 10f;
    [SerializeField] private float minHeight = -10f;
    [SerializeField] private float speed = 18f;
    [SerializeField] private Vector2 limitWidth;
    [SerializeField] private RangeCheck rangeCheck;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform RocketParent;

    // Action
    public Action OnHit;
    public Action OnMiss;
    public Action OnAttack;
    public bool IsStart { get => isStart; set => isStart = value; }

    // Cache
    private bool canHit = true;
    private bool isUp = false;
    private bool canAttack = true;
    private bool isStart = false;
    private bool isDeath = false;
    private Vector2 position;
    private float downSpeed = 2f;
    private AxieCharacter axieCharacter;
    private string attackAnim = "";
    private string idleAnim = "";
    private Transform rocket;
    private void Awake()
    {
        rangeCheck.OnHitMonster = HitMonster;
    }
    public void Initialized(AxieCharacter axie)
    {
        this.axieCharacter = axie;
        attackAnim = axie.AttackAnim;
        idleAnim = axie.IdleAnimn;
    }
    private void HitMonster()
    {
        if (isDeath == true) return;
        if (isStart == false) return;
        OnAttack?.Invoke();
        canHit = false;
        canAttack = false;
        rangeCheck.CanAttack = false;
        Up();
        model.AnimationState.SetAnimation(0, attackAnim, false);
        model.timeScale = 3.0f;
        Logger.Debug("NNKKK 1");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDeath == true) return;
        if (isStart == false) return;
        if (canHit == false) return;
        if (collision.CompareTag(Constants.BulletTag))
        {
            SimplePool.Despawn(collision.gameObject);
            GotHit();
        }
    }
    private void GotHit()
    {
        int rd = UnityEngine.Random.Range(0, 100);
        if(rd < axieCharacter.ChanceEva)
        {

        }
        else
        {
            OnHit?.Invoke();
        }
    }
    private void Update()
    {
        if (isDeath == true) return;
        if (isStart == false) return;
        if (isUp == true) return;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            position = goTransform.position;
            position.x -= speed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, limitWidth.x, limitWidth.y);
            goTransform.position = position;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            position = goTransform.position;
            position.x += speed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, limitWidth.x, limitWidth.y);
            goTransform.position = position;
        }
        if(goTransform.position.y <= minHeight)
        {
            OnMiss?.Invoke();
            downSpeed = defaultDownSpeed;
            isUp = true;
            goTransform.DOMoveZ(0, 1.3f).OnComplete(() =>
            {
                Up();
                SpawnRocket();
            });
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            downSpeed = attackDownSpeed;
        }
        if(axieCharacter.IsDownOverTime)
        {
            position = goTransform.position;
            position.y -= downSpeed * Time.deltaTime;
            goTransform.position = position;
        }
    }
    private void Up()
    {
        float timeMove = (maxHeight - goTransform.position.y) / (maxHeight - minHeight) * timeMoveUp;
        goTransform.DOMoveY(maxHeight, timeMove).SetEase(Ease.Linear).OnComplete(() =>
        {
            downSpeed = defaultDownSpeed;
            if (rocket != null)
            {
                rocket.SetParent(null);
            }
            canHit = true;
            isUp = false;
            canAttack = true;
            rangeCheck.CanAttack = true;
            model.AnimationState.SetAnimation(0, idleAnim, true);
            model.timeScale = 1.0f;
        });
    }
    public void Death()
    {
        isDeath = true;
    }
    private void SpawnRocket()
    {
        GameObject go = SimplePool.Spawn(rocketPrefab, RocketParent.position, Quaternion.identity);
        Transform goTf = go.transform;
        rocket = goTf;
        goTf.SetParent(RocketParent);
        goTf.localPosition = Vector2.zero;
        goTf.DOMoveZ(0, 1.0f).OnComplete(() =>
        {
            goTf.SetParent(null);
            goTf.DOMoveZ(0, 0.5f).OnComplete(() =>
            {
                goTf.DOMoveY(-20, 3).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    SimplePool.Despawn(go);
                    rocket = null;
                });
            });
        });
    }
}
