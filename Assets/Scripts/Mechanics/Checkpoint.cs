using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint: MonoBehaviour
{
    private void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Checkpoint reached!");
            GameManager.Instance.UpdateCheckpoint(transform.position);
        }
    }
}
