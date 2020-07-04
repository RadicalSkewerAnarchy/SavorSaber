using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedSwitch : PoweredObject
{
    public Slider castSlider;
    public int timerLength = 1;
    public PoweredObject[] targetObjects;

    private int timerLengthQuarters;
    private WaitForSeconds timerTic;

    // Start is called before the first frame update
    void Start()
    {
        timerTic = new WaitForSeconds(0.25f);
        timerLengthQuarters = timerLength * 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        StopAllCoroutines();
        castSlider.gameObject.SetActive(true);

        foreach (PoweredObject targetObject in targetObjects)
        {
            targetObject.TurnOn();
        }

        StartCoroutine(Cast(timerLengthQuarters));
    }

    public override void ShutOff()
    {

    }

    private IEnumerator Cast(int numTics)
    {
        //Debug.Log("Charge time remaining: " + numTics);
        castSlider.value = 1f - ((float)(timerLengthQuarters - numTics) / timerLengthQuarters);

        if (numTics > 0)
        {
            yield return timerTic;
            yield return Cast(numTics - 1);
        }
        else
        {
            //once we've ticked down to 0, shut the target objects back off
            castSlider.gameObject.SetActive(false);
            foreach (PoweredObject targetObject in targetObjects)
            {
                targetObject.ShutOff();
            }
        }
        yield return null;
    }
}
