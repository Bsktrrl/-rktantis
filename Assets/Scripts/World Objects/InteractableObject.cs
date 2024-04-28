using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    #region Variables
    [Header("Is the player in range?")]
    [HideInInspector] public bool playerInRange;

    [Header("Stats")]
    public Items itemName;
    public int amount;
    public int durability_Current;
    public InteracteableType interactableType;

    [Header("If Object is an Inventory")]
    public int inventoryIndex;

    [Header("If Object is a Plant")]
    public GameObject plantParent;

    [Header("If Object is a Journal Page")]
    public JournalMenuState journalType;
    public int journalPageIndex;

    [Header("If Object is a BlueprintInfo")]
    public string blueprintName;
    public List<BlueprintInfo> blueprintInfo;

    //bool isHittingGround;
    #endregion


    //--------------------


    private void Start()
    {
        PlayerButtonManager.objectInterraction_isPressedDown += ObjectInteraction;
        PlayerButtonManager.objectInteraction_GhostRelease_isPressedDown += ObjectInteraction_ReleaseGhost;

        //Add SphereCollider for the item
        Vector3 scale = gameObject.transform.lossyScale;
    }


    //--------------------


    void ObjectInteraction()
    {
        //if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        if (SelectionManager.Instance.selectedObject)
        {
            if (SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject
            && MainManager.Instance.menuStates == MenuStates.None)
            {
                //If Object is an item
                #region
                if (interactableType == InteracteableType.Item)
                {
                    //print("Interact with a Pickup");

                    //Check If item can be added
                    for (int i = 0; i < amount; i++)
                    {
                        if (InventoryManager.Instance.AddItemToInventory(0, gameObject, false))
                        {
                            SoundManager.Instance.Play_Inventory_PickupItem_Clip();

                            //Remove Object from the worldObjectList
                            WorldObjectManager.Instance.WorldObject_SaveState_RemoveObjectFromWorld(gameObject);

                            //Update the ArídianKey Journal Page
                            if (gameObject.GetComponent<ArídianKey>())
                            {
                                gameObject.GetComponent<ArídianKey>().ArídianKeyInteraction();
                            }
                            //Update the AriditeCrystal Journal Page
                            else if (gameObject.GetComponent<AríditeCrystal>())
                            {
                                gameObject.GetComponent<AríditeCrystal>().AríditeCrystalInteraction();
                            }

                            //Destroy gameObject
                            DestroyThisObject();
                        }
                    }
                }
                #endregion

                //If Object is a PlantItem
                #region
                else if (interactableType == InteracteableType.Plant)
                {
                    //print("Interract with a PlantItem");

                    //Pick the Plant
                    if (plantParent)
                    {
                        if (plantParent.GetComponent<Plant>() && !plantParent.GetComponent<Plant>().isPicked)
                        {
                            if (plantParent.GetComponent<Plant>().isInCropPlot)
                            {
                                if (plantParent.GetComponent<Plant>().plantIsReadyInCropPlot)
                                {
                                    SoundManager.Instance.Play_Inventory_PickupItem_Clip();

                                    //Check If item can be added
                                    InventoryManager.Instance.AddItemToInventory(0, itemName);

                                    plantParent.GetComponent<Plant>().pickablePart.GetComponent<InteractableObject>().DestroyThisObject();
                                    plantParent.GetComponent<Plant>().DestroyThisObject();
                                }
                            }
                            else
                            {
                                for (int i = 0; i < amount; i++)
                                {
                                    SoundManager.Instance.Play_Inventory_PickupItem_Clip();

                                    //Check If item can be added
                                    InventoryManager.Instance.AddItemToInventory(0, itemName);

                                    plantParent.GetComponent<Plant>().PickPlant();
                                }
                            }
                        }
                    }
                }
                #endregion

                //If Object is an Inventory
                #region
                else if (interactableType == InteracteableType.Inventory)
                {
                    //print("Interract with an Inventory");

                    TabletManager.Instance.objectInteractingWith_Object = gameObject;

                    //Set Open Chest Animation
                    if (gameObject.GetComponent<Animations_Objects>())
                    {
                        gameObject.GetComponent<Animations_Objects>().StartAnimation();
                    }

                    //Open the chest Inventory
                    InventoryManager.Instance.chestInventoryOpen = inventoryIndex;
                    InventoryManager.Instance.PrepareInventoryUI(inventoryIndex, false); //Prepare Chest Inventory
                    TabletManager.Instance.chestInventory_Parent.GetComponent<RectTransform>().sizeDelta = InventoryManager.Instance.inventories[inventoryIndex].inventorySize * InventoryManager.Instance.cellsize;
                    TabletManager.Instance.chestInventory_Parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(InventoryManager.Instance.cellsize, InventoryManager.Instance.cellsize);
                    TabletManager.Instance.chestInventory_Parent.SetActive(true);

                    InventoryManager.Instance.chestInventory_Fake_Parent.GetComponent<RectTransform>().sizeDelta = InventoryManager.Instance.inventories[inventoryIndex].inventorySize * InventoryManager.Instance.cellsize;
                    InventoryManager.Instance.chestInventory_Fake_Parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(InventoryManager.Instance.cellsize, InventoryManager.Instance.cellsize);
                    InventoryManager.Instance.chestInventory_Fake_Parent.SetActive(true);

                    MainManager.Instance.menuStates = MenuStates.ChestMenu;
                    TabletManager.Instance.objectInteractingWith = ObjectInteractingWith.Chest;

                    InventoryManager.Instance.ClosePlayerInventory();
                    InventoryManager.Instance.OpenPlayerInventory();
                    TabletManager.Instance.OpenTablet(TabletMenuState.ChestInventory);
                }
                #endregion

                //If Object is a Crafting Table
                #region
                else if (interactableType == InteracteableType.CraftingTable)
                {
                    //print("Interract with a CraftingTable");

                    SoundManager.Instance.Play_InteractableObjects_OpenCraftingTable_Clip();

                    TabletManager.Instance.objectInteractingWith_Object = gameObject;

                    //Set Crafting Table Animation
                    if (gameObject.GetComponent<Animations_Objects>())
                    {
                        gameObject.GetComponent<Animations_Objects>().StartAnimation();
                    }

                    //Open the crafting menu
                    TabletManager.Instance.OpenTablet(TabletMenuState.CraftingTable);

                    TabletManager.Instance.objectInteractingWith = ObjectInteractingWith.CraftingTable;
                }
                #endregion

                //If Object is a Research Table
                #region
                else if (interactableType == InteracteableType.ResearchTable)
                {
                    //print("Interact with a Research Table");

                    SoundManager.Instance.Play_InteractableObjects_OpenResearchTable_Clip();

                    TabletManager.Instance.objectInteractingWith_Object = gameObject;

                    //Set Research Table Animation
                    if (gameObject.GetComponent<Animations_Objects>())
                    {
                        gameObject.GetComponent<Animations_Objects>().StartAnimation();
                    }

                    //Open the Research menu
                    TabletManager.Instance.OpenTablet(TabletMenuState.ResearchTable);

                    TabletManager.Instance.objectInteractingWith = ObjectInteractingWith.ResearchTable;
                }
                #endregion

                //If Object is a SkillTree
                #region
                else if (interactableType == InteracteableType.SkillTreeTable)
                {
                    //print("Interact with a SkillTree");

                    SoundManager.Instance.Play_InteractableObjects_OpenSkillTreeTable_Clip();

                    TabletManager.Instance.objectInteractingWith_Object = gameObject;

                    //Set SkillTree Animation
                    if (gameObject.GetComponent<Animations_Objects>())
                    {
                        gameObject.GetComponent<Animations_Objects>().StartAnimation();
                    }

                    //Open the crafting menu
                    TabletManager.Instance.OpenTablet(TabletMenuState.SkillTree);

                    TabletManager.Instance.objectInteractingWith = ObjectInteractingWith.SkillTree;
                }
                #endregion

                //If Object is a GhostTank
                #region
                else if (interactableType == InteracteableType.GhostTank)
                {
                    //print("Interact with a GhostTank");

                    if (gameObject.GetComponent<GhostTank>())
                    {
                        gameObject.GetComponent<GhostTank>().InteractWithGhostTank();
                    }
                }
                #endregion

                //If Object is a CropPlot
                #region
                else if (interactableType == InteracteableType.CropPlot_x1
                         || interactableType == InteracteableType.CropPlot_x2
                         || interactableType == InteracteableType.CropPlot_x4)
                {
                    if (GetComponent<CropPlotSlot>())
                    {
                        GetComponent<CropPlotSlot>().InteractWithCropPlotSlot();
                    }
                }
                #endregion

                //If Object is a JournalPage
                #region
                else if (interactableType == InteracteableType.JournalPage)
                {
                    //print("Interact with a Journal Page");

                    SoundManager.Instance.Play_JournalPage_GetNewJournalPage_Clip();

                    JournalManager.Instance.JournalNotification();

                    JournalManager.Instance.AddJournalPageToList(journalType, journalPageIndex);

                    //Update the JournalPage
                    if (gameObject.GetComponent<JournalObject>())
                    {
                        SoundManager.Instance.Play_Inventory_PickupItem_Clip();
                        gameObject.GetComponent<JournalObject>().JournalPageInteraction();
                    }

                    //Destroy gameObject
                    DestroyThisObject();
                }
                #endregion

                //If Object is a Blueprint
                #region
                else if (interactableType == InteracteableType.Blueprint)
                {
                    //print("Interact with a BlueprintInfo");

                    for (int i = 0; i < blueprintInfo.Count; i++)
                    {
                        if (blueprintInfo[i].blueprint_BuildingBlock_Name != BuildingBlockObjectNames.None)
                        {
                            BlueprintManager.Instance.AddBlueprint(blueprintInfo[i].blueprint_BuildingBlock_Name, blueprintInfo[i].buildingMaterial);
                        }
                        else if (blueprintInfo[i].blueprint_Furniture_Name != FurnitureObjectNames.None)
                        {
                            BlueprintManager.Instance.AddBlueprint(blueprintInfo[i].blueprint_Furniture_Name);
                        }
                        else if (blueprintInfo[i].blueprint_Machine_Name != MachineObjectNames.None)
                        {
                            BlueprintManager.Instance.AddBlueprint(blueprintInfo[i].blueprint_Machine_Name);
                        }
                    }

                    //Update the Blueprint
                    if (gameObject.GetComponent<Blueprint>())
                    {
                        SoundManager.Instance.Play_Inventory_PickupItem_Clip();
                        gameObject.GetComponent<Blueprint>().BlueprintInteraction();
                    }

                    //Destroy gameObject
                    DestroyThisObject();
                }
                #endregion

                //If Object is the Arídea Gate
                #region
                else if (interactableType == InteracteableType.ArídeaGate)
                {
                    if (SelectionManager.Instance.selectedObject.GetComponent<ArideaGateTest>())
                    {
                        SelectionManager.Instance.selectedObject.GetComponent<ArideaGateTest>().ActivateArídeaGate();
                    }
                }
                #endregion

                //If Object is a Connection
                #region
                else if (interactableType == InteracteableType.Connection)
                {
                    if (SelectionManager.Instance.selectedObject.GetComponent<ConnectionPoint>())
                    {
                        if (SelectionManager.Instance.selectedObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith < 0)
                        {
                            SelectionManager.Instance.selectedObject.GetComponent<ConnectionPoint>().AddConnectPoint();
                        }
                        else
                        {
                            SelectionManager.Instance.selectedObject.GetComponent<ConnectionPoint>().RemoveConnectPoint();
                        }
                    }
                }
                #endregion
            }
        }
    }

    void ObjectInteraction_ReleaseGhost()
    {
        if (SelectionManager.Instance.selectedObject)
        {
            if (SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject
            && MainManager.Instance.menuStates == MenuStates.None)
            {
                if (interactableType == InteracteableType.GhostTank)
                {
                    if (gameObject.GetComponent<GhostTank>())
                    {
                        gameObject.GetComponent<GhostTank>().RemoveGhost();
                    }
                }
            }
        }      
    }


    //--------------------


    private void OnCollisionEnter(Collision collision)
    {
        //Spawn on the gorund, if an item
        if (collision.gameObject.tag == "Ground" && interactableType == InteracteableType.Item)
        {
            //isHittingGround = true;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //If a player is entering the area
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        //If a player is exiting the area
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }


    //--------------------


    public void DestroyThisObject()
    {
        //Unsubscribe from Event
        PlayerButtonManager.objectInterraction_isPressedDown -= ObjectInteraction;

        Destroy(gameObject);
    }
}

public enum InteracteableType
{
    [Description("None")][InspectorName("None")] None,

    [Description("Item")][InspectorName("Item/Item")] Item,
    [Description("Inventory")][InspectorName("Inventory/Inventory")] Inventory,

    [Description("Crafting Table")][InspectorName("Furniture/Crafting Table")] CraftingTable,
    [Description("SkillTree Table")][InspectorName("Furniture/SkillTree Table")] SkillTreeTable,

    [Description("GhostTank")][InspectorName("Machine/GhostTank")] GhostTank,
    [Description("Extractor")][InspectorName("Machine/Extractor")] Extractor,
    [Description("Ghost Repeller")][InspectorName("Machine/Ghost Repeller")] GhostRepeller,
    [Description("Heat Regulator")][InspectorName("Machine/Heat Regulator")] HeatRegulator,
    [Description("Resource Converter")][InspectorName("Machine/Resource Converter")] ResourceConverter,

    [Description("CropPlotMenu x1")][InspectorName("Machine/CropPlotMenu x1")] CropPlot_x1,
    [Description("CropPlotMenu x2")][InspectorName("Machine/CropPlotMenu x2")] CropPlot_x2,
    [Description("CropPlotMenu x4")][InspectorName("Machine/CropPlotMenu x4")] CropPlot_x4,

    [Description("Grill x1")][InspectorName("Machine/Grill x1")] Grill_x1,
    [Description("Grill x2")][InspectorName("Machine/Grill x2")] Grill_x2,
    [Description("Grill x4")][InspectorName("Machine/Grill x4")] Grill_x4,

    [Description("Battery x1")][InspectorName("Buff/Battery x1")] Battery_x1,
    [Description("Battery x2")][InspectorName("Buff/Battery x2")] Battery_x2,
    [Description("Battery x3")][InspectorName("Buff/Battery x3")] Battery_x3,

    [Description("Plant")][InspectorName("Plant/Plant")] Plant,

    [Description("Blender")][InspectorName("Machine/Blender")] Blender,
    [Description("Energy Storage Tank")][InspectorName("Machine/Energy Storage Tank")] EnergyStorageTank,

    //Ores - Mining
    [Description("Tungsten Ore")][InspectorName("Ore/Tungsten Ore")] Tungsten_Ore,
    [Description("Gold Ore")][InspectorName("Ore/Gold Ore")] Gold_Ore,
    [Description("Stone Ore")][InspectorName("Ore/Stone Ore")] Stone_Ore,
    [Description("Cryonite Ore")][InspectorName("Ore/Cryonite Ore")] Cryonite_Ore,
    [Description("Magnetite Ore")][InspectorName("Ore/Magnetite Ore")] Magnetite_Ore,
    [Description("Viridian Ore")][InspectorName("Ore/Viridian Ore")] Viridian_Ore,
    [Description("Arídite Crystal Ore")][InspectorName("Ore/Arídite Crystal Ore")] AríditeCrystal_Ore,

    //Journal Pages
    [Description("Journal Page")][InspectorName("Journal Page/Journal Page")] JournalPage,


    [Description("Research Table")][InspectorName("Furniture/Research Table")] ResearchTable,

    //Tree Types
    [Description("Palm Tree")][InspectorName("Trees/Palm Tree")] Palm_Tree,
    [Description("Blood Tree")][InspectorName("Trees/Blood Tree")] BloodTree,
    [Description("Blood Tree Bush")][InspectorName("Trees/Blood Tree Bush")] BloodTreeBush,
    [Description("Tree 4")][InspectorName("Trees/Tree 4")] Tree_4,
    [Description("Tree 5")][InspectorName("Trees/Tree 5")] Tree_5,
    [Description("Tree 6")][InspectorName("Trees/Tree 6")] Tree_6,
    [Description("Tree 7")][InspectorName("Trees/Tree 7")] Tree_7,
    [Description("Tree 8")][InspectorName("Trees/Tree 8")] Tree_8,
    [Description("Tree 9")][InspectorName("Trees/Tree 9")] Tree_9,
    [Description("Cactus")][InspectorName("Trees/Cactus")] Cactus,

    //Ghost
    [Description("Ghost")][InspectorName("Ghost/Ghost")] Ghost,

    //Blueprint
    [Description("BlueprintInfo")][InspectorName("BlueprintInfo/BlueprintInfo")] Blueprint,

    //Arídea Gate
    [Description("Arídea Gate")][InspectorName("Arídea Gate/Arídea Gate")] ArídeaGate,

    //Connection
    [Description("Connection")][InspectorName("Connection/Connection")] Connection,
}

[Serializable]
public class BlueprintInfo
{
    public BuildingBlockObjectNames blueprint_BuildingBlock_Name;
    public BuildingMaterial buildingMaterial;
    public FurnitureObjectNames blueprint_Furniture_Name;
    public MachineObjectNames blueprint_Machine_Name;
}