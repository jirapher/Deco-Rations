using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject designTab, gatherTab, craftTab, questTab, designUI;

    public TMP_Text dayText;
    private int curDay = 0;
    public Slider HP;

    public QuestManager questMan;
    public DesignUIManager designMan;
    public MissionManager gatherMan;
    //need crafting?

    private bool firstDay = true;
    //quest > gather > craft > build
    private void Start()
    {
        curDay = 1;
        SetDayDisplay();
        HP.value = 10;
        AllTabsOff();
        OpenQuest();
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
        designUI.SetActive(true);
        craftTab.SetActive(true);
    }

    public void OpenDesign()
    {
        AllTabsOff();
        designTab.SetActive(true);
    }




}
