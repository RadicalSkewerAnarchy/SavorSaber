using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public enum ControllerMode
    {
        Free,
        Radial,
    }


    [Header("Free Mode Settings")]
    public float crosshairSpeed = 10;
    [Header("Radial Mode Settings")]
    public float radius = 1;
    public float yOffset = 40;

    private ControllerMode mode = ControllerMode.Radial;
    private Vector2 controllerInput;
    private RectTransform rt;
    private Image image;

    private Vector2 screenBounds;

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
            if(mode == ControllerMode.Free)
            {
                controllerInput *= (crosshairSpeed * Time.deltaTime);
                rt.localPosition += (Vector3)controllerInput;
            }
            else // Mode == Radial
            {
                if (controllerInput.sqrMagnitude >= 0.9f)
                {
                    rt.localPosition = (controllerInput * radius) + new Vector2(0, yOffset);
                }
                    
            }
            image.color = Color.white; 
        }
        else
        {
            transform.position = GetMouseTarget();
            image.color = new Color(0, 0, 0, 0);
        }
    }

    private void LateUpdate()
    {
        if (InputManager.ControllerMode)
        {
            Vector3 viewPos = rt.position;
            viewPos.x = Mathf.Clamp(viewPos.x, 0, Screen.width);
            viewPos.y = Mathf.Clamp(viewPos.y, 0, Screen.height);
            rt.position = viewPos;
        }
        //Vector3 viewPos = transform.localPosition;
        //viewPos.x = Mathf.Clamp(viewPos.x, 0, Screen.width);
        //viewPos.y = Mathf.Clamp(viewPos.y, 0, Screen.height);
        //transform.localPosition = viewPos;
        //Debug.Log("screen width: " + Screen.width);
        //Debug.Log("screen height: " + Screen.height);
        //Debug.Log("rect absolute: " + rt.position);
    }

    private Vector2 GetMouseTarget()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return target;
    }
}
