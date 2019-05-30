using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMacros : MonoBehaviour
{
    public const char delimLeft = '{';
    public const char delimRight = '}';
    public const char separator = ',';
    public static TextMacros instance;
    public delegate string MacroFn(params string[] args);

    private Dictionary<string, MacroFn> macroMap;

    private Dictionary<string, string> colorMap = new Dictionary<string, string>
    {
        // Flavors
        {"spicy", "#F16651" },
        {"sweet", "#EF9D9E" },
        {"bitter", "#8BC8FF" },
        {"sour", "#c1ff3c" },
        {"umami", "#FF8D00" },
        {"salty", "#BBAA87" },
        {"noFlavor", "#cccccc" },
        // Characters
        {"soma","#159DB2"},
        {"mana", "#DB5A78" },
        {"amrita", "#FEF4D1" },
        {"nec", "#D0FF78" },
    };

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            InitMacros();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void InitMacros()
    {
        macroMap = new Dictionary<string, MacroFn>()
        {
            {"test", (args) => "test" },
            {"flag", (args) => FlagManager.GetFlag(args[1]) },
            {"color", ColorMacro },
            {"c", ColorMacro },
            {"img", ImgMacro },
            {"control", ControlMacro },
            {"goalItem", GoalItemMacro },
            {"goalFruitant", GoalFruitantMacro },
            {"devourer", DevourerMacro },
            #region Character Name Shortcuts
            {"soma", (args) => ColorMacro("color","soma", "Soma") },
            {"mana", (args) => ColorMacro("color","mana", "Mana") },
            {"amrita", (args) => ColorMacro("color","amrita", "Amrita") },
            {"nec", (args) => ColorMacro("color","nec", "Nec") },
            {"nectar", (args) => ColorMacro("color","nec", "Nectar") },
            #endregion
            #region Flavor Name Shortcuts
            {"spicy",  (args) => ColorMacro("color","spicy", "Spicy") + " " + ImgMacro("img","IconSpicy") },
            {"sweet",  (args) => ColorMacro("color","sweet", "Sweet") + " " + ImgMacro("img","IconSweet") },
            {"bitter", (args) => ColorMacro("color","bitter", "Bitter") + " " + ImgMacro("img","IconBitter") },
            {"sour",   (args) => ColorMacro("color","sour", "Sour") + " " + ImgMacro("img","IconSour") },
            {"umami",  (args) => ColorMacro("color","umami", "Umami") + " " + ImgMacro("img","IconUmami") },
            {"salty",  (args) => ColorMacro("color","salty", "Salty") + " " + ImgMacro("img","IconSalty") },
            #endregion
        };
    }

    public string Parse(string line)
    {
        string output = string.Empty;
        for (int i = 0; i < line.Length; ++i)
        {
            char c = line[i];
            if (c == delimLeft)
            {
                int j = line.IndexOf(delimRight, i);
                string[] macro = line.Substring(i + 1, (j - i) - 1).Split(separator);
                if (macroMap.ContainsKey(macro[0]))
                    output += macroMap[macro[0]](macro);
                else
                    output += macro[0] + ": is an undefined macro ";
                i = j;
            }
            else
                output += c;

        }
        return output;
    }

    public string ColorMacro(params string[] args)
    {
        if (args.Length < 3)
            return "Error: format is {color, colorName, text}";
        return "<color=" + colorMap[args[1]] + ">" + args[2] + "</color>";
    }

    public string ImgMacro(params string[] args)
    {
        if (args.Length < 2)
            return "Error: format is {img, imgName}";
        return "<sprite=\"" + args[1] + "\" index=0> ";
    }

    public string GoalItemMacro(params string[] args)
    {
        if(FlagManager.GetFlag("goal") == "Desert")
            return "Golden Prickle Pear " + ImgMacro("img", "DropGoldenPricklepear");
        return "Golden Pear " + ImgMacro("img", "DropGoldenPear");            
    }

    public string GoalFruitantMacro(params string[] args)
    {
        if (FlagManager.GetFlag("goal") == "Desert")
            return "Prickle Patriarch";
        return "Paddle Patriarch";      
    }

    public string DevourerMacro(params string[] args)
    {
        return (FlagManager.GetFlag("goal") == "Desert") ? "Chinchilla" : "Raindeer";
    }

    private Dictionary<KeyCode, string> controlMap = new Dictionary<KeyCode, string>()
    {
        {KeyCode.J, "KeyJ" },
        {KeyCode.K, "KeyK" },
        {KeyCode.L, "KeyL" },
        {KeyCode.W, "KeyW" },
        {KeyCode.A, "KeyA" },
        {KeyCode.S, "KeyS" },
        {KeyCode.D, "KeyD" },
        {KeyCode.Z, "KeyZ" },
        {KeyCode.X, "KeyX" },
        {KeyCode.C, "KeyC" },
        {KeyCode.UpArrow, "KeyUp" },
        {KeyCode.DownArrow, "KeyDown" },
        {KeyCode.LeftArrow, "KeyLeft" },
        {KeyCode.RightArrow, "KeyRight" },
        {KeyCode.Space, "KeySpace" },
        {KeyCode.Joystick1Button0, "JoyA" },
        {KeyCode.Joystick1Button1, "JoyB" },
        {KeyCode.Joystick1Button2, "JoyX" },
        {KeyCode.Joystick1Button3, "JoyY" },
    };

    public string ControlMacro(params string[] args)
    {
        var keyBinds = InputManager.Controls.keyBinds;
        string imgKey = string.Empty;
        switch (args[1])
        {
            case "slash":
                var keySlash = keyBinds[Control.Knife];
                if (keySlash == KeyCode.None)
                    imgKey = "JoyRT";
                else
                    imgKey = controlMap[keySlash];
                break;
            case "skewer":
                var keySkewer = keyBinds[Control.Skewer];
                if (keySkewer == KeyCode.None)
                    imgKey = "JoyLT";
                else
                    imgKey = controlMap[keySkewer];
                break;
            case "throw":
                imgKey = controlMap[keyBinds[Control.Throw]];
                break;
            case "interact":
                imgKey = controlMap[keyBinds[Control.Interact]];
                break;
        }
        return ImgMacro("img", imgKey);
    }
}
