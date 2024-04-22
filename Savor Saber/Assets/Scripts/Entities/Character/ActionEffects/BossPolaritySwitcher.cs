using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPolaritySwitcher : MonoBehaviour
{
    public int timeToSwitch = 1;
    public Sprite blueSprite;
    public Sprite redSprite;
    public SpriteRenderer[] spritesToSwap;
    private WaitForSeconds SwitchTimer;
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        SwitchTimer = new WaitForSeconds(timeToSwitch);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePolarity(bool autoSwap, RadialAttack.BulletColors color)
    {
        foreach (SpriteRenderer sr in spritesToSwap)
        {
            switch (color)
            {
                case RadialAttack.BulletColors.Red:
                    sr.sprite = redSprite;
                    break;
                case RadialAttack.BulletColors.Blue:
                    sr.sprite = blueSprite;
                    break;
                default:
                    return;
            }



        }
    }
}
