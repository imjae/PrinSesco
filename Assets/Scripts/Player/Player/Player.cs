using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerInfo")]
    [SerializeField] float speed;
    [SerializeField] float hp;
    [SerializeField] float level;
    [SerializeField] float exp;
    public float Hp
    {
        get { return hp; }
        set
        {
            hp = value;
        }
    }
    public float Level
    {
        get { return level; }
        set
        {
            level = value;
        }
    }
    public float Exp
    {
        get { return exp; }
        set
        {
            exp = value;
        }
    }
    public bool reversal = false; // 반전처리
    SpriteRenderer spriteRenderer;
    Animator ani;
    GameObject whip;

    [SerializeField] PlayerUIManager playerUIManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        whip = transform.GetChild(0).gameObject;
        //StartCoroutine(AttackCo());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            transform.GetChild(0).gameObject.SetActive(true);
        PlayerMove();
    }

    //IEnumerator AttackCo() // 자동으로 공격함(TEST)
    //{
    //    while(true)
    //    {
    //        yield return new WaitForSeconds(2);
    //        whip.SetActive(true);
    //        yield return new WaitForSeconds(0.5f);
    //        whip.SetActive(false);
    //    }
    //}
    void PlayerMove()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime * 5,
                                        Input.GetAxis("Vertical") * speed * Time.deltaTime * 5);
        //transform.position = new Vector2(transform.position.x + moveInput.x, transform.position.y+ moveInput.y);
        transform.Translate(new Vector2(moveInput.x, moveInput.y));
        //bool isWalk = Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0;
        bool isWalk = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W);
        bool isAttack = whip.activeSelf;
        ani.SetBool("isWalk", isWalk);
        if (Input.GetKeyDown(KeyCode.D)) // Horizontal의 Velocity를 받아서 구현해도 됨.
            reversal = true;
        if (Input.GetKeyDown(KeyCode.A))
            reversal = false;
        if(reversal && !isAttack) // 이 부분은 무기 클래스에 들어갈 수도 있음
            whip.transform.localPosition = new Vector2(0.4f, whip.transform.localPosition.y);
        else if(!isAttack)
            whip.transform.localPosition = new Vector2(-0.4f, whip.transform.localPosition.y);
        spriteRenderer.flipX = reversal; // 캐릭터 반전
    }
    //진영 : 추가 코드 입니다
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Candy")
        {
            Destroy(other.gameObject);
        }
    }
}
