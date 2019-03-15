using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDash : MonoBehaviour
{
    private UpdatedController playerData;
    private Image barCover;

    // Start is called before the first frame update
    void Start()
    {
        GameObject play = GameObject.FindGameObjectWithTag("Player");
        playerData = play.GetComponent<UpdatedController>();
        barCover = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        barCover.fillAmount = playerData.CurrDashes / playerData.maxDashes;
        barCover.color = playerData.RechargingFromEmpty ? Color.red : Color.white;
    }
}
