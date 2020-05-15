using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandFade : MonoBehaviour
{
    float start;
    float change;
    float maxChange = 5f;
    void Start()
    {
        start = transform.localScale.magnitude;
        change = maxChange * .9f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Vector3.one * change * Time.deltaTime;
        if (transform.localScale.magnitude > maxChange + start)
        {
            Destroy(gameObject);
        }
    }
}
