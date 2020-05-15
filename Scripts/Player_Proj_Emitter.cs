using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Proj_Emitter : MonoBehaviour
{

    public Projectile projProto;

    public float shootWait;
    public Transform singleGun;
    public Transform gunA;
    public Transform gunB;
    //[Tooltip("If true, both guns fire simultaneously, effectively doubling number of shots.\n" +
    //    "If false, guns alternate shooting.")]
    //public bool doubleShot;


    private float currWait;

    // Start is called before the first frame update
    void Start()
    {
        currWait = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currWait > 0.0f)
        {
            currWait -= Time.deltaTime;
        }
        else if (Input.GetButton("Fire1")) //I don't know why it's asking for an axis and not a key, but ok
        {
            Projectile shot;
            if (singleGun.gameObject.activeInHierarchy)
            {
                shot = Instantiate(projProto);
                shot.transform.SetPositionAndRotation(singleGun.TransformPoint(0, 0, 0), singleGun.rotation);

                shot.gameObject.SetActive(true);
               
            }
            else {

                if (gunA.gameObject.activeInHierarchy)
                {
                    shot = Instantiate(projProto);
                    shot.transform.SetPositionAndRotation(gunA.TransformPoint(0, 0, 0), gunA.rotation);

                    shot.gameObject.SetActive(true);
                }
                if (gunB.gameObject.activeInHierarchy)
                {
                    shot = Instantiate(projProto);
                    shot.transform.SetPositionAndRotation(gunB.TransformPoint(0, 0, 0), gunB.rotation);

                    shot.gameObject.SetActive(true);
                }
            } 

            currWait = shootWait;
        }
    }
}
