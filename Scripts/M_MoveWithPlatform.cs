using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_MoveWithPlatform : MonoBehaviour
{
    Dictionary<Transform, Transform> originalParents = new Dictionary<Transform, Transform>();
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.TryGetComponent<CharacterController>(out var controller) 
            || other.gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            if (originalParents.ContainsKey(other.transform))
            {
                originalParents[other.transform] = other.transform.parent;
            }
            else
            {
                originalParents.Add(other.transform, other.transform.parent);
            }
            other.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.TryGetComponent<CharacterController>(out var controller) 
            || other.gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            other.transform.parent = originalParents[other.transform];
        }
    }
}
