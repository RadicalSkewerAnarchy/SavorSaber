using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectOverTimeHeal : AreaEffectOverTime
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

        foreach (CharacterData characterData in characterList)
        {
            if (characterData == null)
                continue;

            characterData.DoHeal(magnitude * damageModifier);

        }
    }

    protected override void EffectVFX(Collider2D collision)
    {
        base.EffectVFX(collision);
        Instantiate(vfxTemplate, collision.gameObject.transform.position, Quaternion.identity);
        SFXPlayer.Play(SFX);
    }
}
