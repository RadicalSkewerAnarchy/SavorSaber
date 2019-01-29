using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TestMove : MonoBehaviour
{
    public float translateSpeed = 0.25f;
    public Direction facing = Direction.East;
    public float speed = 4;

    private Vector3 direction;
    private SpriteRenderer spr;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.zero;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction.x = 1;
            spr.flipX = facing == Direction.West;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction.x = -1;
            spr.flipX = facing == Direction.East;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            direction.y = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            direction.y = -1;

        transform.Translate(direction.normalized * Time.deltaTime * speed);
    }
}
