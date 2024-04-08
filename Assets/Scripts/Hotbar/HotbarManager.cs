using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : Singleton<HotbarManager>
{
    public GameObject hotbar_Parent;
    public GameObject EquipmentHolder;
    public List<GameObject> EquipmentList = new List<GameObject>();

    public HotbarSave hotbarSave = new HotbarSave();

    public List<Hotbar> hotbarList = new List<Hotbar>();
    public int selectedSlot = 0;
    public Items selectedItem;

    bool loadDataisFinished = false;


    //--------------------


    private void Start()
    {
        PlayerButtonManager.hotbarSelectionDown_isPressed += HandSelection_Down;
        PlayerButtonManager.hotbarSelectionUp_isPressed += HandSelection_UP;

        PlayerButtonManager.isPressed_1 += QuickHotbarSelect_1;
        PlayerButtonManager.isPressed_2 += QuickHotbarSelect_2;
        PlayerButtonManager.isPressed_3 += QuickHotbarSelect_3;
        PlayerButtonManager.isPressed_4 += QuickHotbarSelect_4;
        PlayerButtonManager.isPressed_5 += QuickHotbarSelect_5;

        hotbar_Parent.SetActive(true);
    }
    private void Update()
    {
        if (loadDataisFinished && hotbar_Parent.activeInHierarchy)
        {
            for (int i = 0; i < hotbarList.Count; i++)
            {
                hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarItemID(hotbarList[i].itemID);
                hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotDisplay();
            }
        }
    }


    //--------------------


    public void LoadData()
    {
        //Set selectedSlot
        #region
        selectedSlot = DataManager.Instance.selectedSlot_Store;
        #endregion

        //Setup each HotbarSlot based on saved data
        #region

        List<Hotbar> hotbarTemp = new List<Hotbar>();
        hotbarTemp = DataManager.Instance.hotbarItem_StoreList;

        for (int i = 0; i < hotbarList.Count; i++)
        {
            if (hotbarTemp.Count == hotbarList.Count)
            {
                //Set Name
                hotbarList[i].itemName = hotbarTemp[i].itemName;
                hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName = hotbarList[i].itemName;

                //Set ID
                hotbarList[i].itemID = hotbarTemp[i].itemID;
                hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemsID = hotbarList[i].itemID;

                //Set Image
                hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotDisplay();

                //Set Durability
                hotbarList[i].durabilityCurrent = hotbarTemp[i].durabilityCurrent;
                hotbarList[i].durabilityMax = hotbarTemp[i].durabilityMax;
            }
        }

        hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();

        //If a BuildingHammer is selected
        if (hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName == Items.WoodBuildingHammer
            || hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName == Items.StoneBuildingHammer
            || hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName == Items.CryoniteBuildingHammer)
        {
            BuildingManager.Instance.SetBuildingRequirements(BuildingManager.Instance.GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected), BuildingManager.Instance.buildingRequirement_Parent);
            BuildingManager.Instance.buildingRequirement_Parent.SetActive(true);
        }
        else
        {
            BuildingManager.Instance.buildingRequirement_Parent.SetActive(false);
        }
        #endregion

        loadDataisFinished = true;
    }
    public void SaveData()
    {
        //Save Selected Slot
        DataManager.Instance.selectedSlot_Store = selectedSlot;

        //Save Hotbar Items
        DataManager.Instance.hotbarItem_StoreList.Clear();
        for (int i = 0; i < hotbarList.Count; i++)
        {
            Hotbar hotbarTemp = new Hotbar();
            hotbarTemp.itemName = hotbarList[i].itemName;
            hotbarTemp.itemID = hotbarList[i].itemID;
            hotbarTemp.durabilityCurrent = hotbarList[i].durabilityCurrent;
            hotbarTemp.durabilityMax = hotbarList[i].durabilityMax;

            if (hotbarTemp.itemID <= 0)
            {
                hotbarTemp.itemID = -1;
            }
            else
            { 
                hotbarTemp.itemID = hotbarList[i].itemID;
            }

            //Safty Check for empty slot
            if (hotbarTemp.itemID <= -1)
            {
                hotbarTemp.itemName = Items.None;
            }

            DataManager.Instance.hotbarItem_StoreList.Add(hotbarTemp);
        }
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_Hotbar");
    }



    //--------------------


    public void ChangeItemInHand()
    {
        //Check Visibility Objects
        UpdateVisibilityObjects();

        //Change ArmStates
        EquippmentManager.Instance.GetEquipmentStates(selectedItem);

        //if selected item is empty, leave the hand empty
        if (selectedItem == Items.None)
        {
            //Remove all equipped models
            RemoveAllEquipedModels();
            EquipmentList.Clear();

            SaveData();
            return;
        }

        //if selected Item doesn't have an "equipped model" or is suited for equipment to Hand, leave the Hand empty
        if (MainManager.Instance.GetItem(selectedItem).equippedPrefab == null
            || MainManager.Instance.GetItem(selectedItem).isEquipableInHand == false)
        {
            //Remove all equipped models
            RemoveAllEquipedModels();
            EquipmentList.Clear();

            SaveData();
            return;
        }

        //if selected Item has an "equipped model" AND are suited for equipment to Hand, spawn it in Hand
        else
        {
            //Remove all equipped models
            RemoveAllEquipedModels();
            EquipmentList.Clear();

            //Add the correct model to the Hand
            EquipmentList.Add(Instantiate(MainManager.Instance.GetItem(selectedItem).equippedPrefab, MainManager.Instance.GetItem(selectedItem).equippedPrefab.gameObject.transform.position, EquipmentHolder.transform.rotation, EquipmentHolder.transform));
            EquipmentList[EquipmentList.Count - 1].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            //If the model is a Cup or a Bucket
            if (selectedItem == Items.Cup || selectedItem == Items.Bucket)
            {
                if (EquipmentList[EquipmentList.Count - 1].GetComponent<WaterContainer>())
                {
                    EquipmentList[EquipmentList.Count - 1].GetComponent<WaterContainer>().SetupWaterContainer();
                }
            }

            SaveData();
            return;
        }
    }
    void RemoveAllEquipedModels()
    {
        for (int i = 0; i < EquipmentList.Count; i++)
        {
            if (EquipmentList[i].GetComponent<EquippedItem>())
            {
                EquipmentList[i].GetComponent<EquippedItem>().DestroyObject();
            }
        }
    }


    //--------------------


    void HandSelection_Down()
    {
        if (MainManager.Instance.menuStates == MenuStates.JournalMenu
            || MainManager.Instance.menuStates == MenuStates.ResearchMenu) { return; }

        SoundManager.Instance.Play_Hotbar_ChangeSelectedItem_Clip();

        selectedSlot--;

        if (selectedSlot < 0)
        {
            selectedSlot = 4;
        }

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        LookAtManager.Instance.TurnOffScreens();

        SaveData();
    }
    void HandSelection_UP()
    {
        if (MainManager.Instance.menuStates == MenuStates.JournalMenu
            || MainManager.Instance.menuStates == MenuStates.ResearchMenu) { return; }

        SoundManager.Instance.Play_Hotbar_ChangeSelectedItem_Clip();

        selectedSlot++;

        if (selectedSlot > 4)
        {
            selectedSlot = 0;
        }

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        LookAtManager.Instance.TurnOffScreens();

        SaveData();
    }

    public void SetSelectedItem()
    {
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        EquippmentManager.Instance.GetArmState(selectedItem);

        //Check Visibility Objects
        UpdateVisibilityObjects();

        SaveData();
    }

    void UpdateVisibilityObjects()
    {
        for (int i = 0; i < WorldObjectManager.Instance.worldInvisibleObjectList.Count; i++)
        {
            if (WorldObjectManager.Instance.worldInvisibleObjectList[i] == null)
            {
                WorldObjectManager.Instance.worldInvisibleObjectList.RemoveAt(i);

                continue;
            }

            if (WorldObjectManager.Instance.worldInvisibleObjectList[i].GetComponent<InvisibleObject>())
            {
                WorldObjectManager.Instance.worldInvisibleObjectList[i].GetComponent<InvisibleObject>().UpdateVisibility();
            }
        }

        for (int i = 0; i < WorldObjectManager.Instance.worldObjectList.Count; i++)
        {
            if (WorldObjectManager.Instance.worldObjectList[i].GetComponent<InvisibleObject>())
            {
                WorldObjectManager.Instance.worldObjectList[i].GetComponent<InvisibleObject>().UpdateVisibility();
            }
        }
    }

    #region QuickSlots
    void QuickHotbarSelect_1()
    {
        SoundManager.Instance.Play_Hotbar_ChangeSelectedItem_Clip();

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 0;
        hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void QuickHotbarSelect_2()
    {
        SoundManager.Instance.Play_Hotbar_ChangeSelectedItem_Clip();

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 1;
        hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void QuickHotbarSelect_3()
    {
        SoundManager.Instance.Play_Hotbar_ChangeSelectedItem_Clip();

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 2;
        hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void QuickHotbarSelect_4()
    {
        SoundManager.Instance.Play_Hotbar_ChangeSelectedItem_Clip();

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 3;
        hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void QuickHotbarSelect_5()
    {
        SoundManager.Instance.Play_Hotbar_ChangeSelectedItem_Clip();

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 4;
        hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].hotbar.GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    #endregion

}

public class HotbarSave
{
    //HotbarManager
    public int selectedSlot;

    //HotbarSlot
    public List<Items> hotbarItem = new List<Items>();
}

[Serializable]
public class Hotbar
{
    public GameObject hotbar;
    public Items itemName;
    public int itemID;

    public int durabilityCurrent;
    public int durabilityMax;
}