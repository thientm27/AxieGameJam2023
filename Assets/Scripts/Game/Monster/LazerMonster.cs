using DG.Tweening;
using Services;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerMonster : Monster
{
    [SerializeField] private GameObject thorn;
    [SerializeField] private Transform model;
    public override void Init(Transform player, AudioService audioService)
    {
        base.Init(player, audioService);
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

        int rd = Random.Range(0, 2) == 0 ? 1 : -1;
        goTransform.position = new Vector3(v.x * rd, -v.y - 2f);
        if (rd < 0)
        {
            model.rotation = Quaternion.Euler(0, 180, 0);
        }

        goTransform.DOMoveY(UnityEngine.Random.Range(-2.3f, 4f), 2.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            StartCoroutine(Atk(v));
        });
    }
    private IEnumerator Atk(Vector3 v)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, attackAnim, false).Complete += (TrackEntry x) =>
        {
        };
        yield return new WaitForSeconds(0.65f);
        audioService.Laser();
        thorn.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        thorn.SetActive(false);
        goTransform.DOMoveY(-v.y - 3.0f, 3.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SimplePool.Despawn(gameObject);
        });
    }
}
