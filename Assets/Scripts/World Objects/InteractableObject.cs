using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("Is the player in range?")]
    public bool playerInRange;

    [Header("Stats")]
    public Items itemName;
    public InteracteableType interacteableType;
    public bool isMachine;

    [Header("If Object is an Inventory")]
    [SerializeField] int inventoryIndex;

    //SphereCollider accessCollider = new SphereCollider();


    //--------------------


    private void Start()
    {
        PlayerButtonManager.objectInterraction_isPressedDown += ObjectInteraction;

        //Add SphereCollider for the item
        Vector3 scale = gameObject.transform.lossyScale;
        //accessCollider = gameObject.AddComponent<SphereCollider>();
        //accessCollider.radius = WorldObjectManager.instance.objectColliderRadius / scale.x / 2; //Chenge to only "objectColliderRadius"
        //accessCollider.isTrigger = true;
    }


    //--------------------


    void ObjectInteraction()
    {
        if (SelectionManager.instance.onTarget && SelectionManager.instance.selecedObject == gameObject
            && MainManager.instance.menuStates == MenuStates.None)
        {
            //If Object is a Pickup
            if (interacteableType == InteracteableType.Pickup)
            {
                print("Interract with a Pickup");

                //Check If item can be added
                if (InventoryManager.instance.AddItemToInventory(0, gameObject, false))
                {
                    //Remove Object from the worldObjectList
                    for (int i = 0; i < InventoryManager.instance.worldObjectList.Count; i++)
                    {
                        if (gameObject == InventoryManager.instance.worldObjectList[i].gameObject)
                        {
                            InventoryManager.instance.worldObjectList.RemoveAt(i);

                            break;
                        }
                    }

                    //Unsubscribe from Event
                    PlayerButtonManager.objectInterraction_isPressedDown -= ObjectInteraction;

                    //Destroy gameObject
                    Destroy(gameObject);
                }
            }

            //If Object is an Inventory
            else if (interacteableType == InteracteableType.Inventory)
            {
                print("Interract with an Inventory");

                //Open the player Inventory
                InventoryManager.instance.OpenPlayerInventory();

                //Open the chest Inventory
                InventoryManager.instance.chestInventoryOpen = inventoryIndex;
                InventoryManager.instance.PrepareInventoryUI(inventoryIndex, false); //Prepare Player Inventory
                InventoryManager.instance.chestInventory_Parent.GetComponent<RectTransform>().sizeDelta = InventoryManager.instance.inventories[inventoryIndex].inventorySize * InventoryManager.instance.cellsize;
                InventoryManager.instance.chestInventory_Parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(InventoryManager.instance.cellsize, InventoryManager.instance.cellsize);
                InventoryManager.instance.chestInventory_Parent.SetActive(true);

                MainManager.instance.menuStates = MenuStates.chestMenu;
            }

            //If Object is a Crafting Table
            else if (interacteableType == InteracteableType.CraftingTable)
            {
                print("Interract with a CraftingTable");

                //Open the crafting menu
                CraftingManager.instance.OpenCraftingScreen();
            }

            //If Object is a machine
            else if (interacteableType == InteracteableType.Machine)
            {
                print("Interract with a Machine");

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
}

public enum InteracteableType
{
    None,

    Pickup,
    Inventory,
    CraftingTable,
    Machine
}