using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerInfo")]
    [SerializeField] float speed;
    [SerializeField] float maxHp;
    public float hp;
    [SerializeField] float level;
    [SerializeField] float exp;
    public float Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            playerUIHp.fillAmount = hp / maxHp;
            if (hp > maxHp / 2)
                playerUIHp.color = Color.green;
            else
                playerUIHp.color = Color.red;
        }
    }
    public float Level
    {
        get { return level; }
        set
        {
            level = value;
            playerManager.PlayerLevelUp();
        }
    }
    public float Exp
    {
        get { return exp; }
        set
        {
            exp = value;
            if (exp > 0)
                Level++;
        }
    }
    public bool reversal = false; // 반전처리
    SpriteRenderer spriteRenderer;
    Animator ani;
    GameObject whip;

    [SerializeField] Transform equipWeaponPos;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] Image playerUIHp;

    void Awake()
    {
        hp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        whip = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        PlayerMove();
    }
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

    public void GetWeapon(int weaponIndex) // 무기 획득
    {
        for (int i = 0; i < equipWeaponPos.childCount; i++)
        {
            if (equipWeaponPos.GetChild(i).GetComponent<Weapons>().GetType().Name ==
                playerManager.weaponPrefabs[weaponIndex].GetComponent<Weapons>().GetType().Name)
            {
                equipWeaponPos.GetChild(i).GetComponent<Weapons>().level++;
                break;
            }
            if (equipWeaponPos.GetChild(i).GetComponent<Weapons>().GetType().Name !=
                playerManager.weaponPrefabs[weaponIndex].GetComponent<Weapons>().GetType().Name)
            {
                Instantiate(playerManager.weaponPrefabs[weaponIndex], equipWeaponPos);
                break;
            }
        }
        playerManager.playerLevelUp.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Candy")
        {
            Destroy(other.gameObject);
            Exp++;
        }
    }
}
