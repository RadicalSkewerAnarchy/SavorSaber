using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Must have access to behaviors to operate
/// </summary>
[RequireComponent(typeof(MonsterBehavior))]
/// <summary>
/// not sure if this should be a hard requirement or not but I'll leave it here for now
/// </summary>
[RequireComponent(typeof(AIData))]
public class MonsterProtocols : MonoBehaviour
{
    MonoBehaviour Behaviour;
    private void Start()
    {
        Behaviour = GetComponent<MonsterBehavior>();
    }

    /// <summary>
    /// Every Behavior that is part of the protocol
    /// chain returns a boolean, 
    /// thus, they may be chained to create an order of operations
    /// Each protocol is of the format:
    ///     void X()
    ///     {
    ///         if (Behavior){
    ///             if (Behavior)
    ///             {
    ///                 ...
    ///             }
    ///         }
    ///     }
    /// </summary>

    void Melee()
    {
        
    }
    void Ranged()
    {

    }
    void Lazy()
    {

    }
    void Guard()
    {

    }
    void Party()
    {

    }
    void Swarm()
    {

    }
    void Feast()
    {

    }
    void Console()
    {

    }
}
