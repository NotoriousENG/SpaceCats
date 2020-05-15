using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForChildren : MonoBehaviour
{
    public bool isRoot = false;
    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0)
        {
            if (!isRoot)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else if (isRoot && TryGetComponent<M_Health>(out var h))
            {
                if(h.enabled == false)
                {
                    h.enabled = true;
                }
            }
            
        }
    }
}
