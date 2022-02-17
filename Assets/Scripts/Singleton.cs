using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(!_instance)
            {
                _instance = FindObjectOfType(typeof(T)) as T;

                if(_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = $"{typeof(T).ToString()}(Singleton)";
                }
            }

            return _instance;
        }
        set
        {
            if(_instance == null)
                _instance = value;
        }
    }
    
    public virtual void Awake()
    {
        Instance = this as T;
    }
}
