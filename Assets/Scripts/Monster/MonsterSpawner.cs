using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        newMonster.transform.position = new Vector3(-3, 3, 0);
        newMonster.monsterData = monsterDatas[(int)type];
        newMonster.name = newMonster.monsterData.MonsterName;
        newMonster.GetComponent<SpriteRenderer>().sprite = newMonster.monsterData.MonsterImage;
        newMonster.gameObject.AddComponent<BoxCollider2D>();
        return newMonster;
    }
    #endregion

    private void Start()
    {
        //for (int i = 0; i < monsterDatas.Count; i++)
        //{
        //    Monster monster = SpawnMonster((MONSTER_TYPE)i);
        //    monster.PrintMonsterData();
        //}
        Monster monster = SpawnMonster((MONSTER_TYPE)0);
        //monster.PrintMonsterData();
        Debug.Log(monster.monsterData.MonsterSpecies);
    }
}
