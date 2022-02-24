using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    #region IDamageable Implementation
    public void GetHit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
    }
    #endregion

    #region Monster Variables
    public MonsterData monsterData;
    public SpriteRenderer monsterImage;
    private int hp;
    private WaitForSeconds attackRate;
    private IEnumerator attackCoroutine;

    public Transform player;
    private Vector2 heading;
    private Vector2 direction;
    private float distance;
    #endregion

    #region Monster Methods
    public void SetMonster()
    {
        hp = monsterData.MaxHP;
        attackRate = new WaitForSeconds(monsterData.AttackRate);
        attackCoroutine = Attack(player.GetComponent<TempPlayer>());
        UpdateManager.onFixedUpdate += MoveToPlayer;
    }
    public void MoveToPlayer()
    {
        heading = player.transform.position - transform.position;
        distance = heading.magnitude;
        direction = heading / distance;
        Debug.DrawRay(transform.position, direction, Color.red);

        transform.Translate(direction * monsterData.MoveSpeed * Time.deltaTime);
    }
    public IEnumerator Attack(TempPlayer player)
    {
        while (true)
        {
            //player.Hp -= monsterData.Damage;
            Debug.Log("Attacking");
            yield return attackRate;
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
        GameObject drop = Instantiate(monsterData.Drop);
        drop.transform.position = transform.position;
    }
    #endregion

    private void Start()
    {
        SetMonster();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<TempPlayer>() != null)
            StartCoroutine(attackCoroutine);
        Debug.Log("Attack Start");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<TempPlayer>() != null)
            StopCoroutine(attackCoroutine);
        Debug.Log("Attack Stop");
    }

}