using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<M_Health>(out var health) && other.tag.Equals("Player"))
        {
            health.respawnPos = transform.position;
        }
    }
}
