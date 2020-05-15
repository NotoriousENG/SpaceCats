using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Reticle : MonoBehaviour
{
    public GameObject reticle;
    public LayerMask layer;
    public float maxDistance;
    public Material retMat;
    public Color defColor;
    public Color targColor;
    private bool isOnTarget;

    // Start is called before the first frame update
    void Start()
    {
        //retMat = reticle.GetComponent<Renderer>().GetComponent<Material>();
        isOnTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(reticle.transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance, layer))
        {
            Debug.DrawRay(reticle.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (!isOnTarget)
            {
                retMat.SetColor("_Color", targColor);
                isOnTarget = true;
            }
        }
        else
        {
            if (isOnTarget)
            {
                retMat.SetColor("_Color", defColor);
            }
            isOnTarget = false;
        }
    }
}
