using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Camera : MonoBehaviour
{
    public Transform player;
    public float playerHeight = 1;
    float heading = 0;
    public float tilt = 15;
    public float minTilt = -80;
    public float maxTilt = 80;
    public float minHeading = -80;
    public float maxHeading = 80;
    public float camDist = 10;
    public bool isRotatable = true;
    public bool isRotatableX = true;
    public bool isRotatableY = true;
    public float SensitivityX = 1f;
    public float SensitivityY = 1f;

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
        heading = Mathf.Clamp(heading, minHeading, maxHeading);

        // Set the new camera rotation
        transform.rotation = Quaternion.Euler(tilt, heading, 0);

        // set the camera position
        // TODO: Bounding box like star fox
        transform.position = player.position - transform.forward * camDist + Vector3.up * playerHeight;
    }

    private void handleInput()
    {
        if (isRotatableX)
        {
            heading += Input.GetAxis("Mouse X") * Time.deltaTime * 180 * SensitivityX;
            // heading += Input.GetAxis("RHorizontal") * Time.deltaTime * 180 * SensitivityX;
        }
        if (isRotatableY)
        {
            tilt += Input.GetAxis("Mouse Y") * Time.deltaTime * 180 * SensitivityY;
            // tilt += Input.GetAxis("RVertical") * Time.deltaTime * 180 *SensitivityY;
        }
    }
}
