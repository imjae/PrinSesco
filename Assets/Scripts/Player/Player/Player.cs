using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("PlayerInfo")]
    [SerializeField] private float speed;
    [SerializeField] private float maxHp;
    private float hp;
    [SerializeField] private float level;
    [SerializeField] private float exp;
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
            Mainsystem_Chan.Instance.Player_Manager.PlayerLevelUp(level < 2);
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
    private SpriteRenderer spriteRenderer;
    private Animator ani;
    private GameObject whip;

    [SerializeField] private Transform equipWeaponPos = null;
    public List<Weapons> equipWeaponsObj = new List<Weapons>();
    [SerializeField] private Image playerUIHp = null;

    void Awake()
    {
        hp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        whip = transform.GetChild(0).gameObject;
        //Temp Code

    }
    private void Update()
    {
        PlayerMove();
    }
    private void PlayerMove()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime * 5,
                                        Input.GetAxis("Vertical") * speed * Time.deltaTime * 5);
        transform.position = new Vector2(transform.position.x + moveInput.x, transform.position.y+ moveInput.y);
        bool isWalk = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W);
        //bool isAttack = whip.activeSelf;
        ani.SetBool("isWalk", isWalk);
        if (Input.GetKeyDown(KeyCode.D)) // Horizontal의 Velocity를 받아서 구현해도 됨.
            reversal = true;
        if (Input.GetKeyDown(KeyCode.A))
            reversal = false;
        //if(reversal && !isAttack) // 이 부분은 무기 클래스에 들어갈 수도 있음
        //    whip.transform.localPosition = new Vector2(0.4f, whip.transform.localPosition.y);
        //else if(!isAttack)
        //    whip.transform.localPosition = new Vector2(-0.4f, whip.transform.localPosition.y);
        spriteRenderer.flipX = reversal; // 캐릭터 반전
    }
    public void GetWeapon(int weaponIndex) // 무기 획득
    {
        Time.timeScale = 1;
        PlayerManager playerManager = Mainsystem_Chan.Instance.Player_Manager;
        playerManager.PlayerLevelUpUI(false);
        for (int i = 0; i <equipWeaponsObj.Count; i++)
        {
            if (equipWeaponsObj[i].GetType() == Mainsystem_Chan.Instance.Player_Manager.weaponPrefabs[weaponIndex].GetComponent<Weapons>().GetType())
            {
                equipWeaponsObj[i].Level++;
                ShowEquipWeapon();
                return;
            }
        }
        equipWeaponsObj.Add(Mainsystem_Chan.Instance.Player_Manager.weaponPrefabs[weaponIndex].GetComponent<Weapons>());
        equipWeaponsObj[equipWeaponsObj.Count - 1].Init();

        Instantiate(Mainsystem_Chan.Instance.Player_Manager.weaponPrefabs[weaponIndex], equipWeaponPos);
        ShowEquipWeapon();
    }

    private void ShowEquipWeapon()
    {
        for (int i = 0; i < equipWeaponsObj.Count; i++)
        {
            Mainsystem_Chan.Instance.Player_Manager.currentEquipWeapon[i].text = equipWeaponsObj[i].GetType().Name + "\n Level : " + equipWeaponPos.GetChild(i).GetComponent<Weapons>().level;
        }
    }

    //진영 : 추가 코드 입니다
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Candy")
        {
            Destroy(other.gameObject);
            Exp += other.GetComponent<Candy>().exp;
        }
    }
}
