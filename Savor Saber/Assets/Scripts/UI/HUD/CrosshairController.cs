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

    public ControllerMode mode = ControllerMode.Radial;
    [Header("Free Mode Settings")]
    public float crosshairSpeed = 10;
    [Header("Radial Mode Settings")]
    public float radius = 1;
    public Vector2 offset;

    private SpriteRenderer spr;
    private CrosshairClicker click;
    public List<GameObject> lastClickedOn;
    public Texture2D cursorTexture;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        click = GetComponent<CrosshairClicker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.ControllerMode)
        {
            var controllerInput = InputManager.GetAxesAsVector2(InputAxis.HorizontalAim, InputAxis.VerticalAim);
            if(mode == ControllerMode.Free)
            {
                controllerInput *= (crosshairSpeed * Time.deltaTime);
                transform.position += (Vector3)controllerInput;
            }
            else // Mode == Radial
            {
                if (controllerInput.sqrMagnitude >= 0.9f)
                {
                    var posOffset = (controllerInput.normalized * radius) + offset;
                    transform.position = transform.parent.position + (Vector3)posOffset;
                }                
            }
            spr.color = Color.white; 
        }
        else
        {
            //Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            transform.position = GetMouseTarget();

            if (InputManager.GetButtonDown(Control.Skewer, InputAxis.Skewer))
                lastClickedOn = click.ClickArea();
            if (InputManager.GetButton(Control.Skewer, InputAxis.Skewer))
                spr.color = new Color(1, 1, 1, 1);
            else spr.color = new Color(0, 0, 0, 1);
        }
    }

    private void LateUpdate()
    {
        if (InputManager.ControllerMode)
        {
            Vector3 viewPos = Camera.main.WorldToScreenPoint(transform.position);
            viewPos.x = Mathf.Clamp(viewPos.x, 0, Screen.width);
            viewPos.y = Mathf.Clamp(viewPos.y, 0, Screen.height);
            transform.position = Camera.main.ScreenToWorldPoint(viewPos);
        }
    }

    private Vector2 GetMouseTarget()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return target;
    }

    public Vector2 GetTarget()
    {
        if (InputManager.ControllerMode)
        {
            Vector2 target = gameObject.transform.position;
            return target;
        }
        else
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return target;
        }
    }

    public void SetVisible(bool isVisible)
    {
        spr.enabled = isVisible;
    }
}
