using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeSun : MonoBehaviour
{
    private SpriteRenderer sr;
    public Color nightCol;
    public float fadeTime;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeColor(false));
    }

    public IEnumerator ChangeColor(bool toDay)
    {
        float t = 0;
        Color start = sr.color;
        Color destination = Color.white;
        if (!toDay) { destination = nightCol; }

        while(t < fadeTime)
        {
            t += Time.deltaTime;
            sr.color = Color.Lerp(start, destination, t / fadeTime);
            yield return null;
        }
    }


}
