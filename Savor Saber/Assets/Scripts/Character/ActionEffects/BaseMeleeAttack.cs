using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class BaseMeleeAttack : MonoBehaviour
{

    public float meleeDamage;
    public bool ignoreIFrames = false;
    public GameObject myAttacker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attack trigger entered");
    }

}
