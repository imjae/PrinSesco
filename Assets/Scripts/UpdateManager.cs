using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public delegate void OnUpdate();
    public static OnUpdate onUpdate;

    public delegate void OnFixedUpdate();
    public static OnFixedUpdate onFixedUpdate;

    //private void Update()
    //{
    //    if (onUpdate != null)
    //        onUpdate();
    //}

    private void FixedUpdate()
    {
        if (onFixedUpdate != null)
            onFixedUpdate();
    }
}
