using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public FurnitureSO database;
    public List<QuestSO> allQuests = new();
    public List<QuestSO> activeQuests = new();
    public List<QuestSO> completedQuests = new();
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
    public GameObject questButtonParent;
    private List<GameObject> allQuestButtons = new();
    private void Start()
    {
        GameStartQuestAdd();
        QuestReset();
    }

    private void QuestReset()
    {
        for(int i = 0; i < allQuests.Count; i++)
        {
            allQuests[i].complete = false;
        }
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


        GameObject q = Instantiate(questButtonPrefab, questButtonParent.transform);
        q.GetComponent<QuestButton>().SetData(quest.questName, quest.questNum, quest.drainsHP);
        allQuestButtons.Add(q);
        activeQuests.Add(quest);
        curQuestNum++;
    }

    public int DailyHPDrainCheck()
    {
        int drain = 0;

        for(int i = 0; i < allQuestButtons.Count; i++)
        {
            if (allQuestButtons[i].GetComponent<QuestButton>().drainHP)
            {
                drain++;
            }
        }

        return drain;
    }

    public int DailyHPRestoreCheck()
    {
        int restore = 0;

        for(int i = 0; i < database.furnitureData.Count; i++)
        {
            if (database.furnitureData[i].restoresHP)
            {
                restore += database.furnitureData[i].quantity;
            }
        }

        return restore;
    }

    public void DailyQuestAdd()
    {
        if (allQuests[curQuestNum].complete)
        {
            curQuestNum++;
        }

        if(curQuestNum > allQuests.Count - 1) { return; }

        AddNewQuest(allQuests[curQuestNum]);
    }

    public void QuestComplete(QuestSO quest)
    {
        quest.complete = true;

        if(quest.rewardId.Length > 0)
        {
            UnlockReward(quest);
        }

        RemoveQuest(quest);
    }

    public void RemoveQuest(QuestSO quest)
    {
        for (int i = 0; i < allQuestButtons.Count; i++)
        {
            if (allQuestButtons[i].GetComponent<QuestButton>().GetNum() == quest.questNum)
            {
                GameObject g = allQuestButtons[i];
                allQuestButtons.Remove(allQuestButtons[i]);
                Destroy(g);
            }
        }


        activeQuests.Remove(quest);
        completedQuests.Add(quest);
    }

    public void UnlockReward(QuestSO quest)
    {
        string rewards = "";
        foreach(int reward in quest.rewardId)
        {
            for(int i = 0; i < database.furnitureData.Count; i++)
            {
                if(reward == database.furnitureData[i].id)
                {
                    database.furnitureData[i].isLocked = false;
                    rewards += database.furnitureData[i].name + " <br>";
                }
            }
        }

        if(rewards != "")
        {
            GameManager.instance.SetGameNotice("You completed the " + quest.questName + " quest and unlocked: <br>" + rewards, "Quest Complete!");
        }
        else
        {
            GameManager.instance.SetGameNotice("You completed the " + quest.questName + " quest!", "Quest Complete!");
        }
        
    }

    public void QuestCheck()
    {

        foreach(QuestSO q in activeQuests)
        {
            int place = 0;
            int checksToPass = q.requiredItems.Length;
            int totalChecks = 0;
            foreach(GameObject reqItem in q.requiredItems)
            {
                int reqAmt = q.requiredAmount[place];

                for(int i = 0; i < database.furnitureData.Count; i++)
                {
                    if (reqItem.GetComponent<Item>().itemID == database.furnitureData[i].id)
                    {
                        int numPlacedObjects = database.furnitureData[i].totalInCirculation - database.furnitureData[i].quantity;
                        if(numPlacedObjects >= reqAmt)
                        {
                            totalChecks++;
                        }
                    }
                }

                place++;
            }

            if(totalChecks >= checksToPass)
            {
                QuestComplete(q);
            }
        }
    }


}
