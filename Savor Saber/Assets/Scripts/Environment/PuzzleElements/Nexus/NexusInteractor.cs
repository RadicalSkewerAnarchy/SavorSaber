﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NexusInteractor : MonoBehaviour
{
    public Nexus Parent { get; set; }
    public Nexus.State State => Parent.CurrState;
    public AreaSpawner spawner;
    public float hackTime = 2.5f;
    public float spawnXOnActivate = 8;
    [Header("UI")]
    public UnityEngine.UI.Image progressbar;
    public GameObject promptObj;

    private bool playerInInteractArea;
    private bool hacking;

    private void Awake()
    {
        if (Parent == null)
            Parent = GetComponentInParent<Nexus>();
    }

    public void Initialize(Nexus parent, GameObject ingredientPrefab)
    {
        Parent = parent;
        spawner.spawnObjects.Add(ingredientPrefab);
        hacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInInteractArea || Parent.CurrState == Nexus.State.Activated)
            return;
        if (!hacking)
        {  
            if(InputManager.GetButtonDown(Control.Interact))
            {
                hacking = true;
                StartCoroutine(HackCr());
            }
        }
        else if (InputManager.GetButtonUp(Control.Interact)) // currently hacking
        {
            CancelHack();
        }
    }

    public void Hack()
    {
        if (State == Nexus.State.Protected)
        {
            spawner.Spawn();
        }
        else if (State == Nexus.State.Unprotected)
        {
            Parent.CurrState = Nexus.State.Activated;
            for (int i = 0; i < spawnXOnActivate; ++i)
                spawner.Spawn();
            promptObj.SetActive(false);
        }
    }

    public void CancelHack()
    {
        StopAllCoroutines();
        progressbar.gameObject.SetActive(false);
        hacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Parent.CurrState != Nexus.State.Activated)
        {
            playerInInteractArea = true;
            promptObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInInteractArea = false;
        promptObj.SetActive(false);
        if(hacking)
            CancelHack();
    }

    private IEnumerator HackCr()
    {
        progressbar.color = Color.white;
        progressbar.gameObject.SetActive(true);
        float time = 0;
        while(time <= hackTime)
        {
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
            progressbar.fillAmount = time / hackTime;
        }
        time = 0;
        const float timeInc = 0.075f;
        Color flickerColor = new Color(0.75f, 0.75f, 0.75f, 0.75f);
        while(time < timeInc * 8)
        {
            progressbar.color = flickerColor;
            yield return new WaitForSeconds(timeInc);
            progressbar.color = Color.white;
            yield return new WaitForSeconds(timeInc);
            time += timeInc * 2;
        }
        Hack();
        progressbar.fillAmount = 0;
        progressbar.gameObject.SetActive(false);
    }
}
