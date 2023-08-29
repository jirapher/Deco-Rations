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
    public int sfxToPlay = -1;
    public int gatherAmt = -1;

    private bool selectable = true;

    private void OnMouseDown()
    {
        if (!selectable || missionMan.someoneIsSelected) { return; }
        missionMan.SetSelectedPlayer(playerName, portrait.sprite, gatherAmt);
        portrait.enabled = false;
        selectable = false;
        AudioManager.instance.PlaySFX(sfxToPlay);
    }

    public void ResetPortrait()
    {
        portrait.enabled = true;
    }

    public void SetSelectable(bool isSelectable)
    {
        selectable = isSelectable;
    }
}
