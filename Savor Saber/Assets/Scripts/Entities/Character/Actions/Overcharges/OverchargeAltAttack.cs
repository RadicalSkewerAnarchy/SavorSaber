using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargeAltAttack : Overcharge
{

    private MonsterBehavior behavior;
    private GameObject newAttackTemplateRanged;
    private GameObject newAttackTemplateMelee;
    private GameObject baseAttackMelee;
    private GameObject baseAttackRanged;

    // Start is called before the first frame update
    void Start()
    {
        behavior = GetComponent<MonsterBehavior>();
        baseAttackMelee = behavior.attack;
        baseAttackRanged = behavior.projectile;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        base.Activate();
        behavior.attack = newAttackTemplateMelee;
        behavior.projectile = newAttackTemplateRanged;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        behavior.attack = baseAttackMelee;
        behavior.projectile = baseAttackRanged;
    }
}
