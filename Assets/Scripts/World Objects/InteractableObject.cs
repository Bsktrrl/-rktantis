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
    public int durability_Current;
    public InteracteableType interactableType;

    [Header("If Object is an Ore")]
    public int oreHealth;
    float woodPickaxe_Droprate = 60;
    float stonePickaxe_Droprate = 75;
    float cryonitePickaxe_Droprate = 90;

    [Header("If Object is an Inventory")]
    public int inventoryIndex;

    [Header("If Object is a Plant")]
    public GameObject plantParent;

    bool isHittingGround;


    //--------------------


    private void Start()
    {
        PlayerButtonManager.objectInterraction_isPressedDown += ObjectInteraction;

        //Add SphereCollider for the item
        Vector3 scale = gameObject.transform.lossyScale;
    }

    private void Update()
    {
        //If Item, reduce the velocity on the ground
        if (isHittingGround && GetComponent<Rigidbody>() && interactableType == InteracteableType.Item)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
        }
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
                print("Interract with a PlantItem");

                //Pick the Plant
                if (plantParent)
                {
                    if (plantParent.GetComponent<Plant>())
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

            //If Object is a SkillTree
            #region
            else if (interactableType == InteracteableType.SkillTreeTable)
            {
                //print("Interract with a SkillTree");

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
                print("Interract with a GhostTank");
            }
            #endregion
        }
    }

    public void OreInteraction(Items itemName)
    {
        //If Object is an Ore
        #region
        if (interactableType == InteracteableType.Tungsten_Ore || interactableType == InteracteableType.Stone_Ore
                 || interactableType == InteracteableType.Cryonite_Ore || interactableType == InteracteableType.Magnetite_Ore
                 || interactableType == InteracteableType.Viridian_Ore || interactableType == InteracteableType.Gold_Ore
                 || interactableType == InteracteableType.Ar�diteCrystal_Ore)
        {
            print("Interact with an Ore - " + itemName);

            //Play HackingSound
            float tempPitchCount = (float)(oreHealth / 5);

            if (itemName == Items.WoodPickaxe)
            {
                if (oreHealth >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(50);
                }
                else if (oreHealth >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(40);
                }
                else if (oreHealth >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(30);
                }
                else if (oreHealth >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(20);
                }
                else if (oreHealth >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(10);
                }
                else
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(0);
                }
            }
            else if (itemName == Items.StonePickaxe)
            {
                if (oreHealth >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(50);
                }
                else if (oreHealth >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(40);
                }
                else if (oreHealth >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(30);
                }
                else if (oreHealth >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(20);
                }
                else if (oreHealth >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(10);
                }
                else
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(0);
                }
            }
            else if (itemName == Items.CryonitePickaxe)
            {
                if (oreHealth >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(50);
                }
                else if (oreHealth >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(40);
                }
                else if (oreHealth >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(30);
                }
                else if (oreHealth >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(20);
                }
                else if (oreHealth >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(10);
                }
                else
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(0);
                }
            }

            //Reduce the Ore's health
            oreHealth -= MainManager.Instance.GetItem(itemName).orePower;

            //Check if the OreHealth is 0
            if (oreHealth <= 0)
            {
                //Play OreDestroy sound
                SoundManager.Instance.Play_PickaxeUsage_OreIsDestroid_Clip();

                //Spawn at least 1 item into the World
                SpawnOreItems();

                //Spawn additional items into the World based on the pickaxe used
                bool isSpawningItems = true;
                float modifier = 0;
                while (isSpawningItems)
                {
                    float rand = Random.Range(0, 100);

                    if (itemName == Items.WoodPickaxe && rand <= (woodPickaxe_Droprate - modifier))
                    {
                        SpawnOreItems();
                    }
                    else if (itemName == Items.StonePickaxe && rand <= (stonePickaxe_Droprate - modifier))
                    {
                        SpawnOreItems();
                    }
                    else if (itemName == Items.CryonitePickaxe && rand <= (cryonitePickaxe_Droprate - modifier))
                    {
                        SpawnOreItems();
                    }
                    else
                    {
                        isSpawningItems = false;
                    }

                    modifier += 5;
                }
                
                //Destroy Ore
                DestroyThisObject();
            }
        }
        #endregion
    }

    void SpawnOreItems()
    {
        if (interactableType == InteracteableType.Tungsten_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Tungsten, gameObject);
        }
        else if (interactableType == InteracteableType.Stone_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Stone, gameObject);
        }
        else if (interactableType == InteracteableType.Cryonite_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Cryonite, gameObject);
        }
        else if (interactableType == InteracteableType.Magnetite_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Magnetite, gameObject);
        }
        else if (interactableType == InteracteableType.Viridian_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Viridian, gameObject);
        }
        else if (interactableType == InteracteableType.Gold_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Gold, gameObject);
        }
        else if (interactableType == InteracteableType.Ar�diteCrystal_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Ar�diteCrystal, gameObject);
        }
    }


    //--------------------


    private void OnCollisionEnter(Collision collision)
    {
        //Spawn on the gorund, if an item
        if (collision.gameObject.tag == "Ground" && interactableType == InteracteableType.Item)
        {
            isHittingGround = true;
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
    [Description("")][InspectorName("None")] None,

    [Description("Item")][InspectorName("Item/Item")] Item,
    [Description("Inventory")][InspectorName("Inventory/Inventory")] Inventory,

    [Description("Crafting Table")][InspectorName("Furniture/Crafting Table")] CraftingTable,
    [Description("SkillTree Table")][InspectorName("Furniture/SkillTree Table")] SkillTreeTable,

    [Description("GhostTank")][InspectorName("Machine/GhostTank")] GhostTank,
    [Description("Extractor")][InspectorName("Machine/Extractor")] Extractor,
    [Description("Ghost Repeller")][InspectorName("Machine/Ghost Repeller")] GhostRepeller,
    [Description("Heat Regulator")][InspectorName("Machine/Heat Regulator")] HeatRegulator,
    [Description("Resource Converter")][InspectorName("Machine/Resource Converter")] ResourceConverter,

    [Description("CropPlot x1")][InspectorName("Machine/CropPlot x1")] CropPlot_x1,
    [Description("CropPlot x2")][InspectorName("Machine/CropPlot x2")] CropPlot_x2,
    [Description("CropPlot x4")][InspectorName("Machine/CropPlot x4")] CropPlot_x4,

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
    [Description("Ar�dite Crystal Ore")][InspectorName("Ore/Ar�dite Crystal Ore")] Ar�diteCrystal_Ore
}