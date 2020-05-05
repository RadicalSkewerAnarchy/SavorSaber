﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class AICharacterData : CharacterData
{
    public RecipeData.Flavors BuffFlavor = default;

    public enum LifeState
    {
        alive,
        overcharged,
        dead
    }
    public LifeState currentLifeState = LifeState.alive;
    [HideInInspector]
    public LifeState previousLifeState = LifeState.alive;

    public AIBrain Brain;
    public bool CommandCompleted = true;
    public bool CommandInProgress = false;

    [HideInInspector]
    public SpriteRenderer sRenderer;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public List<RecipeData.Flavors> FoodPreference;
    [HideInInspector]
    public Queue<IngredientData> Stomach = new Queue<IngredientData>();

    private Squeezer squeeze;

    public bool meleeHunter = true;


    /// <summary>
    /// set necessary values and components
    /// </summary>
    private void Awake()
    {
        Brain = GetComponentInChildren<AIBrain>();
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        InitializeCharacterData();

        squeeze = GetComponent<Squeezer>();
        squeeze?.RandomInitialize();
    }


    /// <summary>
    /// CALL THIS BEFORE OnStateEnter()
    /// modifies fruitant when becoming overcharged, alive and dead
    /// </summary>
    /// <param name="p">old protocol</param>
    public virtual void OnStateExit(LifeState s)
    {
        switch (s)
        {
            default:
                // nothing at all
                break;
        }
    }

    /// <summary>
    /// CALL THIS BEFORE OnStateEnter()
    /// modifies fruitant when becoming overcharged, alive and dead
    /// </summary>
    /// <param name="p">old protocol</param>
    private void AlwaysOnStateExit(LifeState s)
    {
        switch (s)
        {
            case LifeState.overcharged:
                sRenderer.color = Color.white;
                break;
            case LifeState.dead:
                sRenderer.color = Color.white;
                this.GetComponent<Animator>().StopPlayback();
                break;
            default:
                // nothing at all
                break;
        }
    }

    /// <summary>
    /// CALL THIS AFTER OnStateExit()
    /// modifies fruitant when having just been overcharged, alive, or dead
    /// </summary>
    /// <param name="p">new protocol</param>
    public virtual void OnStateEnter(LifeState s)
    {
        switch (s)
        {
            default:
                // nothing at all
                break;
        }
    }

    /// <summary>
    /// CALL THIS AFTER OnStateExit()
    /// modifies fruitant when having just been overcharged, alive, or dead
    /// </summary>
    /// <param name="p">new protocol</param>
    private void AlwaysOnStateEnter(LifeState s)
    {
        switch (s)
        {
            case LifeState.dead:
                sRenderer.color = Color.grey;
                this.GetComponent<Animator>().StartPlayback();
                break;
            default:
                // nothing at all
                break;
        }
    }


    public void Wiggle(int dmg = 1)
    {
        if (squeeze != null)
        {
            if (squeeze.activate)
            {
                StopCoroutine(Wiggling());
                squeeze.horiSpeed = dmg * 2;
                squeeze.vertSpeed = dmg * 2;
                StartCoroutine(Wiggling(dmg * 2));
            }
            else
            {
                squeeze.activate = true;
                squeeze.horiSpeed = dmg;
                squeeze.vertSpeed = dmg;
                StartCoroutine(Wiggling(dmg));
            }
        }
    }

    private IEnumerator Wiggling(float time = 3)
    {
        yield return new WaitForSeconds(time);
        squeeze.activate = false;
    }
}
