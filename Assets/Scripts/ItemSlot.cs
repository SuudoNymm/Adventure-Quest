using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ItemSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{

    public GameObject Item
    {
        get
        {
            foreach (Transform child in transform)
            {
                if (child.name != "SlotNumber")
                {
                    return child.gameObject;
                }
            }

            return null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Item == null)
            {
                // Check if this slot is a quick slot
                if (InventorySystem.Instance.quickSlotList.Contains(gameObject))
                {
                    InventorySystem.Instance.UnequipItem();
                }
            }
        }
    }






    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        //if there is not item already then set our item.
        if (!Item)
        {

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

        }


    }




}
