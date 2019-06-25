using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BonusPearManager : MonoBehaviour
{

    public GameObject[] pridePears;
    public GameObject vanillaPear;
    public String[] genericTexts;
    public Text splashText;
    private bool isPrideMonth = false;

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log(DateTime.Now);
        if (splashText == null)
            splashText = GameObject.Find("SplashText").GetComponent<Text>();

        //Spawn pride pears during pride month
        //Array is setup so that there is 33% chance for a general pride pear, 66% for one of seven specific pride pears
        if (DateTime.Now.Month == 6)
        {
            int pearIndex = UnityEngine.Random.Range(0, pridePears.Length - 1);
            Instantiate(pridePears[pearIndex], transform.position, Quaternion.identity);
            splashText.text = "Happy Pride Month!";
        }
        else if(DateTime.Now.Month == 8 && DateTime.Now.Day <= 7)
        {
            Instantiate(vanillaPear, transform.position, Quaternion.identity);
            splashText.text = "Happy International Clown Week!";
        }
        else
        {
            Instantiate(vanillaPear, transform.position, Quaternion.identity);
            splashText.text = genericTexts[UnityEngine.Random.Range(0, genericTexts.Length - 1)];
        }
            

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
