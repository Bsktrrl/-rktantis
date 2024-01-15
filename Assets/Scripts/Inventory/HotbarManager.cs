using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : Singleton<HotbarManager>
{
    public GameObject EquipmentHolder;
    public List<GameObject> EuipmentList = new List<GameObject>();

    public HotbarSave hotbarSave = new HotbarSave();

    public List<GameObject> hotbarList = new List<GameObject>();
    public int selectedSlot = 0;
    public Items selectedItem;


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
        for (int i = 0; i < hotbarList.Count; i++)
        {
            if (DataManager.Instance.hotbarItem_StoreList.Count == hotbarList.Count)
            {
                hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName = DataManager.Instance.hotbarItem_StoreList[i];
                hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotImage();
            }
        }

        hotbarList[selectedSlot].GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();

        if (hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName == Items.BuildingHammer)
        {
            BuildingManager.Instance.SetBuildingRequirements(BuildingManager.Instance.GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected), BuildingManager.Instance.buildingRequirement_Parent);
            BuildingManager.Instance.buildingRequirement_Parent.SetActive(true);
        }
        #endregion
    }
    public void SaveData()
    {
        DataManager.Instance.selectedSlot_Store = selectedSlot;

        DataManager.Instance.hotbarItem_StoreList.Clear();
        for (int i = 0; i < hotbarList.Count; i++)
        {
            DataManager.Instance.hotbarItem_StoreList.Add(hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName);
        }
    }
    public void SaveData(ref GameData gameData)
    {
        DataManager.Instance.selectedSlot_Store = selectedSlot;

        DataManager.Instance.hotbarItem_StoreList.Clear();
        for (int i = 0; i < hotbarList.Count; i++)
        {
            DataManager.Instance.hotbarItem_StoreList.Add(hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName);
        }
        print("Save_Hotbar");
    }



    //--------------------


    void ChangeItemInHand()
    {
        //if selected item is empty, leave the hand empty
        if (selectedItem == Items.None)
        {
            //Remove all equipped models
            for (int i = 0; i < EuipmentList.Count; i++)
            {
                EuipmentList[i].GetComponent<EquippedItem>().DestroyObject();
            }

            EuipmentList.Clear();

            //Remove BuildingmMenu
            BuildingSystemMenu.instance.buildingSystemMenu.SetActive(false);

            return;
        }

        //if selected Item doesn't have an "equipped model"
        if (MainManager.instance.GetItem(selectedItem).equippedPrefab == null)
        {
            //Remove all equipped models
            for (int i = 0; i < EuipmentList.Count; i++)
            {
                EuipmentList[i].GetComponent<EquippedItem>().DestroyObject();
            }

            EuipmentList.Clear();
        }

        //if selected Item have an "equipped model"
        else
        {
            //Remove all equipped models
            for (int i = 0; i < EuipmentList.Count; i++)
            {
                EuipmentList[i].GetComponent<EquippedItem>().DestroyObject();
            }

            EuipmentList.Clear();

            //Add the correct model to the hand
            EuipmentList.Add(Instantiate(MainManager.instance.GetItem(selectedItem).equippedPrefab, MainManager.instance.GetItem(selectedItem).equippedPrefab.gameObject.transform.position, EquipmentHolder.transform.rotation, EquipmentHolder.transform));
            EuipmentList[EuipmentList.Count - 1].transform.SetLocalPositionAndRotation(MainManager.instance.GetItem(selectedItem).equippedPrefab.transform.position, Quaternion.identity);
        }

        //Remove BuildingmMenu
        BuildingSystemMenu.instance.buildingSystemMenu.SetActive(false);
    }


    //--------------------


    void HandSelection_Down()
    {
        //if (MainManager.instance.menuStates != MenuStates.None) { return; }

        selectedSlot--;

        if (selectedSlot < 0)
        {
            selectedSlot = 4;
        }

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        hotbarList[selectedSlot].GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void HandSelection_UP()
    {
        //if (MainManager.instance.menuStates != MenuStates.None) { return; }

        selectedSlot++;

        if (selectedSlot > 4)
        {
            selectedSlot = 0;
        }

        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        hotbarList[selectedSlot].GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }

    public void SetSelectedItem()
    {
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

        SaveData();
    }

    #region QuickSlots
    void QuickHotbarSelect_1()
    {
        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 0;
        hotbarList[selectedSlot].GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void QuickHotbarSelect_2()
    {
        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 1;
        hotbarList[selectedSlot].GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void QuickHotbarSelect_3()
    {
        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 2;
        hotbarList[selectedSlot].GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void QuickHotbarSelect_4()
    {
        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 3;
        hotbarList[selectedSlot].GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

        ChangeItemInHand();
        SaveData();
    }
    void QuickHotbarSelect_5()
    {
        for (int i = 0; i < hotbarList.Count; i++)
        {
            hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotUnactive();
        }

        selectedSlot = 4;
        hotbarList[selectedSlot].GetComponent<HotbarSlot>().SetHotbarSlotActive();
        selectedItem = hotbarList[selectedSlot].GetComponent<HotbarSlot>().hotbarItemName;

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