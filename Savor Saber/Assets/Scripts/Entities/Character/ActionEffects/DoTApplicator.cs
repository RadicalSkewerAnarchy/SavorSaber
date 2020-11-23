using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTApplicator : SkewerBonusEffect
{

    private WaitForSeconds Tic;

    // Start is called before the first frame update
    void Start()
    {
        Tic = new WaitForSeconds(1);
        StartCoroutine(DamageLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DamageLoop()
    {
        if (target != null && targetData.health > 0)
        {
            yield return Tic;
            targetData.DoDamage(magnitude, true);
        }
        else yield return null;
    }


}
