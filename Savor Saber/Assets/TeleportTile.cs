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

    public GameObject[] otherTeleports;

    // Start is called before the first frame update
    void Start()
    {
        sprite = teleporter.GetComponent<SpriteRenderer>();
        data = teleporter.GetComponent<AIData>();
        teleTimer = teleTimerReset + 10 * Random.Range(-teleTimerVariance, teleTimerVariance);
    }

    // update
    private void Update()
    {
        teleTimer -= Time.deltaTime;
        if (teleTimer <= 0 && notAlreadyTeleporting())
        {
            teleporting = true;
            teleTimer = teleTimerReset + 10 * Random.Range(-teleTimerVariance, teleTimerVariance);
        }

        if (teleporting)
        {
            Teleport();
        }
    }

    private bool notAlreadyTeleporting()
    {
        bool tele = true;
        foreach (var tile in otherTeleports)
        {
            TeleportTile t = GetComponent<TeleportTile>();
            if (t.teleporting)
            {
                tele = false;
            }
        }

        return tele;
    }

    private void Teleport()
    {
        Color col = sprite.color;
        // affect color
        if (fadeOut)
        {
            col.a = Mathf.Max(col.a - fadeSpeed, 0);
            //Debug.Log("sprite alpha after " + col.a);
            sprite.color = col;
            if (col.a <= 0)
            {
                // update position here

                teleporter.transform.position = this.transform.position;

                // shift to fading back in
                fadeOut = false;
            }
        }
        else
        {
            col.a = Mathf.Min(col.a + fadeSpeed, 1);
            //Debug.Log("sprite alpha after " + col.a);
            sprite.color = col;
            if (col.a >= 1)
            {
                teleporting = false;
                fadeOut = true;
            }
        }
    }

}
