using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEquipmentManager : Singleton<MenuEquipmentManager>
{
    [Header("HeadEquipment")]
    [SerializeField] Image HeadEquipmentSlot_Image;
    public Items HeadItemSelected;

    [Header("HandEquipment")]
    [SerializeField] Image HandEquipmentSlot_Image;
    public Items HandItemSelected;

    [Header("FootEquipment")]
    [SerializeField] Image FootEquipmentSlot_Image;
    public Items FootItemSelected;

    [Header("Save/Load")]
    public List<Items> menuEquipedItemList = new List<Items>();


    //--------------------


    public void LoadData()
    {
        List<Items> tempLoadList = DataManager.Instance.menuEquipedItemList_StoreList;

        if (tempLoadList.Count >= 3)
        {
            HeadItemSelected = tempLoadList[0];
            HandItemSelected = tempLoadList[1];
            FootItemSelected = tempLoadList[2];
        }
        else
        {
            HeadItemSelected = Items.None;
            HandItemSelected = Items.None;
            FootItemSelected = Items.None;
        }

        //Set correct images
        LoadImages(HeadItemSelected);
        LoadImages(HandItemSelected);
        LoadImages(FootItemSelected);

        //Hide the images, if not active
        if (HeadItemSelected == Items.None)
            HeadEquipmentSlot_Image.gameObject.SetActive(false);
        if (HandItemSelected == Items.None)
            HandEquipmentSlot_Image.gameObject.SetActive(false);
        if (FootItemSelected == Items.None)
            FootEquipmentSlot_Image.gameObject.SetActive(false);
    }
    public void SaveData()
    {
        List<Items> tempSaveList = new List<Items>();

        tempSaveList.Add(HeadItemSelected);
        tempSaveList.Add(HandItemSelected);
        tempSaveList.Add(FootItemSelected);

        menuEquipedItemList = tempSaveList;

        DataManager.Instance.menuEquipedItemList_StoreList = menuEquipedItemList;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_MenuEquipments");
    }


    //--------------------


    void LoadImages(Items itemName)
    {
        switch (itemName)
        {
            case Items.None:
                if (itemName == Items.AutoFeeder || itemName == Items.Headlight || itemName == Items.Helmet)
                {
                    HeadEquipmentSlot_Image.gameObject.SetActive(false);
                    HeadEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                }

                if (itemName == Items.ConstructionGloves || itemName == Items.MiningGloves || itemName == Items.PowerGloves)
                {
                    HandEquipmentSlot_Image.gameObject.SetActive(false);
                    HandEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                }

                if (itemName == Items.LightShoes || itemName == Items.RunningShoes || itemName == Items.Slippers)
                {
                    FootEquipmentSlot_Image.gameObject.SetActive(false);
                    FootEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                }

                break;

            case Items.AutoFeeder:
                HeadEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                HeadEquipmentSlot_Image.gameObject.SetActive(true);
                break;
            case Items.Headlight:
                HeadEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                HeadEquipmentSlot_Image.gameObject.SetActive(true);
                break;
            case Items.Helmet:
                HeadEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                HeadEquipmentSlot_Image.gameObject.SetActive(true);
                break;

            case Items.MiningGloves:
                HandEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                HandEquipmentSlot_Image.gameObject.SetActive(true);
                break;
            case Items.PowerGloves:
                HandEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                HandEquipmentSlot_Image.gameObject.SetActive(true);
                break;
            case Items.ConstructionGloves:
                HandEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                HandEquipmentSlot_Image.gameObject.SetActive(true);
                break;

            case Items.RunningShoes:
                FootEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                FootEquipmentSlot_Image.gameObject.SetActive(true);
                break;
            case Items.LightShoes:
                FootEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                FootEquipmentSlot_Image.gameObject.SetActive(true);
                break;
            case Items.Slippers:
                FootEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                FootEquipmentSlot_Image.gameObject.SetActive(true);
                break;

            default:
                if (itemName == Items.AutoFeeder || itemName == Items.Headlight || itemName == Items.Helmet)
                {
                    HeadEquipmentSlot_Image.gameObject.SetActive(false);
                    HeadEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                }

                if (itemName == Items.ConstructionGloves || itemName == Items.MiningGloves || itemName == Items.PowerGloves)
                {
                    HandEquipmentSlot_Image.gameObject.SetActive(false);
                    HandEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                }

                if (itemName == Items.LightShoes || itemName == Items.RunningShoes || itemName == Items.Slippers)
                {
                    FootEquipmentSlot_Image.gameObject.SetActive(false);
                    FootEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                }
                break;
        }
    }


    //--------------------


    public void HeadEquipmentSlot_isClicked()
    {
        if (HeadItemSelected != Items.None)
        {
            ResetEquipmentSlot(0);

            SaveData();
        }
    }
    public void HandEquipmentSlot_isClicked()
    {
        if (HandItemSelected != Items.None)
        {
            ResetEquipmentSlot(1);

            SaveData();
        }
    }
    public void FootEquipmentSlot_isClicked()
    {
        if (FootItemSelected != Items.None)
        {
            ResetEquipmentSlot(2);

            SaveData();
        }
    }


    //--------------------


    public void AddItemToEquipmentSlot(Items itemName)
    {
        for (int i = 0; i < InventoryManager.Instance.inventories[0].itemsInInventory.Count; i++)
        {
            if (itemName == InventoryManager.Instance.inventories[0].itemsInInventory[i].itemName)
            {
                //Remove Item from Inventory
                InventoryManager.Instance.RemoveItemFromInventory(0, itemName, InventoryManager.Instance.inventories[0].itemsInInventory[i].itemID, true);

                //Update Equipment Display
                switch (itemName)
                {
                    case Items.None:
                        if (itemName == Items.AutoFeeder || itemName == Items.Headlight || itemName == Items.Helmet)
                        {
                            HeadEquipmentSlot_Image.gameObject.SetActive(false);
                            HeadEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                        }

                        if (itemName == Items.ConstructionGloves || itemName == Items.MiningGloves || itemName == Items.PowerGloves)
                        {
                            HandEquipmentSlot_Image.gameObject.SetActive(false);
                            HandEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                        }

                        if (itemName == Items.LightShoes || itemName == Items.RunningShoes || itemName == Items.Slippers)
                        {
                            FootEquipmentSlot_Image.gameObject.SetActive(false);
                            FootEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                        }

                        break;

                    case Items.AutoFeeder:
                        if (HeadItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, HeadItemSelected);
                        }
                        HeadItemSelected = itemName;
                        HeadEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        HeadEquipmentSlot_Image.gameObject.SetActive(true);
                        break;
                    case Items.Headlight:
                        if (HeadItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, HeadItemSelected);
                        }
                        HeadItemSelected = itemName;
                        HeadEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        HeadEquipmentSlot_Image.gameObject.SetActive(true);
                        break;
                    case Items.Helmet:
                        if (HeadItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, HeadItemSelected);
                        }
                        HeadItemSelected = itemName;
                        HeadEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        HeadEquipmentSlot_Image.gameObject.SetActive(true);
                        break;

                    case Items.MiningGloves:
                        if (HandItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, HandItemSelected);
                        }
                        HandItemSelected = itemName;
                        HandEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        HandEquipmentSlot_Image.gameObject.SetActive(true);
                        break;
                    case Items.PowerGloves:
                        if (HandItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, HandItemSelected);
                        }
                        HandItemSelected = itemName;
                        HandEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        HandEquipmentSlot_Image.gameObject.SetActive(true);
                        break;
                    case Items.ConstructionGloves:
                        if (HandItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, HandItemSelected);
                        }
                        HandItemSelected = itemName;
                        HandEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        HandEquipmentSlot_Image.gameObject.SetActive(true);
                        break;

                    case Items.RunningShoes:
                        if (FootItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, FootItemSelected);
                        }
                        FootItemSelected = itemName;
                        FootEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        FootEquipmentSlot_Image.gameObject.SetActive(true);
                        break;
                    case Items.LightShoes:
                        if (FootItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, FootItemSelected);
                        }
                        FootItemSelected = itemName;
                        FootEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        FootEquipmentSlot_Image.gameObject.SetActive(true);
                        break;
                    case Items.Slippers:
                        if (FootItemSelected != Items.None)
                        {
                            InventoryManager.Instance.AddItemToInventory(0, FootItemSelected);
                        }
                        FootItemSelected = itemName;
                        FootEquipmentSlot_Image.sprite = MainManager.Instance.GetItem(itemName).hotbarSprite;
                        FootEquipmentSlot_Image.gameObject.SetActive(true);
                        break;

                    default:
                        if (itemName == Items.AutoFeeder || itemName == Items.Headlight || itemName == Items.Helmet)
                        {
                            HeadEquipmentSlot_Image.gameObject.SetActive(false);
                            HeadEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                        }

                        if (itemName == Items.ConstructionGloves || itemName == Items.MiningGloves || itemName == Items.PowerGloves)
                        {
                            HandEquipmentSlot_Image.gameObject.SetActive(false);
                            HandEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                        }

                        if (itemName == Items.LightShoes || itemName == Items.RunningShoes || itemName == Items.Slippers)
                        {
                            FootEquipmentSlot_Image.gameObject.SetActive(false);
                            FootEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                        }
                        break;
                }

                SaveData();
                break;
            }
        }
    }
    void ResetEquipmentSlot(int index)
    {
        switch (index)
        {
            case 0:
                InventoryManager.Instance.AddItemToInventory(0, HeadItemSelected);
                HeadItemSelected = Items.None;
                HeadEquipmentSlot_Image.gameObject.SetActive(false);
                HeadEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                break;
            case 1:
                InventoryManager.Instance.AddItemToInventory(0, HandItemSelected);
                HandItemSelected = Items.None;
                HandEquipmentSlot_Image.gameObject.SetActive(false);
                HandEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                break;
            case 2:
                InventoryManager.Instance.AddItemToInventory(0, FootItemSelected);
                FootItemSelected = Items.None;
                FootEquipmentSlot_Image.gameObject.SetActive(false);
                FootEquipmentSlot_Image.GetComponent<Image>().sprite = null;
                break;

            default:
                break;
        }

        SaveData();
    }
}
