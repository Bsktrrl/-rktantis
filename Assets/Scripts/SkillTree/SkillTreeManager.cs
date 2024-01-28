using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : Singleton<SkillTreeManager>
{
    [Header("General")]
    public SkillTreeType skillTreeMenu_Type;
    public Perk activePerk;
    [SerializeField] List<GameObject> perktList = new List<GameObject>();

    [Header("LineParents")]
    public GameObject skillTree_Inventory_Lines;
    public GameObject skillTree_Equipment_Lines;
    public GameObject skillTree_GhostCapture_Lines;
    public GameObject skillTree_CrystalLight_Lines;

    [Header("Line")]
    public Sprite line;
    public Color lineColor;

    [Header("Sprites")]
    public Sprite BG_Passive;
    public Sprite BG_Ready;
    public Sprite BG_Active;

    [Header("information")]
    [SerializeField] TextMeshProUGUI header_Text;
    [SerializeField] TextMeshProUGUI perkName_Text;
    [SerializeField] TextMeshProUGUI perkDescription_Text;

    [SerializeField] GameObject requirement_Parent;
    [SerializeField] GameObject requirement_Prefab;
    [SerializeField] List<GameObject> perkRequirementList = new List<GameObject>();

    bool perkSetup;


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
        if (!perkSetup)
        {
            for (int i = 0; i < perktList.Count; i++)
            {
                PerkRequirements(perktList[i].GetComponent<Perk>());
            }
        }
    }


    //--------------------


    void PerkRequirements(Perk perk)
    {
        if (perk.perkInfo.perkState == PerkState.Active) { return; }

        if (perk.perkInfo.requirementList.Count <= 0 && perk.perkInfo.perkConnectionList.Count <= 0)
        {
            perk.perkInfo.perkState = PerkState.Ready;

            return;
        }

        #region Requirements
        int requirementCounter = 0;

        for (int i = 0; i < perk.perkInfo.requirementList.Count; i++)
        {
            //If player has the required amount of items in inventory
            if (InventoryManager.Instance.GetAmountOfItemInInventory(0, perk.perkInfo.requirementList[i].itemName) >= perk.perkInfo.requirementList[i].amount)
            {
                requirementCounter++;
            }
            else
            {
                requirementCounter = -100;

                //perk.perkInfo.perkState = PerkState.Passive;
                //return;
            }
        }
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
        if (requirementCounter >= perk.perkInfo.requirementList.Count && connectionsCounter >= perk.perkInfo.perkConnectionList.Count)
        {
            perk.perkInfo.perkState = PerkState.Ready;
        }
        else
        {
            perk.perkInfo.perkState = PerkState.Passive;
        }
    }
    
    void UpdateRequirementDisplay(Perk perk)
    {
        for (int i = 0; i < perkRequirementList.Count; i++)
        {
            //If player has the required amount of items in inventory
            if (InventoryManager.Instance.GetAmountOfItemInInventory(0, perk.perkInfo.requirementList[i].itemName) >= perk.perkInfo.requirementList[i].amount)
            {
                perkRequirementList[i].GetComponent<PerkRequirementSlot>().requirement_BGimage.sprite = TabletManager.Instance.squareButton_Active;
            }
            else
            {
                perkRequirementList[i].GetComponent<PerkRequirementSlot>().requirement_BGimage.sprite = TabletManager.Instance.squareButton_Passive;
            }
        }
    }

    public void ResetSkillTree_Information()
    {
        perkSetup = true;

        header_Text.text = skillTreeMenu_Type.ToString();
        perkName_Text.text = "";
        perkDescription_Text.text = "";

        //Reset the perkRequirementList
        for (int i = 0; i < perkRequirementList.Count; i++)
        {
            perkRequirementList[i].GetComponent<PerkRequirementSlot>().DestroyThisObject();
        }
        perkRequirementList.Clear();

        perkSetup = false;
    }
    public void SetupSkillTree_Information(GameObject perk)
    {
        perkSetup = true;

        activePerk = perk.GetComponent<Perk>();

        header_Text.text = skillTreeMenu_Type.ToString();
        perkName_Text.text = perk.GetComponent<Perk>().perkInfo.perkName;
        perkDescription_Text.text = perk.GetComponent<Perk>().perkInfo.perkDescription;

        //Reset the perkRequirementList
        for (int i = 0; i < perkRequirementList.Count; i++)
        {
            perkRequirementList[i].GetComponent<PerkRequirementSlot>().DestroyThisObject();
        }
        perkRequirementList.Clear();

        for (int i = 0; i < perk.GetComponent<Perk>().perkInfo.requirementList.Count; i++)
        {
            perkRequirementList.Add(Instantiate(requirement_Prefab, requirement_Parent.transform));

            perkRequirementList[perkRequirementList.Count - 1].GetComponent<PerkRequirementSlot>().SetRequirementSlot(MainManager.Instance.GetItem(perk.GetComponent<Perk>().perkInfo.requirementList[i].itemName).hotbarSprite, perk.GetComponent<Perk>().perkInfo.requirementList[i].itemName, perk.GetComponent<Perk>().perkInfo.requirementList[i].amount);
        }

        UpdateRequirementDisplay(perk.gameObject.GetComponent<Perk>());

        perkSetup = false;
    }
}

[Serializable]
public class PerkInfo
{
    public string perkName;
    [TextArea(5, 10)] public string perkDescription;
    public SkillTreeType skillTreeType;

    public Sprite perkIcon;
    public PerkState perkState;

    public List<ItemRequirement> requirementList = new List<ItemRequirement>();

    public List<GameObject> perkConnectionList = new List<GameObject>();
}

public enum PerkState
{
    Invisible,

    Passive,
    Ready,
    Active
}
public enum SkillTreeType
{
    Inventory,
    Equipment,
    GhostCapture,
    CrystalLight
}