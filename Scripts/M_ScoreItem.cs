using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ScoreItem : MonoBehaviour
{
    public M_Score score;
    public int value = 500;
    // public InvCanvasManagment invManagement;
    // public AudioSource audio;
    // public AudioClip pickupSound;
    // public AudioClip rockSound;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<CharacterController>(out var controller))
        {
            // score.ModifyScore(value);
            GameManager.score += value;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        /* 
        if (other.tag == "Pickup")
        {
            playClip(pickupSound);
            invManagement.addPickupToUI(other.GetComponent<PickupItemDetails>().associatedSprite);
            Destroy(other.gameObject);
        }
        if (other.tag == "Rock")
        {
            playClip(rockSound);
            invManagement.addRockToUI();
            Destroy(other.gameObject);
        } */
    }

    /* private void playClip(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    } */
}
