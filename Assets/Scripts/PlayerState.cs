using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class PlayerState : MonoBehaviour
{
   
    public static PlayerState Instance { get; set; }


    //Player Health

    public float maxHealth;
    public float currentHealth;

    //Player Food
    public float maxFood;
    public float currentFood;

    float distanceTraveled = 0;
    Vector3 lastPosition;


    public GameObject playerBody;


    private void Start()
    {
        currentHealth = maxHealth;
        currentFood = maxFood;
    }


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

    // Update is called once per frame
    void Update()
    {
        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);

        lastPosition = playerBody.transform.position;

        if (distanceTraveled >= 100)
        {
            currentFood -= 1;
            distanceTraveled = 0;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;

        }

    }
}
