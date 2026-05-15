using UnityEngine;

public class ChoppableTree : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject logsPrefab;
    public GameObject stumpPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            FellTree();
        }
    }

    private void FellTree()
    {
        if (stumpPrefab != null)
        {
            GameObject stump = Instantiate(stumpPrefab, transform.position, transform.rotation);
            stump.transform.localScale = transform.localScale;
        }

        if (logsPrefab != null)
        {
            // Offset the logs slightly so they don't overlap with the stump
            // We'll use a random direction in the XZ plane
            float angle = Random.Range(0f, 360f);
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 offset = direction * 2.0f * transform.localScale.x; 
            
            GameObject logs = Instantiate(logsPrefab, transform.position + offset, Quaternion.Euler(0, Random.Range(0, 360), 0));
            logs.transform.localScale = transform.localScale;
        }

        Destroy(gameObject);
    }
}
