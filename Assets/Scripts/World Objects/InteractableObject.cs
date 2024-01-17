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


        if (SelectionManager.instance.onTarget && SelectionManager.instance.selecedObject == gameObject
            && MainManager.instance.menuStates == MenuStates.None)
        {
            //If Object is a Pickup
            if (interacteableType == InteracteableType.Pickup)
            {
                print("Interract with a Pickup");

                //Check If item can be added
                if (InventoryManager.Instance.AddItemToInventory(0, gameObject, false))
                {
                    //Remove Object from the worldObjectList
                    for (int i = 0; i < InventoryManager.Instance.worldObjectList.Count; i++)
                    {
                        if (gameObject == InventoryManager.Instance.worldObjectList[i].gameObject)
                        {
                            InventoryManager.Instance.worldObjectList.RemoveAt(i);

                            break;
                        }
                    }

                    //Destroy gameObject
                    DestroyThisObject();
                }
            }

            //If Object is an Inventory
            else if (interacteableType == InteracteableType.Inventory)
            {
                print("Interract with an Inventory");

                //Open the player Inventory
                InventoryManager.Instance.OpenPlayerInventory();

                //Open the chest Inventory
                InventoryManager.Instance.chestInventoryOpen = inventoryIndex;
                InventoryManager.Instance.PrepareInventoryUI(inventoryIndex, false); //Prepare Player Inventory
                InventoryManager.Instance.chestInventory_Parent.GetComponent<RectTransform>().sizeDelta = InventoryManager.Instance.inventories[inventoryIndex].inventorySize * InventoryManager.Instance.cellsize;
                InventoryManager.Instance.chestInventory_Parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(InventoryManager.Instance.cellsize, InventoryManager.Instance.cellsize);
                InventoryManager.Instance.chestInventory_Parent.SetActive(true);

                MainManager.instance.menuStates = MenuStates.chestMenu;
            }

            //If Object is a Crafting Table
            else if (interacteableType == InteracteableType.CraftingTable)
            {
                print("Interract with a CraftingTable");

                //Open the crafting menu
                CraftingManager.instance.OpenCraftingScreen();
            }

            //If Object is another machine
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


    //--------------------


    public void DestroyThisObject()
    {
        print("9000. Destroy an InteractableObject");

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
    Machine
}