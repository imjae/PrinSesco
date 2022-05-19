using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : ProjectionWeapons
{

    [SerializeField] private GameObject attackEffectObj;
    public override void LevelUp()
    {
        Debug.Log(GetType().Name + "레벨업~");
    }
    public override void Init()
    {
        ATTACK_Start += StartAttack;
    }
    public override void WeaponEffect()
    {

    }
    IEnumerator StartAttackCo_Handle = null;
    private void StartAttack()
    {
        if (StartAttackCo_Handle != null)
        {
            StopCoroutine(StartAttackCo_Handle);
            StartAttackCo_Handle = null;
        }
        StartCoroutine(StartAttackCo_Handle = StartAttackCo());
    }
    private IEnumerator StartAttackCo() // 발사 간격(공격 속도)
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            attackEffectObj.SetActive(true);
            boxCol.enabled = true;
            weaponAni.SetTrigger("isAttack");
            yield return new WaitForSeconds(0.4f);
            boxCol.enabled = false;
            attackEffectObj.SetActive(false);
            Debug.Log("ehf?");
        }
    }
}
