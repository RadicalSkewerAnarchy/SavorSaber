using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreakShield : MonoBehaviour
{
    private WaitForSeconds secondTic;
    public int maxTics = 10;
    private int numTics = 0;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        secondTic = new WaitForSeconds(1);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
