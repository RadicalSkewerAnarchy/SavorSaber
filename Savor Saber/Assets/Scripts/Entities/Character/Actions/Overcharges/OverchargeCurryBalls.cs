using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;
public class OverchargeCurryBalls : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected PointVector pv = new PointVector();
    public float dotTicLength = 1;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public virtual void CurryBalls(bool favorite)
    {
        // the amount of time that a fruitant is charmed
        int shots = 2;
        int pellets = 6;
        dotTicLength = 0.5f;
        StartCoroutine(ExecuteCurry(dotTicLength, shots, pellets));
    }


    protected virtual IEnumerator ExecuteCurry(float time, int shots, int pellets)
    {
        //things to happen before delay
        GameObject newAttack;
        MonsterBehavior behave = this.GetComponent<MonsterBehavior>();

        // null check on curryball projectile
        if (behave.projectile == null)
            yield return null;

        // spawn the amount of shots with the amount of pellets
        var s = shots;

        // toggle between red and more red
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        float colorLerp = 0.25f;
        sr.color = Color.Lerp(Color.yellow, Color.red, colorLerp);
        while (s > 0)
        {
            colorLerp += Time.deltaTime * 15;
            sr.color = Color.Lerp(Color.yellow, Color.red, colorLerp);

            yield return new WaitForSeconds(time);
            colorLerp = 0.5f;
            // spawn curry balls
            var split = 360 / pellets;
            for (int i = 0; i < pellets; i++)
            {
                // spawn curry ball at an angle
                newAttack = Instantiate(behave.projectile, transform.position, Quaternion.identity);
                Vector2 dir = Vector2.ClampMagnitude(pv.Ang2Vec((split * i) + (s * 30)), 1f); //+ Random.Range(-split/4, split/4)

                BaseProjectile projectileData = newAttack.GetComponent<BaseProjectile>();

                projectileData.directionVector = dir;
                projectileData.penetrateTargets = true;
                projectileData.attacker = this.gameObject;
            }
            s--;
        }
        //things to happen after delay
        Debug.Log("no longer CURRIED");
        spriteRenderer.color = Color.white;
        yield return null;
    }
}
