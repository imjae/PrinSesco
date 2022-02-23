using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapons : MonoBehaviour
{
    public string weaponName;
    public string weaponInfo;
    public int level;
    [SerializeField] protected int att; // 플레이어에서 처리할거면 protected가 나은 것 같아서 수정
    [SerializeField] protected float duration;

    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            LevelUp();
        }
    }

    public abstract void LevelUp();
    public abstract void WeaponEffect();
}
