using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuffWeapon : Weapons
{
    public override void LevelUp()
    {
        throw new System.NotImplementedException();
    }
    public override void WeaponEffect()
    {
        throw new System.NotImplementedException();
    }
    public IEnumerator DeBuffDamage(GameObject monster)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            monster.GetComponent<JinMonster>().hp -= att;
            Debug.Log(monster.GetComponent<JinMonster>().hp);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {
            StartCoroutine(DeBuffDamage(other.gameObject));
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            Debug.Log("나옴");
            StopAllCoroutines();
        }
    }
}
