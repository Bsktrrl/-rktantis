using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class Perk : MonoBehaviour, IPointerEnterHandler
{
    public Image perk_BG_Image;
    public Image perkImage;
    public PerkInfo perkInfo;

    float lineWidth = 5;
    Vector2 graphScale = Vector2.one;


    //--------------------


    private void Start()
    {
        perk_BG_Image.sprite = SkillTreeManager.Instance.BG_Passive;

        for (int i = 0; i < perkInfo.perkConnectionList.Count; i++)
        {
            MakeLine(gameObject.GetComponent<RectTransform>().localPosition, perkInfo.perkConnectionList[i].GetComponent<RectTransform>().localPosition, PrepareLine());
        }
    }
    private void Update()
    {
        //UpdateState();

        SetSprite();
    }


    //--------------------


    GameObject PrepareLine()
    {
        if (perkInfo.skillTreeType == SkillTreeType.Inventory)
        {
            return SkillTreeManager.Instance.skillTree_Inventory_Lines;
        }
        else if (perkInfo.skillTreeType == SkillTreeType.Equipment)
        {
            return SkillTreeManager.Instance.skillTree_Equipment_Lines;
        }
        else if (perkInfo.skillTreeType == SkillTreeType.GhostCapture)
        {
            return SkillTreeManager.Instance.skillTree_GhostCapture_Lines;
        }
        else if (perkInfo.skillTreeType == SkillTreeType.CrystalLight)
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


    void UpdateState()
    {
        if (perkInfo.perkState == PerkState.Active) { return; }


        //-----


        if (perkInfo.perkConnectionList.Count > 0)
        {
            int count = 0;

            //Check if this perk can become active
            for (int i = 0; i < perkInfo.perkConnectionList.Count; i++)
            {
                if (perkInfo.perkConnectionList[i])
                {
                    if (perkInfo.perkConnectionList[i].GetComponent<Perk>().perkInfo.perkState == PerkState.Active)
                    {
                        count++;
                    }
                }
            }

            if (count >= perkInfo.perkConnectionList.Count)
            {
                perkInfo.perkState = PerkState.Ready;
            }
            else
            {
                perkInfo.perkState = PerkState.Passive;
            }
        }
        else
        {
            perkInfo.perkState = PerkState.Ready;
        }

        SetSprite();
    }

    public void PerkButton_OnCLick()
    {
        //Change State
        if (perkInfo.perkState == PerkState.Ready)
        {
            print("Pressed a Ready Perk");

            perkInfo.perkState = PerkState.Active;
            perk_BG_Image.sprite = SkillTreeManager.Instance.BG_Active;

            //Remove items from player inventory
            for (int i = 0; i < perkInfo.requirementList.Count; i++)
            {
                for (int j = 0; j < perkInfo.requirementList[i].amount; j++)
                {
                    InventoryManager.Instance.RemoveItemFromInventory(0, perkInfo.requirementList[i].itemName, -1, false);
                }
            }

            SkillTreeManager.Instance.SetupSkillTree_Information(gameObject);
        }
    }

    void SetSprite()
    {
        perk_BG_Image.gameObject.SetActive(true);

        //Change Sprite
        if (perkInfo.perkState == PerkState.Passive)
        {
            perk_BG_Image.sprite = SkillTreeManager.Instance.BG_Passive;
        }
        else if (perkInfo.perkState == PerkState.Ready)
        {
            perk_BG_Image.sprite = SkillTreeManager.Instance.BG_Ready;
        }
        else if (perkInfo.perkState == PerkState.Active)
        {
            perk_BG_Image.sprite = SkillTreeManager.Instance.BG_Active;
        }
        else if (perkInfo.perkState == PerkState.Invisible)
        {
            perk_BG_Image.gameObject.SetActive(false);
        }
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillTreeManager.Instance.SetupSkillTree_Information(gameObject);
    }
}
