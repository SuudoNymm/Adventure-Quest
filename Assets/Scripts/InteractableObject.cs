using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;

    public bool PlayerInRange;

    public string GetItemName()
    {
        return ItemName;
    }

    private void Start()
    {
        // Check if player is already in range (e.g. if spawned right next to player)
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f); // 2f is the radius I set earlier
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                PlayerInRange = true;
                break;
            }
        }
    }

    private void Update()
{
        if (Input.GetKeyDown(KeyCode.Mouse0) && PlayerInRange)
        {
            if (!InventorySystem.Instance.CheckIfFull())
            {
                InventorySystem.Instance.AddItemToInventory(ItemName);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
            
        }

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}