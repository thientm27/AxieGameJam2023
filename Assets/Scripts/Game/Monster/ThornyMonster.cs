using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornyMonster : Monster
{
    [SerializeField] private GameObject thorn;
    public override void Init(Transform player)
    {
        base.Init(player);
        AttackRate = 0.5f;
        Move();
        HP = 1;
        colliderTf.enabled = true;
        thorn.SetActive(false);
    }
    public override void GotHit()
    {
        HP -= 1;
        if (HP <= 0)
        {
            OnDeath?.Invoke(Speed);
            DOTween.Kill(goTransform);
            colliderTf.enabled = false;
            skeletonAnimation.AnimationState.SetAnimation(0, deathAnimn, false).Complete += (TrackEntry v) =>
            {
                SimplePool.Despawn(gameObject);
            };
            //goTransform.DOMoveY(goTransform.position.y - 0.2f, 3.0f).SetEase(Ease.Linear).OnComplete(() =>
            //{

            //});
        }
    }
    private void Move()
    {
        var v = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        goTransform.position = new Vector3(UnityEngine.Random.Range(-v.x + 3, v.x - 3), -v.y - 2);

        Vector3 infPos = goTransform.position;
        Vector3 temp = goTransform.position + new Vector3(UnityEngine.Random.Range(0, 2) == 0 ? 2 : -2, 0, 0);
        goTransform.DOMove(infPos + temp / 2 + Vector3.up * (UnityEngine.Random.Range(11f, 14.5f)), 2.0f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            thorn.SetActive(true);
            skeletonAnimation.AnimationState.SetAnimation(0, attackAnim, false);
            //skeletonAnimation.timeScale = 0.5f;
            goTransform.transform.DOMoveZ(0, 1f).OnComplete(() =>
            {
                thorn.SetActive(false);
                skeletonAnimation.timeScale = 1f;
                skeletonAnimation.AnimationState.SetAnimation(0, idleAnim, true);
                goTransform.DOMove(infPos + temp, 2.0f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    SimplePool.Despawn(gameObject);
                });
            });
        });
    }
}
