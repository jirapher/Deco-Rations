using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public Image itemImage;
    public int itemQuantity = 0;
    public ItemMenu menu;
    public TMP_Text quantityTxt;

    public void SetItemDetails(Sprite image, int quantity, int itemNum)
    {
        itemQuantity = quantity;
        itemImage.sprite = image;
        UpdateQuantity();
    }

    public void SubtractQuantity(int toSubtract)
    {
        itemQuantity -= toSubtract;
        if(itemQuantity <= 0)
        {
            ClearButton();
            return;
        }

        UpdateQuantity();
    }

    public void UpdateQuantity()
    {
        quantityTxt.text = itemQuantity.ToString();
    }

    public void ClearButton()
    {
        itemImage = null;
        itemQuantity = 0;
        quantityTxt.text = "";
    }

    public void ButtonClick()
    {
        menu.InstantiateItem();
    }
}
