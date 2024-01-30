using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("Is the player in range?")]
    [HideInInspector] public bool playerInRange;

    [Header("Stats")]
    public Items itemName;
    public InteracteableType interacteableType;
    //public bool isMachine;

    [Header("If Object is an Inventory")]
    [HideInInspector] [SerializeField] int inventoryIndex;


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
            //If Object is a Pickup
            if (interacteableType == InteracteableType.Pickup)
            {
                //print("Interract with a Pickup");

                //Check If item can be added
                if (InventoryManager.Instance.AddItemToInventory(0, gameObject, false))
                {
                    //Remove Object from the worldObjectList
                    WorldObjectManager.Instance.WorldObject_SaveState_RemoveObjectFromWorld(gameObject);

                    //Destroy gameObject
                    DestroyThisObject();
                }
            }

            //If Object is an Inventory
            else if (interacteableType == InteracteableType.Inventory)
            {
                //print("Interract with an Inventory");

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

                //Open the crafting menu
                TabletManager.Instance.OpenTablet(TabletMenuState.CraftingTable);

                TabletManager.Instance.objectInteractingWith = ObjectInteractingWith.CraftingTable;
            }

            //If Object is a SkillTree
            else if (interacteableType == InteracteableType.SkillTree)
            {
                //print("Interract with a SkillTree");

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
    None,

    Pickup,
    Inventory,

    CraftingTable,
    SkillTree,
    GhostTank,
    Extractor,
    GhostRepeller,
    HeatRegulator,
    ResourceConverter,

    BatteryCharger_1,
    BatteryCharger_2,
    BatteryCharger_3,

    CropPlot_1,
    CropPlot_2,
    CropPlot_3,

    Grill_Manual,
    Grill_1,
    Grill_2,
    Grill_4,

    SkillTreeTable
}