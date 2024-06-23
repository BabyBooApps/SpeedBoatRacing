using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSc : MonoBehaviour
{
    public MapGrounds mapGrounds;
    //public int id;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !mapGrounds.isChanging)
        {
            mapGrounds.CenterTheGround(transform);
        }
    }
}
