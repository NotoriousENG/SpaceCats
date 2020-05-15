using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_EventTrigger : MonoBehaviour
{

    public List<W_Event> events;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger!");
        if (other.tag == "Player")
        {
            Debug.Log("Entered player trigger!");
            for (int i = 0; i < events.Count; i++)
            {
                Debug.Log("Triggering event");
                events[i].Activate();
            }
        }
        else
        {
            Debug.Log("??");
        }
    }
}
