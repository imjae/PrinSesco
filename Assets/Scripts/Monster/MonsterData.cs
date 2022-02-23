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
    UNDEAD_101,
    UNDEAD_102,
    UNDEAD_103,
    UNDEAD_104,
    UNDEAD_105,
    UNDEAD_106,
    UNDEAD_107,
    UNDEAD_108,
    UNDEAD_109,
    UNDEAD_110,
    UNDEAD_111,
    UNDEAD_112,

    JUNGLE_201,
    JUNGLE_202,
    JUNGLE_203,
    JUNGLE_204,
    JUNGLE_205,
    JUNGLE_206,
    JUNGLE_207,
    JUNGLE_208,
    JUNGLE_209,
    JUNGLE_210,
    JUNGLE_211,
    JUNGLE_212,

    SEA_301,
    SEA_302,
    SEA_303,
    SEA_304,
    SEA_305,
    SEA_306,
    SEA_307,
    SEA_308,
    SEA_309,
    SEA_310,
    SEA_311,
    SEA_312,

    FOREST_401,
    FOREST_402,
    FOREST_403,
    FOREST_404,
    FOREST_405,
    FOREST_406,
    FOREST_407,
    FOREST_408,
    FOREST_409,
    FOREST_410,
    FOREST_411,
    FOREST_412,

    FIELD_501,
    FIELD_502,
    FIELD_503,
    FIELD_504,
    FIELD_505,
    FIELD_506,
    FIELD_507,
    FIELD_508,
    FIELD_509,
    FIELD_510,
    FIELD_511,
    FIELD_512,

    CAVE_601,
    CAVE_602,
    CAVE_603,
    CAVE_604,
    CAVE_605,
    CAVE_606,
    CAVE_607,
    CAVE_608,
    CAVE_609,
    CAVE_610,
    CAVE_611,
    CAVE_612,

    DEVIL_701,
    DEVIL_702,
    DEVIL_703,
    DEVIL_704,
    DEVIL_705,
    DEVIL_706,
    DEVIL_707,
    DEVIL_708,
    DEVIL_709,
    DEVIL_710,
    DEVIL_711,
    DEVIL_712,

    ICE_801,
    ICE_802,
    ICE_803,
    ICE_804,
    ICE_805,
    ICE_806,
    ICE_807,
    ICE_808,
    ICE_809,
    ICE_810,
    ICE_811,
    ICE_812
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
