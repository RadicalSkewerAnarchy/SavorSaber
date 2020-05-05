using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public bool disableDestroyOnLoad = false;
    // Start is called before the first frame update
    void Start()
    {
        if (disableDestroyOnLoad)
            DisableDestroyOnLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableDestroyOnLoad()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
