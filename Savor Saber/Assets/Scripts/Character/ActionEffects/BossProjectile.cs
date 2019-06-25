using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : BaseProjectile
{
    public GameObject mainProjectile;
    public GameObject boss;
    public int numProj;
    public int numProjectilesConstant;
    public int numProjectilesVariable;
    public AnimationCurve numProjectilesDistribution;
    public float spread;
    public float spreadProjectilesMin;
    public float spreadProjectilesShift;
    public AnimationCurve spreadProjectilesDistribution;
    public float angleDiv;
    public AnimationCurve angleDividerDistribution;
    // Start is called before the first frame update
    void Awake()
    {
        // randomly determine firing mode
        // create more projectiles based on firing mode
        numProj = (int) (numProjectilesConstant + numProjectilesVariable * numProjectilesDistribution.Evaluate(Random.Range(0f, 1f)));
        spread = spreadProjectilesMin + ((360 - spreadProjectilesMin + spreadProjectilesShift) * spreadProjectilesDistribution.Evaluate(Random.Range(0f, 1f)));

        GameObject newAttack;
        for (float i = 0; i < numProj; i++)
        {
            newAttack = Instantiate(mainProjectile, transform.position, Quaternion.identity, transform);
        }
    }

    private void Start()
    {

        //Debug.Log("Boss Proj: attacker = " + this.attacker.name + "... angle = " + this.directionVector);
        // go through children and set values

        BaseProjectile projectileData;
        float firingAngleStart = Vec2Ang(this.directionVector) - (spread / 2);
        float firingAngleIterate = (spread / numProj);
        var i = 0;
        foreach (Transform child in transform)
        {
            projectileData = child.GetComponent<BaseProjectile>();

            projectileData.directionVector = Vector2.ClampMagnitude(Ang2Vec(firingAngleIterate * (i) + firingAngleStart), 1f);
            //Debug.Log("Projectile angle: " + projectileData.directionVector);

            projectileData.penetrateTargets = true;
            projectileData.attacker = this.attacker;
            projectileData.projectileSpeed = 3f;
            projectileData.range = 30f;

            //ignore attacker
            Physics2D.IgnoreCollision(child.GetComponent<Collider2D>(), attacker.GetComponent<Collider2D>());

            // iterate
            i++;
        }
        spawnPosition = transform.position;

        Destroy(this.gameObject, Random.Range(2f, 5f));
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
