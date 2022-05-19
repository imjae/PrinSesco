using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectPoolExtensions
{
    public static GameObjectPool<T> CreatePool<T>(this T poolRef,
                                                  string rootName,
                                                  Transform parent,
                                                  int size = 5,
                                                  bool autoPooling = true,
                                                  bool autoActivating = true) where T : Component
    {
        var poolRoot = new GameObject(rootName);
        var poolRootTransform = poolRoot.AddComponent<RectTransform>();
        poolRootTransform.sizeDelta = Vector3.zero;
        poolRootTransform.SetParent(parent, false);

        return new GameObjectPool<T>(poolRoot,
                                     size,
                                     () => Object.Instantiate(poolRef).GetComponent<T>(),
                                     autoPooling,
                                     autoActivating);
    }
}

public class GameObjectPool<T> where T : Component
{
    public delegate T CreateFunc();

    public int Size { get; private set; }

    public int IdleCount
    {
        get
        {
            return pool.Count;
        }
    }

    public int BusyCount
    {
        get
        {
            return Size - IdleCount;
        }
    }


    private CreateFunc create;
    private Stack<T> pool;
    private bool autoPooling;
    private GameObject root;
    private bool autoActivating;

    public GameObjectPool(GameObject root,
                          int size,
                          CreateFunc create,
                          bool autoPooling = true,
                          bool autoActivating = true)
    {
        this.root = root;
        Size = size;
        this.create = create;
        this.autoPooling = autoPooling;
        this.autoActivating = autoActivating;

        pool = new Stack<T>();

        Allocate(size);
    }

    private void Allocate(int size)
    {
        Size += size;

        for (int i = 0; i < size; ++i)
        {
            Return(create());
        }
    }

    public List<K> ToList<K>()
    {
        return pool.Select(item => item.GetComponent<K>()).ToList();
    }

    public T Get()
    {
        if (pool.Count <= 0)
        {
            if (autoPooling == true)
            {
                Allocate(1);
            }
            else
            {
                return null;
            }
        }

        T obj = pool.Pop();

        if (obj == null)
        {
            return Get();
        }

        if (autoActivating == true)
        {
            obj.gameObject.SetActive(true);
        }
        return obj;
    }

    public void Return(T obj)
    {
        if (obj.transform.parent == root.transform && obj.gameObject.activeInHierarchy == false)
        {
            return;
        }

        obj.transform.SetParent(root.transform, false);
        if (autoActivating == true)
        {
            obj.gameObject.SetActive(false);
        }

        pool.Push(obj);
    }

    public void Release()
    {
        while (pool.Count > 0)
        {
            T a = pool.Pop();
            if (a != null)
            {
                Object.Destroy(a.gameObject);
            }
        }
    }
}