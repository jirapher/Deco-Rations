using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DesignButtons : MonoBehaviour
{
    public TMP_Text quantityTxt;
    public Image itemPicture;
    private int quantity;
    private int itemID;
    private PlacementSystem placementSystem;

    private void Start()
    {
        placementSystem = FindObjectOfType<PlacementSystem>();
    }
    public void AdjustQuantity(int newQuantity)
    {
        quantity = newQuantity;
        UpdateText();
    }

    public void SetDetails(Sprite itemPic, int quantityNum, int id)
    {
        itemPicture.sprite = itemPic;
        itemID = id;
        quantity = quantityNum;
        UpdateText();
    }

    private void UpdateText()
    {
        quantityTxt.text = quantity.ToString();
    }

    public void OnClick()
    {
        placementSystem.StartPlacement(itemID);
    }

    public int GetItemID()
    {
        return itemID;
    }
}
