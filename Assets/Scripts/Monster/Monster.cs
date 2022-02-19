using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    public void PrintMonsterData()
    {
        Debug.Log(monsterData.MonsterName);
        Debug.Log(monsterData.HP);
        Debug.Log(monsterData.Damage);
        Debug.Log(monsterData.MoveSpeed);
        Debug.Log("----------------------");
    }
}