using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public GameObject itemInfoUI;
    public TMPro.TextMeshProUGUI itemNameText;
    public TMPro.TextMeshProUGUI itemDescriptionText;
    public TMPro.TextMeshProUGUI itemFunctionText;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    //public bool isFull;

    //Pickup Popup

    public GameObject pickupAlert;
    public TextMeshProUGUI pickupName;
    public Image pickupImage;

    public List<GameObject> quickSlotList = new List<GameObject>();
    public Transform weaponHolder;
    private GameObject currentEquippedItem;

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
        PopulateQuickSlotList();
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

    private void PopulateQuickSlotList()
    {
        GameObject quickSlotPanel = GameObject.Find("QuickSlotPanel");
        if (quickSlotPanel != null)
        {
            foreach (Transform child in quickSlotPanel.transform)
            {
                if (child.name.StartsWith("QuickSlot"))
                {
                    quickSlotList.Add(child.gameObject);
                }
            }
        }

        if (weaponHolder == null)
        {
            GameObject holder = GameObject.Find("WeaponHolder");
            if (holder != null) weaponHolder = holder.transform;
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            itemInfoUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            itemInfoUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
           
            isOpen = false;
        }

        // Hotbar input
        for (int i = 0; i < quickSlotList.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipItem(i);
            }
        }
    }

    private void EquipItem(int index)
    {
        if (index >= quickSlotList.Count) return;

        GameObject slot = quickSlotList[index];
        GameObject itemObj = GetItemInSlot(slot);

        if (itemObj != null)
        {
            InventoryItem item = itemObj.GetComponent<InventoryItem>();

            if (item != null && item.isTool && item.itemPrefab != null)
            {
                // Unequip current
                if (currentEquippedItem != null)
                {
                    Destroy(currentEquippedItem);
                }

                // Equip new
                currentEquippedItem = Instantiate(item.itemPrefab, weaponHolder);
                currentEquippedItem.transform.localPosition = Vector3.zero;
                currentEquippedItem.transform.localRotation = Quaternion.identity;
                
                // Add swing functionality
                if (currentEquippedItem.GetComponent<EquippedTool>() == null)
                {
                    currentEquippedItem.AddComponent<EquippedTool>();
                }

                // Disable interaction components on held item
    InteractableObject io = currentEquippedItem.GetComponent<InteractableObject>();
                if (io != null) io.enabled = false;
                
                Debug.Log("Equipped: " + item.thisName);
            }
            else
            {
                Debug.Log("Cannot equip this item.");
            }
        }
        else
        {
            UnequipItem();
        }
    }

    public void UnequipItem()
    {
        if (currentEquippedItem != null)
        {
            Destroy(currentEquippedItem);
            currentEquippedItem = null;
            Debug.Log("Unequipped item.");
        }
    }

    private GameObject GetItemInSlot(GameObject slot)
    {
        foreach (Transform child in slot.transform)
        {
            if (child.name != "SlotNumber")
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public void AddItemToInventory(string itemName, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            whatSlotToEquip = FindNextSlot();
            if (whatSlotToEquip == null) break; // Inventory full

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            itemList.Add(itemName);

            if (i == 0) // Only trigger popup for the first item to avoid spam
            {
                string popupName = amount > 1 ? itemName + " x" + amount : itemName;
                TriggerPickupPop(popupName, itemToAdd.GetComponent<Image>().sprite);
            }
        }
    }

    void TriggerPickupPop(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        pickupName.text = itemName;

        pickupImage.sprite = itemSprite;

        StartCoroutine(HidePickupAlertAfterDelay(2f));
    }

    private IEnumerator HidePickupAlertAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);


        pickupAlert.SetActive(false);

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

        // Check main inventory
        for(var i = slotList.Count - 1; i >= 0; i--)
        {
            GameObject itemObj = GetItemInSlot(slotList[i]);
            if (itemObj != null)
            {
                if (itemObj.name.Contains(nameToRemove) && counter !=0)
                {
                    Destroy(itemObj);
                    counter--;
                }
            }
            if (counter <= 0) break;
        }

        if (counter > 0)
        {
            // Check quick slots
            for (var i = quickSlotList.Count - 1; i >= 0; i--)
            {
                GameObject itemObj = GetItemInSlot(quickSlotList[i]);
                if (itemObj != null)
                {
                    if (itemObj.name.Contains(nameToRemove) && counter != 0)
                    {
                        Destroy(itemObj);
                        counter--;
                    }
                }
                if (counter <= 0) break;
            }
        }
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            GameObject itemObj = GetItemInSlot(slot);
            if (itemObj != null)
            {
                string name = itemObj.name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }

        foreach (GameObject slot in quickSlotList)
        {
            GameObject itemObj = GetItemInSlot(slot);
            if (itemObj != null)
            {
                string name = itemObj.name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }




}

