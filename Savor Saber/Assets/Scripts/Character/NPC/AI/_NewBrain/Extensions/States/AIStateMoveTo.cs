using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMoveTo : AIState
{
    private GameObject Target;

    public override void Perform()
    {
        MoveTo(Target.transform, myBrain.CharacterData.Speed, 1);
    }


    public void MoveTo(Transform target, float speed, float threshold)
    {
        Transform current = myBrain.gameObject.transform;

        if (Vector2.Distance(current.position, target.position) > threshold)
        {
            Rigidbody2D body = myBrain.gameObject.GetComponent<Rigidbody2D>();

            Vector2 move = (target.position - current.position);
            move = Vector2.ClampMagnitude(move, speed);

            body.velocity = Vector2.ClampMagnitude(body.velocity + move, speed);
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
