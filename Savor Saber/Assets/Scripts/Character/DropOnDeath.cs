using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All this component does is instantiate all of its prefabs when called
/// </summary>
public class DropOnDeath : MonoBehaviour
{
    /// <summary> The objects to be instatiated when Drop() is called (should be prefabs) </summary>
    public GameObject[] drops;
    /// <summary> Instantiates all prefabs in drops[] </summary>
    public void Drop()
    {
        foreach (var obj in drops)
        {
            var d = GameObject.Instantiate(obj);
            d.transform.position = transform.position;
        }

    }
}
