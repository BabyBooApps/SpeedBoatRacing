using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontShocDet : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Cop"))
        {
            transform.parent.GetComponent<PlayerBehavior>().OnDamage();
        }
    }
}
