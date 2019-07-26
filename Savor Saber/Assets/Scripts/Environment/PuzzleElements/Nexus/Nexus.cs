using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Attatch this to an object to make it a nexus.
/// Works in cooperation with a NexusInteractor instance, which must be on a child object.
/// Base prefab located at Asssets->Prefabs->Entities->Environment->Nexus->NexusBase
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class Nexus : MonoBehaviour
{
    /// <summary>
    /// The states of a nexus.
    /// Protected  : The nexus is protected by a boss and can only be hacked for one ingredient
    /// Unprotected: The nexus is unprotected and may be hacked to full activation
    /// Activated  : The nexus has been activated.
    /// </summary>
    public enum State
    {
        Protected,
        Unprotected,
        Activated,
    }

    private State currState = State.Protected;
    public State CurrState
    {
        get => currState;
        set
        {
            currState = value;
            FlagManager.SetFlag(nexusID, value.ToString());
            if (value == State.Protected)
                spriteRenderer.color = Color.red;
            else if (value == State.Unprotected)
                spriteRenderer.color = Color.green;
            if (value == State.Activated)
                Activate();
        }
    }
    [Tooltip("The ID of this nexus. Used to set flags relevant to NexusEntity instances (and saving)")]
    public string nexusID;
    [Tooltip("The ingredient to be spawned when this nexus is hacked")]
    public GameObject ingredientPrefab;
    public List<GameObject> protectedBy = new List<GameObject>() { null };
    public UnityEvent callOnActivation = new UnityEvent();

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Initialize Nexus Interactor
        var interactionTrigger = gameObject.GetComponentInChildren<NexusInteractor>();
        Debug.Assert(interactionTrigger != null, "Nexus Interaction Trigger not found in children. ID: " + nexusID + " GO: " + name);
        interactionTrigger.Initialize(this, ingredientPrefab);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Set Logging flag
        FlagManager.SetFlag(nexusID, State.Protected.ToString());
        // Remove all dead entities from protection list
        protectedBy.RemoveAll((g) => g == null);
        // Calculate appropriate state
        CurrState = protectedBy.Count == 0 ? State.Unprotected : State.Protected;
    }

    private void Update()
    {
        // If we are already activated, return
        if (CurrState == State.Activated)
            return;
        // Remove all dead entities from protection list
        protectedBy.RemoveAll((g) => g == null);
        // Calculate appropriate state
        var nextState = protectedBy.Count == 0 ? State.Unprotected : State.Protected;
        // Only set state if it should change to prevent extra flag setting
        if (CurrState != nextState)
            CurrState = nextState;
    }

    private void Activate()
    {
        spriteRenderer.color = Color.white;
        callOnActivation.Invoke();
    }
}
