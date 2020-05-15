using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_Score : MonoBehaviour
{
    int score;
    public Text scoreText;
    

    private void Update() {
        score = GameManager.score;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
