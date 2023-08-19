using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public FurnitureSO database;
    public List<QuestSO> allQuests = new();
    private int curQuestNum = 0;

    [Header("Objectives")]
    public GameObject requiredItemsButtonPrefab;
    public GameObject requiredItemsButtonsParent;
    private List<GameObject> questRequirementsRewardsButtons = new();

    public TMP_Text objectiveText;

    public GameObject rewardPrefab;
    public GameObject rewardParent;


    [Header("Quests")]
    public GameObject questButtonPrefab;
    public GameObject questButtionParent;
    private List<GameObject> allQuestButtons = new();


    private void Start()
    {
        GameStartQuestAdd();
    }

    public void DisplayQuestDetails(int questNum)
    {
        WipePreviousQuestDetails();
        QuestSO quest = FetchQuest(questNum);
        objectiveText.text = quest.questDetails;
        GenerateRequiredItemsIcons(quest);
        GenerateRewardItemsIcons(quest);
    }

    private void GenerateRequiredItemsIcons(QuestSO quest)
    {
        int track = 0;
        foreach(GameObject item in quest.requiredItems)
        {
            GameObject newButton = Instantiate(requiredItemsButtonPrefab, requiredItemsButtonsParent.transform);
            newButton.GetComponent<ItemsToMakeButton>().SetInformation(item.GetComponent<SpriteRenderer>().sprite, quest.requiredAmount[track]);
            questRequirementsRewardsButtons.Add(newButton);
            track++;
        }
    }

    private void GenerateRewardItemsIcons(QuestSO quest)
    {
        //get reward image from furniture id
        foreach(int id in quest.rewardId)
        {
            for(int i = 0; i < database.furnitureData.Count; i++)
            {
                if(id == database.furnitureData[i].id)
                {
                    GameObject newButton = Instantiate(rewardPrefab, rewardParent.transform);
                    newButton.GetComponent<ItemsToMakeButton>().SetInformation(database.furnitureData[i].itemImage, 0);
                    questRequirementsRewardsButtons.Add(newButton);
                }
            }
        }
    }

    private void WipePreviousQuestDetails()
    {
        foreach(GameObject g in questRequirementsRewardsButtons)
        {
            Destroy(g);
        }
    }

    public void GameStartQuestAdd()
    {
        AddNewQuest(allQuests[0]);
        AddNewQuest(allQuests[1]);
        AddNewQuest(allQuests[2]);
    }

    public void UnlockNewItem(int itemID)
    {
        for(int i = 0; i < database.furnitureData.Count; i++)
        {
            if (database.furnitureData[i].id == itemID)
            {
                database.furnitureData[i].isLocked = false;
            }
        }
    }

    private QuestSO FetchQuest(int questNum)
    {
        QuestSO quest = null;

        for(int i = 0; i < allQuests.Count; i++)
        {
            if (allQuests[i].questNum == questNum)
            {
                quest = allQuests[i];
            }
        }

        return quest;
    }

    private void AddNewQuest(QuestSO quest)
    {
        //make sure it's not currently a button

        if(allQuestButtons.Count > 0)
        {
            for (int i = 0; i < allQuestButtons.Count; i++)
            {
                if (allQuestButtons[i].GetComponent<QuestButton>().GetNum() == quest.questNum)
                {
                    return;
                }
            }
        }


        GameObject q = Instantiate(questButtonPrefab, questButtionParent.transform);
        q.GetComponent<QuestButton>().SetData(quest.questName, quest.questNum, quest.drainsHP);
        allQuestButtons.Add(q);
        curQuestNum++;
    }

    public void DailyHPDrainCheck()
    {
        int drain = 0;

        for(int i = 0; i < allQuestButtons.Count; i++)
        {
            if (allQuestButtons[i].GetComponent<QuestButton>().drainHP)
            {
                drain++;
            }
        }

        print("HP DRAIN HERE");
        //GAME MAN UPDATE HP
    }

    public void DailyQuestAdd()
    {
        if (allQuests[curQuestNum].complete)
        {
            print("Issue here...");
            curQuestNum++;
        }
        AddNewQuest(allQuests[curQuestNum]);
    }

    public void QuestComplete(QuestSO quest)
    {
        quest.complete = true;

        UnlockReward(quest);

        for(int i = 0; i < allQuestButtons.Count; i++)
        {
            if (allQuestButtons[i].GetComponent<QuestButton>().GetNum() == quest.questNum)
            {
                Destroy(allQuestButtons[i]);
                allQuestButtons.Remove(allQuestButtons[i]);
            }
        }
    }

    public void UnlockReward(QuestSO quest)
    {

        foreach(int reward in quest.rewardId)
        {
            for(int i = 0; i < database.furnitureData.Count; i++)
            {
                if(reward == database.furnitureData[i].id)
                {
                    database.furnitureData[i].isLocked = false;
                }
            }
        }
    }


}
