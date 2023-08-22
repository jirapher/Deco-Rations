using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeSun : MonoBehaviour
{
    public Color midnight, midday;
    public float fadeTime = 4f;
    public Image uiImage;

    private void Start()
    {
        uiImage.color = midnight;
        StartCoroutine(LightTransition(true));
    }

    public IEnumerator LightTransition(bool fadeToLight)
    {

        if (fadeToLight)
        {
            for(float t = 0.0f; t < 1f; t += Time.deltaTime / fadeTime)
            {
                Color newCol = Color.Lerp(uiImage.color, midnight, t);
                uiImage.color = newCol;
                yield return null;
            }
        }
        else
        {
            for (float t = 0.0f; t < 1f; t += Time.deltaTime / fadeTime)
            {
                Color newCol = Color.Lerp(uiImage.color, midday, Mathf.PingPong(Time.time, fadeTime));
            }
        }

        yield break;
    }
}
