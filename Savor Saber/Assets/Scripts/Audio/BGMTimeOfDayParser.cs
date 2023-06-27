using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMTimeOfDayParser : MonoBehaviour
{
    private BGMManager manager;
    private DayNightController controller;
    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<DayNightController>();
        manager = GetComponentInParent<BGMManager>();

        controller.SetTimeOfDayImmediate(controller.timeOfDayTransitionHolder);
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Parse()
    {
        if (!active) return;
        if (!controller.IsDayTime)
        {
            Debug.Log("BGM Time of Day parser detects nighttime");
            manager.gameObject.GetComponent<EventFadeAudio>().Play();
            manager.GoToNightMusic(true);
        }
        else
        {
            Debug.Log("BGM Time of Day parser detects daytime");
            manager.gameObject.GetComponent<EventFadeAudio>().Play();
            manager.GoToDayMusic(true);
        }
    }
}
