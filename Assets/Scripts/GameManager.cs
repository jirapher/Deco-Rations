using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject designTab, gatherTab, craftTab, questTab, designUI;
    public static GameManager instance;
    public TMP_Text dayText;
    public int curDay = 0;
    public Slider HP;

    [Header("Managers")]
    public QuestManager questMan;
    public DesignUIManager designMan;
    public MissionManager gatherMan;
    public BlueprintManager craftMan;
    public TDManager tdMan;

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
    //quest > gather > craft > build
    private void Start()
    {
        instance = this;
        curDay = 1;
        SetDayDisplay();
        HP.value = 10;
        AllTabsOff();
        ToggleSpecialFunctionObjects(false);
        OpenQuest();
        IntroNotice();
    }

    private void IntroNotice()
    {
        SetGameNotice("Click on a button in the Quest Tracker to show details. When you're done, use the green button in the corner to start gathering resources!", "The Quest Screen");
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
        Time.timeScale = 0;
    }

    public void CloseNotice()
    {
        Time.timeScale = 1;
        noticeHeaderTxt.text = "";
        noticeTxt.text = "";
        NoticePanel.SetActive(false);
    }

    public void NewDay()
    {
        //idea is this cuts to night island, displays stats, then goes back to quests.
        //will need to transition cam after TD
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

        int subtract = questMan.DailyHPDrainCheck();
        HP.value -= subtract;
        notice += "You lost " + subtract + " HP from incomplete quests.<br>";

        int tdMinus = tdMan.HPCalc();
        HP.value -= tdMinus;
        notice += "You lost " + tdMinus + " HP from invaders.<br>";

        int add = questMan.DailyHPRestoreCheck();
        HP.value += add;
        notice += "You restored " + subtract + " HP from your interior design skills.<br> <br>";

        if (HP.value > HP.maxValue)
        {
            HP.value = HP.maxValue;
            notice += "The party is happy. You're at max HP!";
        }

        if(HP.value <= 0)
        {
            notice += "Everyone died...";
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
        AllTabsOff();
        if (firstDay) { firstDay = false; }
        gatherTab.SetActive(true);
    }

    public void OpenCraft()
    {
        AllTabsOff();
        ToggleSpecialFunctionObjects(false);
        designUI.SetActive(true);
        craftTab.SetActive(true);
        craftMan.ItemUIVisible(false);
    }

    public void OpenDesign()
    {
        AllTabsOff();

        designTab.SetActive(true);
        tdMan.gameObject.SetActive(true);
        ToggleSpecialFunctionObjects(true);
    }




}
