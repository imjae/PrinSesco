using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // T로 설정되는 타입의 인스턴스가 유지됨.
    static T _instance;

    public static T Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(T)) as T;

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = $"{typeof(T).ToString()}(Singleton)";
                } 
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
            _instance = this as T;
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
            Destroy(gameObject); 
    }
}