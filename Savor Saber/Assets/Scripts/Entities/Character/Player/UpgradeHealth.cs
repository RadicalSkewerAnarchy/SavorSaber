using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHealth : MonoBehaviour
{
    public Sprite replaceImage;
    public void Upgrade()
    {
        var data = PlayerController.instance.GetComponent<PlayerData>();
        data.maxHealth = 6;
        data.health = data.maxHealth;
        DisplayInventory.instance.HealthBarImage.sprite = replaceImage;
    }
}
