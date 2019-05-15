using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurryBallProjectile : BaseProjectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        DestructableEnvironment envData = collision.GetComponent<DestructableEnvironment>();
        bool destroyed = true;
        if (envData != null)
            destroyed = envData.destroyed;
        base.OnTriggerEnter2D(collision);
        if(envData != null && !destroyed)
        {
            if(envData.destroyed)
            {
                string value = FlagManager.GetFlag("EnvDestroyedByFire");
                if (value == "Done")
                    return;
                if (value == FlagManager.undefined)
                {
                    FlagManager.SetFlag("EnvDestroyedByFire", "9");
                    if (QuestManager.instance.GetText().Contains("Burn"))
                        QuestManager.instance.SetText(QuestManager.instance.GetText().Replace("10", "9"));
                }                    
                else
                {
                    int numLeft = int.Parse(value);
                    if(numLeft <= 1)
                    {
                        FlagManager.SetFlag("EnvDestroyedByFire", "Done");
                        if (QuestManager.instance.GetText().Contains("Burn"))
                            QuestManager.instance.SetText("Talk to Amrita");
                    }          
                    else
                    {
                        FlagManager.SetFlag("EnvDestroyedByFire", (numLeft - 1).ToString());
                        if (QuestManager.instance.GetText().Contains("Burn"))
                            QuestManager.instance.SetText(QuestManager.instance.GetText().Replace(numLeft.ToString(), (numLeft - 1).ToString()));
                    }                     
                }                  
            }               
        }
    }
}
