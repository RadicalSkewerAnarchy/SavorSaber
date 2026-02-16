using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowApplicator : SkewerBonusEffect
{

    private WaitForSeconds Tic;
    private int numTics = 1;
    private float originalSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        Tic = new WaitForSeconds(1);

        if (targetData != null)
        {
            originalSpeed = targetData.Speed;
            targetData.Speed = 0;
        }
        Cooldown();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetData != null)
            transform.position = targetData.gameObject.transform.position;
    }
    public override void SetTarget(GameObject obj, int mag)
    {
        base.SetTarget(obj, mag);

        //overwrite any existing DoTs
        if (targetData.currentDot != null)
            Destroy(targetData.currentDot);

        targetData.currentDot = this.gameObject;

    }

    private void Cooldown()
    {
        if (targetData == null)
        {
            Debug.Log("Applicator targetData found to be null, terminating");
            Destroy(this.gameObject);
        }

        numTics++;
        if (numTics > (magnitude * 5))
        {
            StopAllCoroutines();
            targetData.Speed = originalSpeed;
            Destroy(this.gameObject);
        }
        StartCoroutine(ExecuteAfterSeconds());

    }

    private IEnumerator ExecuteAfterSeconds()
    {
        yield return Tic;
        Cooldown();
    }


}
