using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LookAtManager : Singleton<LookAtManager>
{
    public GameObject LookAt_Parent;

    public InteracteableType typeLookingAt;

    [Header("CenterImage")]
    [SerializeField] GameObject centerImage;

    [Header("Item")]
    [SerializeField] GameObject Item_Panel;
    [SerializeField] Image ItemImage;
    [SerializeField] TextMeshProUGUI ItemName;

    [Header("Plant")]
    [SerializeField] GameObject Plant_Panel;
    [SerializeField] TextMeshProUGUI PlantName;
    [SerializeField] Image PlantResourceImage;
    [SerializeField] TextMeshProUGUI PlantGrowthInfo;

    [Header("MovableObject")]
    [SerializeField] GameObject MovableObject_Panel;
    [SerializeField] Image MovableObjectImage;
    [SerializeField] TextMeshProUGUI MovableObjectName;

    [Header("Water")]
    [SerializeField] GameObject WaterDisplay_Panel;
    [SerializeField] Image WaterDisplay_Image;
    [SerializeField] TextMeshProUGUI WaterDisplay_Text;

    [Header("Ore Mine")]
    [SerializeField] GameObject oreDisplay_Panel;
    [SerializeField] Image oreDisplay_Image;
    [SerializeField] TextMeshProUGUI oreDisplay_Text;



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


        //-------------------- Other than InteracteableType


        //If looking at water
        #region
        else if ((HotbarManager.Instance.selectedItem == Items.Cup || HotbarManager.Instance.selectedItem == Items.Bottle || HotbarManager.Instance.selectedItem == Items.Bucket)
            && SelectionManager.Instance.tag == "Water")
        {
            //Turn off all screens
            TurnOffScreens();

            centerImage.SetActive(false);

            WaterDisplay();

            WaterDisplay_Panel.SetActive(true);

            return;
        }
        #endregion

        //If looking at an Ore
        #region
        else if ((HotbarManager.Instance.selectedItem == Items.WoodPickaxe || HotbarManager.Instance.selectedItem == Items.StonePickaxe || HotbarManager.Instance.selectedItem == Items.CryonitePickaxe)
            && SelectionManager.Instance.tag == "Ore")
        {
            //Turn off all screens
            TurnOffScreens();

            centerImage.SetActive(false);

            OreDisplay();

            oreDisplay_Panel.SetActive(true);

            return;
        }
        #endregion


        //-------------------- InteracteableType


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

        //If looking at an Inventory
        #region
        else if (typeLookingAt == InteracteableType.Inventory)
        {
            //Turn off all screens
            TurnOffScreens();

            FurnitureMachineDisplay();

            MovableObject_Panel.SetActive(true);

            return;
        }
        #endregion

        //If looking at a Furniture
        #region
        else if (typeLookingAt == InteracteableType.CraftingTable
            || typeLookingAt == InteracteableType.SkillTreeTable)
        {
            //Turn off all screens
            TurnOffScreens();

            FurnitureMachineDisplay();

            MovableObject_Panel.SetActive(true);

            return;
        }
        #endregion

        //If looking at a Machine
        #region
        else if (typeLookingAt == InteracteableType.Battery_x1
            || typeLookingAt == InteracteableType.Battery_x2
            || typeLookingAt == InteracteableType.Battery_x3

            || typeLookingAt == InteracteableType.CropPlot_x1
            || typeLookingAt == InteracteableType.CropPlot_x2
            || typeLookingAt == InteracteableType.CropPlot_x4

            || typeLookingAt == InteracteableType.Grill_x1
            || typeLookingAt == InteracteableType.Grill_x2
            || typeLookingAt == InteracteableType.Grill_x4

            || typeLookingAt == InteracteableType.GhostTank
            || typeLookingAt == InteracteableType.GhostRepeller
            || typeLookingAt == InteracteableType.EnergyStorageTank

            || typeLookingAt == InteracteableType.ResourceConverter
            || typeLookingAt == InteracteableType.HeatRegulator
            || typeLookingAt == InteracteableType.Extractor
            || typeLookingAt == InteracteableType.Blender)
        {
            //Turn off all screens
            TurnOffScreens();

            FurnitureMachineDisplay();

            MovableObject_Panel.SetActive(true);

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

                PlantName.text = SpaceTextConverting.Instance.SetText(plant.plantType.ToString());
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
    void FurnitureMachineDisplay()
    {
        if (SelectionManager.Instance.selecedObject && SelectionManager.Instance.onTarget)
        {
            if (SelectionManager.Instance.selecedObject.GetComponent<MoveableObject>())
            {
                MoveableObjectInfo tempObject = MainManager.Instance.GetMovableObject(SelectionManager.Instance.selecedObject.GetComponent<MoveableObject>());

                MovableObjectImage.sprite = tempObject.objectSprite;
                MovableObjectName.text = tempObject.Name;
            }
        }
    }

    void WaterDisplay()
    {
        WaterDisplay_Image.sprite = MainManager.Instance.GetItem(HotbarManager.Instance.selectedItem).hotbarSprite;
        WaterDisplay_Text.text = "Press E to refill your " + HotbarManager.Instance.selectedItem.ToString();
    }
    void OreDisplay()
    {
        WaterDisplay_Image.sprite = MainManager.Instance.GetItem(HotbarManager.Instance.selectedItem).hotbarSprite;
        WaterDisplay_Text.text = "Press E to refill your " + HotbarManager.Instance.selectedItem.ToString();
    }


    //--------------------


    public void TurnOffScreens()
    {
        Item_Panel.SetActive(false);
        Plant_Panel.SetActive(false);
        MovableObject_Panel.SetActive(false);

        WaterDisplay_Panel.SetActive(false);
        oreDisplay_Panel.SetActive(false);

        centerImage.SetActive(true);
    }
}
