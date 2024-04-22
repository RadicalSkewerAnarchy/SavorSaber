using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectOverTimeTesla : AreaEffectOverTime
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Effect()
    {
        base.Effect();

        bool killingBlow = false;
        foreach (CharacterData characterData in characterList)
        {
            if (characterData == null)
                continue;

            //test to see if this tic will inflict a killing blow
            //if (characterData.health - magnitude * damageModifier <= 0)
            //    killingBlow = true;

            killingBlow = characterData.DoDamage(magnitude * damageModifier, overCharged);

            //Debug.Log("Health reduced to " + characterData.health + " by DoT effect");

            if (killingBlow)
                continue;
        }
    }

    protected override void EffectVFX(Collider2D collision)
    {
        base.EffectVFX(collision);
        Instantiate(vfxTemplate, collision.gameObject.transform.position, Quaternion.identity);
        SFXPlayer.Play(SFX);
    }
}
