using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPos : MonoBehaviour
{
    public Transform objectToFollow;

    void Update()
    {
        transform.position = objectToFollow.position;
    }
}
