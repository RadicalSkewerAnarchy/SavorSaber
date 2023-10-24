using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreakShield : PoweredObject
{
    private WaitForSeconds secondTic;
    public int maxTics = 10;
    private int numTics = 0;
    private GameObject player;
    public GameObject shieldTemplate;
    private GameObject shieldReference;

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

    public override void TurnOn()
    {
        base.TurnOn();
        shieldReference = GameObject.Instantiate(shieldTemplate, player.transform);
        shieldReference.transform.position = player.transform.position;
    }

    public override void ShutOff()
    {
        base.ShutOff();
        Destroy(shieldReference);
    }
}
