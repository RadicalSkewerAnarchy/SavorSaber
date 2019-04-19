using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTile : MonoBehaviour
{
    public GameObject teleporter;
    public SpriteRenderer sprite;
    public AIData data;
    public bool teleporting = false;


    public float teleTimer;
    public float teleTimerReset = 30;
    public int teleTimerVariance = 2;

    public bool fadeOut = true;
    [Range(0.0f, 1.0f)]
    public float fadeSpeed = 0.01f;
    public float fadeAmount = 1.0f;

    public GameObject[] otherTeleports;

    // Start is called before the first frame update
    void Start()
    {
        sprite = teleporter.GetComponent<SpriteRenderer>();
        data = teleporter.GetComponent<AIData>();
        teleTimer = teleTimerReset + 5 * Random.Range(-teleTimerVariance, teleTimerVariance);
    }

    // update
    private void Update()
    {
        if (notAlreadyTeleporting())
        {
            teleTimer -= Time.deltaTime;
            if (teleporting)
            {
                if (teleporter != null)
                    Teleport();
                else
                    Destroy(this.gameObject);
            }
            else if (teleTimer <= 0)
            {
                teleporting = true;
            }
        }
    }

    private bool notAlreadyTeleporting()
    {
        foreach (var tile in otherTeleports)
        {
            TeleportTile t = tile.GetComponent<TeleportTile>();
            if (t.teleporting)
            {
                return false;
            }
        }
        return true;
    }

    private void Teleport()
    {
        Color col = sprite.color;
        // affect color
        if (fadeOut)
        {
            fadeAmount = Mathf.Max(fadeAmount - fadeSpeed, 0);
            col.a = fadeAmount;
            //Debug.Log("sprite alpha after " + col.a);
            sprite.color = col;
            if (fadeAmount <= 0)
            {
                // update position here

                teleporter.transform.position = this.transform.position;

                // shift to fading back in
                fadeOut = false;
            }
        }
        else
        {
            fadeAmount = Mathf.Min(fadeAmount + fadeSpeed, 1);
            col.a = fadeAmount;
            sprite.color = col;
            if (fadeAmount >= 1)
            {
                teleporting = false;
                fadeOut = true;

                teleTimer = teleTimerReset + 5 * Random.Range(-teleTimerVariance, teleTimerVariance);
            }
        }
    }

}
