using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
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

    public void SetMonster()
    {
        hp = monsterData.MaxHP;
        attackRate = new WaitForSeconds(monsterData.AttackRate);

        UpdateManager.onFixedUpdate += MoveToPlayer;
    }
    public void MoveToPlayer()
    {
        heading = player.transform.position - transform.position;
        distance = heading.magnitude;
        direction = heading / distance;
        if (direction.x > 0 && !monsterImage.flipX)
            monsterImage.flipX = true;
        else if (direction.x < 0 && monsterImage.flipX)
            monsterImage.flipX = false;
        Debug.DrawRay(transform.position, direction, Color.red);

        transform.Translate(direction * monsterData.MoveSpeed * Time.deltaTime);
    }
    public IEnumerator Attack()
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

    private void Start()
    {
        SetMonster();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Attack Start");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Attack Stop");
    }
}