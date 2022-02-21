using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionWeapons : Weapons
{
    [SerializeField] protected float coolTime;
    [SerializeField] protected float proSpeed;
    [SerializeField] protected float proDamage;
    [SerializeField] protected float proRange;
    [SerializeField] protected float proDuration;

    public override void LevelUp()
    {
        throw new System.NotImplementedException(); // 개발 되지 않은 메서드에 대해 이 예외를 throw
    }
    public override void WeaponEffect()
    {
        throw new System.NotImplementedException(); // 개발 되지 않은 메서드에 대해 이 예외를 throw
    }
}
