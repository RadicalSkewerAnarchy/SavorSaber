using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSwitcher : MonoBehaviour
{
    public GameObject desertMode;
    public GameObject marshMode;
    // Start is called before the first frame update
    void Start()
    {
        if(FlagManager.GetFlag("goal") == "desert")
        {
            desertMode.SetActive(true);
            marshMode.SetActive(false);
        }
    }
}
