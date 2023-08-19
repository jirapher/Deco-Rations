using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButton : MonoBehaviour
{
    public TMP_Text questTitle;
    private int questNum;
    public bool drainHP = false;
    private QuestManager manager;

    private void Start()
    {
        manager = FindObjectOfType<QuestManager>();
    }
    public void SetData(string title, int num, bool drainsHP)
    {
        questTitle.text = title;
        drainHP = drainsHP;
        questNum = num;
    }

    public void SendDetailsToManager()
    {
        manager.DisplayQuestDetails(questNum);
    }

    public int GetNum()
    {
        return questNum;
    }
}
