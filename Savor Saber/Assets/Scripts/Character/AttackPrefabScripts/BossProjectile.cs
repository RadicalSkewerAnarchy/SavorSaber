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
    void Start()
    {
        Debug.Log("BOSS PROJECTILE MADE");
        // randomly determine firing mode
        // create more projectiles based on firing mode
        int numProjectiles = Random.Range(1, 4) + Random.Range(1, 4) + 3;
        float spreadProjectiles = Random.Range(45f, 90f);
        float angleDiv = spreadProjectiles / numProjectiles;

        GameObject attacker = this.GetComponent<BaseProjectile>().attacker;
        GameObject newAttack;
        BaseProjectile projectileData;

        float firingAngle = Vec2Ang(this.directionVector) - (spreadProjectiles / 2);//Vector3.Angle(this.directionVector, Vector3.right) - (angleDiv / 2);
        for (float i = 0; i < numProjectiles; i++)
        {
            Debug.Log("Projectile angle: " + i);
            newAttack = Instantiate(mainProjectile, transform.position, Quaternion.identity, transform);
            Physics2D.IgnoreCollision(newAttack.GetComponent<Collider2D>(), attacker.GetComponent<Collider2D>());
            projectileData = newAttack.GetComponent<BaseProjectile>();
            projectileData.directionVector = Vector2.ClampMagnitude(Ang2Vec(i*angleDiv + firingAngle), 1f);
            //.directionVector = RotatePoint(this.transform.position, firingAngle * i , (Vector2)this.transform.position + this.directionVector);
            projectileData.penetrateTargets = true;
            projectileData.projectileSpeed = 8f;
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
    // vector to angle
    public float Vec2Ang(Vector2 angle)
    {
        return Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg;
    }
    // angle to vector
    public Vector2 Ang2Vec(float angle)
    {
        return new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
    }
}
