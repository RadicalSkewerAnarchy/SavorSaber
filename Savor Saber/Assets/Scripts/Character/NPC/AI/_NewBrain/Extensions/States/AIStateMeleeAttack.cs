using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMeleeAttack : AIStateTargettable
{
    [SerializeField]
    private GameObject attackPrefab;
    [SerializeField]
    private float meleeAttackDelay = 0;
    [SerializeField]
    private float meleeAttackDuration = 0.5f;
    [SerializeField]
    private float attackCooldown = 0.5f;
    [SerializeField]
    private Vector2 meleeAttackDimensions = Vector2.one;
    [SerializeField]
    private AudioClip meleeSFX;

    private bool isAttacking;

    public override void Perform()
    {
        if (Target != null)
        {
            MeleeAttack(Target.transform.position);
        }
        else
        {
            ChooseTarget();
        }
    }

    /// <summary>
    /// If you're not attacking, make attack collider and attack
    /// </summary>
    public void MeleeAttack(Vector2 target)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            if (meleeSFX != null)
            {
                Instantiate(myBrain.CharacterData.sfxPlayer, transform.position, transform.rotation).GetComponent<PlayAndDestroy>().Play(meleeSFX);
            }
            StartCoroutine(MeleeDelay(target));
        }
    }

    /// <summary>
    /// time before attack is spawned
    /// then spawn it
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator MeleeDelay(Vector2 target)
    {
        yield return new WaitForSeconds(meleeAttackDelay);

        GameObject newAttack = Instantiate(attackPrefab, transform.position, Quaternion.identity, transform);
        CapsuleCollider2D newAttackCollider = newAttack.GetComponent<CapsuleCollider2D>();

        var body = this.myBrain.CharacterData.gameObject;
        newAttack.GetComponent<BaseMeleeAttack>().myAttacker = body;
        newAttackCollider.size = meleeAttackDimensions;
        newAttackCollider.transform.position += new Vector3(body.GetComponent<Collider2D>().offset.x, body.GetComponent<Collider2D>().offset.y, 0);
        newAttack.transform.Rotate(target - (Vector2)body.transform.position);

        StartCoroutine(EndAttackAfterSeconds(meleeAttackDuration, newAttack, true));
    }

    /// <summary>
    /// Ends attack after time passes and destroys
    /// </summary>
    protected IEnumerator EndAttackAfterSeconds(float time, GameObject newAttack, bool destroy)
    {
        yield return new WaitForSeconds(time);
        if (destroy)
            Destroy(newAttack);
        StartCoroutine(EnableAttacking(attackCooldown));
        yield return null;
    }
    /// <summary>
    /// reenables attacking
    /// </summary>
    protected IEnumerator EnableAttacking(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        yield return null;
    }
}
