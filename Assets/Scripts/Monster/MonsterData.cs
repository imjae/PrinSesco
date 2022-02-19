using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private string monsterName;
    public string MonsterName => monsterName;

    [SerializeField]
    private int hp;
    public int HP => hp;

    [SerializeField]
    private int damage;
    public int Damage => damage;

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;

}
