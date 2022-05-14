using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSpin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.rotation = Quaternion.EulerAngles(0f, 0f, 10f);
    }
}
