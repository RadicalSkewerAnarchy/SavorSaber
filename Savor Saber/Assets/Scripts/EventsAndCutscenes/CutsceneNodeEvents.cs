using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameflow;
using SerializableCollections;

public static class CutsceneNodeEvents
{
    public static IEnumerator PanCamera(PanCameraNode node, GameObject player, EventGraph.ActorDict actors, GameObjectDict dependencies)
    {
        
        var pNode = node as PanCameraNode;
        var target = actors.ContainsKey(pNode.target) ? actors[pNode.target].gameObject : dependencies[pNode.target];
        var cam = player.GetComponent<CameraController>();
        Debug.Log("Panning Camera to " + target);
        switch (pNode.movementType)
        {
            case PanCameraNode.MoveType.Smoothed:
                yield return cam.StartCoroutine(cam.MoveToPointSmoothCr(target.transform.position, pNode.maxPullSpeed, pNode.snapTime));
                break;
            case PanCameraNode.MoveType.Linear:
                yield return cam.StartCoroutine(cam.MoveToPointLinearCr(target.transform.position, pNode.maxPullSpeed / 100));
                break;
        }
    }

    public static IEnumerator MoveCharacter(MoveCharacterNode node, EventGraph.ActorDict actors, GameObjectDict dependencies)
    {
        var character = actors.ContainsKey(node.character) ? actors[node.character].gameObject : dependencies[node.character];
        var characterTr = character.transform;
        var destinationTr = actors.ContainsKey(node.destination) ? actors[node.destination].transform : dependencies[node.destination].transform;
        var xLeniency = node.xLeniency.Shifted(destinationTr.position.x);
        var yLeniency = node.yLeniency.Shifted(destinationTr.position.y);
        var animator = character.GetComponent<Animator>();
        var controller = character.GetComponent<EntityController>();
        if (animator != null)
            animator.SetBool("Moving", true);
        while (!xLeniency.Contains(characterTr.position.x))
        {
            yield return new WaitForFixedUpdate();
            float lastX = characterTr.position.x;
            bool negative = characterTr.position.x > xLeniency.max;
            float dist = node.speed * Time.fixedDeltaTime * (negative ? -1 : 1);
            if(controller != null)
                controller.Direction = negative ? Direction.West : Direction.East;
            if (Mathf.Abs(dist) > Mathf.Abs(destinationTr.position.x - characterTr.position.x))
            {
                characterTr.position = destinationTr.position;
                break;
            }
            else
                characterTr.Translate(dist, 0, 0);
            if (lastX == characterTr.position.x)
                yield break;
        }
        while (!yLeniency.Contains(characterTr.position.y))
        {
            yield return new WaitForFixedUpdate();
            float lastY = characterTr.position.y;
            bool negative = characterTr.position.y > yLeniency.max;
            float dist = node.speed * Time.fixedDeltaTime * (negative ? -1 : 1);
            if (controller != null)
                controller.Direction = negative ? Direction.South : Direction.North;
            if (Mathf.Abs(dist) > Mathf.Abs(destinationTr.position.y - characterTr.position.y))
            {
                characterTr.position = destinationTr.position;
                break;
            }
            else
                characterTr.Translate(0, dist, 0);
            if (lastY == characterTr.position.y)
                yield break;
        }
        if (animator != null)
            animator.SetBool("Moving", false);
        if (controller != null)
            controller.Direction = node.endFacing;
    }

    public static IEnumerator SetCharacterDirection(SetCharacterDirectionNode node, EventGraph.ActorDict actors, GameObjectDict dependencies)
    {
        var character = actors.ContainsKey(node.character) ? actors[node.character].gameObject : dependencies[node.character];
        var controller = character.GetComponent<EntityController>();
        controller.Direction = node.facing;
        yield return null;
    }

    public static IEnumerator Wait(WaitNode node)
    {
        yield return new WaitForSeconds(node.waitTime);
    }
}
