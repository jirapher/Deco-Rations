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

    [Header("Managers")]
    public QuestManager questMan;
    public DesignUIManager designMan;
    public MissionManager gatherMan;
    public BlueprintManager craftMan;

    [Header("Crafting-Specific")]
    public GameObject[] specialFunctionObjects;
    //need crafting?

    private bool firstDay = true;
    //quest > gather > craft > build
    private void Start()
    {
        curDay = 1;
        SetDayDisplay();
        HP.value = 10;
        AllTabsOff();
        ToggleSpecialFunctionObjects(false);
        OpenQuest();
    }

    public void ToggleSpecialFunctionObjects(bool on)
    {
        foreach (GameObject g in specialFunctionObjects)
        {
            g.SetActive(on);
        }
    }

    public void NewDay()
    {
        ToggleSpecialFunctionObjects(false);

        curDay++;

        if(curDay > 3)
        {
            //questMan.DailyQuestAdd();
        }

        SetDayDisplay();
        HPCheck();
        gatherMan.NewDayUnlock();
        OpenQuest();
    }

    private void HPCheck()
    {
        HP.value -= questMan.DailyHPDrainCheck();
        HP.value += questMan.DailyHPRestoreCheck();
        if(HP.value > HP.maxValue)
        {
            HP.value = HP.maxValue;
        }

        if(HP.value < HP.minValue)
        {
            print("You died.");
        }
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
        ToggleSpecialFunctionObjects(false);
        designUI.SetActive(true);
        craftTab.SetActive(true);
        craftMan.ItemUIVisible(false);
    }

    public void OpenDesign()
    {
        AllTabsOff();

        designTab.SetActive(true);

        ToggleSpecialFunctionObjects(true);
    }




}
