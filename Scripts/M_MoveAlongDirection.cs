using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_MoveAlongDirection : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 1;
    public bool fade;
    private void Update() 
    {
        Move();
    }
    void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        if (fade)
        {
            StartCoroutine(fadeColor(GetComponent<TextMesh>(), 15));
            fade = false;
        }
    }
    IEnumerator fadeColor (TextMesh textMesh, float fadeTime = 1)
    {
        Color currentColor = textMesh.color;
        Color fadedColor = currentColor;
        fadedColor.a = 0;
        float counter = 0;
        while (counter < fadeTime)
        {
            counter += Time.deltaTime;
            textMesh.color = Color.Lerp(currentColor, fadedColor, counter / fadeTime);
            yield return null;
        }
    }
}
