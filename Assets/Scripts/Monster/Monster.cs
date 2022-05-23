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

    private SpriteRenderer monsterRenderer;

    private TestPlayer player;
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

        // 플레이어 스크립트에 따라 수정 필요
        if (player == null)
            player = TestPlayer.Inst;
        if (player != null)
            StartCoroutine(moveToPlayerCo = MoveToPlayer());
        Debug.Log($"[{monsterName}] Target Set : {player.name}.");
    }


    private IEnumerator MoveToPlayer()
    {
        isChasingPlayer = true;
        while (isChasingPlayer == true)
        {
            heading = player.transform.position - transform.position;
            distance = heading.magnitude;
            //Debug.Log($"[Monster] Distance : {distance}");
            if (distance > 0.01f)
            {
                direction = heading / distance;
                Debug.DrawRay(transform.position, direction, Color.red, 0.02f);
        
                if (direction.x > 0 && monsterRenderer.flipX == false)
                    monsterRenderer.flipX = true;
                else if (direction.x < 0 && monsterRenderer.flipX == true)
                    monsterRenderer.flipX = false;

                transform.Translate(direction * moveSpeed * Time.deltaTime);
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
            yield return new WaitForEndOfFrame();
        }
        isChasingPlayer = false;
    }
    private void AttackPlayer()
    {
        Debug.Log($"[Monster] {monsterName} attacked Player! Damage<{attackPower}>");
        player.Hp -= attackPower;
        if (player.Hp <= 0)
        {
            if (moveToPlayerCo != null)
            {
                StopCoroutine(moveToPlayerCo);
                moveToPlayerCo = null;
            }
        }
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