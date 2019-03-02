using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : BaseProjectile
{
    public GameObject mainProjectile;

    enum FireMode
    {
        shotgun,
        circle
    }
    FireMode firingMode = FireMode.shotgun;

    // Start is called before the first frame update
    void Start()
    {
        // randomly determine firing mode
        // create more projectiles based on firing mode
    }
   
}
