using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player2Input : MonoBehaviour
{
    public GameObject pressStartToJoin;
    public GameObject characterSelect;
    bool playerTwoActive = false;
    // Update is called once per frame
    void Update()
    {
        var names = Input.GetJoystickNames();
        if (names.Where((s) => !string.IsNullOrWhiteSpace(s)).Count() < 2)
            return;
        if(!playerTwoActive)
        {
            pressStartToJoin.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Joystick2Button7))
            {
                characterSelect.SetActive(true);
                playerTwoActive = true;
                pressStartToJoin.SetActive(false);
            }
        }
    }
}
