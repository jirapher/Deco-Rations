using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject designTab, gatherTab, craftTab, questTab, designUI;
    public static GameManager instance;
    public TMP_Text dayText;
    public int curDay = 0;
    public Slider HP;
    public TMP_Text hpTxt;
    public GameObject introSlides;

    [Header("Managers")]
    public QuestManager questMan;
    public DesignUIManager designMan;
    public MissionManager gatherMan;
    public BlueprintManager craftMan;
    public TDManager tdMan;
    public AudioManager audioMan;

    [Header("Specific Things")]
    public GameObject[] specialFunctionObjects;
    public GameObject[] buildingSpecific;
    public GameObject tdTimer;
    public GameObject nightStatsScreen;
    //need crafting?

    private bool firstDay = true;

    [Header("NoticeSys")]
    public GameObject NoticePanel;
    public TMP_Text noticeTxt;
    public TMP_Text noticeHeaderTxt;
    public GameObject dayHPHolder;
    public GameObject quitButton;
    public GameObject resetButton;
    //quest > gather > craft > build
    private void Start()
    {
        instance = this;
        curDay = 1;
        SetDayDisplay();
        SetHPText();
        AllTabsOff();
        ToggleSpecialFunctionObjects(false);
        OpenQuest();
        IntroNotice();
    }

    public void IntroSlidesDone()
    {
        introSlides.SetActive(false);
    }
    private void SetHPText()
    {
        hpTxt.text = "HP: " + HP.value + "/" + HP.maxValue;
    }

    private void IntroNotice()
    {
        SetGameNotice("Click on a button in the Quest Tracker to show details. When you're done, use the green button in the corner to start gathering resources!", "The Quest Screen");
    }

    private void DesignNotice()
    {
        SetGameNotice("Click items in your inventory and start designing! Right click over placed furniture to rotate it. Storing puts things back in inventory. Click 'Finished' when you're uh, ya know, finished.", "The Design Screen");
    }

    private void DeathNotice()
    {
        SetGameNotice("Oof. How unfashionable can you get? You're so passé that everyone perished. You should probably take a redo on that.", "Death by Design");
        quitButton.SetActive(true);
        resetButton.SetActive(true);
    }

    private void CollectNotice()
    {
        SetGameNotice("Click a character on the left and then click a destination to send them to. <br> Beach returns wood <br> River returns stones <br> Cave returns seeds <br> Woods return vines", "The Collection Screen");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ResetButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleSpecialFunctionObjects(bool on)
    {
        foreach (GameObject g in specialFunctionObjects)
        {
            g.SetActive(on);
        }
    }

    public void SetGameNotice(string notice, string header)
    {
        noticeHeaderTxt.text = header;
        noticeTxt.text = notice;
        NoticePanel.SetActive(true);
        //Time.timeScale = 0;
    }

    public void CloseNotice()
    {
        //Time.timeScale = 1;
        noticeHeaderTxt.text = "";
        noticeTxt.text = "";
        NoticePanel.SetActive(false);
    }

    public void NewDay()
    {
        tdTimer.SetActive(false);
        ToggleSpecialFunctionObjects(false);
        dayHPHolder.SetActive(true);
        curDay++;
        nightStatsScreen.SetActive(true);

        if(curDay > 3)
        {
            questMan.DailyQuestAdd();
        }

        string header = "Day " + curDay + ".";
        SetDayDisplay();
        string notice = HPCheck();
        gatherMan.NewDayUnlock();
        //OpenQuest();
        SetGameNotice(notice, header);
        tdMan.gameObject.SetActive(false);
    }

    public void EnterNight()
    {
        //StartCoroutine(audioMan.DayToNightTransition());
        AllTabsOff();
        ToggleSpecialFunctionObjects(false);
        dayHPHolder.SetActive(false);
        tdTimer.SetActive(true);
        specialFunctionObjects[0].SetActive(true);
        tdMan.Sunset();
        foreach(GameObject g in buildingSpecific)
        {
            g.SetActive(false);
        }
    }

    private string HPCheck()
    {
        string notice = "";

        int difference = 0;

        int subtract = questMan.DailyHPDrainCheck();
        difference -= subtract;
        notice += "You lost " + subtract + " HP from incomplete quests.<br>";

        int tdMinus = tdMan.HPCalc();
        difference -= tdMinus;
        notice += "You lost " + tdMinus + " HP from invaders.<br>";

        int add = questMan.DailyHPRestoreCheck();
        difference += add;
        notice += "You restored " + add + " HP from your interior design skills.<br> <br>";

        //print("difference:" + difference);

        HP.value += difference;

        SetHPText();

        if (HP.value > HP.maxValue)
        {
            HP.value = HP.maxValue;
            notice += "The party is happy. You're at max HP!";
        }

        if(HP.value <= 0)
        {
            DeathNotice();
        }

        return notice;
    }

    public void SetDayDisplay()
    {
        dayText.text = "Day " + curDay.ToString();
    }

    public void AllTabsOff()
    {
        designUI.SetActive(false);
        designTab.SetActive(false);
        gatherTab.SetActive(false);
        craftTab.SetActive(false);
        questTab.SetActive(false);
        tdTimer.SetActive(false);
        nightStatsScreen.SetActive(false);
    }

    public void OpenQuest()
    {
        AllTabsOff();
        questTab.SetActive(true);
    }

    public void OpenCollection()
    {
        if(curDay == 1) { CollectNotice(); }

        audioMan.StopLoopedSFX();
        audioMan.StartLoopedSFX(4);
        AllTabsOff();
        if (firstDay) { firstDay = false; }
        gatherTab.SetActive(true);
    }

    public void OpenCraft()
    {
        audioMan.StopLoopedSFX();
        audioMan.StartLoopedSFX(1);

        AllTabsOff();
        ToggleSpecialFunctionObjects(false);
        designUI.SetActive(true);
        craftTab.SetActive(true);
        craftMan.ItemUIVisible(false);
    }

    public void OpenDesign()
    {
        if(curDay == 1) { DesignNotice(); }

        audioMan.StopLoopedSFX();
        foreach (GameObject g in buildingSpecific)
        {
            g.SetActive(true);
        }
        AllTabsOff();

        designTab.SetActive(true);
        tdMan.gameObject.SetActive(true);
        ToggleSpecialFunctionObjects(true);
    }




}
