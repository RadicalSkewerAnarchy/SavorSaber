using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAIAfterDelay : MonoBehaviour
{
    public float initialDelay = 1;
    public float delayBetween = 0.25f;
    public AIData[] arr1;
    public AIData[] arr2;
    
    public void EnableAfterDelay()
    {
        StartCoroutine(EnableAfterDelayCR());
    }

    private IEnumerator EnableAfterDelayCR()
    {
        int minLength = Mathf.Min(arr1.Length, arr2.Length);
        yield return new WaitForSeconds(initialDelay);
        for(int i = 0; i < minLength; ++i)
        {
            arr1[i].enabled = true;
            arr2[i].enabled = true;
            yield return new WaitForSeconds(delayBetween);
        }
    }

}
