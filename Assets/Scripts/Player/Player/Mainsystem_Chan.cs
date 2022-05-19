using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Mainsystem_Chan : MonoBehaviour
{
    public const int PlayerWeaponCount = 10;

    private static Mainsystem_Chan instance = null;
    public static Mainsystem_Chan Instance
    {
        get
        {
            instance = FindObjectOfType(typeof(Mainsystem_Chan)) as Mainsystem_Chan;
            if (instance == null)
            {
                instance = new Mainsystem_Chan();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }
    public PlayerManager Player_Manager;

}
