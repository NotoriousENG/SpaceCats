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
public class M_PlayerController : MonoBehaviour
{
    // Objects
    CharacterController controller;
    public Transform cam;

    // Camera
    Vector3 camF, camR;

    // Input
    
    Vector2 input;

    // Physics
    Vector3 intent;
    Vector3 velocity;
    Vector3 velocityXZ;
    public float speed = 5f;
    public float jumpVelocity = 10;
    public float acceleration = 11;
    float turnSpeed = 5f;
    public float turnSpeedLow = 7f;
    public float turnSpeedHigh = 20f;
    Vector3 forward;
    RaycastHit hit;

    // Gravity
    float grav = 9.81f;
    public bool grounded = false;
    public bool canJump = true;
    private Animator animator;
    public float raycastDist = .2f;
    private bool doRaycast = true;
    [HideInInspector]
    public bool zeroMovement = false;
    private void Start() 
    {
        // load the CharacterController attatched to this object
        controller = GetComponent<CharacterController>();
        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("Add an Animator to your player");
        }
    }
    private void Update() 
    {
        DoInput();
        CalculateCamera();
        CalculateGround();
        CalculateForward();
        DoMove();
        DoGravity();
        if (canJump)
        {
            DoJump();
        }
        DoAttack();

        HandleMovement();
        // Debug.Log(velocity);
    }

    // Stores the input for later use
    void DoInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);
    }
    
    // sets variables representing the orientation of the camera
    // this way we will move dependent on our camera rotation
    void CalculateCamera()
    {
        camF = cam.forward;
        camR = cam.right;

        camF.y = 0;
        camF = camF.normalized;

        camR.y = 0;
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

    void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }
        forward = Vector3.Cross(transform.right, hit.normal);
    }
    
    // set the velocity for movement with respect to player input and camera orientation
    void DoMove()
    {
        // this is the direction that we will move in based on input and camera orientation
        intent = (camF * input.y + camR * input.x);

        // this is the speed the character will rotate (turn) at
        // we want to base the turn speed based off of how fast we are moving
        // i.e. turn quickly if we are still and slower if we are moving quickly
        float tS = velocity.magnitude/speed;
        turnSpeed = Mathf.Lerp(turnSpeedHigh, turnSpeedLow, tS);
        
        // if we are getting movement input
        // turn the character to face the direction of movement
        if (input.magnitude > 0 && !zeroMovement)
        {
            Quaternion rot = Quaternion.LookRotation(intent);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
        
        // seperate out the y component of velocity so that it does not affect our XZ movement
        velocityXZ = velocity;
        velocityXZ.y = 0;

        // get the appropriate velocity (which accounts for direction) and apply speed 
        // and Linearly Interpolate based off of rotation (start moving slower and then speed up)
        // then we can add the y velocity back in
        velocityXZ = Vector3.Lerp(velocityXZ, forward * input.magnitude * speed, acceleration* Time.deltaTime);
        velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
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

    // Jump, by using the velocity we have set up
    // but only if the character is grounded
    void DoJump()
    {
        if (grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                doRaycast = false;
                velocity.y = jumpVelocity;
                // animator.SetTrigger("Jump");
            }
        }
    }
    void DoAttack()
    {
        if (Input.GetButtonDown("Fire1") && grounded)
        {
            animator.SetTrigger("Attack");
        }
        
    }
    void HandleMovement()
    {
        zeroVelocityXZ(); // zero velocity if zeroMovement is true
        Vector3 timedVelocity = velocity * Time.deltaTime;
        controller.Move(timedVelocity);
        timedVelocity.y = 0;
        animator.SetFloat("MoveSpeed", timedVelocity.magnitude * 10);
    }

    public void StopMovement()
    {
        animator.SetFloat("MoveSpeed", 0);
        this.enabled = false;
    }
    void zeroVelocityXZ()
    {
        if(zeroMovement)
        {
            velocity.x = 0;
            velocity.z = 0;
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }
}
