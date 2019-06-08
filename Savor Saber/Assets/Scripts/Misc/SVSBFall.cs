using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVSBFall : MonoBehaviour
{
    public float speed;
    public float time;
    private float currTime = 0;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(currTime >= time)
        {
            Destroy(gameObject);
            return;
        }
        transform.Translate(0, -speed * Time.deltaTime, 0);
    }
}
