using Spine.Unity;
using System;
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
    public virtual void Init(Transform player)
    {
        this.player = player;
        skeletonAnimation.AnimationState.SetAnimation(0, idleAnim, true);
        Speed = 1;
    }
    public virtual void GotHit()
    {

    }
    public virtual void Attack()
    {

    }
}
