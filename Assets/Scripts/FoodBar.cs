using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FoodBar : MonoBehaviour
{


    private Slider Slider;
    public TextMeshProUGUI foodCounter;

    public GameObject playerState;

    private float maxFood, currentFood;
    void Awake()
    {
        Slider = GetComponent<Slider>();


    }


    void Update()
    {
        currentFood = playerState.GetComponent<PlayerState>().currentFood;

        maxFood = playerState.GetComponent<PlayerState>().maxFood;


        float FoodPercent = currentFood / maxFood;

        Slider.value = FoodPercent;

        foodCounter.text = currentFood + " / " + maxFood;
    }
}