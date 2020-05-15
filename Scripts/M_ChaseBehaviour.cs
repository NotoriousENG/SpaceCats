using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ChaseBehaviour : MonoBehaviour
{
    private GameObject Player;
    public float speed = 7;
    public bool lookAt = false;
    private void Awake() 
    {
        Player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        DoChase();
    }
    void DoChase()
    {
        transform.position =  Vector3.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
        if (lookAt)
        {
            transform.LookAt(Player.transform);
        }
    }
}
