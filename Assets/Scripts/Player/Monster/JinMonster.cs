using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinMonster : MonoBehaviour
{
    public int hp = 10;
    public void Update()
    {
      if(hp <= 0)
        {
            Debug.Log("죽음");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
            other.GetComponent<Player>().Hp--;
    }
}
