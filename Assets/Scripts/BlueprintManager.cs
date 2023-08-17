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
    private void Start()
    {
        instance = this;
        GenerateButtons();
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
        requirements.text = "Requires: <br>";

        foreach(int mat in selectedFurniture.requiredMaterialType)
        {
            print("Mat: " + mat);
            switch (mat)
            {
                case 0:
                    requirements.text += selectedFurniture.requiredMaterialQuantity[0].ToString() + " vine<br>";
                    break;

                case 1:
                    requirements.text += selectedFurniture.requiredMaterialQuantity[1].ToString() + " seed<br>";
                    break;

                case 2:
                    requirements.text += selectedFurniture.requiredMaterialQuantity[2].ToString() + " wood<br>";
                    break;

                case 3:
                    requirements.text += selectedFurniture.requiredMaterialQuantity[3].ToString() + " rock<br>";
                    break;
            }
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

}
