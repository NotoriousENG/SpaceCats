using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ShipCamera : MonoBehaviour
{
    public M_OnRailsScript onRailsScript;
    List<Transform> points;
    private Vector3 next;
    public Transform player;
    public float playerHeight = 1;
    float heading = 0;
    public float tilt = 15;
    public float minTilt = -80;
    public float maxTilt = 80;
    public float camDist = 10;
    public bool isRotatable = true;
    public bool isRotatableX = true;
    public bool isRotatableY = true;
    public float SensitivityX = 1f;
    public float SensitivityY = 1f;

    private void Start() {
        points = onRailsScript.getVectors();
        next = points[0].position;
        points.RemoveAt(0);
    }

    // camera should always move after the player moves
    // LateUpdate is called after update
    private void LateUpdate() 
    {
        // Move The camera with the mouse
        if (isRotatable)
        {
            handleInput();
        }
        
        // Only allow values between -80 and 80 for the tilt
        tilt = Mathf.Clamp(tilt, minTilt, maxTilt);

        // Set the new camera rotation
        transform.rotation = Quaternion.Euler(tilt, heading, 0);

        // set the camera position
        // TODO: Bounding box like star fox
        getPos();
        // transform.position = transform.position - transform.forward * camDist + Vector3.up * playerHeight;
    }

    private void handleInput()
    {
        if (isRotatableX)
        {
            heading += Input.GetAxis("Mouse X") * Time.deltaTime * 180 * SensitivityX;
            //heading += Input.GetAxis("RHorizontal") * Time.deltaTime * 180 * SensitivityX;
        }
        if (isRotatableY)
        {
            tilt += Input.GetAxis("Mouse Y") * Time.deltaTime * 180 * SensitivityY;
            //tilt += Input.GetAxis("RVertical") * Time.deltaTime * 180 *SensitivityY;
        }
    }
    void getNextPoint()
    {
        var temp = points[0].position;
        var currPlusOne = (temp - player.transform.position).magnitude;
        var curr = (next - player.transform.position).magnitude;
        if (currPlusOne < curr)
        {
            next = temp;
            points.RemoveAt(0);
        }
    }
    void getPos()
    {
        getNextPoint();
        if (player.TryGetComponent<M_PlayerShipController>(out var controller))
        {
            transform.position = Vector3.MoveTowards(transform.position, next, controller.forwardSpeed * Time.deltaTime);
        }
    }
}
