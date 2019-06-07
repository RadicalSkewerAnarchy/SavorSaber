using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    Neutral,
    NoSprite,
    Silhouette,
    SomaDetermined,
    SomaGasp,
    SomaGlad,
    SomaKind,
    SomaWorried,
    ManaDistant,
    ManaDistant2,
    ManaGlad,
    ManaHappy,
    ManaResigned,
    ManaShooketh,
    ManaSilhoutteEdge,
    ManaTehe,
    ManaThink,
    ManaUpset,
    AmritaHmm,
    AmritaMad,
    AmritaSmile,
    NecBigSmile,
    NecOmg,
    NecSad,
    NecSmile,
    ManaEmbarassed,
    ManaTsun,
    AmritaSurprised,
    AmritaSerious,
    AmritaGrim,
}

[System.Serializable]
public class DialogItem
{
    public string text;
    public Emotion emotion; 
    public DialogItem(string text, Emotion emotion)
    {
        this.text = text;
        this.emotion = emotion;
    }
}
