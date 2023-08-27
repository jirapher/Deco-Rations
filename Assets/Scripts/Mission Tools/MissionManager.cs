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
    public bool someoneIsSelected = false;

    public TMP_Text noticeTxt;
    public GameObject sendButton;

    public string[] searchingText;

    public string initialNoticeText;
    private string foundItemsText;
    //public Image woodsPortrait, cavesPortrait, beachPortrait, riverPortrait;
    public Image[] destinationPortraits;
    public bool[] destinationOccupied;
    public SearchFillBar[] searchProgressBars;

    private int missionsComplete = 0;
    private bool lockedUntilNewDay = false;
    public InventoryManager inventory;
    private int rockHold, woodHold, vineHold, seedHold;

    public PlayerPanelMission[] players;

    public GameObject toCraftingButton;

    //0 woods, 1 cave, 2 beach, 3 river

    private void Start()
    {
        ClearTextValues();
        ProgressBarsActive(false);
    }

    public void ClearTextValues()
    {
        noticeTxt.text = initialNoticeText;
        selectedPlayerTxt.text = "";
        sendButton.SetActive(false);
        toCraftingButton.SetActive(false);
        selectedPlayerName = "";
        selectedPlayerProficiency = -1;
        selectedPlayerPortrait = null;
        foundItemsText = "";
        someoneIsSelected = false;
    }

    public void SetSelectedPlayer(string playerName, Sprite portrait, int proficiency)
    {
        if (lockedUntilNewDay || someoneIsSelected) { return; }

        someoneIsSelected = true;
        selectedPlayerProficiency = proficiency;
        selectedPlayerTxt.text = "Where are you sending " + playerName + "?";
        selectedPlayerPortrait = portrait;
        selectedPlayerName = playerName;
    }

    public void DestinationButtonClick(int destinationNum)
    {
        if (lockedUntilNewDay || destinationOccupied[destinationNum] || AllPlayersPlaced()) { return; }

        destinationPortraits[destinationNum].sprite = selectedPlayerPortrait;
        destinationPortraits[destinationNum].gameObject.SetActive(true);
        destinationOccupied[destinationNum] = true;
        searchProgressBars[destinationNum].inUse = true;

        AddHoldResources(destinationNum);
        placedPlayersNum++;
        ReadyToSendCheck();

        selectedPlayerName = "";
        selectedPlayerPortrait = null;
        selectedPlayerProficiency = -1;
        selectedPlayerTxt.text = "";
        someoneIsSelected = false;
    }

    private void AddHoldResources(int destination)
    {
        switch (destination)
        {
            case 0:
                SetFoundItemsText(selectedPlayerName + " retrieved " + selectedPlayerProficiency + " vines from the woods.");
                vineHold += selectedPlayerProficiency;
                break;

                case 1:
                SetFoundItemsText(selectedPlayerName + " retrieved " + selectedPlayerProficiency + " seeds from the cave.");
                seedHold += selectedPlayerProficiency;
                break;

            case 2:
                SetFoundItemsText(selectedPlayerName + " retrieved " + selectedPlayerProficiency + " wood from the beach.");
                woodHold += selectedPlayerProficiency;
                break;

            case 3:
                SetFoundItemsText(selectedPlayerName + " retrieved " + selectedPlayerProficiency + " stones from the river.");
                rockHold += selectedPlayerProficiency;
                break;

        }
    }

    private void DistributeResources()
    {
        inventory.AdjustRock(rockHold);
        inventory.AdjustWood(woodHold);
        inventory.AdjustSeed(seedHold);
        inventory.AdjustVines(vineHold);

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
        AudioManager.instance.StartLoopedSFX(4);

        sendButton.SetActive(false);

        //noticeTxt.text = "Searching...";
        noticeTxt.text = searchingText[Random.Range(0, searchingText.Length)];

        ProgressBarsActive(true);

        foreach(SearchFillBar bar in searchProgressBars)
        {
            if (bar.inUse)
            {
                StartCoroutine(bar.FillBar());
            }
            
        }
    }

    private void ProgressBarsActive(bool isActive)
    {
        foreach (SearchFillBar bar in searchProgressBars)
        {
            bar.gameObject.SetActive(isActive);
        }
    }
    public void MissionCompleteCheck()
    {
        missionsComplete++;

        if(missionsComplete >= 3)
        {
            //we're done!
            ProgressBarsActive(false);
            RetrievalReset();
            ResetPlayers();
            DistributeResources();
            DisplayRetrievalText();
            toCraftingButton.SetActive(true);
            missionsComplete = 0;
            lockedUntilNewDay = true;
        }
    }

    public void NewDayUnlock()
    {
        ClearTextValues();
        foreach (PlayerPanelMission p in players)
        {
            p.SetSelectable(true);
        }
        lockedUntilNewDay = false;
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
