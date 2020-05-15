using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**************************************************************************************************
 *                                                                                                *
 * This script is heavily inspired by Mix and Jam's tutorial on starfox-like on-rails movement.   *
 * https://www.youtube.com/watch?v=JVbr7osMYTo                                                    *
 *                                                                                                *
 **************************************************************************************************
 */
public class W_PlayerMove : MonoBehaviour
{
    //private GameObject playerPlane;

    [System.Serializable]
    public class ShipParts
    {
        public GameObject LeftGun, RightGun, LeftWing, RightWing, CenterGun;
        public float noWingMovementScale = 0.9f;
    }
    public ShipParts shipParts;
    public float turnModifier;

    Vector2 input = Vector2.zero;

    [Header("Settings")]
    public bool inverted = true;

    [Space]

    [Header("Parameters")]
    public float xySpeed = 18;
    public float lookSpeed = 340;
    public float forwardSpeed = 6;
    public float maxZTilt = 60;
    [Range(0, 0.5f)]
    public float cameraBorderX;
    [Range(0, 0.5f)]
    public float cameraBorderY;

    [Space]

    [Header("Public References")]
    public Transform aimTarget;
    public Camera followCam;
    public CinemachineDollyCart dolly;
    public GameObject projectile;
    public float projectileCooldown;
    void Start()
    {
        //playerPlane = transform.parent.gameObject; ; ;
        SetSpeed(forwardSpeed);
    }


    private void Update()
    {
        DoInput(out float h, out float v);

        LocalMove(h, v, xySpeed);
        RotationLook(h, v, lookSpeed);
        HorizontalLean(transform, h, maxZTilt, 0.1f);
        DoAttack();
    }

    void DoInput(out float h, out float v)
    {
        h = Input.GetAxis("Horizontal");
        v = inverted ? Input.GetAxis("Vertical") : (-1 * Input.GetAxis("Vertical"));
        input = new Vector2(h, v);
        input = input.normalized;
        RestrictInput();
    }

    void RestrictInput()
    {
        if ((!shipParts.RightWing.activeSelf && input.x >= 0) 
            || (!shipParts.LeftWing.activeSelf && input.x <= 0))
        {
            turnModifier = shipParts.noWingMovementScale; // make turning harder
            return;
        }
        turnModifier = 1.0f;
    }

    /// <summary>
    ///  Toggles between powered-up and default state
    /// </summary>
    public void changeLasers(bool isPoweringUp)
    {
        if (isPoweringUp)
        {
            shipParts.CenterGun.SetActive(false);
            shipParts.LeftGun.SetActive(true);
            shipParts.RightGun.SetActive(true);

            M_Health tmpHealth;

            tmpHealth = shipParts.LeftGun.GetComponent<M_Health>();
            tmpHealth.current = tmpHealth.capacity;

            tmpHealth = shipParts.RightGun.GetComponent<M_Health>();
            tmpHealth.current = tmpHealth.capacity;
        }
        else
        {
            shipParts.CenterGun.SetActive(true);
            shipParts.LeftGun.SetActive(false);
            shipParts.RightGun.SetActive(false);
        }
    }
    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x * turnModifier, y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = followCam.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, cameraBorderX, (1 - cameraBorderX));
        pos.y = Mathf.Clamp(pos.y, cameraBorderY, (1 - cameraBorderY));
        transform.position = followCam.ViewportToWorldPoint(pos);
    }

    void RotationLook(float h, float v, float speed)
    {
        aimTarget.parent.position = Vector3.zero;
        aimTarget.localPosition = new Vector3(h, v, 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * speed * Time.deltaTime);

    }

    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    void SetSpeed(float x)
    {
        dolly.m_Speed = x;
    }

    void DoAttack()
    {
        if (Input.GetAxis("Fire1") >= 0.5f)
        {
            var canons = GetComponentsInChildren<M_ProjectileEmitter>();
            foreach (var canon in canons)
            {
                // Debug.Log(canon.name);
                if (canon.gameObject.activeSelf)
                {
                    canon.FireProj(projectile, projectileCooldown, out Projectile p);
                    if (p != null)
                    {
                        p.speed = forwardSpeed * 6f;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTarget.position, .5f);
        Gizmos.DrawSphere(aimTarget.position, .15f);
    }

}
