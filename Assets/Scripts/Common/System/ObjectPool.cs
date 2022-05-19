using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    public int Size
    {
        get { return size; }
    }
    public int IdleCount
    {
        get
        {
            return pool.Count;
        }
    }

    private int size;
    public Func<T> create;
    private bool autoPooling;
    private Stack<T> pool;

    public ObjectPool(int size, Func<T> create, bool autoPooling = true)
    {
        this.size = 0;
        this.create = create;
        this.autoPooling = autoPooling;

        pool = new Stack<T>();

        Allocate(size);
    }

    private void Allocate(int size)
    {
        this.size += size;

        for (int i = 0; i < size; i++)
        {
            Return(create());
        }
    }

    public void Return(T obj)
    {
        pool.Push(obj);
    }

    public T Get()
    {
        if (pool.Count <= 0)
        {
            if (autoPooling == false)
            {
                return default(T);
            }

            Allocate(1);
        }

        return pool.Pop();
    }
}