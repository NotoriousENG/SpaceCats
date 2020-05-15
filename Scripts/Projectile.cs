using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update

    [Tooltip("Projectile speed, in unity meters per second")]
    public float speed;
    [Tooltip("Time in seconds until object is destroyed")]
    public float lifetime;
    public AudioClip sound;
    void Start()
    {
        Destroy(this.gameObject, lifetime);
        if (TryGetComponent<AudioSource>(out var src))
        {
            src.PlayOneShot(sound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
