using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private FMOD.Studio.EventInstance bkg;
    private FMOD.Studio.EventInstance sfx;

    public FMODUnity.EventReference dayTheme, nightTheme;
    public FMODUnity.EventReference[] allsfx;

    private void Start()
    {
        instance = this;
        IntroMusic();
    }

    public void IntroMusic()
    {
        bkg = FMODUnity.RuntimeManager.CreateInstance(dayTheme);
        bkg.start();
        if(GameManager.instance.curDay == 1) { return; }
        Invoke("EnterDayTheme", 8f);
    }

    public void EnterDayTheme()
    {
        bkg.setParameterByName("day intro transition", 1);
    }

    public IEnumerator DayToNightTransition()
    {
        if(GameManager.instance.curDay == 10) { yield break; }
        StopLoopedSFX();
        bkg.setParameterByName("day intro transition", 0);
        bkg.setParameterByName("day stop and start", 0);

        yield return new WaitForSeconds(1f);

        bkg = FMODUnity.RuntimeManager.CreateInstance(nightTheme);
        bkg.start();
        bkg.setParameterByName("night stop start", 1);

        StartLoopedSFX(9);
    }

    public void NightToDayTransition()
    {
        if(GameManager.instance.curDay == 10) { return; }
        StopLoopedSFX();
        PlaySFX(2);
        bkg.setParameterByName("night stop start", 0);
        IntroMusic();
    }

    public void WinEndMusic()
    {
        //make sure it's still night theme?
        bkg.setParameterByName("win theme", 1);
    }

    //craft : 1, gather : 4 both loops
    public void StartLoopedSFX(int sound)
    {
        sfx = FMODUnity.RuntimeManager.CreateInstance(allsfx[sound]);
        sfx.start();
    }

    public void StopLoopedSFX()
    {
        sfx.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlaySFX(int num)
    {
        FMODUnity.RuntimeManager.PlayOneShot(allsfx[num]);
    }


}
