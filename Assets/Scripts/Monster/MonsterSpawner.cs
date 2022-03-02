using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct Wave
{
    public float spawnRate;
    public int monsterCount;
    public MonsterData[] monsterDatas;
}

[System.Serializable]
public struct MonsterTypes
{
    public MonsterData[] monsterRanks;
}

public class MonsterSpawner : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private MonsterTypes[] monsterTypes;
    [SerializeField]
    private GameObject monsterPrefab;

    public List<List<Monster>> typePool = new List<List<Monster>>();
    public List<Monster> rankPool = new List<Monster>();
    public int poolAmount;

    public Transform player;
    public int enemyCount;

    public Dictionary<int, Action> monsterWave = new Dictionary<int, Action>();
    public Action wave;

    private Monster tempMonster;
    private WaitForSeconds second = new WaitForSeconds(1);
    private int timeCount = 0;
    #endregion

    #region Methods
    public void ObjectPooling()
    {

        while (rankPool.Count < poolAmount)
        {
            rankPool.Add(CreateMonster(0, 0));
        }
        typePool.Add(rankPool);
    }
    public Monster CreateMonster(MONSTER_TYPE type, MONSTER_RANK rank)
    {
        tempMonster = Instantiate(monsterPrefab).GetComponent<Monster>();
        tempMonster.monsterData = monsterTypes[(int)type].monsterRanks[(int)rank];
        tempMonster.name = tempMonster.monsterData.MonsterName;
        tempMonster.monsterImage.sprite = tempMonster.monsterData.MonsterImage;
        tempMonster.gameObject.AddComponent<BoxCollider2D>();
        tempMonster.player = this.player;

        return tempMonster;
    }
    public Monster SpawnMonster(MONSTER_TYPE type, MONSTER_RANK rank)
    {
        tempMonster = typePool[(int)type][(int)rank];
        tempMonster.transform.position = new Vector3(-3, 3, 0);
        return tempMonster;
    }
    public void GuerillaPattern()
    {
        Debug.Log("게릴라 패턴");

    }
    public void CirclePattern()
    {
        Debug.Log("써클 패턴");
    }
    public void RectanglePattern()
    {
        Debug.Log("랙탱글 패턴");
    }
    public void CrowdPattern()
    {
        Debug.Log("크라우드 패턴");
    }
    public IEnumerator WaveFlow()
    {
        while (true)
        {
            yield return second;
            timeCount++;
            if (monsterWave.ContainsKey(timeCount))
                monsterWave[timeCount]();
        }
    }
    #endregion

    private void Start()
    {
        ObjectPooling();


        monsterWave.Add(1, GuerillaPattern);
        monsterWave.Add(10, RectanglePattern);
        monsterWave.Add(15, CrowdPattern);
        monsterWave.Add(20, CrowdPattern);
        monsterWave.Add(25, CrowdPattern);
        monsterWave.Add(30, CrowdPattern);
        monsterWave.Add(35, CrowdPattern);
        StartCoroutine(WaveFlow());

        //Monster monster = SpawnMonster((MONSTER_TYPE)1);
    }
}
