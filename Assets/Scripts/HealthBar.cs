using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class HealthBar : MonoBehaviour
{
   

    private Slider Slider;
    public TextMeshProUGUI healthCounter;

    public GameObject playerState;

    private float maxHealth, currentHealth;
    void Awake()
    {
        Slider = GetComponent<Slider>();


    }

    
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;

        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;


        float healthPercent = currentHealth / maxHealth;

        Slider.value = healthPercent;

        healthCounter.text = currentHealth + " / " + maxHealth;
    }
}
