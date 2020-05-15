using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_SingleBoid_AI : MonoBehaviour
{

    private Vector3 velocity = Vector3.zero;


    // shooting stuff here later
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 getVelocity()
    {
        return velocity;
    }

    public void setVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    public void localMove(float speed)
    {
        transform.localPosition += velocity * speed * Time.deltaTime;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
}
