using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIBrain : MonoBehaviour
{

    // Protocols
    // Series of Behaviors
    public enum Protocol
    {
        Melee,
        Ranged,
        Lazy,
        Party,
        Feast,
        Console,
        Runaway,
        Conga,
        Chase,
        Wander,
        Ride,
        Scare,
        Dead,
        Ability,
        Overcharged,
        Pollinate,
        Attack
    }
    [SerializeField]
    private Protocol currentProtocol = Protocol.Lazy;
    private Protocol previousProtocol = Protocol.Lazy;

}
