using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    #region Monster Variables and Methods
    public MonsterData monsterData;
    private int hp;
    private GameObject drop;

    public Transform player;
    private Vector3 direction;

    public void SetMonster()
    {
        hp = monsterData.MaxHP;
        drop = Instantiate(monsterData.Drop);
        drop.SetActive(false);
    }
    public void MoveToPlayer()
    {
        direction = player.transform.position - transform.position;
        transform.Translate(direction * monsterData.MoveSpeed * Time.deltaTime);
    }
    public void GetHit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
        drop.SetActive(true);
        drop.transform.position = transform.position;
    }
    #endregion

    private void OnEnable()
    {
        SetMonster();
    }
}