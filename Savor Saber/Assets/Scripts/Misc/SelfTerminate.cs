using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTerminate : MonoBehaviour
{

    public int lifespan = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Terminate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Terminate()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(this.gameObject);
        yield return null;
    }
}
