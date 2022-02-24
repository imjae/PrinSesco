using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionWeapons : Weapons
{
    [SerializeField] protected float coolTime;
    [SerializeField] protected float proSpeed;
    //[SerializeField] protected float proDamage;
    [SerializeField] protected float proRange;
    //[SerializeField] protected float proDuration;

    protected Animator weaponAni;
    public BoxCollider2D boxCol;

    private void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        weaponAni = GetComponent<Animator>();
        //StartCoroutine(LaunchInterval());
    }

    public override void LevelUp()
    {
        throw new System.NotImplementedException(); // 구현 되지 않은 메서드에 대해 이 예외를 throw
    }
    public override void WeaponEffect()
    {
        throw new System.NotImplementedException(); // 구현 되지 않은 메서드에 대해 이 예외를 throw
    }
    protected IEnumerator LaunchInterval() // 발사 간격(공격 속도)
    {
        while (true)
        {
            boxCol.enabled = true;
            weaponAni.SetTrigger("isAttack");
            yield return new WaitForSeconds(coolTime);
            boxCol.enabled = false;
        }
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
            other.GetComponent<Monster>().Hp -= att;
    }


}
