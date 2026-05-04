using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public GameObject interaction_Info_UI;
    TMP_Text interaction_text;

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))//
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().PlayerInRange)
            {
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else // if the object doesn't have the InteractableObject script, hide the UI
            {
                interaction_Info_UI.SetActive(false);
            }
        }
        else// if the raycast doesn't hit anything, hide the UI
        {
            interaction_Info_UI.SetActive(false);
        }
    }

}




