using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    enum MONSTER_TYPE
    {
        UNDEAD,
        JUNGLE,
        SEA,
        FOREST,
        FIELD,
        CAVE,
        DEVIL,
        ICE,
    }
    enum MONSTER_RANK
    {
        _01,
        _02,
        _03,
        _04,
        _05,
        _06,
        _07,
        _08,
        _09,
        _10,
        _11,
        _12
    }

    [SerializeField] private MONSTER_TYPE monsterType;
    public int MonsterType => (int)monsterType;

    [SerializeField] private MONSTER_RANK monsterRank;
    public int MonsterRank => (int)monsterRank;

    [SerializeField] private string monsterName;
    public string MonsterName => monsterName;
    
    [SerializeField] private int maxHP;
    public int MaxHP => maxHP;

    [SerializeField] private int hp;
    public int HP => hp;

    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private float attackRate;
    public float AttackRate => attackRate;

    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [SerializeField] private Sprite monsterImage;
    public Sprite MonsterImage => monsterImage;

    [SerializeField] private GameObject drop;
    public GameObject Drop => drop;



    [SerializeField] private SpriteRenderer monsterRenderer;

    private WaitForSeconds attackRateSeconds;
    private IEnumerator attackCoroutine;

    [SerializeField] private Transform player;
    private Vector2 heading;
    private Vector2 direction;
    private float distance;
    private IEnumerator moveToPlayer;



    private void Start()
    {
        try
        {
            if (player == null)
                player = FindObjectOfType<Player>().transform;
            if (monsterRenderer == null)
                TryGetComponent(out monsterRenderer);
            if (player != null)
                StartCoroutine(moveToPlayer = MoveToPlayer());
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            Debug.Log($"[{gameObject.name}] Target Set : {player.name}.");
        }
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
        bool isChasing = true;
        while (isChasing)
        {
            heading = player.position - transform.position;
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
                isChasing = false;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
}