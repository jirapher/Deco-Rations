using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour
{
    private int placedPlayersNum = 0;
    public TMP_Text selectedPlayerTxt;
    private string selectedPlayerName;
    private int selectedPlayerProficiency = -1;
    public Sprite selectedPlayerPortrait;

    public TMP_Text noticeTxt;
    public GameObject sendButton;

    public string initialNoticeText;
    private string foundItemsText;
    //public Image woodsPortrait, cavesPortrait, beachPortrait, riverPortrait;
    public Image[] destinationPortraits;
    public bool[] destinationOccupied;
    public SearchFillBar[] searchProgressBars;

    private int missionsComplete = 0;

    public InventoryManager inventory;
    private int rockHold, woodHold, vineHold, seedHold;

    public PlayerPanelMission[] players;

    //0 woods, 1 cave, 2 beach, 3 river

    private void Start()
    {
        noticeTxt.text = initialNoticeText;
        selectedPlayerTxt.text = "";
        sendButton.SetActive(false);
    }


    public void SetSelectedPlayer(string playerName, Sprite portrait, int proficiency)
    {
        selectedPlayerProficiency = proficiency;
        selectedPlayerTxt.text = "Where are you sending " + playerName + "?";
        selectedPlayerPortrait = portrait;
        selectedPlayerName = playerName;
    }

    public void DestinationButtonClick(int destinationNum)
    {
        //have to send a lot of this to a gathering script...

        if (destinationOccupied[destinationNum]) { return; }
        destinationPortraits[destinationNum].sprite = selectedPlayerPortrait;
        destinationPortraits[destinationNum].gameObject.SetActive(true);
        destinationOccupied[destinationNum] = true;
        searchProgressBars[destinationNum].inUse = true;
        AddHoldResources(destinationNum);
        placedPlayersNum++;
        ReadyToSendCheck();
    }

    private void AddHoldResources(int destination)
    {
        switch (destination)
        {
            case 0:
                SetFoundItemsText(selectedPlayerName + " retrieved " + selectedPlayerProficiency + " vines.");
                vineHold += selectedPlayerProficiency;
                break;

                case 1:
                SetFoundItemsText(selectedPlayerName + " retrieved " + selectedPlayerProficiency + " seeds.");
                seedHold += selectedPlayerProficiency;
                break;

            case 2:
                SetFoundItemsText(selectedPlayerName + " retrieved " + selectedPlayerProficiency + " wood.");
                woodHold += selectedPlayerProficiency;
                break;

            case 3:
                SetFoundItemsText(selectedPlayerName + " retrieved " + selectedPlayerProficiency + " rocks.");
                rockHold += selectedPlayerProficiency;
                break;

        }
    }

    private void DistributeResources()
    {
        inventory.wood += woodHold;
        inventory.rocks += rockHold;
        inventory.seeds += seedHold;
        inventory.vines += vineHold;

        woodHold = 0;
        rockHold = 0;
        seedHold = 0;
        vineHold = 0;
    }

    private void SetFoundItemsText(string textToAdd)
    {
        foundItemsText += " " + textToAdd + "<br>";
    }

    private bool AllPlayersPlaced()
    {
        if(placedPlayersNum >= 3)
        {
            return true;
        }

        return false;
    }

    private void ReadyToSendCheck()
    {
        if (!AllPlayersPlaced()) { return; }

        sendButton.SetActive(true);
    }

    public void SendButtonClicked()
    {
        sendButton.SetActive(false);
        foreach(SearchFillBar bar in searchProgressBars)
        {
            if (bar.inUse)
            {
                StartCoroutine(bar.FillBar());
            }
            
        }
    }

    public void MissionCompleteCheck()
    {
        missionsComplete++;

        if(missionsComplete >= 3)
        {
            //we're done!
            RetrievalReset();
            ResetPlayers();
            DistributeResources();
            DisplayRetrievalText();
            missionsComplete = 0;
        }
    }

    public void ResetPlayers()
    {
        foreach(PlayerPanelMission p in players)
        {
            p.ResetPortrait();
        }
    }

    public void DisplayRetrievalText()
    {
        noticeTxt.text = foundItemsText;
    }

    public void RetrievalReset()
    {
        foreach (SearchFillBar bar in searchProgressBars)
        {
            bar.ResetBar();
        }

        for(int i = 0; i < destinationPortraits.Length; i++)
        {
            destinationPortraits[i].sprite = null;
            destinationPortraits[i].gameObject.SetActive(false);
            destinationOccupied[i] = false;
        }

        placedPlayersNum = 0;
        selectedPlayerTxt.text = "";
        sendButton.SetActive(false);
    }
}
