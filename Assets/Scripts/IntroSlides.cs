using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSlides : MonoBehaviour
{
    public Sprite[] allSlides;
    public Image image;
    public SpriteRenderer fader;
    public float pauseTime;
    public Animator slideAnim;

    private void Start()
    {
        StartCoroutine(StartSlides());
    }
    public IEnumerator StartSlides()
    {
        yield return new WaitForSeconds(1f);

        int curSlide = 0;
        while (curSlide < allSlides.Length -1)
        {
            print(curSlide);
            yield return new WaitForSeconds(pauseTime);
            StartCoroutine(FadeIn());
            yield return new WaitForSeconds(1f);
            curSlide++;
            image.sprite = allSlides[curSlide];
            StartCoroutine(FadeOut());
            yield return new WaitForSeconds(1f);
            if(curSlide == 3)
            {
                slideAnim.SetTrigger("Crash");
            }
            yield return null;
        }

        yield return new WaitForSeconds(pauseTime);
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1f);
        image.gameObject.SetActive(false);
        AudioManager.instance.EnterDayTheme();
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1f);
        GameManager.instance.IntroSlidesDone();
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1;

        for (float t = 0; t < 1.0f; t += Time.deltaTime / 1)
        {
            Color newCol = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            fader.color = newCol;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float alpha = 0;

        for (float t = 0; t < 1.0f; t += Time.deltaTime / 1)
        {
            Color newCol = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
            fader.color = newCol;
            yield return null;
        }
    }


}
