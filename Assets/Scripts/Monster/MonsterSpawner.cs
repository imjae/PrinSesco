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

public class MonsterSpawner : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private MonsterData[] monsterDatas;
    [SerializeField]
    private GameObject monsterPrefab;

    public Transform player;
    public int enemyCount;

    public Dictionary<int, Action> monsterWave = new Dictionary<int, Action>();
    public Action wave;
    private WaitForSeconds second = new WaitForSeconds(1);
    private int timeCount = 0;
    #endregion

    #region Methods
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
        monsterWave.Add(5, CirclePattern);
        monsterWave.Add(10, RectanglePattern);
        monsterWave.Add(15, CrowdPattern);
        StartCoroutine(WaveFlow());

        //Monster monster = SpawnMonster((MONSTER_TYPE)1);
    }
}
