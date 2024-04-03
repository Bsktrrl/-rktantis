using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : Singleton<ResearchManager>
{
    List<int> hotbarMarkerInt = new List<int>();
    List<int> durabilityMarkerInt = new List<int>();

    [SerializeField] Color researchedColor;
    [SerializeField] Color notResearchedColor;
    [SerializeField] Color visibleColor;
    [SerializeField] Color unvisibleColor;


    //--------------------


    public void UpdateResearchMarkers()
    {
        hotbarMarkerInt.Clear();
        durabilityMarkerInt.Clear();

        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                Item tempItem = MainManager.Instance.GetItem(InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName);

                //Hide Hotbar Marker
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.activeInHierarchy)
                {
                    hotbarMarkerInt.Add(i);

                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.SetActive(false);
                }

                //Hide Durability Marker
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.activeInHierarchy)
                {
                    durabilityMarkerInt.Add(i);

                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterImage.gameObject.SetActive(false);
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.SetActive(false);
                }
            }
        }
    }
    public void UpdateResearchItemColor()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                Item tempItem = MainManager.Instance.GetItem(InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName);

                //Set Image Color
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().gameObject.GetComponent<Image>())
                {
                    if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName != Items.None)
                    {
                        if (tempItem.isResearched)
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(researchedColor);
                        }
                        else
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(notResearchedColor);
                        }
                    }
                }
            }
        }
    }
    
    public void ResetResearchMarkers()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            for (int j = 0; j < hotbarMarkerInt.Count; j++)
            {
                if (hotbarMarkerInt[j] == i)
                {
                    if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<Image>() && InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
                    {
                        //Show Hotbar Marker
                        if (!InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.activeInHierarchy)
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.SetActive(true);
                        }
                    }

                    break;
                }
            }

            for (int j = 0; j < durabilityMarkerInt.Count; j++)
            {
                if (durabilityMarkerInt[j] == i)
                {
                    if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<Image>() && InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
                    {
                        //Show Durability Marker
                        if (!InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.activeInHierarchy)
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterImage.gameObject.SetActive(true);
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.SetActive(true);
                        }
                    }

                    break;
                }
            }
        }
    }
    public void ResetResearchItemColor()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            //Set Image Color
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().gameObject.GetComponent<Image>())
            {
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName == Items.None)
                {
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(unvisibleColor /*new Color(255, 255, 255, 0)*/);
                }
                else
                {
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(visibleColor /*new Color(255, 255, 255, 255)*/);
                }
            }
        }
    }
}
