using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    //public bool isFull;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        

        PopulateteSlotList();
    }

    private void PopulateteSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
           
            isOpen = false;
        }
    }

    public void AddItemToInventory(string itemName)
    {
          whatSlotToEquip = FindNextSlot();

          itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);

          itemToAdd.transform.SetParent(whatSlotToEquip.transform);

          itemList.Add(itemName);
        
    }

    private GameObject FindNextSlot() //Finds the next available slot by checking each slot in the slot list and returning the first one that is empty (has no child objects). If all slots are full, it returns a new GameObject (which should not happen if the inventory is properly managed).
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return new GameObject();

    }

    public bool CheckIfFull() //Checks if the inventory is full by counting how many slots are occupied and comparing it to the total number of slots.
    {
        int count = 0;

        foreach (GameObject slot in slotList) {

            if (slot.transform.childCount > 0)
            {
                count++;
            }

        }

        if (count >= slotList.Count)
        {

            return true;
        }
        else
        {
            return false;
        }

    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for(var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name.Contains(nameToRemove) && counter !=0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter--;
                }
            }

            if (counter <= 0)
            {
                break;
            }
        }

    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;

                
                string str2 = "(Clone)";

                string result = name.Replace(str2, "");

                itemList.Add(result);

            }
        }
    }




}

