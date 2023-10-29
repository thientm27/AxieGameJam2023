using DG.Tweening;
using Services;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightShotMonster : Monster
{
    [SerializeField] private GameObject bullet;
    public override void Init(Transform player, AudioService audioService)
    {
        base.Init(player, audioService);
        HP = 1;
        AttackRate = 0.5f;
        Move();
        colliderTf.enabled = true;
    }
    public override void GotHit()
    {
        HP -= 1;
        if (HP <= 0)
        {
            OnDeath?.Invoke(Speed);
            colliderTf.enabled = false;
            DOTween.Kill(goTransform);
            skeletonAnimation.AnimationState.SetAnimation(0, deathAnimn, false).Complete += (TrackEntry v) =>
            {
                SimplePool.Despawn(gameObject);
            };
            StartCoroutine(DeathCoroutine());
            //goTransform.DOMoveY(goTransform.position.y - 0.2f, 3.0f).SetEase(Ease.Linear).OnComplete(() =>
            //{

            //});
        }
    }
    private void Move()
    {
        var v = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        v.x -= 3;
        Debug.Log(v);

        goTransform.position = new Vector3(v.x * (Random.Range(0, 2) == 0 ? 1 : -1), -v.y - 2f);
        goTransform.DOMoveY(UnityEngine.Random.Range(-4.3f, 0f), 2.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            MoveAround();
            StartCoroutine(ShootCoroutine());
        });
    }
    private void MoveAround()
    {
        goTransform.DOMoveX(-goTransform.position.x, 6.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        goTransform.DOMoveY(goTransform.position.y + 2, 1.3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
    private IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range((float)(1 / AttackRate), (float)(1 / AttackRate + 1.0f)));
        StartCoroutine(Shoot());
        StartCoroutine(ShootCoroutine());
    }
    private IEnumerator Shoot()
    {
        goTransform.DOPause();
        skeletonAnimation.AnimationState.SetAnimation(0, attackAnim, false).Complete += (TrackEntry v) =>
        {
            skeletonAnimation.AnimationState.SetAnimation(0, idleAnim, true);
        };
        yield return new WaitForSeconds(0.3f);
        audioService.FireBall();
        GameObject bl = SimplePool.Spawn(bullet, goTransform.position + Vector3.up * 1.0f, Quaternion.identity);
        bl.transform.DOMoveY(20f, 2.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SimplePool.Despawn(bl);
            goTransform.DOPlay();
        });
    }
}
