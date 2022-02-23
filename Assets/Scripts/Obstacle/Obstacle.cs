using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    [SerializeField, Range(1, 10)]
    private int hp = 10;

    #region IDamageable Implementation
    public void GetHit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
            Destroy(gameObject);
    }
    #endregion
}
