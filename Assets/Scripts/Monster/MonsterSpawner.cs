using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefab;
    [SerializeField] private TestPlayer player;

    [SerializeField] private GameObject test;

    private void Awake()
    {
        // 플레이어 스크립트에 따라 수정 필요
        if (player == null)
            player = TestPlayer.Inst;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnRandomMonster();
    }

    private void SpawnRandomMonster()
    {
        Vector3 position = RandomScreenPoint();

        Debug.Log($"[Monster Spawner] Relative Position : {position - player.transform.position}");

        Instantiate(prefab[Random.Range(0, prefab.Length)], position, Quaternion.identity);
    }
    private Vector3 RandomScreenPoint()
    {
        Vector3 position = new Vector3();
        int randomSelect = Random.Range(0, 4); // 0~3
        switch (randomSelect)
        {
            case 0:
                position.Set(Screen.width, Screen.height, 0f);
                break;
            case 1:
                position.Set(Screen.width, 0f, 0f);
                break;
            case 2:
                position.Set(0f, Screen.height, 0f);
                break;
            case 3:
                position.Set(0f, 0f, 0f);
                break;
            default:
                break;
        }
        position = Camera.main.ScreenToWorldPoint(position);
        position.x += Random.Range(-1f, 1f);
        position.y += Random.Range(-1f, 1f);
        position.z = 0;
        return position;
    }
}
