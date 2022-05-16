using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private int hp;
    public int Hp
    {
        get
        {
            Debug.Log($"Getting HP : {hp}");
            return hp;
        }
        set
        {
            Debug.Log($"Setting HP : {hp}");
            hp = value;
        }
    }


    private int damage;
    public int Damage
    {
        get
        {
            Debug.Log($"Getting Damage : {damage}");
            return damage;
        }
        private set
        {
            Debug.Log($"Setting Damage : {damage}");
            damage = value;
        }
    }


    private float attackRate;
    public float AttackRate
    {
        get
        {
            Debug.Log($"Getting AttackRate : {attackRate}");
            return attackRate;
        }
        private set
        {
            Debug.Log($"Setting AttackRate : {attackRate}");
            attackRate = value;
        }
    }


    private float moveSpeed;
    public float MoveSpeed
    {
        get
        {
            Debug.Log($"Getting MoveSpeed : {moveSpeed}");
            return moveSpeed;
        }
        private set
        {
            Debug.Log($"Setting MoveSpeed : {moveSpeed}");
            moveSpeed = value;
        }
    }


    [SerializeField] private GameObject drop;
    public GameObject Drop
    {
        get
        {
            Debug.Log($"Getting Drop : {drop}");
            return drop;
        }
        set
        {
            Debug.Log($"Setting Drop : {drop}");
            drop = value;
        }
    }


    private SpriteRenderer monsterRenderer;

    private WaitForSeconds attackRateSeconds;
    private IEnumerator attackCoroutine;

    [SerializeField] private Player player;
    private Vector2 heading;
    private Vector2 direction;
    private float distance;



    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<Player>();
        if (monsterRenderer == null)
            TryGetComponent(out monsterRenderer);
        if (player != null)
            StartCoroutine(moveToPlayerCo = MoveToPlayer());
        Debug.Log($"[{gameObject.name}] Target Set : {player.name}.");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[{gameObject.name}] Attack Start.");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"[{gameObject.name}] Attack Stop.");
    }



    private bool isChasingPlayer = false;
    private IEnumerator moveToPlayerCo = null;
    private IEnumerator MoveToPlayer()
    {
        isChasingPlayer = true;
        while (isChasingPlayer)
        {
            heading = player.transform.position - transform.position;
            distance = heading.magnitude;
            direction = heading / distance;
            Debug.DrawRay(transform.position, direction, Color.red);
        
            if (direction.x > 0 && monsterRenderer.flipX == false)
                monsterRenderer.flipX = true;
            else if (direction.x < 0 && monsterRenderer.flipX == true)
                monsterRenderer.flipX = false;

            transform.Translate(direction * MoveSpeed * Time.deltaTime);

            if (player.gameObject.activeSelf == false)
            {
                isChasingPlayer = false;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        isChasingPlayer = false;
    }


    private void AttackPlayer()
    {
        player.Hp -= damage;
    }

    public void GetAttacked(int damage)
    {
        Hp -= damage;
    }

    private void MonsterDie()
    {

    }

    private void DropItem()
    {

    }
}