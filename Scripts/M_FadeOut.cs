using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_FadeOut : MonoBehaviour
{
    public Image canvasImage;
    public float increment = 0.5f;
    public bool reverse = false;
    public bool complete = false;
    float t = 0.0f;

    private void Start() 
    {
        canvasImage.color = new Color (canvasImage.color.r, canvasImage.color.g, canvasImage.color.b, 1);
        RestartFade(true);
    }
    private void Update() 
    {
        if (!complete)
        {
            Fade();
        }
    }
    void Fade()
    {
        var alpha = canvasImage.color.a;
        int r = 1;
        if (reverse)
            r = 0;
        
        alpha = Mathf.Lerp(alpha, r, t);
        canvasImage.color = new Color (canvasImage.color.r, canvasImage.color.g, canvasImage.color.b, alpha);

        t += increment * Time.deltaTime;

        if (t > 1.0f)
        {
            complete = true;
            t = 0;
        }
    }
    public void RestartFade(bool reverse)
    {
        this.reverse = reverse;
        complete = false;
    }
}
