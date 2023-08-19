using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int rocks, wood, vines, seeds;

    public InventoryButton[] buttons;

    private void Start()
    {
        ClearInventory();
        QuantityUpdated();
    }

    private void ClearInventory()
    {
        rocks = 0;
        wood = 0;
        vines = 0;
        seeds = 0;
    }

    public void QuantityUpdated()
    {
        buttons[0].SetResourceAmount(vines);
        buttons[1].SetResourceAmount(seeds);
        buttons[2].SetResourceAmount(wood);
        buttons[3].SetResourceAmount(rocks);
    }

    public void AdjustRock(int adjustAmt)
    {
        rocks += adjustAmt;
        QuantityUpdated();
    }

    public void AdjustVines(int adjustAmt)
    {
        vines += adjustAmt;
        QuantityUpdated();
    }

    public void AdjustWood(int adjustAmt)
    {
        wood += adjustAmt;
        QuantityUpdated();
    }

    public void AdjustSeed(int adjustAmt)
    {
        seeds += adjustAmt;
        QuantityUpdated();
    }


}
