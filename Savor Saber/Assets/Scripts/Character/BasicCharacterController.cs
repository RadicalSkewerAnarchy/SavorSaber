using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BasicCharacterController : EntityController
{
    [SerializeField] private Direction _direction;
    public override Direction Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            animatorBody.SetFloat("Direction", (float)value);
        }
    }
    private Animator animatorBody;
    private void Start()
    {
        animatorBody = GetComponent<Animator>();
        Direction = _direction;
    }
}
