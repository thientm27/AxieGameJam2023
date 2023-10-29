using DG.Tweening;
using Services;
using Spine;
using System.Collections;
using UnityEngine;

public class DragonMonster : Monster
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootTf;
    [SerializeField] private Transform model;
    public override void Init(Transform player, AudioService audioService)
    {
        base.Init(player, audioService);
        HP = 1;
        AttackRate = 0.15f;
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
            skeletonAnimation.AnimationState.SetAnimation(0, deathAnimn, false).Complete += (TrackEntry v) =>
            {
                DOTween.Kill(goTransform);
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
        if(rd < 0)
        {
            model.rotation = Quaternion.Euler(0, 180, 0);
        }

        goTransform.DOMoveY(UnityEngine.Random.Range(-2.3f, 4f), 2.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            goTransform.DOMoveY(goTransform.position.y - 7f, 6.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            StartCoroutine(ShootCoroutine());
        });
    }
    private IEnumerator ShootCoroutine()
    {
        StartCoroutine(Shoot());
        yield return new WaitForSeconds(1 / AttackRate);
        StartCoroutine(ShootCoroutine());
    }
    private IEnumerator Shoot()
    {
        //for(int i = 0; i < 3; i++)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, attackAnim, false).Complete += (TrackEntry v) =>
            {
                skeletonAnimation.AnimationState.SetAnimation(0, idleAnim, true);
            };
            yield return new WaitForSeconds(0.8f);
            audioService.FireBall();
            GameObject bl = SimplePool.Spawn(bullet, shootTf.position, Quaternion.identity);
            var blTf = bl.transform;
            blTf.position = shootTf.position;
            blTf.rotation = Quaternion.Euler(0f, 0f, (goTransform.position.x > 0 ? 180 : 0));

            blTf.DOMoveX((goTransform.position.x > 0 ? - 25 : 25), 15f).SetEase(Ease.Linear).OnComplete(() =>
            {
                blTf.DOMoveX((goTransform.position.x > 0 ? -25 : 25), 3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    SimplePool.Despawn(bl);
                });
            });
        }
    }
}
