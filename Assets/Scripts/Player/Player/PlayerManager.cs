using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player player;
    public GameObject[] weaponPrefabs;
    public GameObject playerLevelUp;
    [SerializeField] TextMeshProUGUI[] uiWeaponName = new TextMeshProUGUI[3];
    [SerializeField] Button[] selectWeapon = new Button[3];

    void Start()
    {
        weaponPrefabs = Resources.LoadAll<GameObject>("PlayerWeapon");
    }
    public void PlayerLevelUp()
    {
        playerLevelUp.SetActive(true);
        List<int> randWeapon = new List<int>();
        for (int i = 0; i < 3; i++)
            randWeapon.Add(Random.Range(0, weaponPrefabs.Length));
        bool overlap = OverLap(randWeapon);
        while (overlap)
        {
            randWeapon.Clear();
            for (int i = 0; i < weaponPrefabs.Length; i++)
                randWeapon.Add(Random.Range(0, weaponPrefabs.Length));
            overlap = OverLap(randWeapon);
        }
        for (int i = 0; i < 3; i++)
        {
            uiWeaponName[i].text = weaponPrefabs[randWeapon[i]].GetComponent<Weapons>().weaponName;
            Debug.Log(randWeapon[i]);
        }
        selectWeapon[0].onClick.AddListener(() => player.GetWeapon(randWeapon[0]));
        selectWeapon[1].onClick.AddListener(() => player.GetWeapon(randWeapon[1]));
        selectWeapon[2].onClick.AddListener(() => player.GetWeapon(randWeapon[2]));

    }
    bool OverLap(List<int> _weapons) // 중복 처리
    {
        int count = _weapons.Count - 1;
        for (int i = 0; i < count; i++)
        {
            if (_weapons[i] != _weapons[count - i] && i < count - i)
                return false;
        }
        return true;
    }
}
