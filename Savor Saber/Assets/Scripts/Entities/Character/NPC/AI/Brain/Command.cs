using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    // S
    public GameObject Subject;
    // V
    public AIData.Protocols Verb;
    // O
    public GameObject Object;
    public Vector2 Location;

    // bools
    public bool issued = false;
    public bool inProgress = false;
    public bool complete = false;

    /// <summary>
    /// Basic command structure
    /// </summary>
    /// <param name="S">subject</param>
    /// <param name="V">verb</param>
    /// <param name="O">object</param>
    /// <param name="L">location</param>
    public Command(GameObject S, AIData.Protocols V, GameObject O, Vector2 L)
    {
        this.Subject = S;
        this.Verb = V;
        this.Object = O;
        this.Location = L;
    }

    /// <summary>
    /// Queue self to Subject
    /// </summary>
    public void QueueAction()
    {
        if (!issued)
        {
            AIData data = this.Subject.GetComponent<AIData>();
            data.EnqueueAction(this);
            issued = true;
        }
        else Debug.Log("this command was already issued ==> " + this.ToString());
    }

    /// <summary>
    /// Override old queue with new action for Subject
    /// </summary>
    public void OverrideAction()
    {
        AIData data = this.Subject.GetComponent<AIData>();
        data.ClearActionQueue();
        QueueAction();
    }

    /// <summary>
    /// isn't it obvious?
    /// </summary>
    /// <returns>isn't it obvious?</returns>
    public override string ToString()
    {
        return ("Command: " + Subject + " " + Verb + ", obj-- " + Object + ", loc-- " + Location);
    }
}
