using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandBar : MonoBehaviour
{

    public Text[] numbers;
    private Color grey = new Color(0.75f, 0.75f, 0.75f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(Control.Command1))
        {
            numbers[0].color = Color.cyan;
            numbers[1].color = grey;
            numbers[2].color = grey;
            numbers[3].color = grey;
        }
        else if (InputManager.GetButtonDown(Control.Command2))
        {
            numbers[0].color = grey;
            numbers[1].color = Color.cyan;
            numbers[2].color = grey;
            numbers[3].color = grey;
        }
        else if (InputManager.GetButtonDown(Control.Command3))
        {
            numbers[0].color = grey;
            numbers[1].color = grey;
            numbers[2].color = Color.cyan;
            numbers[3].color = grey;
        }
        else if (InputManager.GetButtonDown(Control.Command4))
        {
            numbers[0].color = grey;
            numbers[1].color = grey;
            numbers[2].color = grey;
            numbers[3].color = Color.cyan;
        }
    }
}
