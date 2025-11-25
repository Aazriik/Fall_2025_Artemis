using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    public GameObject[] pickupPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int randNum = Random.Range(0, pickupPrefabs.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
