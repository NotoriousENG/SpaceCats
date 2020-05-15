using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_LoadSceneOnPress : MonoBehaviour
{
    public float Timeout = 0;
    public float ToggleTime = 43;
    public GameObject ToggleObj;
    public string scene;
    float t = 0;
    private void Update() 
    {
        t += Time.deltaTime;

        if((Input.GetButtonDown("Fire1")))
        {
            SpaceCats.M_LoadScene.LoadScene(scene);
        }
        if (t >= ToggleTime && !ToggleObj.activeSelf)
        {
            ToggleObj.SetActive(true);
        }
    }
}
