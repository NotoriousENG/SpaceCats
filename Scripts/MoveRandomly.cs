using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomly : MonoBehaviour
{
    public Transform Anchor;
    Vector3 nextPos;
    public float maxDist = 1;
    public float speed =1;
    GameObject player;

    private void Awake() 
    {
        getNextPos();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update() 
    {
        DoMove();
        transform.LookAt(player.transform);
    }

    void DoMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        if (transform.position == nextPos)
        {
            getNextPos();
        }
    }

    void getNextPos()
    {
        nextPos = new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f)).normalized * maxDist + Anchor.position;
    }
}
