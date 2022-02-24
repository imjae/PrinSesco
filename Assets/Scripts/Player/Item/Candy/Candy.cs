using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public float magnetPower = 0.5f; // 자석의 세기
    public int exp;
    public bool magnetZone = false;

    public Transform playerTrans;
    public Vector2 vel = Vector2.zero;

    public void FixedUpdate()
    {
        if(magnetZone) transform.position = Vector2.SmoothDamp(gameObject.transform.position, playerTrans.position, ref vel, magnetPower);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Magnet")
        {
            playerTrans = other.GetComponentInParent<Transform>();
            magnetZone = true;
        }
    }
}
