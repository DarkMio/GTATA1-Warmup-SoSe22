using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts;

public class HealthBar : MonoBehaviour
{
   private Image HealthBarRed;
   public float CurrentHealth;
   private float MaxHealth = 100f;
   //to communicate between scripts
   ShipController Player;
    
    private void Start()
    {
        //find image
        HealthBarRed = GetComponent<Image>();
        Player = FindObjectOfType<ShipController>();
    }

    private void Update()
    {
        // health bar updated
        CurrentHealth = Player.Health;
        HealthBarRed.fillAmount = CurrentHealth / MaxHealth;
    }

}
