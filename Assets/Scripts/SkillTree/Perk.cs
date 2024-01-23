using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class Perk : MonoBehaviour
{
    public Image perk_BG_Image;
    public Image perkImage;
    public PerkInfo perkInfo;

    public float lineWidth = 5;
    Vector2 graphScale = Vector2.one;


    //--------------------


    private void Start()
    {
        perk_BG_Image.sprite = SkillTreeManager.Instance.BG_Passive;

        for (int i = 0; i < perkInfo.perkConnectionList.Count; i++)
        {
            MakeLine(gameObject.GetComponent<RectTransform>().localPosition, perkInfo.perkConnectionList[i].GetComponent<RectTransform>().localPosition);
        }
    }
    private void Update()
    {
        UpdateState();
    }


    //--------------------


    void MakeLine(Vector2 lineA, Vector2 lineB)
    {
        GameObject NewObj = new GameObject();
        NewObj.name = "line from " + lineA.x + " to " + lineB.x;

        Image NewImage = NewObj.AddComponent<Image>();
        NewImage.sprite = SkillTreeManager.Instance.line;
        NewImage.color = SkillTreeManager.Instance.lineColor;

        RectTransform rect = NewObj.GetComponent<RectTransform>();
        rect.SetParent(SkillTreeManager.Instance.SkillTreeLine_Parent.transform);
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
        print("Perk: " + perkInfo.perkName + " - is pressed");

        //Change State
        if (perkInfo.perkState == PerkState.Ready)
        {
            perkInfo.perkState = PerkState.Active;
            perk_BG_Image.sprite = SkillTreeManager.Instance.BG_Active;
        }
    }

    void SetSprite()
    {
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
    }
}
