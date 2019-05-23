using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AdaptiveSkewerSprite : MonoBehaviour
{
    public Sprite controllerSprite;
    public Sprite keySprite;
    private Image img;
    private bool controllerMode;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        controllerMode = InputManager.ControllerMode;
        SetImage(controllerMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.ControllerMode)
        {
            if (!controllerMode)
            {
                controllerMode = true;
                SetImage(controllerMode);
            }
        }
        else
        {
            if (controllerMode)
            {
                controllerMode = false;
                SetImage(controllerMode);
            }
        }
    }

    private void SetImage(bool ControllerMode)
    {
        if (controllerMode)
        {
            img.sprite = controllerSprite;
        }
        else
        {
            img.sprite = keySprite;
        }
    }
}
