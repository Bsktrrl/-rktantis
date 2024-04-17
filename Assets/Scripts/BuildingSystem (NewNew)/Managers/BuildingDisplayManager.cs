using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplayManager : Singleton<BuildingDisplayManager>
{
    [Header("TabletMenu")]
    [SerializeField] GameObject buildingObject_RequirementList_Parent;

    [SerializeField] GameObject buildingObject_RequirementListHeader_Prefab;
    [SerializeField] GameObject buildingObject_RequirementListSlot_Prefab;

    public List<GameObject> requirementList = new List<GameObject>();

    [SerializeField] Image selectedObject_Image;

    [SerializeField] List<GameObject> BuildingObjectParentList = new List<GameObject>();
    [SerializeField] List<DisplayBlock> buildingObjectList = new List<DisplayBlock>();

    [Header("ScreenInfo")]
    [SerializeField] GameObject buildingObject_ScreenInfo_Parent;
    public List<GameObject> requirementScreenList = new List<GameObject>();


    //--------------------


    private void Update()
    {
        UpdateScreenBuildingDisplay();
    }


    //--------------------


    #region Display Menu - Tablet
    public void UpdateSelectedDisplay(BuildingBlockInfo buildingBlocksInfo)
    {
        if (buildingBlocksInfo == null)
        {
            ResetDisplay();
            return;
        }

        ResetDisplay();

        selectedObject_Image.gameObject.SetActive(true);
        buildingObject_RequirementList_Parent.SetActive(true);
        selectedObject_Image.sprite = buildingBlocksInfo.objectInfo.objectSprite;

        requirementList = InstantiateRequirementList(buildingObject_RequirementList_Parent);
        DisplayRequirements();
    }
    public void UpdateSelectedDisplay(FurnitureInfo furnitureInfo)
    {
        if (furnitureInfo == null)
        {
            ResetDisplay();
            return;
        }

        ResetDisplay();

        selectedObject_Image.gameObject.SetActive(true);
        buildingObject_RequirementList_Parent.SetActive(true);
        selectedObject_Image.sprite = furnitureInfo.objectInfo.objectSprite;

        requirementList = InstantiateRequirementList(buildingObject_RequirementList_Parent);
        DisplayRequirements();
    }
    public void UpdateSelectedDisplay(MachineInfo machinesInfo)
    {
        if (machinesInfo == null)
        {
            ResetDisplay();
            return;
        }

        ResetDisplay();

        selectedObject_Image.gameObject.SetActive(true);
        buildingObject_RequirementList_Parent.SetActive(true);
        selectedObject_Image.sprite = machinesInfo.objectInfo.objectSprite;

        requirementList = InstantiateRequirementList(buildingObject_RequirementList_Parent);
        DisplayRequirements();
    }

    List<GameObject> InstantiateRequirementList(GameObject parent)
    {
        List<GameObject> requirementSlotTempList = new List<GameObject>();

        //Setup the Header
        requirementSlotTempList.Add(Instantiate(buildingObject_RequirementListHeader_Prefab) as GameObject);
        requirementSlotTempList[requirementSlotTempList.Count - 1].transform.parent = parent.transform;
        requirementSlotTempList[requirementSlotTempList.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        requirementSlotTempList[requirementSlotTempList.Count - 1].GetComponent<RectTransform>().SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        //Setup all requirements
        int index = 0;
        if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
        {
            index = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.buildingBlockObjectName_Active, BuildingSystemManager.Instance.activeBuildingObject_Info.buildingMaterial_Active).objectInfo.buildingRequirements.Count;
        }

        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
        {
            index = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.furnitureObjectName_Active).objectInfo.buildingRequirements.Count;
        }

        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
        {
            index = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.machineObjectName_Active).objectInfo.buildingRequirements.Count;
        }

        //Add all RequirementObjects to the List
        for (int i = 0; i < index; i++)
        {
            requirementSlotTempList.Add(Instantiate(buildingObject_RequirementListSlot_Prefab) as GameObject);
            requirementSlotTempList[requirementSlotTempList.Count - 1].transform.parent = parent.transform;
            requirementSlotTempList[requirementSlotTempList.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            requirementSlotTempList[requirementSlotTempList.Count - 1].GetComponent<RectTransform>().SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        return requirementSlotTempList;
    }
    void DisplayRequirements()
    {
        BuildingBlockInfo buildingBlocksInfo = new BuildingBlockInfo();
        FurnitureInfo furnitureInfo = new FurnitureInfo();
        MachineInfo machineInfo = new MachineInfo();

        //Display the Header
        #region
        if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
        {
            if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingBlockObjectName_Active != BuildingBlockObjectNames.None)
            {
                buildingBlocksInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.buildingBlockObjectName_Active, BuildingSystemManager.Instance.activeBuildingObject_Info.buildingMaterial_Active);

                if (requirementList.Count > 0)
                {
                    if (requirementList[0].GetComponent<BuildingRequirementSlot>())
                    {
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_image.gameObject.SetActive(false);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(200, 0, 0), Quaternion.identity);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 75);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = buildingBlocksInfo.objectName;
                    }
                }
            }
        }
        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
        {
            if (BuildingSystemManager.Instance.activeBuildingObject_Info.furnitureObjectName_Active != FurnitureObjectNames.None)
            {
                furnitureInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.furnitureObjectName_Active);

                if (requirementList.Count > 0)
                {
                    if (requirementList[0].GetComponent<BuildingRequirementSlot>())
                    {
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_image.gameObject.SetActive(false);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(200, 0, 0), Quaternion.identity);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 75);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = furnitureInfo.objectName;
                    }
                }
            }
        }
        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
        {
            if (BuildingSystemManager.Instance.activeBuildingObject_Info.machineObjectName_Active != MachineObjectNames.None)
            {
                machineInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.machineObjectName_Active);

                if (requirementList.Count > 0)
                {
                    if (requirementList[0].GetComponent<BuildingRequirementSlot>())
                    {
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_image.gameObject.SetActive(false);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(200, 0, 0), Quaternion.identity);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 75);
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.gameObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
                        requirementList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = machineInfo.objectName;
                    }
                }
            }
        }
        #endregion

        //Display the Requirements
        #region
        for (int i = 1; i < requirementList.Count; i++)
        {
            if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
            {
                if (requirementList[i].GetComponent<BuildingRequirementSlot>())
                {
                    BuildingSystemManager.Instance.enoughItemsToBuild = true;

                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].itemName).hotbarSprite;
                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].itemName.ToString();
                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + InventoryManager.Instance.GetAmountOfItemInInventory(0, buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].itemName) + "/" + buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].amount;

                    if (InventoryManager.Instance.GetAmountOfItemInInventory(0, buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].itemName) < buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].amount)
                    {
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(true);
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = MainManager.Instance.mainColor_Orange;
                        BuildingSystemManager.Instance.enoughItemsToBuild = false;
                    }
                    else
                    {
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                    }
                }
            }
            else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
            {
                if (requirementList[i].GetComponent<BuildingRequirementSlot>())
                {
                    BuildingSystemManager.Instance.enoughItemsToBuild = true;

                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(furnitureInfo.objectInfo.buildingRequirements[i - 1].itemName).hotbarSprite;
                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = furnitureInfo.objectInfo.buildingRequirements[i - 1].itemName.ToString();
                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + InventoryManager.Instance.GetAmountOfItemInInventory(0, furnitureInfo.objectInfo.buildingRequirements[i - 1].itemName) + "/" + furnitureInfo.objectInfo.buildingRequirements[i - 1].amount;

                    if (InventoryManager.Instance.GetAmountOfItemInInventory(0, furnitureInfo.objectInfo.buildingRequirements[i - 1].itemName) < furnitureInfo.objectInfo.buildingRequirements[i - 1].amount)
                    {
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(true);
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = MainManager.Instance.mainColor_Orange;
                        BuildingSystemManager.Instance.enoughItemsToBuild = false;
                    }
                    else
                    {
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                    }
                }
            }
            else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
            {
                if (requirementList[i].GetComponent<BuildingRequirementSlot>())
                {
                    BuildingSystemManager.Instance.enoughItemsToBuild = true;

                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(machineInfo.objectInfo.buildingRequirements[i - 1].itemName).hotbarSprite;
                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = machineInfo.objectInfo.buildingRequirements[i - 1].itemName.ToString();
                    requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + InventoryManager.Instance.GetAmountOfItemInInventory(0, machineInfo.objectInfo.buildingRequirements[i - 1].itemName) + "/" + machineInfo.objectInfo.buildingRequirements[i - 1].amount;

                    if (InventoryManager.Instance.GetAmountOfItemInInventory(0, machineInfo.objectInfo.buildingRequirements[i - 1].itemName) < machineInfo.objectInfo.buildingRequirements[i - 1].amount)
                    {
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(true);
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = MainManager.Instance.mainColor_Orange;
                        BuildingSystemManager.Instance.enoughItemsToBuild = false;
                    }
                    else
                    {
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                    }
                }
            }
        }
        #endregion
    }


    public void ResetDisplay()
    {
        for (int i = requirementList.Count - 1; i >= 0; i--)
        {
            Destroy(requirementList[i]);
        }

        requirementList.Clear();

        selectedObject_Image.gameObject.SetActive(false);
        buildingObject_RequirementList_Parent.SetActive(false);
    }
    #endregion


    //--------------------


    #region Display Screen - Hammer
    void UpdateScreenBuildingDisplay()
    {
        if ((HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer
            || HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer
            || HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer)
            && MainManager.Instance.menuStates == MenuStates.None)
        {
            buildingObject_ScreenInfo_Parent.SetActive(true);
        }
        else
        {
            buildingObject_ScreenInfo_Parent.SetActive(false);
        }
    }

    public void UpdateScreenBuildingDisplayInfo()
    {
        if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
        {
            UpdateSelectedScreenDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.buildingBlockObjectName_Active, BuildingSystemManager.Instance.activeBuildingObject_Info.buildingMaterial_Active));
        }
        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
        {
            UpdateSelectedScreenDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.furnitureObjectName_Active));
        }
        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
        {
            UpdateSelectedScreenDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.machineObjectName_Active));
        }
        else
        {
            ResetScreenDisplay();
        }
    }

    public void UpdateSelectedScreenDisplay(BuildingBlockInfo buildingBlocksInfo)
    {
        if (buildingBlocksInfo == null)
        {
            ResetScreenDisplay();
            return;
        }

        ResetScreenDisplay();

        buildingObject_ScreenInfo_Parent.SetActive(true);

        requirementScreenList = InstantiateRequirementList(buildingObject_ScreenInfo_Parent);
        DisplayScreenRequirements();
    }
    public void UpdateSelectedScreenDisplay(FurnitureInfo furnitureInfo)
    {
        if (furnitureInfo == null)
        {
            ResetScreenDisplay();
            return;
        }

        ResetScreenDisplay();

        buildingObject_ScreenInfo_Parent.SetActive(true);

        requirementScreenList = InstantiateRequirementList(buildingObject_ScreenInfo_Parent);
        DisplayScreenRequirements();
    }
    public void UpdateSelectedScreenDisplay(MachineInfo machinesInfo)
    {
        if (machinesInfo == null)
        {
            ResetScreenDisplay();
            return;
        }

        ResetScreenDisplay();

        buildingObject_ScreenInfo_Parent.SetActive(true);

        requirementScreenList = InstantiateRequirementList(buildingObject_ScreenInfo_Parent);
        DisplayScreenRequirements();
    }
    void DisplayScreenRequirements()
    {
        BuildingBlockInfo buildingBlocksInfo = new BuildingBlockInfo();
        FurnitureInfo furnitureInfo = new FurnitureInfo();
        MachineInfo machineInfo = new MachineInfo();

        //Display the Header
        #region
        if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
        {
            if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingBlockObjectName_Active != BuildingBlockObjectNames.None)
            {
                buildingBlocksInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.buildingBlockObjectName_Active, BuildingSystemManager.Instance.activeBuildingObject_Info.buildingMaterial_Active);

                if (requirementScreenList.Count > 0)
                {
                    if (requirementScreenList[0].GetComponent<BuildingRequirementSlot>())
                    {
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = buildingBlocksInfo.objectInfo.objectSprite;
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = buildingBlocksInfo.objectName;
                    }
                }
            }
        }
        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
        {
            if (BuildingSystemManager.Instance.activeBuildingObject_Info.furnitureObjectName_Active != FurnitureObjectNames.None)
            {
                furnitureInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.furnitureObjectName_Active);

                if (requirementScreenList.Count > 0)
                {
                    if (requirementScreenList[0].GetComponent<BuildingRequirementSlot>())
                    {
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = furnitureInfo.objectInfo.objectSprite;
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = furnitureInfo.objectName;
                    }
                }
            }
        }
        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
        {
            if (BuildingSystemManager.Instance.activeBuildingObject_Info.machineObjectName_Active != MachineObjectNames.None)
            {
                machineInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.machineObjectName_Active);

                if (requirementScreenList.Count > 0)
                {
                    if (requirementScreenList[0].GetComponent<BuildingRequirementSlot>())
                    {
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = machineInfo.objectInfo.objectSprite;
                        requirementScreenList[0].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = machineInfo.objectName;
                    }
                }
            }
        }
        #endregion

        //Display the Requirements
        #region
        for (int i = 1; i < requirementScreenList.Count; i++)
        {
            if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
            {
                if (requirementScreenList[i].GetComponent<BuildingRequirementSlot>())
                {
                    BuildingSystemManager.Instance.enoughItemsToBuild = true;

                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].itemName).hotbarSprite;
                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].itemName.ToString();
                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + InventoryManager.Instance.GetAmountOfItemInInventory(0, buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].itemName) + "/" + buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].amount;

                    if (InventoryManager.Instance.GetAmountOfItemInInventory(0, buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].itemName) < buildingBlocksInfo.objectInfo.buildingRequirements[i - 1].amount)
                    {
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(true);
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = MainManager.Instance.mainColor_Orange;
                        BuildingSystemManager.Instance.enoughItemsToBuild = false;
                    }
                    else
                    {
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                    }
                }
            }
            else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
            {
                if (requirementScreenList[i].GetComponent<BuildingRequirementSlot>())
                {
                    BuildingSystemManager.Instance.enoughItemsToBuild = true;

                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(furnitureInfo.objectInfo.buildingRequirements[i - 1].itemName).hotbarSprite;
                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = furnitureInfo.objectInfo.buildingRequirements[i - 1].itemName.ToString();
                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + InventoryManager.Instance.GetAmountOfItemInInventory(0, furnitureInfo.objectInfo.buildingRequirements[i - 1].itemName) + "/" + furnitureInfo.objectInfo.buildingRequirements[i - 1].amount;

                    if (InventoryManager.Instance.GetAmountOfItemInInventory(0, furnitureInfo.objectInfo.buildingRequirements[i - 1].itemName) < furnitureInfo.objectInfo.buildingRequirements[i - 1].amount)
                    {
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(true);
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = MainManager.Instance.mainColor_Orange;
                        BuildingSystemManager.Instance.enoughItemsToBuild = false;
                    }
                    else
                    {
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                    }
                }
            }
            else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
            {
                if (requirementScreenList[i].GetComponent<BuildingRequirementSlot>())
                {
                    BuildingSystemManager.Instance.enoughItemsToBuild = true;

                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(machineInfo.objectInfo.buildingRequirements[i - 1].itemName).hotbarSprite;
                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = machineInfo.objectInfo.buildingRequirements[i - 1].itemName.ToString();
                    requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + InventoryManager.Instance.GetAmountOfItemInInventory(0, machineInfo.objectInfo.buildingRequirements[i - 1].itemName) + "/" + machineInfo.objectInfo.buildingRequirements[i - 1].amount;

                    if (InventoryManager.Instance.GetAmountOfItemInInventory(0, machineInfo.objectInfo.buildingRequirements[i - 1].itemName) < machineInfo.objectInfo.buildingRequirements[i - 1].amount)
                    {
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(true);
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = MainManager.Instance.mainColor_Orange;
                        BuildingSystemManager.Instance.enoughItemsToBuild = false;
                    }
                    else
                    {
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.gameObject.SetActive(false);
                        requirementScreenList[i].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = Color.white;
                    }
                }
            }
        }
        #endregion
    }
    
    public void ResetScreenDisplay()
    {
        for (int i = requirementScreenList.Count - 1; i >= 0; i--)
        {
            Destroy(requirementScreenList[i]);
        }

        requirementScreenList.Clear();

        buildingObject_ScreenInfo_Parent.SetActive(false);
    }
    #endregion


    //--------------------


    public void OpenTablet()
    {
        if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
        {
            UpdateSelectedDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.buildingBlockObjectName_Active, BuildingSystemManager.Instance.activeBuildingObject_Info.buildingMaterial_Active));
        }
        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
        {
            UpdateSelectedDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.furnitureObjectName_Active));
        }
        else if (BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
        {
            UpdateSelectedDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingSystemManager.Instance.activeBuildingObject_Info.machineObjectName_Active));
        }

        CheckActiveBuildingObjects();
    }

    void CheckActiveBuildingObjects()
    {
        print("222. CheckActiveBuildingObjects");

        for (int j = 0; j < buildingObjectList.Count; j++)
        {
            bool tempActive = false;

            for (int i = 0; i < buildingObjectList[j].buildingObjectChildList.Count; i++)
            {
                if (buildingObjectList[j].buildingObjectChildList[i].GetComponent<BuildingDisplaySlot>())
                {
                    if (buildingObjectList[j].buildingObjectChildList[i].GetComponent<BuildingDisplaySlot>().buildingObjectType == BuildingObjectTypes.BuildingBlock)
                    {
                        if (BuildingSystemManager.Instance.GetBuildingObjectInfo(buildingObjectList[j].buildingObjectChildList[i].GetComponent<BuildingDisplaySlot>().buildingBlockObjectName, buildingObjectList[j].buildingObjectChildList[i].GetComponent<BuildingDisplaySlot>().buildingMaterial).objectInfo.isActive)
                        {
                            buildingObjectList[j].buildingObjectChildList[i].transform.parent.gameObject.SetActive(true);
                            tempActive = true;
                        }
                        else
                        {
                            buildingObjectList[j].buildingObjectChildList[i].transform.parent.gameObject.SetActive(false);
                        }
                    }
                    else if (buildingObjectList[j].buildingObjectChildList[i].GetComponent<BuildingDisplaySlot>().buildingObjectType == BuildingObjectTypes.Furniture)
                    {
                        if (BuildingSystemManager.Instance.GetBuildingObjectInfo(buildingObjectList[j].buildingObjectChildList[i].GetComponent<BuildingDisplaySlot>().furnitureObjectName).objectInfo.isActive)
                        {
                            buildingObjectList[j].buildingObjectChildList[i].transform.parent.gameObject.SetActive(true);
                            tempActive = true;
                        }
                        else
                        {
                            buildingObjectList[j].buildingObjectChildList[i].transform.parent.gameObject.SetActive(false);
                        }
                    }
                    else if (buildingObjectList[j].buildingObjectChildList[i].GetComponent<BuildingDisplaySlot>().buildingObjectType == BuildingObjectTypes.Machine)
                    {
                        if (BuildingSystemManager.Instance.GetBuildingObjectInfo(buildingObjectList[j].buildingObjectChildList[i].GetComponent<BuildingDisplaySlot>().machineObjectName).objectInfo.isActive)
                        {
                            buildingObjectList[j].buildingObjectChildList[i].transform.parent.gameObject.SetActive(true);
                            tempActive = true;
                        }
                        else
                        {
                            buildingObjectList[j].buildingObjectChildList[i].transform.parent.gameObject.SetActive(false);
                        }
                    }
                }
            }

            if (tempActive)
            {
                BuildingObjectParentList[j].SetActive(true);
            }
            else
            {
                BuildingObjectParentList[j].SetActive(false);
            }
        }
    }
}

[Serializable]
public class DisplayBlock
{
    public List<GameObject> buildingObjectChildList = new List<GameObject>();
}
