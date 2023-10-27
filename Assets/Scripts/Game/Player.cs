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

    // Action
    public Action OnHit;
    public Action OnMiss;

    // Cache
    private bool isUp = false;
    private bool canAttack = true;
    private Vector2 position;
    private float downSpeed = 2f;
    private AxieCharacter axieCharacter;
    private string attackAnim = "";
    private string idleAnim = "";
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
        canAttack = false;
        Up();
        model.AnimationState.SetAnimation(0, attackAnim, false);
        model.timeScale = 3.0f;
        Logger.Debug("NNKKK 1");
    }
    private void GotHit()
    {
        int rd = UnityEngine.Random.Range(0, 100);
        if(rd < axieCharacter.ChanceEva)
        {

        }
        else
        {

        }
    }
    private void Update()
    {
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
        if (isUp == true) return;
        if(goTransform.position.y <= minHeight)
        {
            OnMiss?.Invoke();
            downSpeed = defaultDownSpeed;
            isUp = true;
            Up();
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
            isUp = false;
            canAttack = true;
            model.AnimationState.SetAnimation(0, idleAnim, true);
            model.timeScale = 1.0f;
        });
    }
    public void Death()
    {

    }
}
