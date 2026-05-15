using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{

    public GameObject crafting_UI;
    public GameObject toolScreenUI;
    public List<string> inventoryItemList = new List<string>();

    //Category Buttons

    Button toolsBTN;

    //Craft Buttons
    Button craftAxeBTN;


    //Required Items for Crafting
    TextMeshProUGUI AxeReq1, AxeReq2;

    public bool isOpen;

    //Blueorints for items to be crafted

    public ItemBlueprint axeBlueprint = new ItemBlueprint("Axe", "Stone", 3, "Wood", 3, 2);


    public static CraftingSystem Instance { get; set; }

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOpen = false;

        toolsBTN = crafting_UI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory();});

        //Axe Crafting
        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TextMeshProUGUI>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TextMeshProUGUI>();

        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(axeBlueprint); });

    }

    void CraftAnyItem(ItemBlueprint blueprintToCraft)
    {
        InventorySystem.Instance.AddItemToInventory(blueprintToCraft.itemName);


        
        if (blueprintToCraft.numOfRequirments == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);

        }
        else if (blueprintToCraft.numOfRequirments == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
        }

        


        InventorySystem.Instance.ReCalculateList();


        RefreshNeededItems();
    }

    void OpenToolsCategory()
    {
        crafting_UI.SetActive(false);
        toolScreenUI.SetActive(true);
    }    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

           
            crafting_UI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            crafting_UI.SetActive(false);
            toolScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;

        }

        if (isOpen)
        {
            RefreshNeededItems();
        }

    }

    private void RefreshNeededItems()
    {
        int stoneCount = 0;
        int woodCount = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stoneCount++;
                    break;
                case "Wood":
                    woodCount++;
                    break;
            }
            
        
        }

        //Axe

        if (AxeReq1 != null) AxeReq1.text = "Stone: " + stoneCount + "/3";
        if (AxeReq2 != null) AxeReq2.text = "Wood: " + woodCount + "/3";

        if(stoneCount >= 3 && woodCount >= 3)
        {
            if (craftAxeBTN != null) craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            if (craftAxeBTN != null) craftAxeBTN.gameObject.SetActive(false);
        }
    }
}
