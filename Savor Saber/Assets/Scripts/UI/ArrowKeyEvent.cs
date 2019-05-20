using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowKeyEvent : MonoBehaviour
{
    public SpriteRenderer up;
    private Coroutine upCr = null;
    private Vector2 upTr;
    public SpriteRenderer down;
    private Coroutine downCr;
    private Vector2 downTr;
    public SpriteRenderer left;
    private Coroutine leftCr;
    private Vector2 leftTr;
    public SpriteRenderer right;
    private Coroutine rightCr;
    private Vector2 rightTr;
    public ParticleSystem sparks;
    public int pressGoal = 20;
    private int count = 0;

    public UnityEvent onFinish;

    private void Start()
    {
        upTr = up.transform.position;
        downTr = down.transform.position;
        rightTr = right.transform.position;
        leftTr = left.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.GetButtonDown(Control.Up))
        {
            if (++count >= pressGoal)
                Finish();
            else
            {
                if (upCr != null)
                    StopCoroutine(upCr);
                up.transform.position = upTr;
                StartCoroutine(Shake(upCr, up.gameObject, 0.2f, 0.2f, 0.05f));
            }
                
        }
        else if (InputManager.GetButtonDown(Control.Down))
        {
            if (++count >= pressGoal)
                Finish();
            else
            {
                if (downCr != null)
                    StopCoroutine(downCr);
                down.transform.position = downTr;
                downCr = StartCoroutine(Shake(downCr,down.gameObject, 0.2f, 0.2f, 0.05f));
            }
                
        }
        else if (InputManager.GetButtonDown(Control.Left))
        {
            if (++count >= pressGoal)
                Finish();
            else
            {
                if (leftCr != null)
                    StopCoroutine(leftCr);
                left.transform.position = leftTr;
                leftCr = StartCoroutine(Shake(leftCr, left.gameObject, 0.2f, 0.2f, 0.05f));
            }

        }
        else if (InputManager.GetButtonDown(Control.Right))
        {
            if (++count >= pressGoal)
                Finish();
            else
            {
                if (rightCr != null)
                    StopCoroutine(rightCr);
                right.transform.position = rightTr;
                rightCr = StartCoroutine(Shake(rightCr, right.gameObject, 0.2f, 0.2f, 0.05f));
            }

        }
    }

    private void Finish()
    {
        onFinish.Invoke();
    }

    private IEnumerator Shake(Coroutine cr, GameObject go, float time, float speed, float amplitude)
    {
        Vector2 origin = go.transform.position;
        var count = 0f;
        while (count < time)
        {
            yield return new WaitForEndOfFrame();
            go.transform.position = origin + Random.insideUnitCircle * amplitude;
            count += Time.deltaTime;
        }
        cr = null;
        go.transform.position = origin;
    }
}
