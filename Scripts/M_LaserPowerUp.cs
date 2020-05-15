using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_LaserPowerUp : MonoBehaviour
{
    private void Update() {
        gameObject.transform.Rotate(Vector3.up, Space.World);
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag.Equals("Player"))
        {
            var src = Camera.main.GetComponent<AudioSource>();
            var Explosion = Resources.Load<AudioClip>("Music/Sounds/powerup");
            src.PlayOneShot(Explosion);

            GameManager.score += GameManager.pickupScore;
            if (other.TryGetComponent<M_PlayerShipController>(out var controller))
            {
                controller.changeLasers(true);
            }
            else if (other.TryGetComponent<W_PlayerMove>(out var move))
            {
                move.changeLasers(true);
            }
            Destroy(gameObject);
        }
    }
}
