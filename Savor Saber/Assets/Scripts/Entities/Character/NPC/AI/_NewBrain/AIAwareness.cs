using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwareness
{
    private List<string> CanSee;

    public AIAwareness()
    {
        CanSee = new List<string>();
    }

    public AIAwareness(string see)
    {
        CanSee = new List<string>(){ see };
    }

    public AIAwareness(List<string> see)
    {
        CanSee = see;
    }

    /// <summary>
    /// contains default tags that most AI Agents need
    /// to be aware of
    /// </summary>
    /// <returns> basic AI Awareness </returns>
    static public AIAwareness DefaultAwareness()
    {
        List<string> defaulSeeable = new List<string> { "Player", "Prey" , "Predator", "SkewerableObject", "LargePlant", "ThrowThrough"};
        AIAwareness aware = new AIAwareness(defaulSeeable);
        return aware;
    }

    /// <summary>
    /// find all the objects that this awareness can see
    /// </summary>
    /// <returns> all seeable objects </returns>
    public List<GameObject> Perceive(GameObject observer, float radius)
    {
        List<GameObject> seeable = new List<GameObject>();

        var objects = Physics2D.OverlapCircleAll(observer.transform.position, radius);

        // check tags
        foreach (var found in objects)
        {
            if (CanSee.Contains(found.tag))
            {
                seeable.Add(found.gameObject);
            }
        }

        return seeable;
    }
}
