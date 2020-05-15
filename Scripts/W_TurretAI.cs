using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_TurretAI : MonoBehaviour
{


    public Transform player;
    public Projectile projProto;
    public Transform gunA;
    public Transform gunB;

    public bool isActivated = false;

    [Tooltip("The time the turret has to wait before shooting a\n" +
        "rapid-fire burst of projectiles.")]
    public float chargeTime = 1;

    [Tooltip("The time between bullets fired in a burst of projectiles")]
    public float shotInterval = 0.1f;

    [Tooltip("The number of shots per burst")]
    public int burstNum = 10;

    [Tooltip("The speed at which the turret turns when it is not shooting")]
    public float turnSpeed = 2;
    [Tooltip("The speed at which the turret during its rapid-fire burst")]
    public float shootTurnSpeed = 1;

    private float wait;

    private bool isShooting;
    private int shotsLeft;
    public float lookRange;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        wait = chargeTime;
        shotsLeft = burstNum;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        setActivation();
        doTurretAI();
    }

    void setActivation()
    {
        if ((player.transform.position - transform.position).magnitude < lookRange)
        {
            isActivated = true;
        }
        else
        {
            isActivated = false;
        }
    }

    private void OnBecameInvisible() {
        gameObject.SetActive(false);
    }
    void doTurretAI()
    {
        if (isActivated)
        {
            if (isShooting)
            {
                if (wait > 0.0f)
                {
                    wait -= Time.deltaTime;
                }
                else if ((shotsLeft % 2) == 1)
                {
                    Projectile shot = Instantiate(projProto);
                    shot.transform.SetPositionAndRotation(gunA.TransformPoint(0, 0, 0), gunA.rotation);

                    shot.gameObject.SetActive(true);
                    shotsLeft -= 1;
                }
                else if (shotsLeft > 0)
                {
                    Projectile shot = Instantiate(projProto);
                    shot.transform.SetPositionAndRotation(gunB.TransformPoint(0, 0, 0), gunB.rotation);

                    shot.gameObject.SetActive(true);
                    shotsLeft -= 1;
                }
                else
                {
                    isShooting = false;
                    shotsLeft = burstNum;
                    wait = chargeTime;
                }

                Vector3 targetDir = player.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, (shootTurnSpeed * Time.deltaTime), 0.0f);

                transform.rotation = Quaternion.LookRotation(newDir);

            }
            else if (wait > 0.0f)
            {
                wait -= Time.deltaTime;
                Vector3 targetDir = player.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, (turnSpeed * Time.deltaTime), 0.0f);

                transform.rotation = Quaternion.LookRotation(newDir);
            }
            else
            {
                isShooting = true;
            }
            
        }
    }
}
