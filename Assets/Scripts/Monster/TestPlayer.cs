using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public static TestPlayer Inst;
    public int Hp;
    public float speed;

    private float inputX;
    private float inputY;
    private Vector2 direction;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        direction.x = inputX;
        direction.y = inputY;
        direction.Normalize();
        Debug.DrawRay(transform.position, direction, Color.cyan, 0.02f);

        transform.Translate(direction * speed * Time.deltaTime);
    }
}
