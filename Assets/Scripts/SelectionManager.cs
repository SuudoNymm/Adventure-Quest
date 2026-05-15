using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public GameObject interaction_Info_UI;
    TMP_Text interaction_text;

    private TreeReplacer treeReplacer;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
        treeReplacer = Object.FindAnyObjectByType<TreeReplacer>();
    }

    void Update()
    {
        // For First Person, we usually cast from the center of the screen
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Ignore triggers on the first cast to avoid hitting our own range triggers or others
        if (Physics.Raycast(ray, out hit, 10f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponentInParent<InteractableObject>();
            if (interactable && interactable.PlayerInRange)
            {
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else if (selectionTransform.name == "Terrain" && treeReplacer != null)
            {
                // Check if we are looking at a rock that can be replaced
                if (Vector3.Distance(mainCam.transform.position, hit.point) <= treeReplacer.interactionDistance)
                {
                    treeReplacer.PerformRaycastReplacement(ray);
                }
                interaction_Info_UI.SetActive(false);
            }
            else 
            {
                interaction_Info_UI.SetActive(false);
            }
        }
        else
        {
            interaction_Info_UI.SetActive(false);
        }
    }

}




