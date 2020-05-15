using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class M_ProjectileEmitter : MonoBehaviour
{
    private float NextAttackAllowed = 0;
    
    /// <summary>
    /// Fires a projectile from this GameObject.
    /// NextAttackAllowed is the next time that 
    /// this object can fire.
    /// Cooldown is how long until the next attack.
    /// </summary>
    public void FireProj(GameObject projPrefab, float Cooldown, out Projectile parameters)
    {
        parameters = null;
        if (Time.time > NextAttackAllowed)
        {
            var projectile = Instantiate(projPrefab);
            projectile.tag = "Player";
            projectile.GetComponent<M_ContactDamager>().originObject = gameObject;
            projectile.TryGetComponent<Projectile>(out parameters);
            // Debug.Log(gameObject.name);
            // projectile.transform.SetParent(transform);
            projectile.transform.SetPositionAndRotation(transform.TransformPoint(0, 0, 0), transform.rotation);
            projectile.gameObject.SetActive(true);
            
            NextAttackAllowed = Time.time + Cooldown;
        }
    }
}

