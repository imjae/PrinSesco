using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapons : MonoBehaviour
{
    private int level;
    private float duration;

    public abstract void LevelUp();
    public abstract void WeaponEffect();
}
