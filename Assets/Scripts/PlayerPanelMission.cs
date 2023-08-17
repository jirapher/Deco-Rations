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

    public int gatherAmt = -1;

    private bool selectable = true;

    private void OnMouseDown()
    {
        if (!selectable) { return; }
        missionMan.SetSelectedPlayer(playerName, portrait.sprite, gatherAmt);
        portrait.enabled = false;
        selectable = false;
    }

    public void ResetPortrait()
    {
        portrait.enabled = true;
        //Need this after we're sure we're done with this page
        selectable = true;
    }
}
