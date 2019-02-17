using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalAnimationChild : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(EndAnimationAfterEnd());
    }
    public IEnumerator EndAnimationAfterEnd()
    {
        //Debug.Log("Coroutine for ending animation REACHED");
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        //Debug.Log("Coroutine is done waiting.");
        Destroy(gameObject);
        yield return null;
    }
}
