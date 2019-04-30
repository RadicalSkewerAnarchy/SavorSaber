using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MonsterController : EntityController
{
    [SerializeField] private Direction _direction;
    public override Direction Direction
    {
        get => _direction;
        set 
        {
            _direction = value;
            if (value == Direction.East || value == Direction.SouthEast || value == Direction.NorthEast)
                renderer.flipX = invert;
            else if (value == Direction.West || value == Direction.NorthWest || value == Direction.SouthWest)
                renderer.flipX = !invert;

        }
    }
    public bool invert = false;
    private SpriteRenderer renderer;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        Direction = _direction;
    }
}
