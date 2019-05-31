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

    public static bool InCutscene { get; private set; }
    public Type type = Type.Cutscene;
    public bool IsActive { get; protected set; }
    public bool repeatable = false;
    public UnityEvent callOnStart;
    public UnityEvent callOnCompletion;
    [HideInInspector] public bool playCompletionEvents = true;
    protected EventGraph scene;
    protected GameObject player;
    protected PlayerController plCon;

    protected void InitializeBase()
    {
        scene = GetComponent<EventGraph>();      
        plCon = PlayerController.instance;
        player = plCon.gameObject;
    }
    private void Start()
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
            DoCutscenePrep(true);
        plCon.enabled = false;
        plCon.Stop();
        scene.ResetScene();
        callOnStart.Invoke();
    }
    protected virtual void FinishEvent()
    {
        if (type == Type.Cutscene)
            DoCutscenePrep(false);       
        plCon.enabled = true;
        if(playCompletionEvents)
            callOnCompletion.Invoke();
        playCompletionEvents = true;
        IsActive = false;
        if (!repeatable)
        {  
            Destroy(gameObject);
        }
    }

    protected void DoCutscenePrep(bool start)
    {
        player.GetComponent<CameraController>().Detatched = start;
        player.GetComponent<Rigidbody2D>().isKinematic = start;
        var attacks = player.GetComponents<AttackBase>();
        foreach (var attack in attacks)
            attack.enabled = !start;
        var party = player.GetComponent<PlayerData>().party;
        foreach (var partyMember in party)
        {
            var AI = partyMember.GetComponent<AIData>();
            if (AI != null)
                AI.enabled = !start;
        }
        DisplayInventory.instance?.disableDuringCutscene.SetActive(!start);
        InCutscene = start;
    }
}
