using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All this component does is instantiate all of its prefabs when called
/// </summary>
public class DropOnDeath : MonoBehaviour
{
    [Range(0, 100)]
    public int chance;
    /// <summary> The objects to be instatiated when Drop() is called (should be prefabs). </summary>
    public GameObject[] drops;
    /// <summary> The signals to be dropped (will always drop) </summary>
    public GameObject[] signalDrops;
    /// <summary> Instantiates all prefabs in drops[] </summary>
    public void Drop()
    {
        float thresh = (float)chance / 100;
        foreach (var obj in drops)
        {
            if(Random.value < thresh)
            {
                var d = GameObject.Instantiate(obj);
                d.transform.position = transform.position;
                d.GetComponent<SkewerableObject>().currentTile = GetComponent<MonsterChecks>().currentTile;
            }
        }
        foreach(var obj in signalDrops)
        {
            var d = GameObject.Instantiate(obj);
            d.transform.position = transform.position;
        }
        //Debug.Log(this.gameObject.name + " has ****DIED****");
    }
}
