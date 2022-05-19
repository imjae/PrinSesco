using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectionWeapons : Weapons
{
    [SerializeField] protected float coolTime;
    [SerializeField] protected float proSpeed;
    //[SerializeField] protected float proDamage;
    [SerializeField] protected float proRange;
    //[SerializeField] protected float proDuration;

    protected Animator weaponAni;
    protected BoxCollider2D boxCol;

    private void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        weaponAni = GetComponent<Animator>();
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void LevelUp()
    {
        throw new System.NotImplementedException(); // 구현 되지 않은 메서드에 대해 이 예외를 throw
    }
    public override void WeaponEffect()
    {
        throw new System.NotImplementedException(); // 구현 되지 않은 메서드에 대해 이 예외를 throw
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
            other.GetComponent<Monster>().Hp -= att;
    }


}
