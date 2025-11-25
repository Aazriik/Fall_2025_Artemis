using System;
using UnityEngine;

public class CameraFollow: MonoBehaviour
{
    [SerializeField] private float minXPos = -4.5f;
    [SerializeField] private float maxXPos = 233.3f;
    [SerializeField] private float minYPos = -5.8f;
    [SerializeField] private float maxYPos = 5.7f;

    //private - private to the class - you would use this for variables that should not be accessed outside of this class. We can add the serializefield attribute to make it show up in the inspector
    //public - public to everyone - you would use this for variables or methods that need to be accessed from other classes
    //protected - protected to this class and derived classes - you would use this for variables or methods that should only be accessible within this class and any subclasses that inherit from it

    [SerializeField] private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager.Instance.OnPlayerSpawned += UpdatePlayerRef;
        //MAKE YOUR CODE DEFENSIVE AGAINST BAD INPUT!!
        //if (!target)
        //{
        //    GameObject player = GameObject.FindGameObjectWithTag("Player");
        //    if (!player)
        //    {
        //        Debug.LogError("CameraFollow: No GameObject with tag 'Player' found in the scene.");
        //        return;
        //    }
        //    target = player.transform;
        //}
    }

    private void UpdatePlayerRef(PlayerController playerInstance)
    {
        target = playerInstance.transform;
    }


    // Update is called once per frame
    void Update()
    {
        //exit parameters that will stop the function from continuing if these things happen
        if (!target)
            return;

        //store our current position
        Vector3 pos = transform.position;
        //update the x position to match the target's x position
        pos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);
        pos.y = Mathf.Clamp(target.position.y, minYPos, maxYPos);
        //apply the updated position back to the transform
        transform.position = pos;
    }
}
