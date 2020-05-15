using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class W_EV_SpawnEnemies : W_Event
{
    public GameObject enemy;
    public Vector3 position;
    public Quaternion rotation = Quaternion.identity;
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Activate()
    {
        Instantiate(enemy, position, rotation);
    }
}
