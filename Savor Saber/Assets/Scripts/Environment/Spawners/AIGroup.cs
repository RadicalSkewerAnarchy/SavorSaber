
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGroup : MonoBehaviour
{
    public List<GameObject> Members
    {
        get
        {
            RemoveDeadObjects();
            return members;
        }
    }
    [SerializeField] private List<GameObject> members = new List<GameObject>();

    public void AddMember(GameObject member)
    {
        members.Add(member);
    }
    public void KickMember(GameObject member)
    {
        members.Remove(member);
    }
    public bool IsMember(GameObject member)
    {
        return members.Find((obj) => obj == member) != null;
    }
    public void RemoveDeadObjects()
    {
        members.RemoveAll((obj) => obj == null);
    }
}