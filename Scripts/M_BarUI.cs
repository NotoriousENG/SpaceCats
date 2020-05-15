using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_BarUI : MonoBehaviour
{
    public Slider HealthSlider;
    public M_Health health;
    static float t = 0.0f;
    float prevValue;
    public Image image;
    private void LateUpdate() 
    {
        fillAmount();
        Slide();
        toggleImage();
    }

    void Slide()
    {
        var nextValue = health.current/health.capacity;
        if(prevValue >= nextValue + 0.0001f || prevValue <= nextValue - 0.0001f)
        {
            prevValue = HealthSlider.value;
            HealthSlider.value =  Mathf.Lerp(HealthSlider.value, nextValue, t);
            t += 0.2f * Time.deltaTime;
        } else
        {
            t = 0;
        }
    }
    void toggleImage()
    {
        if (HealthSlider.value <= 0.0002f)
        {
            image.enabled = false;
        } else
        {
            image.enabled = true;
        }
    }
    void fillAmount()
    {
        var amount = health.current / health.capacity;
        image.fillAmount = amount;
    }
}
