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
        string[] names = Input.GetJoystickNames();

        if(names.Length == 0)
        {
            prompt.text = "(Space) ->";
            Debug.Log("Dialog box detects keyboard");
        }
        else
        {
            prompt.text = "(Y) ->";
            Debug.Log("Dialog box detects xbox controller");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
