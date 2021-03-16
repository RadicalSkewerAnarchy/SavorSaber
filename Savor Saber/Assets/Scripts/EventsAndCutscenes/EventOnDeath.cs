using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnDeath : MonoBehaviour
{
    // Start is called before the first frame update

    private bool active = false;

    private List<GameObject> targetList;
    public UnityEvent deathEvent;

    void Start()
    {
        targetList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTargetState();
    }

    public void AddTarget(GameObject target)
    {
        targetList.Add(target);
    }

    private void CheckTargetState()
    {
        if (active)
        {
            foreach (GameObject target in targetList)
            {
                if (target != null || targetList.Count == 0)
                    return;
            }
            Debug.Log("All targets null, firing event");
            deathEvent.Invoke();
            active = false;
        }
    }

    public void Activate()
    {
        active = true;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
