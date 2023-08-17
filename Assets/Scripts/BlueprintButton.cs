using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlueprintButton : MonoBehaviour
{
    public TMP_Text nameText;
    public int itemID;
    private BlueprintManager manager;

    public void SetDetails(string name, int id)
    {
        manager = BlueprintManager.instance;
        nameText.text = name;
        itemID = id;
    }

    public void DisplayDetails()
    {
        manager.SetSelectedFurniture(itemID);
    }
}
