using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public float spawnRate;
    public int monsterCount;
    public MonsterData[] monsterDatas;
}

public class MonsterSpawner : MonoBehaviour
{
    #region Spawning Implementation
    [SerializeField]
    private Wave waves;
    [SerializeField]
    private MonsterData[] monsterDatas;
    [SerializeField]
    private GameObject monsterPrefab;
    public Transform player;

    public int enemyCount;

    public Monster SpawnMonster(MONSTER_TYPE type)
    {
        Monster newMonster = Instantiate(monsterPrefab).GetComponent<Monster>();
        newMonster.transform.position = new Vector3(-3, 3, 0);
        newMonster.monsterData = monsterDatas[(int)type];
        newMonster.name = newMonster.monsterData.MonsterName;
        newMonster.monsterImage.sprite = newMonster.monsterData.MonsterImage;
        newMonster.gameObject.AddComponent<BoxCollider2D>();
        newMonster.player = this.player;
        return newMonster;
    }
    #endregion

    private void Start()
    {
        Monster monster = SpawnMonster((MONSTER_TYPE)1);
    }
}
