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
    /// returns -1 if too close, +1 if too far, 0 if just right
    /// </summary>
    private int CheckRangedThreshold(Vector2 pos, float dist, float threshold)
    {
        var distance = Vector2.Distance(pos, AiData.gameObject.transform.position);
        if (distance < (dist - threshold))
        {
            return -1;
        }
        else if (distance > (dist + threshold))
        {
            return 1;
        }
        else return 0;
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
