using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMoveTo : AIState
{
    protected GameObject Target;

    public override void Perform()
    {
        MoveTo(Target.transform, myBrain.CharacterData.Speed, 1);
    }


    public void MoveTo(Transform target, float speed, float threshold)
    {
        GameObject body = myBrain.CharacterData.gameObject;
        Transform current = body.transform;

        if (Vector2.Distance(current.position, target.position) > threshold)
        {
            Rigidbody2D rigidBody = body.GetComponent<Rigidbody2D>();

            Vector2 move = (target.position - current.position);
            move = Vector2.ClampMagnitude(move, speed);

            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity + move, speed);
        }
    }

    public override void ChooseTarget()
    {
        // testing
        Target = GameObject.FindGameObjectWithTag("Player");
    }

    public override void OnEnter()
    {
        ChooseTarget();
    }
}
