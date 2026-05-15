using System.Collections;
using UnityEngine;

public class EquippedTool : MonoBehaviour
{
    [Header("Swing Settings")]
    public Vector3 swingRotation = new Vector3(45, 0, 0);
    public float swingSpeed = 10f;
    public float returnSpeed = 5f;

    private Quaternion startRotation;
    private bool isSwinging = false;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSwinging && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen)
        {
            StartCoroutine(SwingRoutine());
        }
    }

    void PerformHit()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3.0f)) // Interaction range for hitting
        {
            ChoppableTree tree = hit.transform.GetComponentInParent<ChoppableTree>();
            if (tree != null)
            {
                tree.TakeDamage(1);
                Debug.Log("Hit tree!");
            }
        }
    }

    IEnumerator SwingRoutine()
{
        isSwinging = true;

        // Perform hit detection at the start or during the swing
        PerformHit();

        Quaternion targetRotation = startRotation * Quaternion.Euler(swingRotation);

        // Swing down
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * swingSpeed;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        // Small pause at the peak of the swing
        yield return new WaitForSeconds(0.05f);

        // Return to start
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;
            transform.localRotation = Quaternion.Slerp(targetRotation, startRotation, t);
            yield return null;
        }

        transform.localRotation = startRotation;
        isSwinging = false;
    }
}
