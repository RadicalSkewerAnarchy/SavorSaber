using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PartyHatSwapper : MonoBehaviour
{
    public Sprite gayhat;
    // Start is called before the first frame update
    void Start()
    {
        if (System.DateTime.Now.Month == 6)
            GetComponent<SpriteRenderer>().sprite = gayhat;
    }
}
