using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : DeBuffWeapon
{
    private void OnEnable()
    {
        att = 1;
        RandomSmoke();
    }
    void RandomSmoke()
    {
        /*
        Vector2 spacePos = Camera.main.WorldToScreenPoint(transform.position);
        float createPosx = Random.Range(0, Screen.width);
        float createPosy = Random.Range(Screen.height, 0);
        Vector2 thisTrans = Camera.main.ScreenToWorldPoint(new Vector2(createPosx, spacePos.y));
        */

        Vector3 spacePos = Camera.main.WorldToScreenPoint(transform.position);
        float createPosx = Random.Range(0, Screen.width);
        float createPosy = Random.Range(Screen.height, 0);
        // Vector3 thisTrans = Camera.main.ScreenToWorldPoint(new Vector3(createPosx, Screen.height, spacePos.z));
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(createPosx, createPosy, spacePos.z));
    }
}
