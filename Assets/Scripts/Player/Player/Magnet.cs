using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] float magnetPower = 5f; // 자석의 세기
    [SerializeField] float distanceStretch = 10f; // 거리에 따른 자석 효과
    [SerializeField] int magnetDirection = 1; // 인력 1, 척력 -1

    private Transform thisTrans;
    private Transform candyTrans;
    private Rigidbody2D candyRigidbody2;
    //private bool magnetZone;

    private void Start()
    {
        thisTrans = gameObject.transform;
    }
    private void GetCandy(Rigidbody2D candyRi)
    {
        Vector2 directionToMagnet = thisTrans.position - candyTrans.position; // Player으로 향하는 벡터 설정
        float distance = Vector2.Distance(candyTrans.position, thisTrans.position); // distance 로 a,b사이의 거리를 구함
        float magnetDistanceStr = (distanceStretch / distance) * magnetPower; // 거리에 따른 힘이 달라져야 하니 거리로 나눔
        candyRi.AddForce(magnetDistanceStr * (directionToMagnet * magnetDirection), ForceMode2D.Force); // 힘의 크기와 방향이 있으니 물리적 힘구현
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Candy")
        {
            candyRigidbody2 = other.GetComponent<Rigidbody2D>();
            candyTrans = other.transform;
            GetCandy(candyRigidbody2);
            //magnetZone = thisTrans;
            //if (magnetZone)
            //{
            //    GetCandy(candyRigidbody2);
            //}
        }
    }
}
