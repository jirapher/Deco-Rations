using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlueprintManager : MonoBehaviour
{
    public FurnitureSO database;
    private FurnitureData selectedFurniture;

    public TMP_Text itemName, itemDescription, bonusDetails, itemSize, requirements;
    public Image itemImage;

    public GameObject blueprintButton;
    public GameObject buttonParent;
    public static BlueprintManager instance;

    public GameObject craftingButton;

    public InventoryManager inventory;
    public DesignUIManager designMan;
    private void Start()
    {
        instance = this;
        GenerateButtons();
        craftingButton.SetActive(false);
        ClearText();
    }
    public void SetSelectedFurniture(int itemID)
    {
        ClearText();

        for(int i = 0; i < database.furnitureData.Count; i++)
        {
            if (database.furnitureData[i].id == itemID)
            {
                selectedFurniture = database.furnitureData[i];
                DisplayFurnitureDetails();
            }
        }
    }

    private void DisplayFurnitureDetails()
    {
        itemName.text = selectedFurniture.name;
        itemDescription.text = selectedFurniture.description;
        itemImage.sprite = selectedFurniture.itemImage;
        itemSize.text = "Size: W: " + selectedFurniture.size.x.ToString() + " H: " + selectedFurniture.size.y.ToString();
        if (selectedFurniture.restoresHP)
        {
            bonusDetails.text = "Restores 1 HP.";
        }

        SetRequirementsText();
    }

    public void SetRequirementsText()
    {
        if (selectedFurniture.isLocked) { itemDescription.text = "Complete missions to unlock this item."; return; }
        requirements.text = "Requires:<br>";
        bool canCraft = true;
        foreach(int mat in selectedFurniture.requiredMaterialType)
        {
            switch (mat)
            {
                case 0:
                    int vines = selectedFurniture.requiredMaterialQuantity[0];
                    requirements.text += vines.ToString() + " vine<br>";
                    if(inventory.vines < vines) { canCraft = false; }
                    break;

                case 1:
                    int seed = selectedFurniture.requiredMaterialQuantity[1];
                    requirements.text += seed.ToString() + " seed<br>";
                    if (inventory.vines < seed) { canCraft = false; }
                    break;

                case 2:
                    int wood = selectedFurniture.requiredMaterialQuantity[2];
                    requirements.text += wood.ToString() + " wood<br>";
                    if (inventory.vines < wood) { canCraft = false; }
                    break;

                case 3:
                    int rock = selectedFurniture.requiredMaterialQuantity[3];
                    requirements.text += rock.ToString() + " rock<br>";
                    if (inventory.vines < rock) { canCraft = false; }
                    break;
            }
        }
        //run check on inventory to see if craft button shown or not
        if (canCraft)
        {
            craftingButton.SetActive(true);
        }
        else
        {
            craftingButton.SetActive(false);
        }
    }

    public void ClearText()
    {
        itemName.text = "";
        itemDescription.text = "";
        itemImage.sprite = null;
        itemSize.text = "";
        bonusDetails.text = "";
        requirements.text = "";
    }

    public void GenerateButtons()
    {
        foreach(FurnitureData d in database.furnitureData)
        {
            GameObject b = Instantiate(blueprintButton, buttonParent.transform);
            b.GetComponent<BlueprintButton>().SetDetails(d.name, d.id);
        }
    }

    public void CraftButton()
    {
        selectedFurniture.quantity++;

        foreach (int mat in selectedFurniture.requiredMaterialType)
        {
            switch (mat)
            {
                case 0:
                    inventory.AdjustVines(-selectedFurniture.requiredMaterialQuantity[0]);
                    break;

                case 1:
                    inventory.AdjustSeed(-selectedFurniture.requiredMaterialQuantity[1]);
                    break;

                case 2:
                    inventory.AdjustWood(-selectedFurniture.requiredMaterialQuantity[2]);
                    break;

                case 3:
                    inventory.AdjustRock(-selectedFurniture.requiredMaterialQuantity[3]);
                    break;
            }
        }

        //do we meet requires to craft again?
        SetRequirementsText();
        designMan.AddItemToInventory(selectedFurniture);
    }

}
