using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : Singleton<SkillTreeManager>
{
    [Header("Parents")]
    [SerializeField] GameObject SkillTree_Parent;
    [SerializeField] GameObject SkillTree_Inventory_Parent;
    [SerializeField] GameObject SkillTree_Clothes_Parent;
    [SerializeField] GameObject SkillTree_GhostCapture_Parent;
    [SerializeField] GameObject SkillTree_Light_Parent;

    [Header("Sprites")]
    public Sprite BG_Passive;
    public Sprite BG_Ready;
    public Sprite BG_Active;

    [Header("Menus")]
    public SkillTreeMenus currentMenu;

    [Header("Lists")]
    public List<GameObject> perkList;


    //--------------------


    private void Start()
    {
        //SkillTree_Parent.SetActive(false);

        SetActiveMenu();
    }


    //--------------------


    public void InventoryMenuButton()
    {
        MenuTransitions(SkillTree_Inventory_Parent);
    }
    public void ClothesMenuButton()
    {
        MenuTransitions(SkillTree_Clothes_Parent);
    }
    public void GhostCaptureMenuButton()
    {
        MenuTransitions(SkillTree_GhostCapture_Parent);
    }
    public void LightMenuButton()
    {
        MenuTransitions(SkillTree_Light_Parent);
    }

    void MenuTransitions(GameObject newMenu)
    {
        SkillTree_Inventory_Parent.SetActive(false);
        SkillTree_Clothes_Parent.SetActive(false);
        SkillTree_GhostCapture_Parent.SetActive(false);
        SkillTree_Light_Parent.SetActive(false);

        SoundManager.Instance.PlaySelectPanel_Clip();

        //Get current selected Menu
        GameObject tempMenu = new GameObject();
        switch (currentMenu)
        {
            case SkillTreeMenus.Inventory:
                tempMenu = SkillTree_Inventory_Parent;
                currentMenu = SkillTreeMenus.Inventory;
                break;
            case SkillTreeMenus.Clothes:
                tempMenu = SkillTree_Clothes_Parent;
                currentMenu = SkillTreeMenus.Clothes;
                break;
            case SkillTreeMenus.GhostCapture:
                tempMenu = SkillTree_GhostCapture_Parent;
                currentMenu = SkillTreeMenus.GhostCapture;
                break;
            case SkillTreeMenus.Light:
                tempMenu = SkillTree_Light_Parent;
                currentMenu = SkillTreeMenus.Light;
                break;

            default:
                tempMenu = SkillTree_Inventory_Parent;
                currentMenu = SkillTreeMenus.Inventory;
                break;
        }

        tempMenu.SetActive(false);
        newMenu.SetActive(true);
    }
    void SetActiveMenu()
    {
        SkillTree_Inventory_Parent.SetActive(false);
        SkillTree_Clothes_Parent.SetActive(false);
        SkillTree_GhostCapture_Parent.SetActive(false);
        SkillTree_Light_Parent.SetActive(false);

        GameObject tempMenu = new GameObject();
        switch (currentMenu)
        {
            case SkillTreeMenus.Inventory:
                tempMenu = SkillTree_Inventory_Parent;
                break;
            case SkillTreeMenus.Clothes:
                tempMenu = SkillTree_Clothes_Parent;
                break;
            case SkillTreeMenus.GhostCapture:
                tempMenu = SkillTree_GhostCapture_Parent;
                break;
            case SkillTreeMenus.Light:
                tempMenu = SkillTree_Light_Parent;
                break;

            default:
                tempMenu = SkillTree_Inventory_Parent;
                break;
        }

        tempMenu.SetActive(true);
    }
}

public enum SkillTreeMenus
{
    Inventory,
    Clothes,
    GhostCapture,
    Light
}

[Serializable]
public class PerkInfo
{
    public string perkName;
    [TextArea(5, 10)] public string perkDescription;
    
    public Sprite perkIcon;
    public PerkState perkState;

    public List<BuildingBlockRequirement> requirementList = new List<BuildingBlockRequirement>();

    public List<GameObject> perkConnectionList = new List<GameObject>();
}

public enum PerkState
{
    Invisible,

    Passive,
    Ready,
    Active
}