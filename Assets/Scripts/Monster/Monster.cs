using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private string monsterName;
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int attackPower;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject candy;

    [SerializeField] private SpriteRenderer monsterRenderer;

    [SerializeField] private Player player;
    private Vector2 heading;
    private Vector2 direction;
    private float distance;

    private float elapsedTime = 0;
    private bool isChasingPlayer = false;
    private IEnumerator moveToPlayerCo = null;


    private void Start()
    {
        if (monsterRenderer == null)
            TryGetComponent(out monsterRenderer);

        if (player == null)
            player = FindObjectOfType<Player>();
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


    private IEnumerator MoveToPlayer()
    {
        isChasingPlayer = true;
        while (isChasingPlayer == true)
        {
            heading = player.transform.position - transform.position;
            distance = heading.magnitude;
            if (distance > 0)
            {
                direction = heading / distance;
                Debug.DrawRay(transform.position, direction, Color.red);
        
                if (direction.x > 0 && monsterRenderer.flipX == false)
                    monsterRenderer.flipX = true;
                else if (direction.x < 0 && monsterRenderer.flipX == true)
                    monsterRenderer.flipX = false;

                transform.Translate(direction * moveSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            if (distance < attackRange)
            {
                if (1 % elapsedTime == attackRate)
                    AttackPlayer();
                elapsedTime += Time.deltaTime;
            }
            else if (distance > attackRange && elapsedTime != 0)
                elapsedTime = 0;

            if (player.gameObject.activeSelf == false)
            {
                isChasingPlayer = false;
                break;
            }
        }
        yield return new WaitForEndOfFrame();
        isChasingPlayer = false;
    }
    private void AttackPlayer()
    {
        Debug.Log($"[Monster] {monsterName} attacked Player! Damage<{attackPower}>");
        player.Hp -= attackPower;
    }
    public void GetAttacked(int damage)
    {
        Debug.Log($"[Monster] {monsterName} got attacked. HP<{hp}> - Damage<{damage}>");
        hp -= damage;
        if (hp <= 0)
            MonsterDie();
    }
    private void MonsterDie()
    {
        Debug.Log($"[Monster] {monsterName} is dead.");
        if (moveToPlayerCo != null)
        {
            StopCoroutine(moveToPlayerCo);
            moveToPlayerCo = null;
        }
        DropCandy();
    }
    private void DropCandy()
    {
        Debug.Log($"[Monster] {monsterName} dropped Candy.");
        candy.SetActive(true);
        candy = null;
    }
    public void RecoverCandy(GameObject candy)
    {
        Debug.Log($"[Monster] {monsterName} recovered Candy.");
        this.candy = candy;
    }
}