using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : Singleton<SkillTreeManager>
{
    [Header("General")]
    public SkillTreeType skillTreeMenu_Type;
    public Perk pressedPerk;
    public Perk activePerk;

    public bool canUpgrade;

    [SerializeField] List<GameObject> perkList = new List<GameObject>();
    public List<bool> perkActivationList = new List<bool>();

    [Header("LineParents")]
    public GameObject skillTree_Inventory_Lines;
    public GameObject skillTree_Equipment_Lines;
    public GameObject skillTree_GhostCapture_Lines;
    public GameObject skillTree_CrystalLight_Lines;

    public GameObject upgrade_Button;
    public TextMeshProUGUI upgrade_Button_Text;

    [Header("Line")]
    public Sprite line;
    public Color lineColor;

    [Header("Colors")]
    public Color perkTier_0_Color;
    public Color perkTier_1_Color;
    public Color perkTier_2_Color;
    public Color perkTier_3_Color;

    public Color perkIconColor_Passive;
    public Color perkIconColor_Ready;
    public Color perkIconColor_Active;

    public Sprite slot_Blue;
    public Sprite slot_Orange;

    [Header("information")]
    [SerializeField] TextMeshProUGUI header_Text;
    [SerializeField] TextMeshProUGUI perkName_Text;
    [SerializeField] TextMeshProUGUI perkDescription_Text;

    [SerializeField] GameObject requirement_Parent;
    [SerializeField] GameObject requirement_Prefab;
    [SerializeField] List<GameObject> perkRequirementList = new List<GameObject>();


    //--------------------


    private void Start()
    {
        TabletManager.Instance.skillTree_Inventory_Parent.SetActive(true);
        TabletManager.Instance.skillTree_Equipment_Parent.SetActive(false);
        TabletManager.Instance.skillTree_GhostCapture_Parent.SetActive(false);
        TabletManager.Instance.skillTree_CrystalLight_Parent.SetActive(false);

        TabletManager.Instance.skillTree_Inventory_Button.GetComponent<Image>().sprite = TabletManager.Instance.menuButton_Active;
        TabletManager.Instance.skillTree_Equipment_Button.GetComponent<Image>().sprite = TabletManager.Instance.menuButton_Passive;
        TabletManager.Instance.skillTree_GhostCapture_Button.GetComponent<Image>().sprite = TabletManager.Instance.menuButton_Passive;
        TabletManager.Instance.skillTree_CrystalLight_Button.GetComponent<Image>().sprite = TabletManager.Instance.menuButton_Passive;

        skillTreeMenu_Type = SkillTreeType.Inventory;

        //Set Default Information
        ResetSkillTree_Information();
    }

    private void Update()
    {
        //if (DataManager.Instance.hasLoaded)
        //{
        //    for (int i = 0; i < perktList.Count; i++)
        //    {
        //        PerkRequirements(perktList[i].GetComponent<Perk>());
        //    }
        //}
    }



    //--------------------


    public void LoadData()
    {
        perkActivationList = DataManager.Instance.perkActivationList_Store;

        //If NewGame
        if (perkActivationList.Count <= 0)
        {
            for (int i = 0; i < perkList.Count; i++)
            {
                perkActivationList.Add(false);
            }
        }

        SetupPerksFromLoad();
    }
    public void SaveData()
    {
        DataManager.Instance.perkActivationList_Store = perkActivationList;
    }


    //--------------------


    public void SetupPerksFromLoad()
    {
        for (int i = 0; i < perkActivationList.Count; i++)
        {
            if (perkActivationList[i])
            {
                perkList[i].GetComponent<Perk>().perkInfo.perkState = PerkState.Active;

                perkList[i].GetComponent<Perk>().UpdatePerk();
            }
        }
    }

    public void UpdateActivePerkList(Perk perk)
    {
        for (int i = 0; i < perkList.Count; i++)
        {
            if (perkList[i].GetComponent<Perk>() == perk)
            {
                perkActivationList[i] = true;

                break;
            }
        }

        SaveData();
    }


    //--------------------


    void PerkRequirements(Perk perk)
    {
        if (perk.perkInfo.perkState == PerkState.Active) { return; }

        if (/*perk.perkInfo.requirementList.Count <= 0 && */perk.perkInfo.perkConnectionList.Count <= 0)
        {
            perk.perkInfo.perkState = PerkState.Ready;

            return;
        }

        #region Requirements
        //int requirementCounter = 0;

        //for (int i = 0; i < perk.perkInfo.requirementList.Count; i++)
        //{
        //    //If player has the required amount of items in inventory
        //    if (InventoryManager.Instance.GetAmountOfItemInInventory(0, perk.perkInfo.requirementList[i].itemName) >= perk.perkInfo.requirementList[i].amount)
        //    {
        //        requirementCounter++;
        //    }
        //    else
        //    {
        //        requirementCounter = -100;

        //        //perk.perkInfo.perkState = PerkState.Passive;
        //        //return;
        //    }
        //}
        #endregion

        #region Connections
        int connectionsCounter = 0;

        for (int i = 0; i < perk.perkInfo.perkConnectionList.Count; i++)
        {
            if (perk.perkInfo.perkConnectionList[i].GetComponent<Perk>().perkInfo.perkState == PerkState.Active)
            {
                connectionsCounter++;
            }
            else
            {
                connectionsCounter = -100;

                //perk.perkInfo.perkState = PerkState.Passive;
                //return;
            }
        }
        #endregion

        //Make the Perk Ready to be purchased (both Requirements and Connections must be fullfilled
        //if (requirementCounter >= perk.perkInfo.requirementList.Count && connectionsCounter >= perk.perkInfo.perkConnectionList.Count)
        //{
        //    perk.perkInfo.perkState = PerkState.Ready;
        //}
        //else
        //{
        //    perk.perkInfo.perkState = PerkState.Passive;
        //}
    }
    
    void UpdateRequirementDisplay(Perk perk)
    {
        for (int i = 0; i < perkRequirementList.Count; i++)
        {
            ////If player has the required amount of items in inventory
            //if (InventoryManager.Instance.GetAmountOfItemInInventory(0, perk.perkInfo.requirementList[i].itemName) >= perk.perkInfo.requirementList[i].amount)
            //{
            //    perkRequirementList[i].GetComponent<PerkRequirementSlot>().requirement_BGimage.sprite = TabletManager.Instance.squareButton_Active;
            //}
            //else
            //{
            //    perkRequirementList[i].GetComponent<PerkRequirementSlot>().requirement_BGimage.sprite = TabletManager.Instance.squareButton_Passive;
            //}
        }
    }

    public void SetupSkillTree_Information(GameObject perk)
    {
        activePerk = perk.GetComponent<Perk>();

        canUpgrade = CheckIfPerkCanUpgrade(activePerk);

        if (canUpgrade) 
        {
            upgrade_Button.GetComponent<Button>().image.sprite = slot_Blue;
            upgrade_Button_Text.color = MainManager.Instance.mainColor_Blue;
        }
        else
        {
            upgrade_Button.GetComponent<Button>().image.sprite = slot_Orange;
            upgrade_Button_Text.color = MainManager.Instance.mainColor_Orange;
        }

        header_Text.text = SpaceTextConverting.Instance.SetText(skillTreeMenu_Type.ToString());
        perkName_Text.text = perk.GetComponent<Perk>().perkInfo.perkName;
        perkDescription_Text.text = perk.GetComponent<Perk>().perkInfo.perkDescription;

        //PerkRequirementList
        #region
        //Reset the perkRequirementList
        for (int i = 0; i < perkRequirementList.Count; i++)
        {
            perkRequirementList[i].GetComponent<PerkRequirementSlot>().DestroyThisObject();
        }
        perkRequirementList.Clear();

        //Build the perkRequirementList
        if (perk)
        {
            for (int i = 0; i < perk.GetComponent<Perk>().perkInfo.requirementList.Count; i++)
            {
                perkRequirementList.Add(Instantiate(requirement_Prefab, requirement_Parent.transform));

                perkRequirementList[perkRequirementList.Count - 1].GetComponent<PerkRequirementSlot>().SetRequirementSlot(MainManager.Instance.GetItem(perk.GetComponent<Perk>().perkInfo.requirementList[i].itemName).hotbarSprite, perk.GetComponent<Perk>().perkInfo.requirementList[i].itemName, perk.GetComponent<Perk>().perkInfo.requirementList[i].amount);
            }
        }
        #endregion

        UpdateRequirementDisplay(perk.gameObject.GetComponent<Perk>());
    }
    public void ResetSkillTree_Information()
    {
        header_Text.text = SpaceTextConverting.Instance.SetText(skillTreeMenu_Type.ToString());
        perkName_Text.text = "";
        perkDescription_Text.text = "";

        //Reset the perkRequirementList
        for (int i = 0; i < perkRequirementList.Count; i++)
        {
            perkRequirementList[i].GetComponent<PerkRequirementSlot>().DestroyThisObject();
        }
        perkRequirementList.Clear();

        upgrade_Button.GetComponent<Button>().image.sprite = slot_Orange;
        upgrade_Button_Text.color = MainManager.Instance.mainColor_Orange;
    }


    //--------------------


    public void Upgrade_Button_isPressed()
    {
        if (activePerk /*&& canUpgrade*/)
        {
            if (activePerk.GetComponent<Perk>().perkInfo.perkState == PerkState.Ready)
            {
                SoundManager.Instance.Play_SkillTree_CompletedPerk_Clip();

                activePerk.GetComponent<Perk>().ActivatePerk();

                for (int i = 0; i < perkList.Count; i++)
                {
                    if (perkList[i].GetComponent<Perk>())
                    {
                        perkList[i].GetComponent<Perk>().UpdatePerk();
                    }
                }
            }

            ResetSkillTree_Information();

            pressedPerk = null;
        }
    }
    public void A_PerkHasBeenPressed(Perk perk)
    {
        //Reset Perk Pressed State
        for (int i = 0; i < perkList.Count; i++)
        {
            if (perkList[i].GetComponent<Perk>())
            {
                perkList[i].GetComponent<Perk>().perkInfo.perkInteractionState = PerkInteractionState.Passive;

                perkList[i].GetComponent<Perk>().UpdatePerk();
            }
        }

        //Change Perk Info
        pressedPerk = perk;

        if (perk)
        {
            SetupSkillTree_Information(perk.gameObject);
        }
    }


    //--------------------


    bool CheckIfPerkCanUpgrade(Perk perk)
    {
        if (!perk) { return false; }

        bool canUpgradeTemp = true;

        for (int i = 0; i < perk.GetComponent<Perk>().perkInfo.requirementList.Count; i++)
        {
            if (perk.GetComponent<Perk>().perkInfo.requirementList[i].amount > InventoryManager.Instance.GetAmountOfItemInInventory(0, perk.GetComponent<Perk>().perkInfo.requirementList[i].itemName))
            {
                canUpgradeTemp = false;

                break;
            }
        }

        return canUpgradeTemp;
    }
}

[Serializable]
public class PerkInfo
{
    [Header("General")]
    public string perkName;
    public SkillTreeType skillTreeType;
    public PerkTier perkTier;
    [TextArea(5, 10)] public string perkDescription;

    [Header("Icon")]
    //public Sprite perkIcon;

    [Header("States")]
    public PerkState perkState;
    public PerkInteractionState perkInteractionState;

    [Header("Connections")]
    public List<GameObject> perkConnectionList = new List<GameObject>();

    [Header("Requirement")]
    public List<CraftingRequirements> requirementList = new List<CraftingRequirements>();

    [Header("What to Activate?")]
    public PerkValues perkValues = new PerkValues();
}

public enum PerkState
{
    Invisible,

    Passive,
    Ready,
    Active
}
public enum PerkInteractionState
{
    Passive,
    Hovered,
    Pressed
}

public enum SkillTreeType
{
    Inventory,
    Player,
    Tools,
    Arídean
}

public enum PerkTier
{
    Tier_0,
    Tier_1,
    Tier_2,
    Tier_3,
    Tier_4
}