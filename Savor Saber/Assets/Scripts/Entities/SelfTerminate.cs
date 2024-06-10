using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfTerminate : MonoBehaviour
{

    public int lifespan = 1;
    private int lifespanQuarterSeconds;
    public Slider timerSlider;
    private WaitForSeconds TimerTic;
    public GameObject dropOnDestroy;
    // Start is called before the first frame update
    void Start()
    {
        lifespanQuarterSeconds = lifespan * 4;
        TimerTic = new WaitForSeconds(0.25f);
        Debug.Log("Self-terminate active on " + this.gameObject.name);
        StartCoroutine(Terminate(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Terminate(float timeActive)
    {
        Debug.Log("Terminate tic on " + this.gameObject.name);
        if (timerSlider != null) timerSlider.value = (float)(1f - (timeActive / lifespanQuarterSeconds));

        if (timeActive >= lifespanQuarterSeconds)
        {
            if (dropOnDestroy != null) Instantiate(dropOnDestroy, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else
        {
            yield return TimerTic;
            yield return Terminate(++timeActive);
        }
    }

}
