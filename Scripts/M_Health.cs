using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_Health : MonoBehaviour
{
    public float current;
    public float capacity;
    public bool respawn = true;
    private M_PlayerController controller = null;
    private W_PlayerMove movement = null;
    public Vector3 respawnPos;
    private Animator animator;
    private M_FadeOut fadeOut;
    public GameObject spawn;
    public float chanceToSpawn;
    public bool isGunHealth;
    static int scoreAmount = 100;

    private void Start() 
    {
        current = capacity;
        respawnPos = transform.position;
        TryGetComponent<M_PlayerController>(out controller);
        TryGetComponent<W_PlayerMove>(out movement);
        TryGetComponent<Animator>(out animator);
        TryGetComponent<M_FadeOut>(out fadeOut);
    }

    public void modifyHealth(float amount)
    {
        // TODO: spot for animation
        current += amount;
        if (current > capacity)
        {
            current = capacity;
        }
        else if (current <= 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDeath"))
        {
            animator.SetTrigger("Die");
            if (respawn)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                
                // // controller.grounded = true;
                // controller.zeroMovement = true;
                // // kill object after animation finishes
                // Invoke("respawnObject", animator.GetCurrentAnimatorStateInfo(0).length);
                //respawnObject();
            } else
            {
                GameManager.score += GameManager.enemySlayScore;
                if (TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    //gameObject.SetActive(false);
                    if (isGunHealth)
                    {
                        var parent = gameObject.transform.parent.gameObject;
                        if (parent.TryGetComponent<M_PlayerShipController>(out var controller))
                        {
                            controller.changeLasers(false);
                        }
                        else if (parent.TryGetComponent<W_PlayerMove>(out var movement))
                        {
                            movement.changeLasers(false);
                        }
                        gameObject.SetActive(false);
                        //GetComponentInParent<W_PlayerMove>().changeLasers(false);
                    }
                    return;
                }
                // kill object after animation finishes
                killObject();
                Invoke("killObject", animator.GetCurrentAnimatorStateInfo(0).length);
            }
        }
    }

    public void killObject()
    {
        // TODO: spot for animation
        SpawnObject();
        var src = Camera.main.GetComponent<AudioSource>();
        var Explosion = Resources.Load<AudioClip>("Music/Sounds/Explosion7");
        src.PlayOneShot(Explosion);
        // Debug.Log("Playing");
        var sprite = Resources.Load<GameObject>("Explosion");
        Instantiate(sprite, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void respawnObject()
    {
        if (fadeOut != null)
            {
                fadeOut.RestartFade(false);
            }
        // TODO: Spot for animation
        if (controller != null)
        {
            Invoke("Reposition", 1.0f);
            Invoke("enableController", 2.5f);
        }
        
    }

    void Reposition()
    {
        transform.position = respawnPos;
        if (fadeOut != null)
            {
                fadeOut.RestartFade(true);
            }
        animator.SetTrigger("Respawn");
    }
    void enableController()
    {
        current = capacity;
        controller.zeroMovement = false;
    }
    void SpawnObject()
    {
        var chance = Random.value;
        if (chance <= chanceToSpawn)
        {
            Instantiate(spawn, transform.position + new Vector3(0,1,0), Quaternion.identity);
        }
    }
    
}
