using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTER_TYPE
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
    UNDEAD_112
}

public class MonsterSpawner : MonoBehaviour
{
    #region Spawning Implementation
    [SerializeField]
    private List<MonsterData> monsterDatas;
    [SerializeField]
    private GameObject monsterPrefab;

    public Monster SpawnMonster(MONSTER_TYPE type)
    {
        Monster newMonster = Instantiate(monsterPrefab).GetComponent<Monster>();
        newMonster.monsterData = monsterDatas[(int)type];
        newMonster.name = newMonster.monsterData.MonsterName;
        return newMonster;
    }
    #endregion

    private void Start()
    {
        for (int i = 0; i < monsterDatas.Count; i++)
        {
            Monster monster = SpawnMonster((MONSTER_TYPE)i);
            monster.PrintMonsterData();
        }
    }
}
