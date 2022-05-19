using UnityEngine;
using System.Collections.Generic;

public class GameObjectPoolGroup<T> where T : MonoBehaviour
{
    private Dictionary<string, GameObjectPool<T>> poolDic;
    private GameObject root;

    public GameObjectPoolGroup(GameObject root)
    {
        this.root = root;
        poolDic = new Dictionary<string, GameObjectPool<T>>();
    }

    public void Add(string id, T referenceObj, int size = 5, bool autoPooling = true)
    {
        if (Contains(id))
        {
            return;
        }

        GameObjectPool<T> op = new GameObjectPool<T>(root
        , size
        , () =>
        {
            return GameObject.Instantiate<T>(referenceObj);
        }
        , autoPooling);

        poolDic.Add(id, op);
    }

    public void Remove(string id)
    {
        if (Contains(id) == false)
        {
            return;
        }

        GameObjectPool<T> op = poolDic[id];
        poolDic.Remove(id);

        op.Release();
        op = null;
    }

    public int Count(string id)
    {
        if (Contains(id) == false)
        {
            return 0;
        }

        return poolDic[id].Size;
    }

    public T Get(string id)
    {
        if (Contains(id) == false)
        {
            return null;
        }

        GameObjectPool<T> op = poolDic[id];
        return op.Get();
    }

    public bool Return(string id, T obj)
    {
        if (Contains(id) == false)
        {
            return false;
        }

        GameObjectPool<T> op = poolDic[id];
        op.Return(obj);

        return true;
    }

    public bool Contains(string id)
    {
        return poolDic.ContainsKey(id);
    }

    public void Release()
    {
        foreach (var item in poolDic)
        {
            item.Value.Release();
        }
        poolDic.Clear();
    }
}