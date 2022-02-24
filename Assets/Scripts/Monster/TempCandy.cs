using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCandy : MonoBehaviour
{
    public GameObject player;
    public void MoveToPlayer()
    {
        Vector3 heading = player.transform.position - transform.position;
        float distance = heading.magnitude;
        float acceleration = heading.sqrMagnitude;
        Vector3 direction = heading / distance;
        Debug.DrawRay(transform.position, direction, Color.red);

        transform.position += (direction*acceleration) * Time.deltaTime;
    }
    private void Start()
    {
        UpdateManager.onFixedUpdate += MoveToPlayer;
    }
}
