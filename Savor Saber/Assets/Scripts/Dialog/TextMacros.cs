﻿using System.Collections;
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
        {"spicy", "#FF2300" },
        {"sweet", "#EF9D9E" },
        {"bitter", "#8BC8FF" },
        {"sour", "#05937C" },
        {"umami", "#FF8D00" },
        {"salty", "#968356" },
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
            instance = this;
            DontDestroyOnLoad(transform);
        }
        
    }

    private void InitMacros()
    {
        macroMap = new Dictionary<string, MacroFn>()
        {
            {"test", (args) => "test" },
            {"flag", (args) => FlagManager.GetFlag(args[1]) },
            {"color", ColorMacro },
            {"img", ImgMacro },
            #region Character Name Shortcuts
            {"soma", (args) => ColorMacro("color","soma", "Soma") },
            {"mana", (args) => ColorMacro("color","mana", "Mana") },
            {"amrita", (args) => ColorMacro("color","amrita", "Amrita") },
            {"nec", (args) => ColorMacro("color","nec", "Nec") },
            #endregion
            #region Flavor Name Shortcuts
            {"spicy",  (args) => ColorMacro("color","spicy", "Spicy") + ImgMacro("img","IconSpicy") },
            {"sweet",  (args) => ColorMacro("color","sweet", "Sweet") + ImgMacro("img","IconSweet") },
            {"bitter", (args) => ColorMacro("color","bitter", "Bitter") + ImgMacro("img","IconBitter") },
            {"sour",   (args) => ColorMacro("color","sour", "Sour") + ImgMacro("img","IconSour") },
            {"umami",  (args) => ColorMacro("color","umami", "Umami") + ImgMacro("img","IconUmami") },
            {"salty",  (args) => ColorMacro("color","salty", "Salty") + ImgMacro("img","IconSalty") },
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
            return "Error: format is {color, imgName}";
        return " <sprite=\"" + args[1] + "\" index=0> ";
    }
}
