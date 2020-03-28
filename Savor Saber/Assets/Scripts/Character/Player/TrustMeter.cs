using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrustMeter : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField]
    private int trust = 0;
    [SerializeField]
    private Slider meterSlider;
    private Image meterFill;

    private int maxTrust = 100;
    private int minTrust = 0;
    // Start is called before the first frame update
    void Start()
    {
        meterFill = meterSlider.fillRect.gameObject.GetComponent<Image>();
        ChangeTrust(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeTrust(10);
        }
    }

    public void ChangeTrust(int amount)
    {
        trust += amount;
        if (trust > maxTrust)
            trust = maxTrust;
        else if (trust < 0)
            trust = 0;

        if(meterSlider != null)
        {
            meterSlider.value = trust;

            float colorvalue = 0.8f - ((float)trust / 100f) / 2;
            meterFill.color = new Color(1, colorvalue, 0.9f);

        }
    }

    public void SetTrust(int amount)
    {
        trust = amount;
        if (trust > maxTrust)
            trust = maxTrust;
        else if (trust < minTrust)
            trust = minTrust;

        if (meterSlider != null)
        {
            meterSlider.value = trust;

            float colorvalue = 0.8f - ((float)trust / 100) / 2;
            meterFill.color = new Color(1, colorvalue, 0.9f);

        }
    }


}
