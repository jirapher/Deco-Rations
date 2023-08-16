using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelMission : MonoBehaviour
{
    public Image portrait;
    public MissionManager missionMan;
    public int playerNum = 0;
    public string playerName;

    private void OnMouseDown()
    {
        missionMan.SetSelectedPlayer(playerName, portrait.sprite);
        //portrait.sprite = null;
        portrait.enabled = false;
    }

    public void ResetPortrait()
    {
        portrait.enabled = true;
    }
}
