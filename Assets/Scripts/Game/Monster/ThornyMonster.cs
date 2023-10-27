using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornyMonster : Monster
{
    [SerializeField] private GameObject thorn;
    private void Awake()
    {
        thorn.SetActive(false);
        AttackRate = 0.5f;
        Move();
        HP = 1;
    }
    public override void GotHit()
    {
        HP -= 1;
        if (HP <= 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, deathAnimn, false).Complete += (TrackEntry v) =>
            {
                DOTween.Kill(goTransform);
                SimplePool.Despawn(gameObject);
            };
            //goTransform.DOMoveY(goTransform.position.y - 0.2f, 3.0f).SetEase(Ease.Linear).OnComplete(() =>
            //{

            //});
        }
    }
    private void Move()
    {
        Vector3 infPos = goTransform.position;
        Vector3 temp = goTransform.position + new Vector3(UnityEngine.Random.Range(0, 2) == 0 ? 2 : -2, 0, 0);
        goTransform.DOMove(infPos + temp / 2 + Vector3.up * (UnityEngine.Random.Range(11f, 14.5f)), 2.0f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            thorn.SetActive(true);
            thorn.transform.DOMoveZ(0, 1f).OnComplete(() =>
            {
                thorn.SetActive(false);
                goTransform.DOMove(infPos + temp, 2.0f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    SimplePool.Despawn(gameObject);
                });
            });
        });
    }
}
