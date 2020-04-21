using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMoveTo : AIStateTargettable
{
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
        List<GameObject> objs = new List<GameObject>();
        // find tags
        foreach (var target in myBrain.ObjectsInPerception)
        {
            if (TargetTags.Contains(target.tag))
            {
                objs.Add(target);
            }
        }

        if (Target == null || !objs.Contains(Target))
        {
            SetRandomTarget(objs);
        }
        else
        {
            Target = null;
        }
    }

    protected void SetRandomTarget(List<GameObject> objs)
    {
        if (objs.Count > 0)
        {
            Target = objs[Random.Range(0, objs.Count)];
        }
        else
        {
            Target = null;
        }
    }

    public override void OnEnter()
    {
        ChooseTarget();
    }
}
