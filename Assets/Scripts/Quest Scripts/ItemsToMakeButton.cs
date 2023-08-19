using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemsToMakeButton : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text quantityToMake;
    public bool isReward = false;
    public void SetInformation(Sprite image, int quantity)
    {
        itemImage.sprite = image;

        if (isReward)
        {
            Destroy(quantityToMake);
            return;
        }

        quantityToMake.text = "x" + quantity.ToString();
    }
}
