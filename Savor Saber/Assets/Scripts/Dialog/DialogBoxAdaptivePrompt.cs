using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DialogBoxAdaptivePrompt : MonoBehaviour
{

    private Text prompt;
    // Start is called before the first frame update
    void Start()
    {
        prompt = GetComponent<Text>();

        if(InputManager.ControllerMode)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
