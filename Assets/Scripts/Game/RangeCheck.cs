using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    public Action OnHitMonster;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.MonsterTag))
        {
            collision.GetComponent<Monster>().GotHit();
            OnHitMonster?.Invoke();
        }
    }
}
