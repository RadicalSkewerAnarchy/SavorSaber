using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player2Input : MonoBehaviour
{
    public static Player2Input instance = null;
    public GameObject pressStartToJoin;
    public GameObject characterSelect;
    public bool playerTwoActive = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
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
