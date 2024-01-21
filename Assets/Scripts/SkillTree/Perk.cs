using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class Perk : MonoBehaviour
{
    public Image perk_BG_Image;
    public Image perkImage;
    public PerkInfo perkInfo;

    //Temp
    //public LineRenderer line;
    //public GameObject pos1;
    //public GameObject pos2;
    //public GameObject pos3;

    //--------------------


    private void Start()
    {
        perk_BG_Image.sprite = SkillTreeManager.Instance.BG_Passive;
    }
    private void Update()
    {
        UpdateState();

        //line = GetComponent<LineRenderer>();
        //line.positionCount = 3;

        //line.SetPosition(0, pos1.GetComponent<RectTransform>().position);
        //line.SetPosition(1, pos2.GetComponent<RectTransform>().position);
        //line.SetPosition(2, pos3.GetComponent<RectTransform>().position);
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
