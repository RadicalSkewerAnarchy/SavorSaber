using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public float crosshairSpeed = 10;
    private Vector2 controllerInput;
    // Start is called before the first frame update
    void Start()
    {
        controllerInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.ControllerMode)
        {
            controllerInput.x = InputManager.GetAxis()
        }
        else
        {
            transform.position = GetMouseTarget();
        }
    }

    private Vector2 GetMouseTarget()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return target;
    }
}
