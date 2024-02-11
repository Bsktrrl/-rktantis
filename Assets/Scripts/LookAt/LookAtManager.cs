using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LookAtManager : Singleton<LookAtManager>
{
    public GameObject LookAt_Parent;

    public InteracteableType typeLookingAt;

    [Header("Item")]
    [SerializeField] GameObject Item_Panel;
    [SerializeField] Image ItemImage;
    [SerializeField] TextMeshProUGUI ItemName;

    [Header("Plant")]
    [SerializeField] GameObject Plant_Panel;
    [SerializeField] TextMeshProUGUI PlantName;
    [SerializeField] Image PlantResourceImage;
    [SerializeField] TextMeshProUGUI PlantGrowthInfo;


    //--------------------


    private void Start()
    {
        //Turn off all screens
        TurnOffScreens();
    }
    private void Update()
    {
        if (!LookAt_Parent.activeInHierarchy)
        {
            return;
        }


        //If looking at nothing
        #region
        if (typeLookingAt == InteracteableType.None)
        {
            //Turn off all screens
            TurnOffScreens();

            return;
        }
        #endregion

        //If looking at an item
        #region
        else if (typeLookingAt == InteracteableType.Item)
        {
            //Turn off all screens
            TurnOffScreens();

            ItemDisplay();

            Item_Panel.SetActive(true);

            return;
        }
        #endregion

        //If looking at a Plant
        #region
        else if (typeLookingAt == InteracteableType.Plant)
        {
            //Turn off all screens
            TurnOffScreens();

            int temp = PlantDisplay();

            if (temp == 1)
            {
                Plant_Panel.SetActive(true);
                Item_Panel.SetActive(false);
            }
            else if (temp == 2)
            {
                Plant_Panel.SetActive(false);
                Item_Panel.SetActive(true);
            }

            return;
        }
        #endregion
    }


    //--------------------


    void ItemDisplay()
    {
        if (SelectionManager.Instance.selecedObject && SelectionManager.Instance.onTarget)
        {
            if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
            {
                Item item = MainManager.Instance.GetItem(SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().itemName);

                ItemImage.sprite = item.hotbarSprite;
                ItemName.text = SpaceTextConverting.Instance.SetText(item.itemName.ToString());
            }
        }
    }
    int PlantDisplay()
    {
        if (SelectionManager.Instance.selecedObject && SelectionManager.Instance.onTarget)
        {
            //If looking at the Plant
            if (SelectionManager.Instance.selecedObject.GetComponent<Plant>())
            {
                Plant plant = SelectionManager.Instance.selecedObject.GetComponent<Plant>();
                InteractableObject plantResource = plant.pickablePart.GetComponent<InteractableObject>();

                PlantName.text = plant.plantType.ToString();
                PlantResourceImage.sprite = MainManager.Instance.GetItem(plantResource.itemName).hotbarSprite;

                if (plant.growthPrecentage >= 100 || plant.growthPrecentage <= 0)
                {
                    PlantGrowthInfo.text = "Growth: Ready";
                }
                else
                {
                    PlantGrowthInfo.text = "Growth: <color=#f08451>" + (int)plant.growthPrecentage + "%";
                }

                return 1;
            }

            //If looking at the PlantResource
            else if(SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
            {
                Item plantItem = MainManager.Instance.GetItem(SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().itemName);

                ItemImage.sprite = plantItem.hotbarSprite;
                ItemName.text = SpaceTextConverting.Instance.SetText(plantItem.itemName.ToString());

                return 2;
            }
        }

        return 0;
    }


    //--------------------


    public void TurnOffScreens()
    {
        Item_Panel.SetActive(false);
        Plant_Panel.SetActive(false);
    }
}
