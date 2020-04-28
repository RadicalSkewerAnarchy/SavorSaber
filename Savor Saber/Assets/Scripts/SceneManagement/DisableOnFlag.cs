using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnFlag : MonoBehaviour
{
    public string flag;

    // Start is called before the first frame update
    void Start()
    {
        if(FlagManager.GetFlag(flag) == "True")
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
