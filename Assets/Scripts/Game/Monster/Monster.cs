using Services;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] protected SkeletonAnimation skeletonAnimation;
    [SerializeField] protected Transform goTransform;
    [SerializeField] protected Collider2D colliderTf;
    public Action<int> OnDeath;
    public string deathAnimn;
    public string attackAnim;
    public string idleAnim;
    public int HP { get; set; }
    public float AttackRate { get; set; }
    public Transform player;
    public int Speed { get; set; }
    protected AudioService audioService;
    public virtual void Init(Transform player, AudioService audioService)
    {
        this.player = player;
        this.audioService = audioService;
        skeletonAnimation.AnimationState.SetAnimation(0, idleAnim, true);
        Speed = 1;
    }
    public virtual void GotHit()
    {

    }
    public virtual void Attack()
    {

    }
    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        SimplePool.Despawn(gameObject);
    }
}
