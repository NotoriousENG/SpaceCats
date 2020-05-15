using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class M_StartDolly : MonoBehaviour
{
    GameObject player;
    CinemachineDollyCart dollyCart;
    
    private void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dollyCart = GetComponent<CinemachineDollyCart>();
    }
    private void Update() 
    {
        DoToggleOn();
    }

    void DoToggleOn()
    {
        var dist = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));
        if (dist <= GameManager.enemyViewDist && !dollyCart.enabled)
        {
            dollyCart.enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (dollyCart.enabled && dist > GameManager.enemyViewDist)
        {
            dollyCart.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
