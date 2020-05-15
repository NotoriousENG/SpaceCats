using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**************************************************************************************************
 *                                                                                                *
 * These scripts, in this state are heavily influenced from this tutorial by Nimso Ny:   *
 * https://www.youtube.com/watch?v=zXv7P6avhHM&list=PLoLkfS-a4sd2nPoeTkO-rj_Kj0FkKZYlw&index=3    *
 *                                                                                                *
 **************************************************************************************************
 *
 * I highly reccomend following his tutorial series for programmers, it's a great refresher.
 *
 * Additionaly, sample scripts onward will use this design as a base. :)
 *
 */

// A Simple 3D Player Controller script that uses Unity's PlayerController component
public class M_PlayerShipController : MonoBehaviour
{
    [System.Serializable]
    public class ShipParts
    {
        public GameObject LeftGun, RightGun, LeftWing, RightWing, CenterGun;
        public float noWingMovementScale = 0.9f;
        // public AudioClip shootingClip;
    }
    public ShipParts shipParts;
    public GameObject projectile;
    public float projectileCooldown;

    // Objects
    CharacterController controller;
    public Transform cam;

    // Camera
    Vector3 camF, camR, camU;

    // Input
    
    Vector2 input;
    public float maxUpDownAngle = 45f;
    public float maxLeftRightAngle = 45f;

    // Physics
    public float forwardSpeed = 5f;
    Vector3 intent;
    Vector3 velocity;
    // Vector3 velocityXZ;
    public float speed = 5f;
    public float acceleration = 11;
    float turnSpeed = 5f;
    public float turnSpeedLow = 7f;
    public float turnSpeedHigh = 20f;
    Vector3 forward;
    RaycastHit hit;
    float turnModifier = 1f;

    // Gravity
    float grav = 9.81f;
    public bool grounded = false;
    public bool useGravity = false;
    private Animator animator;
    public float raycastDist = .2f;
    private bool doRaycast = true;
    [HideInInspector]
    public bool zeroMovement = false;
    int inverter =1;
    // public bool invert = false;
    public bool isStart;
    private void Start() 
    {
        // load the CharacterController attatched to this object
        controller = GetComponent<CharacterController>();
        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("Add an Animator to your player");
        }
    }

    void AdjustControls()
    {
        int mod = 1;
        if (isStart)
        {
            mod = -1;
        }
        if (GameManager.isInverted)
        {
            inverter= -1;
        }
        else
        {
            inverter = 1;
        }
    }
    private void Update() 
    {
        AdjustControls();
        DoInput();
        CalculateCamera();
        CalculateGround();
        DoMove();
        if (useGravity)
        {
            DoGravity();
        }
        DoAttack();

        HandleMovement();
        // Debug.Log(velocity);
    }

    // Stores the input for later use
    void DoInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") * inverter);
        input = Vector2.ClampMagnitude(input, 1);
        RestrictInput();
    }
    
    // sets variables representing the orientation of the camera
    // this way we will move dependent on our camera rotation
    void CalculateCamera()
    {
        camF = cam.forward;
        camU = cam.up;
        camR = cam.right;

        camF.y = 0;
        camF = camF.normalized;

        camU.z = 0;
        camU = camU.normalized;

        camR.z = 0;
        camR = camR.normalized;
    }

    // lets us know if our character is grounded by performing a raycast
    // from the player to the ground
    void CalculateGround()
    {
        if(Physics.Raycast(transform.position+Vector3.up*0.1f, -Vector3.up, out hit, raycastDist) && doRaycast)
        {
            if (hit.collider.isTrigger == false)
            {
                grounded = true;
                // Debug.Log("Standing on: " + hit.transform.gameObject.name);
            }
        }
        else
        {       
            grounded = false;
        }
        animator.SetBool("Grounded", grounded); // set animator value of grounded
        // Debug.Log(velocity.y);
    }
    
    // set the velocity for movement with respect to player input and camera orientation
    void DoMove()
    {
        // this is the direction that we will move in based on input and camera orientation
        // intent = (camF * input.y + camR * input.x);
        intent = (camU * input.y + camR * input.x);

        // this is the speed the character will rotate (turn) at
        // we want to base the turn speed based off of how fast we are moving
        // i.e. turn quickly if we are still and slower if we are moving quickly
        float tS = velocity.magnitude/speed;
        turnSpeed = Mathf.Lerp(turnSpeedHigh, turnSpeedLow, tS);
        
        // if we are getting movement input
        // turn the character to face the direction of movement
        if (input.magnitude > 0 && !zeroMovement)
        {
            Quaternion rot = Quaternion.Euler(-input.y * maxUpDownAngle, 0, -input.x * maxLeftRightAngle * turnModifier);
            
            // transform.Rotate(0, input.x, input.y, Space.Self);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
            // rot = Quaternion.Euler(tempEuler);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
        
        // seperate out the y component of velocity so that it does not affect our XZ movement
        // velocityXZ = velocity;
        // velocityXZ.y = 0;

        // get the appropriate velocity (which accounts for direction) and apply speed 
        // and Linearly Interpolate based off of rotation (start moving slower and then speed up)
        // then we can add the y velocity back in
        // velocityXZ = Vector3.Lerp(velocityXZ, forward * input.magnitude * speed, acceleration* Time.deltaTime);
        // velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
        velocity.y = Mathf.Lerp(velocity.y, 1 * input.y * speed, acceleration * Time.deltaTime);
        velocity.x = Mathf.Lerp(velocity.x, 1 * input.x * speed * turnModifier, acceleration * Time.deltaTime);
        velocity.z = forwardSpeed;
    }
    // apply gravity, but only speed up when we are not grounded
    void DoGravity()
    {
        if (grounded)
        {
            // animator.SetTrigger("Land");
            velocity.y = -0.5f;
        } else
        {
            velocity.y -= grav * Time.deltaTime;
            if (velocity.y <= 0)
            {
                doRaycast = true;
            }
        }
        velocity.y = Mathf.Clamp(velocity.y, -10, 10);
    }
    void DoAttack()
    {
        if (Input.GetAxis("Fire1") >= 0.5f)
        {
            var canons = GetComponentsInChildren<M_ProjectileEmitter>();
            var count = 0;
            foreach (var canon in canons)
            {
                // Debug.Log(canon.name);
                if (canon.gameObject.activeSelf)
                {
                    canon.FireProj(projectile, projectileCooldown, out Projectile p);
                    count ++;
                }
            }
            if (count == 0)
            {
                shipParts.CenterGun.SetActive(true); // make sure we are never without our gun
            }
        }
    }
    void HandleMovement()
    {
        // Debug.Log(turnModifier);
        Vector3 timedVelocity = velocity * Time.deltaTime;
        controller.Move(timedVelocity);
        timedVelocity.y = 0;
        animator.SetFloat("MoveSpeed", timedVelocity.magnitude * 10);
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

    public Vector3 getVelocity()
    {
        return velocity;
    }

}
