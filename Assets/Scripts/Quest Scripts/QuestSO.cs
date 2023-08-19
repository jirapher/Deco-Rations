using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/QuestSO")]
public class QuestSO : ScriptableObject
{
    public string questName;
    public string questDetails;
    public int questNum;
    public GameObject[] requiredItems;
    public int[] requiredAmount;
    public bool drainsHP = false;
    public int[] rewardId;
    public bool complete = false;
}
