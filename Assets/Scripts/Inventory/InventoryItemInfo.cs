using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour, IPointerEnterHandler
{
    [Header("Static Item")]
    public GameObject staticItem_Parent;
    public TextMeshProUGUI staticItem_RightMouseButton_Text;

    [Header("Chest Item")]
    public GameObject chest_Parent;

    [Header("Equipment Item")]
    public GameObject inventoryItemInfo_Parent;
    public GameObject equipment_Parent;

    [Header("Research Item")]
    public GameObject research_Parent;

    [Header("Equip Hand Item")]
    public GameObject equipHandItem_Parent;
    public GameObject equipHandItem_NotHand;
    public GameObject equipHandItem_Hand;
    public TextMeshProUGUI equipHandItem_NotHand_RightMouseButton_Text;
    public TextMeshProUGUI equipHandItem_Hand_RightMouseButton_Text;

    [Header("Equip Clothes Item")]
    public GameObject equipClothesItem_Parent;
    public GameObject equipClothesItem_EquipMenuParent;
    public GameObject equipClothesItem_ChestParent;
    public TextMeshProUGUI equipClothesItem_EquipMenu_RightMouseButton_Text;
    public TextMeshProUGUI equipClothesItem_ChestMenu_RightMouseButton_Text;

    [Header("Consumable Item")]
    public GameObject ConsumableItem_Parent;
    public TextMeshProUGUI ConsumableItem_RightMouseButton_Text;


    [Header("Other")]
    [SerializeField] Vector2 posOffset;

    Vector2 pos;
    Vector2 movePos;


    //--------------------


    public void Start()
    {
        HideAllParents();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            MainManager.Instance.mainCanvas.transform as RectTransform, Input.mousePosition,
            MainManager.Instance.mainCanvas.worldCamera,
            out pos);

        MoveObject();
    }
    private void Update()
    {
        MoveObject();
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveObject();
    }


    //--------------------


    public void SetInfo_StaticItem()
    {
        HideAllParents();

        if (MainManager.Instance.menuStates == MenuStates.ChestMenu)
        {
            staticItem_RightMouseButton_Text.text = "Move";
        }
        else
        {
            staticItem_RightMouseButton_Text.text = "Throw";
        }

        staticItem_Parent.SetActive(true);
    }
    public void SetInfo_ConsumableItem()
    {
        HideAllParents();

        if (MainManager.Instance.menuStates == MenuStates.ChestMenu)
        {
            ConsumableItem_RightMouseButton_Text.text = "Move";
        }
        else
        {
            ConsumableItem_RightMouseButton_Text.text = "Throw";
        }

        ConsumableItem_Parent.SetActive(true);
    }
    public void SetInfo_ResearchableItem(bool isResearched)
    {
        HideAllParents();

        if (isResearched)
        {
            research_Parent.SetActive(false);
        }
        else
        {
            research_Parent.SetActive(true);
        }
    }
    public void SetInfo_EquipableHandItem(ItemSlot itemSlot)
    {
        HideAllParents();

        if (MainManager.Instance.menuStates == MenuStates.ChestMenu)
        {
            equipHandItem_NotHand_RightMouseButton_Text.text = "Move";
            equipHandItem_Hand_RightMouseButton_Text.text = "Move";
        }
        else
        {
            equipHandItem_NotHand_RightMouseButton_Text.text = "Throw";
            equipHandItem_Hand_RightMouseButton_Text.text = "Throw";
        }

        for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
        {
            if (HotbarManager.Instance.hotbarList[i].itemID == itemSlot.itemID)
            {
                equipHandItem_NotHand.SetActive(false);
                equipHandItem_Hand.SetActive(true);

                equipHandItem_Parent.SetActive(true);

                return;
            }
        }

        equipHandItem_NotHand.SetActive(true);
        equipHandItem_Hand.SetActive(false);

        equipHandItem_Parent.SetActive(true);
    }
    public void SetInfo_EquipableClothesItem()
    {
        HideAllParents();

        if (MainManager.Instance.menuStates == MenuStates.ChestMenu)
        {
            equipClothesItem_EquipMenuParent.SetActive(false);
            equipClothesItem_ChestParent.SetActive(true);
        }
        else
        {
            equipClothesItem_EquipMenuParent.SetActive(true);
            equipClothesItem_ChestParent.SetActive(false);
        }

        equipClothesItem_Parent.SetActive(true);
    }

    public void SetInfo_ChestItem()
    {
        HideAllParents();

        if (MainManager.Instance.menuStates == MenuStates.ChestMenu)
        {
            chest_Parent.SetActive(true);
        }
    }
    public void SetInfo_EquippedItem()
    {
        HideAllParents();

        inventoryItemInfo_Parent.SetActive(true);
        equipment_Parent.SetActive(true);
    }
    public void HideInfo_EquippedItem()
    {
        HideAllParents();

        inventoryItemInfo_Parent.SetActive(false);
        equipment_Parent.SetActive(false);
    }


    //--------------------


    void HideAllParents()
    {
        staticItem_Parent.SetActive(false);
        chest_Parent.SetActive(false);
        equipHandItem_Parent.SetActive(false);
        equipClothesItem_Parent.SetActive(false);
        ConsumableItem_Parent.SetActive(false);
        equipment_Parent.SetActive(false);
        research_Parent.SetActive(false);
    }


    //--------------------


    void MoveObject()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            MainManager.Instance.mainCanvas.transform as RectTransform,
            Input.mousePosition,
            MainManager.Instance.mainCanvas.worldCamera,
            out movePos);

        transform.position = MainManager.Instance.mainCanvas.transform.TransformPoint(movePos + posOffset);
    }
}
