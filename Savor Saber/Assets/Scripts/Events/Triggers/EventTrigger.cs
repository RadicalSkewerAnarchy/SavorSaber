using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EventGraph))]
public class EventTrigger : MonoBehaviour
{
    public enum Type
    {
        Event,
        Dialog,
        Cutscene,
    }

    public Type type = Type.Cutscene;
    public bool IsActive { get; protected set; }
    public bool repeatable = false;
    public UnityEvent callOnStart;
    public UnityEvent callOnCompletion;
    public GameObject[] disableOnCutscene;
    protected EventGraph scene;
    protected GameObject player;
    protected UpdatedController plCon;

    protected void InitializeBase()
    {
        scene = GetComponent<EventGraph>();
        player = GameObject.FindGameObjectWithTag("Player");
        plCon = player.GetComponent<UpdatedController>();
    }
    private void Awake()
    {
        InitializeBase();
    }
    public void Trigger()
    {
        if (IsActive)
            return;
        Debug.Log("Trigger Cutscene: " + name);
        BeforeEvent();
        StartCoroutine(PlayEvent());
    }
    protected virtual IEnumerator PlayEvent()
    {
        yield return StartCoroutine(scene.Play());
        FinishEvent();
    }
    protected virtual void BeforeEvent()
    {
        IsActive = true;
        if (type == Type.Cutscene)
        {
            player.GetComponent<CameraController>().Detatched = true;
            foreach(var obj in disableOnCutscene)
                obj.SetActive(false);
        }               
        plCon.enabled = false;
        plCon.Stop();
        scene.ResetScene();
        callOnStart.Invoke();
    }
    protected virtual void FinishEvent()
    {
        if (type == Type.Cutscene)
        {
            player.GetComponent<CameraController>().Detatched = false;
            foreach (var obj in disableOnCutscene)
                obj.SetActive(true);
        }           
        plCon.enabled = true;       
        callOnCompletion.Invoke();
        IsActive = false;
        if (!repeatable)
        {  
            Destroy(gameObject);
        }
    }
}
