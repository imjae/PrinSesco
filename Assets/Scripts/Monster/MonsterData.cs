using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTER_TYPE
{
    NORMAL,
    BOSS
}
public enum MONSTER_SPECIES
{
    UNDEAD,
    JUNGLE,
    SEA,
    FOREST,
    FIELD,
    CAVE,
    DEVIL,
    ICE,
}

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private string monsterName;
    public string MonsterName => monsterName;

    [SerializeField]
    private int maxHP;
    public int MaxHP => maxHP;

    [SerializeField]
    private int damage;
    public int Damage => damage;

    [SerializeField]
    private float attackRate;
    public float AttackRate => attackRate;

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [SerializeField]
    private Sprite monsterImage;
    public Sprite MonsterImage => monsterImage;

    [SerializeField]
    private GameObject drop;
    public GameObject Drop => drop;

    [SerializeField]
    private MONSTER_TYPE monsterType;
    public MONSTER_TYPE MonsterType => monsterType;

    [SerializeField]
    private MONSTER_SPECIES monsterSpecies;
    public MONSTER_SPECIES MonsterSpecies => monsterSpecies;
}
