using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogBoxAdaptivePrompt : MonoBehaviour
{

    private TextMeshProUGUI prompt;
    private bool controllerMode;
    // Start is called before the first frame update
    void Start()
    {
        prompt = GetComponent<TextMeshProUGUI>();
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
            prompt.text = TextMacros.instance.Parse("{control,interact}") + " ->";
            //Debug.Log("Dialog box detects xbox controller");
        }
        else
        {
            prompt.text = TextMacros.instance.Parse("{control,interact}") + " ->";
            //Debug.Log("Dialog box detects keyboard");
        }
    }
}
