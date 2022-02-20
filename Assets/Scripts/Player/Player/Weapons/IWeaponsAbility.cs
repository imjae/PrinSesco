using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponsAbility
{
}

public interface IProjection
{
    public float ProSpeed { get; }
    public void Attack();
}
