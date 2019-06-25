using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventScript : MonoBehaviour
{
    public abstract IEnumerator PlayEvent(GameObject player);
}
