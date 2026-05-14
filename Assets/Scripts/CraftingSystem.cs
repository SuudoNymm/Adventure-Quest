using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Text AxeReq1, AxeReq2;

    public bool isOpen;
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
        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(); });

    }

    void CraftAnyItem()
    {
        if (inventoryItemList.Contains("Wood") && inventoryItemList.Contains("Stone"))
        {
            inventoryItemList.Remove("Wood");
            inventoryItemList.Remove("Stone");
            inventoryItemList.Add("Axe");
            Debug.Log("Crafted an Axe!");
        }
        else
        {
            Debug.Log("You don't have the required items to craft an Axe.");
        }
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
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }

    }
}
