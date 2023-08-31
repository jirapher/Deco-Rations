using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroSlides : MonoBehaviour
{
    public Sprite[] allSlides;
    public Image image;
    public SpriteRenderer fader;
    public float pauseTime;
    public Animator slideAnim;


    [Header("Ending Slides")]
    public Sprite endStay, endLeave;
    public TMP_Text thanksTxt;
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
        yield return new WaitForSeconds(.5f);
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

    public IEnumerator EndGameStay()
    {
        fader.color = Color.white;
        image.sprite = endStay;
        thanksTxt.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1f);
        thanksTxt.gameObject.SetActive(false);
        GameManager.instance.IntroSlidesDone();
    }

    public IEnumerator EndGameLeave()
    {
        fader.color = Color.white;
        image.sprite = endLeave;
        thanksTxt.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }


}
