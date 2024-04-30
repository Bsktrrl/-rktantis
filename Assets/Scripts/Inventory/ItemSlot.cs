using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Items itemName = Items.None;
    public int itemID;
    public int inventoryIndex;

    [Header("HotbarSelectInfo")]
    public GameObject hotbarSelectorParent;
    public TextMeshProUGUI hotbarIndex_Text;

    [Header("DurabilityMeter")]
    public GameObject durabilityMeterParent;
    public Image durabilityMeterImage;
    public int durabilityMax;
    public int durabilityCurrent;


    //--------------------


    private void Start()
    {
        //Change Pos/Rot
        gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, -180f, 0f));

        if (itemName != Items.None)
        {
            InventoryManager.Instance.ChangeItemInfoBox(itemName, this);
        }

        InventoryManager.Instance.ResetItemInfoBox();
    }


    //--------------------


    //When Clicked
    public void OnPointerUp(PointerEventData eventData)
    {
        //If player is in a "Research Table"
        if (eventData.button == PointerEventData.InputButton.Left && MainManager.Instance.menuStates == MenuStates.ResearchMenu
            && !ResearchManager.Instance.isResearching)
        {
            ResearchManager.Instance.SetResearchItemInfo(itemName);
        }

        //If player is in a "CropPlot", add the Seed to the CropPlot, if available
        else if (eventData.button == PointerEventData.InputButton.Left && MainManager.Instance.menuStates == MenuStates.CropPlotMenu)
        {
            //If itemSlot contain a Seed
            if (itemName == Items.ArídisPlantSeed || itemName == Items.GluePlantSeed || itemName == Items.CrimsonCloudBushSeed
                || itemName == Items.RedCottonPlantSeed || itemName == Items.SpikPlantSeed || itemName == Items.SmallCactusplantSeed
                || itemName == Items.LargeCactusplantSeed || itemName == Items.PuddingCactusSeed || itemName == Items.StalkFruitSeed
                || itemName == Items.TripodFruitSeed || itemName == Items.HeatFruitSeed || itemName == Items.FreezeFruitSeed
                || itemName == Items.TwistedMushroomSeed || itemName == Items.GroundMushroomSeed || itemName == Items.SandTubesSeed
                || itemName == Items.PalmTreeSeed || itemName == Items.BloodTreeSeed)
            {
                for (int i = 0; i < CropPlotManager.Instance.CropPlotInfo_Interacting.cropPlotSlotList.Count; i++)
                {
                    if (CropPlotManager.Instance.CropPlotInfo_Interacting.cropPlotSlotList[i].cropState == CropState.Empty)
                    {
                        print("Add Seed to CropPlot | Index: " + i);

                        //Add Seed
                        CropPlotManager.Instance.AddSeed(itemName, i);

                        //Remove Seed from PlayerInventory
                        InventoryManager.Instance.RemoveItemFromInventory(0, itemName, itemID, true);

                        break;
                    }
                }
            }
        }

        //If only player inventory is used
        else if (MainManager.Instance.menuStates == MenuStates.InventoryMenu
            || MainManager.Instance.menuStates == MenuStates.CraftingMenu
            || MainManager.Instance.menuStates == MenuStates.EquipmentMenu)
        {
            //If the right Mouse button is pressed - Remove item from inventory
            #region
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                RemoveItemFromInventory(false);
            }
            #endregion

            //If the left Mouse button is pressed - Mark this item to an available Hotbar space
            #region
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                //If clicked item is a HandEqipable
                if (MainManager.Instance.GetItem(itemName).isEquipableInHand)
                {
                    AssignItemToHotbar();
                }

                //If clicked item is a Consumable
                else if (MainManager.Instance.GetItem(itemName).isConsumeable)
                {
                    EatConsumable();
                }

                //If clicked item is a Clothing
                else if (MainManager.Instance.GetItem(itemName).isEquipableClothes)
                {
                    AssignItemToClothingSlot();
                }
            }
            #endregion
        }

        //If player is in a chest
        else if (MainManager.Instance.menuStates == MenuStates.ChestMenu)
        {
            //If the right Mouse button is pressed - Move this item between the open inventories, if possible
            #region
            if (eventData.button == PointerEventData.InputButton.Right && itemName != Items.None)
            {
                MoveItemBetweenChests();
            }
            #endregion

            //If the left Mouse button is pressed on a playerInventory slot - Mark this item to an available Hotbar space
            #region
            else if (eventData.button == PointerEventData.InputButton.Left && inventoryIndex <= 0)
            {
                //If clicked item is a HandEqipable
                if (MainManager.Instance.GetItem(itemName).isEquipableInHand)
                {
                    AssignItemToHotbar();
                }

                //If clicked item is a Consumable
                else if (MainManager.Instance.GetItem(itemName).isConsumeable)
                {
                    EatConsumable();
                }

                //If clicked item is a Clothing
                else if (MainManager.Instance.GetItem(itemName).isEquipableClothes)
                {
                    AssignItemToClothingSlot();
                }
            }
            #endregion
        }

        if (itemName != Items.None)
        {
            InventoryManager.Instance.ChangeItemInfoBox(itemName, this);
        }
    }
    
    void RemoveItemFromInventory(bool permanentRemove)
    {
        if (permanentRemove)
        {
            InventoryManager.Instance.RemoveItemFromInventory(inventoryIndex, itemName, itemID, true);
        }
        else
        {
            InventoryManager.Instance.RemoveItemFromInventory(inventoryIndex, itemName, itemID);
        }
    }
    public void AssignItemToHotbar()
    {
        if (itemName != Items.None && MainManager.Instance.GetItem(itemName).isEquipableInHand)
        {
            //Check if item is already on the Hotbar, to remove it
            #region
            for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
            {
                if (HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName != Items.None
                    && HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName == itemName
                    && HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemsID == itemID)
                {
                    SoundManager.Instance.Play_Hotbar_RemoveItemFromHotbar_Clip();

                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();

                    HotbarManager.Instance.hotbarList[i].itemName = Items.None;
                    HotbarManager.Instance.hotbarList[i].itemID = -1;
                    HotbarManager.Instance.hotbarList[i].durabilityMax = 0;
                    HotbarManager.Instance.hotbarList[i].durabilityCurrent = 0;

                    HotbarManager.Instance.SetSelectedItem();

                    InventoryManager.Instance.DeselectItemInfoToHotbar(itemName, itemID);

                    //Update the Hand to see if slot is empty
                    HotbarManager.Instance.ChangeItemInHand();

                    if (i == HotbarManager.Instance.selectedSlot)
                    {
                        return;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            #endregion

            //Remove the item currently in the taken HotbarSlot
            #region
            InventoryManager.Instance.DeselectItemInfoToHotbar(HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName, HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemsID);

            HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();
            #endregion

            //Add item to the Hotbar if item isn't already on the Hotbar
            #region
            //Add the new item to the now empty HotbarSlot
            SoundManager.Instance.Play_Hotbar_AssignItemToHotbar_Clip();

            HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName = itemName;
            HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarItemID(itemID);
            HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotDisplay();
            HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].itemName = itemName;
            HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].itemID = itemID;
            HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityMax = durabilityMax;
            HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityCurrent = durabilityCurrent;

            HotbarManager.Instance.SetSelectedItem();
            InventoryManager.Instance.SelectItemInfoToHotbar(HotbarManager.Instance.selectedSlot, itemName, itemID);

            //Update the Hand to see if slot is empty
            HotbarManager.Instance.ChangeItemInHand();
            #endregion
        }
    }
    public void AssignItemToHotbar(int slotNumber)
    {
        if (itemName != Items.None && MainManager.Instance.GetItem(itemName).isEquipableInHand)
        {
            //Check if item is already on the Hotbar, to remove it
            #region
            for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
            {
                if (HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName != Items.None
                    && HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName == itemName
                    && HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemsID == itemID)
                {
                    SoundManager.Instance.Play_Hotbar_RemoveItemFromHotbar_Clip();

                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();

                    HotbarManager.Instance.hotbarList[i].itemName = Items.None;
                    HotbarManager.Instance.hotbarList[i].itemID = -1;
                    HotbarManager.Instance.hotbarList[i].durabilityMax = 0;
                    HotbarManager.Instance.hotbarList[i].durabilityCurrent = 0;

                    HotbarManager.Instance.SetSelectedItem();

                    InventoryManager.Instance.DeselectItemInfoToHotbar(itemName, itemID);

                    //Update the Hand to see if slot is empty
                    HotbarManager.Instance.ChangeItemInHand();

                    if (i == slotNumber)
                    {
                        return;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            #endregion

            //Remove the item currently in the taken HotbarSlot
            #region
            InventoryManager.Instance.DeselectItemInfoToHotbar(HotbarManager.Instance.hotbarList[slotNumber].hotbar.GetComponent<HotbarSlot>().hotbarItemName, HotbarManager.Instance.hotbarList[slotNumber].hotbar.GetComponent<HotbarSlot>().hotbarItemsID);

            HotbarManager.Instance.hotbarList[slotNumber].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();
            #endregion

            //Add item to the Hotbar if item isn't already on the Hotbar
            #region
            //Add the new item to the now empty HotbarSlot
            SoundManager.Instance.Play_Hotbar_AssignItemToHotbar_Clip();

            HotbarManager.Instance.hotbarList[slotNumber].hotbar.GetComponent<HotbarSlot>().hotbarItemName = itemName;
            HotbarManager.Instance.hotbarList[slotNumber].hotbar.GetComponent<HotbarSlot>().SetHotbarItemID(itemID);
            HotbarManager.Instance.hotbarList[slotNumber].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotDisplay();
            HotbarManager.Instance.hotbarList[slotNumber].itemName = itemName;
            HotbarManager.Instance.hotbarList[slotNumber].itemID = itemID;
            HotbarManager.Instance.hotbarList[slotNumber].durabilityMax = durabilityMax;
            HotbarManager.Instance.hotbarList[slotNumber].durabilityCurrent = durabilityCurrent;

            HotbarManager.Instance.SetSelectedItem();
            InventoryManager.Instance.SelectItemInfoToHotbar(slotNumber, itemName, itemID);

            //Update the Hand to see if slot is empty
            HotbarManager.Instance.ChangeItemInHand();
            #endregion
        }
    }

    void MoveItemBetweenChests()
    {
        //print("ChestInventory - Right");
        //Move from Player Inventory to chest
        if (inventoryIndex <= 0)
        {
            InventoryManager.Instance.MoveItemToInventory(inventoryIndex, gameObject, itemID);
        }

        //Move from chest to Player Inventory
        else
        {
            InventoryManager.Instance.MoveItemToInventory(inventoryIndex, gameObject, itemID);
        }
    }
    void EatConsumable()
    {
        SoundManager.Instance.Play_Inventory_ConsumeItem_Clip();

        //If item is healing the MainHealth
        if (MainManager.Instance.GetItem(itemName).mainHealthHeal > 0)
        {
            float percentage = (float)MainManager.Instance.GetItem(itemName).mainHealthHeal / 100;
            HealthManager.Instance.mainHealthValue += percentage;

            if (HealthManager.Instance.mainHealthValue > 1)
            {
                HealthManager.Instance.mainHealthValue = 1;
            }
        }

        //If item is healing the heatResistaceHealth
        if (MainManager.Instance.GetItem(itemName).heatresistanceHealthHeal > 0)
        {
            float percentage = (float)MainManager.Instance.GetItem(itemName).heatresistanceHealthHeal / 100;
            HealthManager.Instance.heatResistanceValue += percentage;

            if (HealthManager.Instance.heatResistanceValue > 1)
            {
                HealthManager.Instance.heatResistanceValue = 1;
            }
        }

        //If item is healing the hungerHealth
        if (MainManager.Instance.GetItem(itemName).hungerHealthHeal > 0)
        {
            float percentage = (float)MainManager.Instance.GetItem(itemName).hungerHealthHeal / 100;
            HealthManager.Instance.hungerValue += percentage;

            if (HealthManager.Instance.hungerValue > 1)
            {
                HealthManager.Instance.hungerValue = 1;
            }
        }

        //If item is healing the thirstHealth
        if (MainManager.Instance.GetItem(itemName).thirstHealthHeal > 0)
        {
            float percentage = (float)MainManager.Instance.GetItem(itemName).thirstHealthHeal / 100;
            HealthManager.Instance.thirstValue += percentage;

            if (HealthManager.Instance.thirstValue > 1)
            {
                HealthManager.Instance.thirstValue = 1;
            }
        }

        //If item is tweaking the playerTemperature
        if (MainManager.Instance.GetItem(itemName).heatColdRegulator > 0 || MainManager.Instance.GetItem(itemName).heatColdRegulator < 0)
        {
            TemperatureFruit temp = new TemperatureFruit();
            temp.value = MainManager.Instance.GetItem(itemName).heatColdRegulator;
            temp.duration = MainManager.Instance.GetItem(itemName).heatColdRegulatorDuration;

            WeatherManager.Instance.temperatureFruitList.Add(temp);
        }

        //Play Eating Sound
        SoundManager.Instance.Play_Inventory_ConsumeItem_Clip();

        //Remove the item
        RemoveItemFromInventory(true);
    }
    void AssignItemToClothingSlot()
    {
        SoundManager.Instance.Play_Inventory_EquipItem_Clip();

        MenuEquipmentManager.Instance.AddItemToEquipmentSlot(itemName);
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        HotbarManager.Instance.hoveredItem = itemName;

        //Play ItemEntering Sound
        if (itemName != Items.None)
        {
            if (InventoryManager.Instance.lastIDToHover != itemID)
            {
                SoundManager.Instance.Play_Inventory_ItemHover_Clip();
            }
            else
            {
                if (InventoryManager.Instance.lastIDToGet == itemID && InventoryManager.Instance.lastIDToHover == itemID)
                {

                }
                else if (InventoryManager.Instance.lastIDToGet != itemID && InventoryManager.Instance.lastIDToHover != itemID)
                {
                    SoundManager.Instance.Play_Inventory_ItemHover_Clip();
                }
            }
        }

        InventoryManager.Instance.lastItemToHover = itemName;
        InventoryManager.Instance.lastIDToHover = itemID;

        if (inventoryIndex <= 0)
        {
            InventoryManager.Instance.SetPlayerItemInfo(itemName, true);
        }
        else
        {
            InventoryManager.Instance.SetPlayerItemInfo(itemName, false);
        }

        InventoryManager.Instance.SetItemSelectedHighlight_Active(inventoryIndex, itemID, itemName, true);

        if (itemName != Items.None)
        {
            InventoryManager.Instance.ChangeItemInfoBox(itemName, this);
        }

        InventoryManager.Instance.lastIDToGet = itemID;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HotbarManager.Instance.hoveredItem = Items.None;

        if (inventoryIndex <= 0)
        {
            InventoryManager.Instance.SetPlayerItemInfo(Items.None, true);
        }
        else
        {
            InventoryManager.Instance.SetPlayerItemInfo(Items.None, false);
        }

        InventoryManager.Instance.ResetItemInfoBox();

        InventoryManager.Instance.SetItemSelectedHighlight_Active(inventoryIndex, itemID, itemName, false);

        InventoryManager.Instance.lastIDToGet = -1;
    }


    //--------------------


    public void ActivateHotbarInfoToItemSlot(int hotbarIndex)
    {
        hotbarIndex_Text.text = hotbarIndex.ToString();
        hotbarSelectorParent.SetActive(true);
    }
    public void DeactivateHotbarInfoToItemSlot()
    {
        hotbarSelectorParent.SetActive(false);
    }

    public void ActivateDurabilityMeter()
    {
        float tempFill = (float)durabilityCurrent / durabilityMax;

        durabilityMeterImage.fillAmount = tempFill;
        durabilityMeterParent.SetActive(true);
    }
    public void DeactivateDurabilityMeter()
    {
        durabilityMeterParent.SetActive(false);
    }


    //--------------------


    public void ChangeImageColor(Color color)
    {
        if (gameObject.GetComponent<Image>())
        {
            gameObject.GetComponent<Image>().color = color;
        }
    }


    //--------------------


    public void DestroyItemSlot()
    {
        Destroy(gameObject);
    }
}
