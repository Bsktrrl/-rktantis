using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("Is the player in range?")]
    [HideInInspector] public bool playerInRange;

    [Header("Stats")]
    public Items itemName;
    public int amount;
    public InteracteableType interacteableType;

    [Header("If Object is a Plant")]
    public GameObject plantParent;

    [Header("If Object is an Inventory")]
    public int inventoryIndex;


    //--------------------


    private void Start()
    {
        PlayerButtonManager.objectInterraction_isPressedDown += ObjectInteraction;

        //Add SphereCollider for the item
        Vector3 scale = gameObject.transform.lossyScale;
    }


    //--------------------


    void ObjectInteraction()
    {
        if (gameObject.GetComponent<MoveableObject>())
        {
            if (gameObject.GetComponent<MoveableObject>().isSelectedForMovement) { return; }
        }
        

        //-----


        if (SelectionManager.Instance.onTarget && SelectionManager.Instance.selecedObject == gameObject
            && MainManager.Instance.menuStates == MenuStates.None)
        {
            //If Object is an item
            if (interacteableType == InteracteableType.Item)
            {
                //print("Interract with a Pickup");

                //Check If item can be added
                for (int i = 0; i < amount; i++)
                {
                    if (InventoryManager.Instance.AddItemToInventory(0, gameObject, false))
                    {
                        //Remove Object from the worldObjectList
                        WorldObjectManager.Instance.WorldObject_SaveState_RemoveObjectFromWorld(gameObject);

                        //Destroy gameObject
                        DestroyThisObject();
                    }
                }
            }

            //If Object is a Plant
            else if (interacteableType == InteracteableType.Plant)
            {
                print("Interract with a Plant");

                //Pick the Plant
                if (plantParent)
                {
                    if (plantParent.GetComponent<Plant>())
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            //Check If item can be added
                            if (InventoryManager.Instance.AddItemToInventory(0, itemName))
                            {
                                plantParent.GetComponent<Plant>().PickPlant();
                            }
                        }
                    }
                }
            }

            //If Object is an Inventory
            else if (interacteableType == InteracteableType.Inventory)
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

            //If Object is a Crafting Table
            else if (interacteableType == InteracteableType.CraftingTable)
            {
                //print("Interract with a CraftingTable");

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

            //If Object is a SkillTree
            else if (interacteableType == InteracteableType.SkillTreeTable)
            {
                //print("Interract with a SkillTree");

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

            //If Object is a GhostTank
            else if (interacteableType == InteracteableType.GhostTank)
            {
                print("Interract with a GhostTank");
            }
        }
    }


    //--------------------


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
    [Description("")][InspectorName("None")] None,

    [Description("Item")][InspectorName("Item")] Item,
    [Description("Inventory")][InspectorName("Inventory")] Inventory,

    [Description("Crafting Table")][InspectorName("Crafting Table")] CraftingTable,
    [Description("SkillTree Table")][InspectorName("SkillTree Table")] SkillTreeTable,

    [Description("GhostTank")][InspectorName("GhostTank")] GhostTank,
    [Description("Extractor")][InspectorName("Extractor")] Extractor,
    [Description("Ghost Repeller")][InspectorName("Ghost Repeller")] GhostRepeller,
    [Description("Heat Regulator")][InspectorName("Heat Regulator")] HeatRegulator,
    [Description("Resource Converter")][InspectorName("Resource Converter")] ResourceConverter,

    [Description("CropPlot x1")][InspectorName("CropPlot x1")] CropPlot_x1,
    [Description("CropPlot x2")][InspectorName("CropPlot x2")] CropPlot_x2,
    [Description("CropPlot x4")][InspectorName("CropPlot x4")] CropPlot_x4,

    [Description("Grill x1")][InspectorName("Grill x1")] Grill_x1,
    [Description("Grill x2")][InspectorName("Grill x2")] Grill_x2,
    [Description("Grill x4")][InspectorName("Grill x4")] Grill_x4,

    [Description("Battery x1")][InspectorName("Battery x1")] Battery_x1,
    [Description("Battery x2")][InspectorName("Battery x2")] Battery_x2,
    [Description("Battery x3")][InspectorName("Battery x3")] Battery_x3,


    [Description("Plant")][InspectorName("Plant")] Plant
}