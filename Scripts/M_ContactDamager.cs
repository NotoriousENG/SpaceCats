using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ContactDamager : MonoBehaviour
{
    public float damageAmmount;
    public bool onlyForPlayer = false;
    public bool destroyWhenDamaged = false;
    public bool disableOnContact = false;
    public GameObject originObject = null;

    private void Awake() 
    {
        originObject = gameObject;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<M_Health>(out M_Health objHealth))
        {
            // Debug.Log(other.name);
            foreach(var h in GetComponentsInParent<M_Health>())
            {
                if ((onlyForPlayer && !objHealth.gameObject.tag.Equals("Player")))
                {
                    return;
                }
                
            }
            if (originObject != null && 
            (
                (objHealth.gameObject == originObject || objHealth.transform.Find(originObject.name) != null)
                || (originObject.TryGetComponent<M_Health>(out var l) && l.isGunHealth && objHealth.isGunHealth))
            )
            {
                // Debug.Log("got here");
                return;
            }
            
            else if (objHealth.current >= 0)
            {
                if (!(onlyForPlayer && (! (objHealth.gameObject.tag.Equals("Player") || objHealth.gameObject.tag.Equals("PlayerGun")))))
                {
                    // Debug.Log(objHealth.gameObject.tag + ": " + onlyForPlayer.ToString());
                    objHealth.modifyHealth(-damageAmmount);
                }
                
                // Debug.Log(other.name);
            }

            // if (disableOnContact)
            // {
            //     GetComponent<Collider>().enabled = false;
            // }
            if (destroyWhenDamaged && objHealth.tag.Equals("PlayerWing"))
            {
                gameObject.SetActive(false);
            //     Destroy(gameObject);
            }
            
        }
    }
}
