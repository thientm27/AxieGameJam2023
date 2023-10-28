using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] protected SkeletonAnimation skeletonAnimation;
    [SerializeField] protected Transform goTransform;
    [SerializeField] protected Collider2D colliderTf;
    public string deathAnimn;
    public string attackAnim;
    public string idleAnim;
    public int HP { get; set; }
    public float AttackRate { get; set; }
    public Transform player;
    public virtual void GotHit()
    {

    }
    public virtual void Attack()
    {

    }
}
