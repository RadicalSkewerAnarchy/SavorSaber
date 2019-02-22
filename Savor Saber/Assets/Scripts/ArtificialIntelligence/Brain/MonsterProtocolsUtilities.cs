using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MonsterProtocols : MonoBehaviour
{
    // returns true if distance from pos to current is less than or equal to threshold
    private bool CheckThreshold(Vector2 pos, float threshold)
    {
        if(Vector2.Distance(pos, AiData.gameObject.transform.position) > threshold)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool EnemiesExist()
    {
        return false;
    }
    protected IEnumerator DecideLeader()
    {
        runningCoRoutine = true;
        //Debug.Log("In coroutine and running...");
        yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
        //sDebug.Log("... in coroutine and Done");
        Checks.BecomeLeader();
        yield return null;
        runningCoRoutine = false;
    }
}
