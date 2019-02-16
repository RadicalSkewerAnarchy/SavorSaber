using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class EventMoveCharacter : EventScript
{
    public GameObject character;
    public Transform destination;
    public FloatRange xLeniency = new FloatRange(-0.5f, 0.5f);
    public FloatRange yLeniency = new FloatRange(-0.5f, 0.5f);
    public bool setFacing = true;
    public float speed;
    public string animationBoolName = "none";
    public Direction endFacing;

    public override IEnumerator PlayEvent(GameObject player)
    {
        var c = character.transform;
        xLeniency.Shift(destination.position.x);
        yLeniency.Shift(destination.position.y);
        var animator = character.GetComponent<Animator>();
        var controller = character.GetComponent<EntityController>();
        if(animationBoolName != "none" && !string.IsNullOrWhiteSpace(animationBoolName))
            animator.SetBool(animationBoolName, true);
        while (!xLeniency.Contains(c.position.x))
        {
            yield return new WaitForFixedUpdate();
            float lastX = c.position.x;       
            bool negative = c.position.x > xLeniency.max;
            float dist = speed * Time.fixedDeltaTime * (negative ? -1 : 1);
            controller.Direction = negative ? Direction.West : Direction.East;
            if (Mathf.Abs(dist) > Mathf.Abs(destination.position.x - c.position.x))
            {
                c.position = destination.position;
                break;
            }
            else
                c.Translate(dist,0,0);
            if (lastX == c.position.x)
                yield break;
        }
        while (!yLeniency.Contains(c.position.y))
        {
            yield return new WaitForFixedUpdate();
            float lastY = c.position.y;
            bool negative = c.position.y > yLeniency.max;
            float dist = speed * Time.fixedDeltaTime * (negative ? -1 : 1);
            controller.Direction = negative ? Direction.South : Direction.North;
            if (Mathf.Abs(dist) > Mathf.Abs(destination.position.y - c.position.y))
            { 
                c.position = destination.position;
                break;
            }
            else
                c.Translate(0, dist, 0);
            if (lastY == c.position.y)
                yield break;
        }
        if (animationBoolName != "none" && !string.IsNullOrWhiteSpace(animationBoolName))
            animator.SetBool(animationBoolName, false);
        controller.Direction = endFacing;
    }

}
