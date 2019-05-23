using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DialogBoxAdaptivePrompt : MonoBehaviour
{

    private Text prompt;
    private bool controllerMode;
    // Start is called before the first frame update
    void Start()
    {
        prompt = GetComponent<Text>();
        controllerMode = InputManager.ControllerMode;
        SetPrompt(controllerMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.ControllerMode)
        {
            if(!controllerMode)
            {
                controllerMode = true;
                SetPrompt(controllerMode);
            }
        }
        else
        {
            if(controllerMode)
            {
                controllerMode = false;
                SetPrompt(controllerMode);
            }
        }
    }

    private void SetPrompt(bool ControllerMode)
    {
        if(controllerMode)
        {
            prompt.text = "(A) ->";
            Debug.Log("Dialog box detects xbox controller");
        }
        else
        {
            prompt.text = "(Space) ->";
            Debug.Log("Dialog box detects keyboard");
        }
    }
}
