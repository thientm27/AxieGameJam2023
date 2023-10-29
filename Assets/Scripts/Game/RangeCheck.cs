using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    public Action OnHitMonster;
    public bool CanAttack { get; set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(CanAttack == false) return;
        if(collision.CompareTag(Constants.MonsterTag))
        {
            collision.GetComponent<Monster>().GotHit();
            OnHitMonster?.Invoke();
        }
    }
}
