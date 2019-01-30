using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeleeAttack))]
public class CharacterData : MonoBehaviour
{
    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    public int maxHealth;
    public int health;
    public float speed;

    //public MeleeAttack attack;
}
