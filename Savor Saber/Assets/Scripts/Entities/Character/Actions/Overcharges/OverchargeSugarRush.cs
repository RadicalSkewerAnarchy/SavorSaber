using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIData))]
public class OverchargeSugarRush : MonoBehaviour
{

    private AIData characterData;

    [HideInInspector]
    public int sugarCount = 0;
    [HideInInspector]
    public bool rushed = false;

    // Start is called before the first frame update
    void Start()
    {
        characterData = GetComponent<AIData>();
    }


    public void SugarStack(int amount)
    {
        if (rushed)
        {
            sugarCount += amount;
        }
        else
        {
            rushed = true;
            sugarCount += amount;
            StartCoroutine(SugarRush(3));
        }
    }

    public IEnumerator SugarRush(float time)
    {
        //things to happen before delay
        float baseSpeed = characterData.Speed;
        float baseAttackDuration = characterData.Behavior.attackDuration;
        float baseCoolDown = characterData.Behavior.attackCooldown;
        characterData.Speed = baseSpeed * 2;
        characterData.Behavior.attackCooldown = baseCoolDown / 2;
        characterData.Behavior.attackDuration = baseAttackDuration / 2;
        while (sugarCount > 0)
        {
            yield return new WaitForSeconds(time);
            sugarCount--;
        }
        //things to happen after delay
        characterData.Speed = baseSpeed;
        characterData.Behavior.attackCooldown = baseCoolDown;
        characterData.Behavior.attackDuration = baseAttackDuration;
        rushed = false;
        yield return null;
    }
}
