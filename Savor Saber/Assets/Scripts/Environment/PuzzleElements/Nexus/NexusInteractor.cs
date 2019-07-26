using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NexusInteractor : MonoBehaviour
{
    public Nexus Parent { get; set; }
    public Nexus.State State => Parent.CurrState;
    public AreaSpawner spawner;
    public float spawnCooldownTime = 2.5f;
    public float spawnXOnActivate = 8;

    private bool playerInInteractArea;
    private bool readyToSpawn;

    private void Awake()
    {
        if (Parent == null)
            Parent = GetComponentInParent<Nexus>();
    }

    public void Initialize(Nexus parent, GameObject ingredientPrefab)
    {
        Parent = parent;
        spawner.spawnObjects.Add(ingredientPrefab);
        readyToSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInInteractArea || !InputManager.GetButtonDown(Control.Interact))
            return;
        if(State == Nexus.State.Protected)
        {
            if (readyToSpawn)
            {
                spawner.Spawn();
                readyToSpawn = false;
                StartCoroutine(SpawnCooldownCr());
            }
        }
        else if(State == Nexus.State.Unprotected)
        {
            Parent.CurrState = Nexus.State.Activated;
            for(int i = 0; i < spawnXOnActivate; ++i)
                spawner.Spawn();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInInteractArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInInteractArea = false;
    }

    private IEnumerator SpawnCooldownCr()
    {
        yield return new WaitForSeconds(spawnCooldownTime);
        readyToSpawn = true;
    }
}
