using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalAnimationChild : MonoBehaviour
{
    private Color SpriteColor;
    private void Awake()
    {
        SpriteColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().sortingLayerName = "AboveObjects";
        StartCoroutine(EndAnimationAfterEnd());
    }
    private void Update()
    {
        Vector3 target = new Vector3(transform.position.x + Random.Range(0f,.25f), transform.position.y + Random.Range(1.5f,3f) , 0);
        target = Vector3.ClampMagnitude(target, 1 * Time.deltaTime);
        SpriteColor.a -= .5f * Time.deltaTime;
        GetComponent<SpriteRenderer>().color = SpriteColor;
        transform.Translate(target);
    }
    public IEnumerator EndAnimationAfterEnd()
    {
        //Debug.Log("Coroutine for ending animation REACHED");
        yield return new WaitForSeconds(2f);
        //Debug.Log("Coroutine is done waiting.");
        Destroy(gameObject);
        yield return null;
    }
}
