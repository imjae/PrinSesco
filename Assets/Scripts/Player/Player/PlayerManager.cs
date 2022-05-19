using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PlayerWeapon { Whip, Somacho, HistoryBook, Wand, FireWand, Alcohol }

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player player;
    public GameObject[] weaponPrefabs;
    public GameObject playerLevelUpObj;
    [SerializeField] private TextMeshProUGUI[] uiWeaponName = new TextMeshProUGUI[3];
    [SerializeField] private Button[] selectWeapon = new Button[3];
    [SerializeField] private TextMeshProUGUI playerLevel_Txt;
    public TextMeshProUGUI[] currentEquipWeapon = new TextMeshProUGUI[6];

    void Start()
    {
        weaponPrefabs = Resources.LoadAll<GameObject>("PlayerWeapon");
    }
    private List<int> ReturnRandamNum(int _numbers, int _count)
    {
        bool[] checkOverlap = new bool[_numbers];
        for (int i = 0; i < checkOverlap.Length; i++)
            checkOverlap[i] = false;
        List<int> randomNums = new List<int>();
        int creatNum = 0;
        while (creatNum < _count)
        {
            int randomNum = UnityEngine.Random.Range(0, checkOverlap.Length);
            if (!checkOverlap[randomNum])
            {
                checkOverlap[randomNum] = true;
                randomNums.Add(randomNum);
                creatNum++;
            }
        }
        for (int i = 0; i < randomNums.Count; i++)
            Debug.Log((i + 1) + " 번째 랜덤 값 " + randomNums[i]);
        return randomNums;
    }
    public void PlayerLevelUp(bool isFirst = false)
    {
        playerLevel_Txt.text = "Player Level : "  + player.Level;
        List<int> randWeapon = ReturnRandamNum(weaponPrefabs.Length, 3);
        PlayerLevelUpUI();
        for (int i = 0; i < selectWeapon.Length; i++)
            selectWeapon[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weaponPrefabs[randWeapon[i]].name;
        if (!isFirst)
        {
            //수정 필요
            selectWeapon[0].onClick.RemoveAllListeners();
            selectWeapon[1].onClick.RemoveAllListeners();
            selectWeapon[2].onClick.RemoveAllListeners();
        }
        selectWeapon[0].onClick.AddListener(() => player.GetWeapon(randWeapon[0]));
        selectWeapon[1].onClick.AddListener(() => player.GetWeapon(randWeapon[1]));
        selectWeapon[2].onClick.AddListener(() => player.GetWeapon(randWeapon[2]));
    }

    public void PlayerLevelUpUI(bool _isActive = true)
    {
        if (_isActive)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        playerLevelUpObj.SetActive(_isActive);
    }
}
