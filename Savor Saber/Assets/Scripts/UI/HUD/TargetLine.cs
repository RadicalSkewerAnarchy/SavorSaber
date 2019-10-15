using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetLine : MonoBehaviour
{
    public Transform controllerTarget;
    private Vector2 mouseTarget;
    private SpriteRenderer parentSR;

    // Start is called before the first frame update
    void Start()
    {
        parentSR = GetComponentInParent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.ControllerMode)
        {
            //follow controller target
            Vector3 target = controllerTarget.position;
            transform.right = target - transform.position;
        }
        else
        {
            //old techniques that didn't work
            //transform.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.back);
            //transform.Rotate(new Vector3(0, 0, -90));

            //Quaternion rotation = Quaternion.LookRotation(target - transform.position, transform.TransformDirection(Vector3.up));
            //transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.right = target - transform.position;


        }
    }
}
