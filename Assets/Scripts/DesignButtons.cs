using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DesignButtons : MonoBehaviour
{
    public TMP_Text quantityTxt;
    public Image itemPicture;
    public int quantity;
    public int itemID;
    public PlacementSystem placementSystem;
    public void AdjustQuantity(int newQuantity)
    {
        quantity = newQuantity;
        UpdateText();
    }

    public void SetDetails(Sprite itemPic, int quantityNum, int id, PlacementSystem ps)
    {
        itemPicture.sprite = itemPic;
        itemID = id;
        quantity = quantityNum;
        placementSystem = ps;
        UpdateText();
    }

    private void UpdateText()
    {
        quantityTxt.text = quantity.ToString();
    }

    public void OnClick()
    {
        if(quantity <= 0) { return; }
        placementSystem.StartPlacement(itemID);
    }

    public int GetItemID()
    {
        if(itemID < 0) { print("This is fucked up"); }
        return itemID;
    }
}
