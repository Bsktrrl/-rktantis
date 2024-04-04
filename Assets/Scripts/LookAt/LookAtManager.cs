using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LookAtManager : Singleton<LookAtManager>
{
    #region Variables
    public GameObject LookAt_Parent;

    public InteracteableType typeLookingAt;

    [Header("CenterImage")]
    [SerializeField] GameObject centerImage;
    [SerializeField] Sprite handSprite;

    [Header("Item")]
    [SerializeField] GameObject Item_Panel;
    [SerializeField] Image ItemImage;
    [SerializeField] TextMeshProUGUI ItemName;

    [Header("Plant")]
    [SerializeField] GameObject Plant_Panel;
    [SerializeField] TextMeshProUGUI PlantName;
    [SerializeField] Image PlantResourceImage;
    [SerializeField] Image plantGrowthBar;
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
    [SerializeField] GameObject oreDisplay_LineObject;
    [SerializeField] TextMeshProUGUI oreDisplay_Text;

    [Header("Trees")]
    [SerializeField] GameObject treeDisplay_Panel;
    [SerializeField] Image treeDisplay_Image;
    [SerializeField] GameObject treeDisplay_LineObject;
    [SerializeField] TextMeshProUGUI treeDisplay_Text;

    [Header("Journal Page")]
    [SerializeField] GameObject journalDisplay_Panel;
    [SerializeField] TextMeshProUGUI journalCategory_Text;
    [SerializeField] TextMeshProUGUI journalEntryNumber_Text;
    #endregion


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

        //If looking at an Ore Vein
        #region
        else if ((HotbarManager.Instance.selectedItem == Items.WoodPickaxe || HotbarManager.Instance.selectedItem == Items.StonePickaxe || HotbarManager.Instance.selectedItem == Items.CryonitePickaxe
                  || HotbarManager.Instance.selectedItem == Items.AríditeCrystal || HotbarManager.Instance.selectedItem == Items.Flashlight || HotbarManager.Instance.selectedItem == Items.None)
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
        
        //If looking at a Tree
        #region
        else if ((HotbarManager.Instance.selectedItem == Items.WoodAxe || HotbarManager.Instance.selectedItem == Items.StoneAxe || HotbarManager.Instance.selectedItem == Items.CryoniteAxe
                  || HotbarManager.Instance.selectedItem == Items.AríditeCrystal || HotbarManager.Instance.selectedItem == Items.Flashlight || HotbarManager.Instance.selectedItem == Items.None)
            && SelectionManager.Instance.tag == "Tree")
        {
            //Turn off all screens
            TurnOffScreens();

            centerImage.SetActive(false);

            TreeDisplay();

            treeDisplay_Panel.SetActive(true);

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

            Arms.Instance.cannotHit = false;

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
            || typeLookingAt == InteracteableType.SkillTreeTable
            || typeLookingAt == InteracteableType.ResearchTable)
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


        //If looking at a Journal Page
        #region
        else if (typeLookingAt == InteracteableType.JournalPage)
        {
            //Turn off all screens
            TurnOffScreens();

            JournalDisplay();

            journalDisplay_Panel.SetActive(true);

            return;
        }
        #endregion


        //-------------------- Other


        //Else
        #region
        else
        {
            Arms.Instance.cannotHit = false;
        }
        #endregion
    }


    //--------------------


    #region Displays
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

                //Set GrowthBar
                if (plant.isPicked)
                {
                    plantGrowthBar.fillAmount = plant.growthPrecentage / 100;
                }
                else
                {
                    plantGrowthBar.fillAmount = 1f;
                }
                
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
        if (SelectionManager.Instance.selecedObject)
        {
            if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
            {
                //Set Pickaxe Image
                if (HotbarManager.Instance.selectedItem == Items.WoodPickaxe || HotbarManager.Instance.selectedItem == Items.StonePickaxe || HotbarManager.Instance.selectedItem == Items.CryonitePickaxe)
                {
                    oreDisplay_Image.sprite = MainManager.Instance.GetItem(HotbarManager.Instance.selectedItem).hotbarSprite;
                }
                else if (HotbarManager.Instance.selectedItem == Items.None)
                {
                    oreDisplay_Image.sprite = handSprite;
                }

                //Hide text
                oreDisplay_LineObject.SetActive(false);
                oreDisplay_Text.text = "";

                //Set text
                if (Arms.Instance.cannotHit)
                {
                    if (HotbarManager.Instance.selectedItem == Items.None)
                    {
                        switch (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType)
                        {
                            case InteracteableType.None:
                                break;

                            case InteracteableType.Tungsten_Ore:
                                break;
                            case InteracteableType.Gold_Ore:
                                oreDisplay_Text.text = "Requires a \"Stone Pickaxe\" or \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.Stone_Ore:
                                break;
                            case InteracteableType.Cryonite_Ore:
                                oreDisplay_Text.text = "Requires a \"Stone Pickaxe\" or \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.Magnetite_Ore:
                                oreDisplay_Text.text = "Requires a \"Stone Pickaxe\" or \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.Viridian_Ore:
                                oreDisplay_Text.text = "Requires a \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.AríditeCrystal_Ore:
                                oreDisplay_Text.text = "Requires a \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;

                            default:
                                break;
                        }
                    }
                    else if (HotbarManager.Instance.selectedItem == Items.WoodPickaxe)
                    {
                        switch (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType)
                        {
                            case InteracteableType.None:
                                break;

                            case InteracteableType.Tungsten_Ore:
                                break;
                            case InteracteableType.Gold_Ore:
                                oreDisplay_Text.text = "Requires a \"Stone Pickaxe\" or \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.Stone_Ore:
                                break;
                            case InteracteableType.Cryonite_Ore:
                                oreDisplay_Text.text = "Requires a \"Stone Pickaxe\" or \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.Magnetite_Ore:
                                oreDisplay_Text.text = "Requires a \"Stone Pickaxe\" or \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.Viridian_Ore:
                                oreDisplay_Text.text = "Requires a \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.AríditeCrystal_Ore:
                                oreDisplay_Text.text = "Requires a \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;

                            default:
                                break;
                        }
                    }
                    else if (HotbarManager.Instance.selectedItem == Items.StonePickaxe)
                    {
                        switch (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType)
                        {
                            case InteracteableType.None:
                                break;

                            case InteracteableType.Tungsten_Ore:
                                break;
                            case InteracteableType.Gold_Ore:
                                break;
                            case InteracteableType.Stone_Ore:
                                break;
                            case InteracteableType.Cryonite_Ore:
                                break;
                            case InteracteableType.Magnetite_Ore:
                                break;
                            case InteracteableType.Viridian_Ore:
                                oreDisplay_Text.text = "Requires a \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;
                            case InteracteableType.AríditeCrystal_Ore:
                                oreDisplay_Text.text = "Requires a \"Cryonite Pickaxe\"";
                                oreDisplay_LineObject.SetActive(true);
                                break;

                            default:
                                break;
                        }
                    }
                    else if (HotbarManager.Instance.selectedItem == Items.CryonitePickaxe)
                    {
                        switch (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType)
                        {
                            case InteracteableType.None:
                                break;

                            case InteracteableType.Tungsten_Ore:
                                break;
                            case InteracteableType.Gold_Ore:
                                break;
                            case InteracteableType.Stone_Ore:
                                break;
                            case InteracteableType.Cryonite_Ore:
                                break;
                            case InteracteableType.Magnetite_Ore:
                                break;
                            case InteracteableType.Viridian_Ore:
                                break;
                            case InteracteableType.AríditeCrystal_Ore:
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
    void TreeDisplay()
    {
        if (SelectionManager.Instance.selecedObject)
        {
            if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
            {
                //Set Axe Image
                if (HotbarManager.Instance.selectedItem == Items.WoodAxe || HotbarManager.Instance.selectedItem == Items.StoneAxe || HotbarManager.Instance.selectedItem == Items.CryoniteAxe)
                {
                    treeDisplay_Image.sprite = MainManager.Instance.GetItem(HotbarManager.Instance.selectedItem).hotbarSprite;
                }
                else if (HotbarManager.Instance.selectedItem == Items.None)
                {
                    treeDisplay_Image.sprite = handSprite;
                }

                //Hide text
                treeDisplay_LineObject.SetActive(false);
                treeDisplay_Text.text = "";

                //Set text
                if (Arms.Instance.cannotHit)
                {
                    if (HotbarManager.Instance.selectedItem == Items.None)
                    {
                        switch (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType)
                        {
                            case InteracteableType.None:
                                break;

                            case InteracteableType.Palm_Tree:
                                break;
                            case InteracteableType.Tree_2:
                                break;
                            case InteracteableType.Tree_3:
                                break;
                            case InteracteableType.Tree_4:
                                break;
                            case InteracteableType.Tree_5:
                                break;
                            case InteracteableType.Tree_6:
                                break;
                            case InteracteableType.Tree_7:
                                break;
                            case InteracteableType.Tree_8:
                                break;
                            case InteracteableType.Tree_9:
                                break;

                            case InteracteableType.Cactus:
                                treeDisplay_Text.text = "Requires a \"Stone Axe\" or \"Cryonite Axe\"";
                                treeDisplay_LineObject.SetActive(true);
                                break;

                            default:
                                break;
                        }
                    }
                    else if (HotbarManager.Instance.selectedItem == Items.WoodAxe)
                    {
                        switch (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType)
                        {
                            case InteracteableType.None:
                                break;

                            case InteracteableType.Palm_Tree:
                                break;
                            case InteracteableType.Tree_2:
                                break;
                            case InteracteableType.Tree_3:
                                break;
                            case InteracteableType.Tree_4:
                                break;
                            case InteracteableType.Tree_5:
                                break;
                            case InteracteableType.Tree_6:
                                break;
                            case InteracteableType.Tree_7:
                                break;
                            case InteracteableType.Tree_8:
                                break;
                            case InteracteableType.Tree_9:
                                break;

                            case InteracteableType.Cactus:
                                treeDisplay_Text.text = "Requires a \"Stone Axe\" or \"Cryonite Axe\"";
                                treeDisplay_LineObject.SetActive(true);
                                break;

                            default:
                                break;
                        }
                    }
                    else if (HotbarManager.Instance.selectedItem == Items.StoneAxe)
                    {
                        switch (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType)
                        {
                            case InteracteableType.None:
                                break;

                            case InteracteableType.Palm_Tree:
                                break;
                            case InteracteableType.Tree_2:
                                break;
                            case InteracteableType.Tree_3:
                                break;
                            case InteracteableType.Tree_4:
                                break;
                            case InteracteableType.Tree_5:
                                break;
                            case InteracteableType.Tree_6:
                                break;
                            case InteracteableType.Tree_7:
                                break;
                            case InteracteableType.Tree_8:
                                break;
                            case InteracteableType.Tree_9:
                                break;

                            case InteracteableType.Cactus:
                                break;

                            default:
                                break;
                        }
                    }
                    else if (HotbarManager.Instance.selectedItem == Items.CryoniteAxe)
                    {
                        switch (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType)
                        {
                            case InteracteableType.None:
                                break;

                            case InteracteableType.Palm_Tree:
                                break;
                            case InteracteableType.Tree_2:
                                break;
                            case InteracteableType.Tree_3:
                                break;
                            case InteracteableType.Tree_4:
                                break;
                            case InteracteableType.Tree_5:
                                break;
                            case InteracteableType.Tree_6:
                                break;
                            case InteracteableType.Tree_7:
                                break;
                            case InteracteableType.Tree_8:
                                break;
                            case InteracteableType.Tree_9:
                                break;

                            case InteracteableType.Cactus:
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    void JournalDisplay()
    {
        if (SelectionManager.Instance.selecedObject)
        {
            if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
            {
                if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().journalType == JournalMenuState.MentorJournal)
                {
                    journalCategory_Text.text = "Mentor Journal Page";
                }
                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().journalType == JournalMenuState.PlayerJournal)
                {
                    journalCategory_Text.text = "Player Journal Page";
                }
                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().journalType == JournalMenuState.PersonalJournal)
                {
                    journalCategory_Text.text = "Personal Journal Page";
                }

                journalEntryNumber_Text.text = "Entry no. " + (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().journalPageIndex + 1);
            }
        }
    }
    #endregion


    //--------------------


    public void TurnOffScreens()
    {
        WaterDisplay_Panel.SetActive(false);
        oreDisplay_Panel.SetActive(false);
        treeDisplay_Panel.SetActive(false);

        Item_Panel.SetActive(false);
        Plant_Panel.SetActive(false);
        MovableObject_Panel.SetActive(false);

        journalDisplay_Panel.SetActive(false);

        centerImage.SetActive(true);
    }
}
