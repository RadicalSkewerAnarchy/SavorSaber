using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class EntityController : MonoBehaviour
{
    public abstract Direction Direction { get; set; }
}
