using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowKeyEvent : MonoBehaviour
{
    public SpriteRenderer up;
    private CrHolder upCr = new CrHolder();
    private Vector2 upTr;
    public SpriteRenderer down;
    private CrHolder downCr = new CrHolder();
    private Vector2 downTr;
    public SpriteRenderer left;
    private CrHolder leftCr = new CrHolder();
    private Vector2 leftTr;
    public SpriteRenderer right;
    private CrHolder rightCr = new CrHolder();
    private Vector2 rightTr;
    public SpriteRenderer joyStick;
    private CrHolder joyCr = new CrHolder();
    private Vector2 joyTr;
    public Sprite[] keySprites = new Sprite[8];
    public ParticleSystem sparks;
    public int pressGoal = 20;
    private float count = 0;
    private ControlProfile currKeyControls = null;
    private bool controllerMode = false;
    public UnityEvent onFinish;

    private void Start()
    {
        upTr = up.transform.position;
        downTr = down.transform.position;
        rightTr = right.transform.position;
        leftTr = left.transform.position;
        joyTr = joyStick.transform.position;
        currKeyControls = InputManager.main.keyboardControls;
        SetKeys();
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.ControllerMode)
        {
            if(!controllerMode)
            {
                SetControllerMode(true);
            }
            if(InputManager.GetAxis(InputAxis.Horizontal) != 0 || InputManager.GetAxis(InputAxis.Vertical) != 0)
            {
                if (joyCr.cr != null)
                    return;
                if (++count >= pressGoal)
                    Finish();
                else
                {                    
                    joyStick.transform.position = joyTr;
                    joyCr.cr = StartCoroutine(Shake(joyCr, joyStick.gameObject, 0.2f, 0.2f, 0.05f));
                }
            }
        }
        else
        {
            if (controllerMode)
            {
                SetControllerMode(false);
            }
            if(currKeyControls != InputManager.main.keyboardControls)
            {
                currKeyControls = InputManager.main.keyboardControls;
                SetKeys();
            }
            if (InputManager.GetButtonDown(Control.Up))
            {
                if (++count >= pressGoal)
                    Finish();
                else
                {
                    if (upCr.cr != null)
                        StopCoroutine(upCr.cr);
                    up.transform.position = upTr;
                    upCr.cr = StartCoroutine(Shake(upCr, up.gameObject, 0.2f, 0.2f, 0.05f));
                }

            }
            else if (InputManager.GetButtonDown(Control.Down))
            {
                if (++count >= pressGoal)
                    Finish();
                else
                {
                    if (downCr.cr != null)
                        StopCoroutine(downCr.cr);
                    down.transform.position = downTr;
                    downCr.cr = StartCoroutine(Shake(downCr, down.gameObject, 0.2f, 0.2f, 0.05f));
                }

            }
            else if (InputManager.GetButtonDown(Control.Left))
            {
                if (++count >= pressGoal)
                    Finish();
                else
                {
                    if (leftCr.cr != null)
                        StopCoroutine(leftCr.cr);
                    left.transform.position = leftTr;
                    leftCr.cr = StartCoroutine(Shake(leftCr, left.gameObject, 0.2f, 0.2f, 0.05f));
                }

            }
            else if (InputManager.GetButtonDown(Control.Right))
            {
                if (++count >= pressGoal)
                    Finish();
                else
                {
                    if (rightCr.cr != null)
                        StopCoroutine(rightCr.cr);
                    right.transform.position = rightTr;
                    rightCr.cr = StartCoroutine(Shake(rightCr, right.gameObject, 0.2f, 0.2f, 0.05f));
                }

            }
        }
    }

    private void SetKeys()
    {
        up.sprite = currKeyControls.sprites[Control.Up];
        down.sprite = currKeyControls.sprites[Control.Down];
        left.sprite = currKeyControls.sprites[Control.Left];
        right.sprite = currKeyControls.sprites[Control.Right];
    }

    private void SetControllerMode(bool c)
    {
        bool k = !c;
        up.gameObject.SetActive(k);
        down.gameObject.SetActive(k);
        left.gameObject.SetActive(k);
        right.gameObject.SetActive(k);
        joyStick.gameObject.SetActive(c);
        controllerMode = c;
    }

    private void Finish()
    {
        Debug.Log("Keypress event finished");
        onFinish.Invoke();
    }

    private IEnumerator Shake(CrHolder cr, GameObject go, float time, float speed, float amplitude)
    {
        Vector2 origin = go.transform.position;
        var count = 0f;
        while (count < time)
        {
            yield return new WaitForEndOfFrame();
            go.transform.position = origin + Random.insideUnitCircle * amplitude;
            count += Time.deltaTime;
        }
        cr.cr = null;
        go.transform.position = origin;
    }

    private class CrHolder
    {
        public Coroutine cr = null;
    }

}
