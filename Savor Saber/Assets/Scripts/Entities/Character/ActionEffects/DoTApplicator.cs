using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTApplicator : SkewerBonusEffect
{

    private WaitForSeconds Tic;
    private int numTics = 1;

    // Start is called before the first frame update
    void Start()
    {
        Tic = new WaitForSeconds(1);
        DamageOverTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DamageOverTime()
    {
        bool killingBlow = false;
        if (targetData == null)
            StartCoroutine(ExecuteAfterSeconds());

        //test to see if this tic will inflict a killing blow
        killingBlow = targetData.DoDamage(magnitude, true);
        //Debug.Log("Health reduced to " + characterData.health + " by DoT effect");

        //termination conditions
        if (killingBlow)
            Destroy(this.gameObject);
        numTics++;
        if (numTics > (magnitude * 5))
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
        StartCoroutine(ExecuteAfterSeconds());


    }

    private IEnumerator ExecuteAfterSeconds()
    {
        yield return Tic;
        DamageOverTime();
    }


}
