using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour
{
    private int placedPlayersNum = 0;
    public TMP_Text selectedPlayerTxt;
    public Sprite selectedPlayerPortrait;

    public TMP_Text noticeTxt;
    public GameObject sendButton;

    public string initialNoticeText;

    public Image woodsPortrait, cavesPortrait, beachPortrait;
    private bool woodsOccupied, cavesOccupied, beachOccupied;

    private void Start()
    {
        noticeTxt.text = initialNoticeText;
        selectedPlayerTxt.text = "";
        sendButton.SetActive(false);
    }


    public void SetSelectedPlayer(string playerName, Sprite portrait)
    {
        selectedPlayerTxt.text = "Where are you sending " + playerName + "?";
        selectedPlayerPortrait = portrait;
    }

    public void WoodsButtonClick()
    {
        if (woodsOccupied) { return; }
        woodsPortrait.sprite = selectedPlayerPortrait;
        woodsPortrait.gameObject.SetActive(true);
        woodsOccupied = true;
        placedPlayersNum++;
        ReadyToSendCheck();
    }

    public void CaveButtonClick()
    {
        if (cavesOccupied) { return; }
        cavesPortrait.sprite = selectedPlayerPortrait;
        cavesPortrait.gameObject.SetActive(true);
        cavesOccupied = true;
        placedPlayersNum++;
        ReadyToSendCheck();
    }

    public void BeachButtonClick()
    {
        if (beachOccupied) { return; }
        beachPortrait.sprite = selectedPlayerPortrait;
        beachPortrait.gameObject.SetActive(true);
        beachOccupied = true;
        placedPlayersNum++;
        ReadyToSendCheck();
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
}
