using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public float crosshairSpeed = 10;
    private Vector2 controllerInput;
    private RectTransform rt;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.ControllerMode)
        {
            controllerInput = InputManager.GetAxesAsVector2(InputAxis.HorizontalAim, InputAxis.VerticalAim);
            controllerInput *= (crosshairSpeed * Time.deltaTime);
            rt.localPosition += (Vector3)controllerInput;
            image.color = Color.white;
        }
        else
        {
            transform.position = GetMouseTarget();
            image.color = new Color(0, 0, 0, 0);
        }
    }

    private Vector2 GetMouseTarget()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return target;
    }
}
