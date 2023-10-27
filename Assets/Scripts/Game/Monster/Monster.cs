using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] protected SkeletonAnimation skeletonAnimation;
    [SerializeField] protected Transform goTransform;
    public string deathAnimn;
    public int HP { get; set; }
    public float AttackRate { get; set; }
    public virtual void GotHit()
    {

    }
    public virtual void Attack()
    {

    }
}
