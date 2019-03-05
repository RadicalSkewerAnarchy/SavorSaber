using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : BaseProjectile
{
    public GameObject mainProjectile;
    public GameObject boss;

    enum FireMode
    {
        shotgun,
        circle
    }
    FireMode firingMode = FireMode.shotgun;

    // Start is called before the first frame update
    void Awake()
    {

        Debug.Log("BOSS PROJECTILE MADE");
        // randomly determine firing mode
        // create more projectiles based on firing mode
        int numProjectiles = 5;
        int spreadProjectiles = 45;
        int angleDiv = (int)spreadProjectiles / numProjectiles;

        GameObject newAttack;
        BaseProjectile projectileData;

        float firingAngle = Vector3.Angle(this.directionVector, Vector3.right);
        for (int i = 0; i < numProjectiles; i++)
        {
            Debug.Log("Projectile number: " + i);
            newAttack = Instantiate(mainProjectile, transform.position, Quaternion.identity, transform);
            Physics2D.IgnoreCollision(newAttack.GetComponent<Collider2D>(), boss.GetComponent<Collider2D>());
            projectileData = newAttack.GetComponent<BaseProjectile>();
            projectileData.directionVector = RotatePoint(this.transform.position, firingAngle * i , this.directionVector) ;
            projectileData.penetrateTargets = true;
        }
    }


    // from pivot to change, rotate by angle
    public Vector2 RotatePoint(Vector2 pivotPoint, float angle, Vector2 changePoint)
    {
        // sin and cos
        float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
        float cos = Mathf.Cos(Mathf.Deg2Rad * angle);

        // translate point back to origin
        changePoint.x -= pivotPoint.x;
        changePoint.y -= pivotPoint.y;

        // rotate point
        float xnew = changePoint.x * cos - changePoint.y * sin;
        float ynew = changePoint.x * sin + changePoint.y * cos;

        // return new vector
        // after readjusting from pivot
        return new Vector2(xnew + pivotPoint.x, ynew + pivotPoint.y);
    }
}
