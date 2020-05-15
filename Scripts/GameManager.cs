using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int score;
    public static int enemySlayScore = 100;
    public static int powerUpScore = 500;
    public static float enemyViewDist = 200;
    public static int pickupScore = 250;
    public static bool isInverted = false;
    private void Awake() 
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Quit();
        }
    }

    public void Quit() 
    {
        SpaceCats.M_LoadScene.ExitGame();
    }
    public void LoadScene(string scene)
    {
        SpaceCats.M_LoadScene.LoadScene(scene);
    }

    public void setInveted()
    {
        isInverted = !isInverted;
    }
}
