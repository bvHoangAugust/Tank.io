using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool isFollow;
    Transform playerTransform;
    // Update is called once per frame
    void Update()
    {
        if (isFollow)
        {
            FollowPlayer();
        }
    }

    public void SetUpTransformFollow(Transform tankIsMineTransform)
    {
        playerTransform = tankIsMineTransform;
        isFollow = true;
    }

    public void FollowPlayer()
    {
        if(playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
    }
}
