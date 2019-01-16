using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Attatch this component to anything that should have HP </summary>
public class Health : MonoBehaviour
{
    public delegate void HealthEvent(int currHp);
    /// <summary> The maximum hp value of the object this component is attatched to </summary>
    public int maxHp = 3;
    private int _hp;
    /// <summary> The current hp value of the object this component is attatched to </summary>
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = Mathf.Clamp(value, 0, maxHp);
            events?.Invoke(_hp);
        }

    }
    /// <summary> The events to invoke when the health value is updated </summary>
    private HealthEvent events = null;

    private void Start()
    {
        _hp = maxHp;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            Hp = 0;
    }

    /// <summary>Adds an event (that takes the current hp as an arg) that is invoked whenever the health value is updated </summary>
    public void AddEvent(HealthEvent h)
    {
        events = (events == null ? h : events + h);
    }
    /// <summary>Adds an event (that takes the current hp as an arg) that is invoked whenever the health value is updated
    /// and the predicate evaluates to true</summary>
    public void AddEvent(HealthEvent h, System.Predicate<int> predicate)
    {
        HealthEvent newEvent = (currHp) =>
        {
            if (predicate(currHp))
                h.Invoke(currHp);
        };
        AddEvent(newEvent);
    }
}
