using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionHealth : AIDecision
{
    [SerializeField]
    [Range(0f, 2f)]
    private float HealthPercent = 0.5f;

    [SerializeField]
    private bool GreaterThan = true; 

    public override bool Evaluate()
    {
        float hp = myBrain.CharacterData.health;
        float mxhp = myBrain.CharacterData.maxHealth;
        float normhp = hp / mxhp;

        if (GreaterThan)
        {
            return normhp > HealthPercent;
        }
        else
        {
            return normhp < HealthPercent;
        }
    }

    public override void Initialize()
    {
        
    }
}
