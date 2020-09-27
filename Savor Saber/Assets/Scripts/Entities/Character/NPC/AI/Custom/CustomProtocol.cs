using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIData))]
public abstract class CustomProtocol : MonoBehaviour
{
    public abstract void Invoke();
}
