using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MonsterProtocols : MonoBehaviour
{
    /// <summary>
    /// returns true if distance from pos to current is less than or equal to threshold
    /// </summary>
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
    /// <summary>
    /// empty for now but will be used in the future when Enemies exists
    /// </summary>
    /// <returns></returns>
    private bool EnemiesExist()
    {
        return false;
    }
    protected IEnumerator DecideLeader()
    {
        runningCoRoutine = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
        Checks.BecomeLeader();
        yield return null;
        runningCoRoutine = false;
    }
}
