using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryButton : MonoBehaviour
{
    public int resourceType = -1;
    public int resourceAmount = 0;
    private string resourceName;

    public TMP_Text nameText, quantity;

    //Vines 0, seed 1, wood 2, rock 3

    private void Start()
    {
        SetName();
        nameText.text = resourceName;
        //SetResourceAmount(0);
    }

    private void SetName()
    {
        switch (resourceType)
        {
            case 0:
                resourceName = "Vines";
                break;

            case 1:
                resourceName = "Seeds";
                break;

            case 2:
                resourceName = "Wood";
                break;

            case 3:
                resourceName = "Stone";
                break;
        }
    }

    public void SetResourceAmount(int newAmount)
    {
        resourceAmount = newAmount;
        ZeroCheck();
        quantity.text = resourceAmount.ToString();
    }

    public void ZeroCheck()
    {
        if (resourceAmount < 0)
        {
            resourceAmount = 0;
        }
    }
}
