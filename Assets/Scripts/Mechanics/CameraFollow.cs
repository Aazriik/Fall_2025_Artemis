using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // X and Y position limits for the camera
    [SerializeField] private float minXPos = -118.15f;
    [SerializeField] private float maxXPos = 119.68f;
    [SerializeField] private float minYPos = -5.77f;
    [SerializeField] private float maxYPos = 5.73f;

    // Reference to the target to follow
    [SerializeField] private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If no target is assigned, try to find the player by tag
        if (!target)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player)
            {
                Debug.LogError("CameraFollow: No GameObject with tag 'Player' found in the scene.");
                return;
            }
            target = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Exit Parameters that will stop the function if not met
        if (!target) return;

        // Store the current position of the camera
        Vector3 pos = transform.position;
        // Clamp the camera's position to stay within the defined limits
        pos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);
        pos.y = Mathf.Clamp(target.position.y, minYPos, maxYPos);
        // Update the camera's position
        transform.position = pos;
    }
}
