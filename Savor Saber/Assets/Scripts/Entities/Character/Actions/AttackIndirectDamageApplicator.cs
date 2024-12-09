using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndirectDamageApplicator : MonoBehaviour
{
    [HideInInspector]
    public CharacterData targetData;
    [HideInInspector]
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoDamage()
    {
        targetData.DoDamage(damage);
        Debug.Log("Indirect attack doing damage...");
        return;
    }
}
