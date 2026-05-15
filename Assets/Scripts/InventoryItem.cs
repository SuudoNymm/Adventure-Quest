using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // --- Is this item trashable --- //
    public bool isTrashable;

    // --- Item Info UI --- //
    private GameObject itemInfoUI;

    public string thisName, thisDescription, thisFunctionality;

    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float caloriesEffect;

    // --- Hotbar & Equipping --- //
    public bool isTool;
    public GameObject itemPrefab;

    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.itemInfoUI;
    }

    // Triggered when the mouse enters into the area of the item that has this script.
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        InventorySystem.Instance.itemNameText.text = thisName;
        InventorySystem.Instance.itemDescriptionText.text = thisDescription;
        InventorySystem.Instance.itemFunctionText.text = thisFunctionality;
    }

    // Triggered when the mouse exits the area of the item that has this script.
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        //Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            itemPendingConsumption = gameObject;

            if (isConsumable)
            {
                consumingFunction(healthEffect, caloriesEffect);
            }
        }
    }

    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemPendingConsumption == gameObject)
            {
                InventorySystem.Instance.itemInfoUI.SetActive(false);
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    private void consumingFunction(float healthEffect, float caloriesEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);
        caloriesEffectCalculation(caloriesEffect);
    }


    private static void healthEffectCalculation(float healthEffect)
    {
        // Health
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.currentHealth = maxHealth;
            }
            else
            {
                PlayerState.Instance.currentHealth = healthBeforeConsumption + healthEffect;
            }
        }
    }


    private static void caloriesEffectCalculation(float caloriesEffect)
    {
        // Calories
        float caloriesBeforeConsumption = PlayerState.Instance.currentFood;
        float maxCalories = PlayerState.Instance.maxFood;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                PlayerState.Instance.currentFood = maxCalories;
            }
            else
            {
                PlayerState.Instance.currentFood = caloriesBeforeConsumption + caloriesEffect;
            }
        }
    }
}
