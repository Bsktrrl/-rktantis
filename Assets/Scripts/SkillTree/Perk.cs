using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Perk : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Perk Info")]
    public PerkInfo perkInfo;

    [Space(20)]

    [Header("Perk Image Objects")]
    public Image perk_Frame;
    public Image perk_BG;
    public Image perk_Icon;

    float lineWidth = 5;
    Vector2 graphScale = Vector2.one;


    //--------------------


    private void Start()
    {
        for (int i = 0; i < perkInfo.perkConnectionList.Count; i++)
        {
            MakeLine(gameObject.GetComponent<RectTransform>().localPosition, perkInfo.perkConnectionList[i].GetComponent<RectTransform>().localPosition, PrepareLine());
        }

        UpdatePerk();
    }


    //--------------------


    GameObject PrepareLine()
    {
        if (perkInfo.skillTreeType == SkillTreeType.Inventory)
        {
            return SkillTreeManager.Instance.skillTree_Inventory_Lines;
        }
        else if (perkInfo.skillTreeType == SkillTreeType.Player)
        {
            return SkillTreeManager.Instance.skillTree_Equipment_Lines;
        }
        else if (perkInfo.skillTreeType == SkillTreeType.Tools)
        {
            return SkillTreeManager.Instance.skillTree_GhostCapture_Lines;
        }
        else if (perkInfo.skillTreeType == SkillTreeType.Arídean)
        {
            return SkillTreeManager.Instance.skillTree_CrystalLight_Lines;
        }

        return null;
    }
    void MakeLine(Vector2 lineA, Vector2 lineB, GameObject SkillTreeLine_Parent)
    {
        GameObject NewObj = new GameObject();
        NewObj.name = "line from " + lineA.x + " to " + lineB.x;

        Image NewImage = NewObj.AddComponent<Image>();
        NewImage.sprite = SkillTreeManager.Instance.line;
        NewImage.color = SkillTreeManager.Instance.lineColor;

        RectTransform rect = NewObj.GetComponent<RectTransform>();
        rect.SetParent(SkillTreeLine_Parent.transform);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.localScale = Vector3.one;

        Vector3 a = new Vector3(lineA.x * graphScale.x, lineA.y * graphScale.y, 0);
        Vector3 b = new Vector3(lineB.x * graphScale.x, lineB.y * graphScale.y, 0);

        rect.localPosition = (a + b) / 2;

        Vector3 dif = a - b;
        rect.sizeDelta = new Vector3(dif.magnitude, lineWidth);
        rect.localRotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
    }


    //--------------------


    public void PerkButton_OnCLick()
    {
        if (perkInfo.perkState == PerkState.Active) { return; }

        if (perkInfo.perkInteractionState == PerkInteractionState.Pressed)
        {
            SkillTreeManager.Instance.A_PerkHasBeenPressed(null);
            perkInfo.perkInteractionState = PerkInteractionState.Passive;
        }
        else
        {
            SkillTreeManager.Instance.A_PerkHasBeenPressed(this);
            perkInfo.perkInteractionState = PerkInteractionState.Pressed;
        }

        UpdatePerk();
    }

    public void ActivatePerk()
    {
        //Change State
        if (perkInfo.perkState == PerkState.Ready)
        {
            perkInfo.perkInteractionState = PerkInteractionState.Passive;

            UpdatePerk();

            perkInfo.perkState = PerkState.Active;

            //Remove items from player inventory
            for (int i = 0; i < perkInfo.requirementList.Count; i++)
            {
                for (int j = 0; j < perkInfo.requirementList[i].amount; j++)
                {
                    InventoryManager.Instance.RemoveItemFromInventory(0, perkInfo.requirementList[i].itemName, -1, false);
                }
            }

            PerkManager.Instance.UpdatePerkValues(this);
            SkillTreeManager.Instance.UpdateActivePerkList(perkInfo.perkName);
        }

        UpdatePerk();
    }


    //--------------------


    public void UpdatePerk()
    {
        //Colors
        #region

        //Perk BG Color
        #region

        //Change Background Color
        switch (perkInfo.perkTier)
        {
            case PerkTier.Tier_0:
                perk_BG.color = SkillTreeManager.Instance.perkTier_0_Color;
                break;
            case PerkTier.Tier_1:
                perk_BG.color = SkillTreeManager.Instance.perkTier_1_Color;
                break;
            case PerkTier.Tier_2:
                perk_BG.color = SkillTreeManager.Instance.perkTier_2_Color;
                break;
            case PerkTier.Tier_3:
                perk_BG.color = SkillTreeManager.Instance.perkTier_3_Color;
                break;
            case PerkTier.Tier_4:
                perk_BG.color = Color.yellow;
                break;

            default:
                perk_BG.color = Color.white;
                break;
        }

        //Change Color if Active
        switch (perkInfo.perkState)
        {
            case PerkState.Invisible:
                break;
            case PerkState.Passive:
                perk_BG.color = new Color(perk_BG.color.r - 0.1f, perk_BG.color.g - 0.1f, perk_BG.color.b - 0.1f, 1);
                break;
            case PerkState.Ready:
                perk_BG.color = new Color(perk_BG.color.r, perk_BG.color.g, perk_BG.color.b, 1);
                break;
            case PerkState.Active:
                perk_BG.color = new Color(perk_BG.color.r - 0.6f, perk_BG.color.g - 0.6f, perk_BG.color.b - 0.6f, 1);
                break;

            default:
                break;
        }

        //Change darkness of color based on PerkInteractionState
        switch (perkInfo.perkInteractionState)
        {
            case PerkInteractionState.Passive:
                break;
            case PerkInteractionState.Hovered:
                if (perkInfo.perkState == PerkState.Active) { return; }
                perk_BG.color = new Color(perk_BG.color.r - 0.15f, perk_BG.color.g - 0.15f, perk_BG.color.b - 0.15f, 1);
                break;
            case PerkInteractionState.Pressed:
                if (perkInfo.perkState == PerkState.Active) { return; }
                perk_BG.color = new Color(perk_BG.color.r - 0.3f, perk_BG.color.g - 0.3f, perk_BG.color.b - 0.3f, 1);
                break;

            default:
                break;
        }

        #endregion

        //Perk Frame Color
        #region

        //Change Frame Color
        if (perkInfo.perkState != PerkState.Active)
        {
            perk_Frame.color = perk_BG.color;
        }

        //Change Color if Active
        switch (perkInfo.perkState)
        {
            case PerkState.Invisible:
                break;
            case PerkState.Passive:
                break;
            case PerkState.Ready:
                break;
            case PerkState.Active:
                perk_Frame.color = new Color(perk_BG.color.r - 0.6f, perk_BG.color.g - 0.6f, perk_BG.color.b - 0.6f, 1);
                break;

            default:
                break;
        }

        //Change darkness of Frame color based on PerkInteractionState
        switch (perkInfo.perkInteractionState)
        {
            case PerkInteractionState.Passive:
                break;
            case PerkInteractionState.Hovered:
                if (perkInfo.perkState == PerkState.Active) { return; }
                perk_Frame.color = new Color(perk_Frame.color.r - 0.15f, perk_Frame.color.g - 0.15f, perk_Frame.color.b - 0.15f, 1);
                break;
            case PerkInteractionState.Pressed:
                if (perkInfo.perkState == PerkState.Active) { return; }
                perk_Frame.color = new Color(perk_Frame.color.r - 0.3f, perk_Frame.color.g - 0.3f, perk_Frame.color.b - 0.3f, 1);
                break;

            default:
                break;
        }

        #endregion

        //Icon
        #region

        //Change Icon Color
        switch (perkInfo.perkState)
        {
            case PerkState.Invisible:
                break;
            case PerkState.Passive:
                perk_Icon.color = SkillTreeManager.Instance.perkIconColor_Passive;
                break;
            case PerkState.Ready:
                perk_Icon.color = SkillTreeManager.Instance.perkIconColor_Ready;
                break;
            case PerkState.Active:
                perk_Icon.color = SkillTreeManager.Instance.perkIconColor_Active;
                break;

            default:
                break;
        }

        perk_Icon.sprite = perkInfo.perkIcon;

        #endregion

        #endregion

        //State based on connections
        if (perkInfo.perkState == PerkState.Passive)
        {
            bool makeReady = true;

            for (int i = 0; i < perkInfo.perkConnectionList.Count; i++)
            {
                if (perkInfo.perkConnectionList[i].GetComponent<Perk>().perkInfo.perkState != PerkState.Active)
                {
                    makeReady = false;

                    break;
                }
            }

            if (makeReady)
            {
                perkInfo.perkState = PerkState.Ready;

                UpdatePerk();
            }
        }
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillTreeManager.Instance.SetupSkillTree_Information(gameObject);

        if (perkInfo.perkState == PerkState.Active) { return; }


        //-----


        SoundManager.Instance.Play_Inventory_ItemHover_Clip();

        if (perkInfo.perkInteractionState != PerkInteractionState.Pressed)
        {
            perkInfo.perkInteractionState = PerkInteractionState.Hovered;
        }

        UpdatePerk();
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        if (SkillTreeManager.Instance.pressedPerk)
        {
            SkillTreeManager.Instance.SetupSkillTree_Information(SkillTreeManager.Instance.pressedPerk.gameObject);
        }
        else
        {
            SkillTreeManager.Instance.ResetSkillTree_Information();
        }

        if (perkInfo.perkState == PerkState.Active) { return; }


        //-----


        if (perkInfo.perkInteractionState != PerkInteractionState.Pressed)
        {
            perkInfo.perkInteractionState = PerkInteractionState.Passive;
        }

        UpdatePerk();
    }
}
