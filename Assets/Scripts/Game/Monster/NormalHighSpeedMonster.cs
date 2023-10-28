using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHighSpeedMonster : Monster
{
    public override void Init(Transform player)
    {
        base.Init(player);
        HP = 1;
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
            //goTransform.DOMoveY(goTransform.position.y - 0.2f, 3.0f).SetEase(Ease.Linear).OnComplete(() =>
            //{

            //});
        }
    }
    private void Move()
    {
        var v = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        if(goTransform.position.x > 0)
        {
            v.x = -v.x;
        }
        goTransform.DOMove(new Vector3(v.x, goTransform.position.y - 4.0f), 1.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SimplePool.Despawn(gameObject);
        });
    }
}
