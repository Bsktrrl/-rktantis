using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] MeshRenderer renderer;

    [Header("Prefabs")]
    public GameObject oreVein;

    [Header("Stats")]
    public float oreHealth;
    [SerializeField] float tempOreHealth;
    public Vector2 dropRate;

    [Header("Dormant")]
    public bool isHacked;
    public float dormantTimer;
    public float dormantPercentage;

    public int oreIndex_x;
    public int oreIndex_y;
    public int percentageCheck = 0;

    [Header("Cracks")]
    public List<Renderer> rendererList = new List<Renderer>();
    public MaterialPropertyBlock propertyBlock;


    //--------------------


    private void Start()
    {
        propertyBlock = new MaterialPropertyBlock();

        for (int i = 0; i < rendererList.Count; i++)
        {
            rendererList[i].SetPropertyBlock(propertyBlock);
        }

        tempOreHealth = oreHealth;
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        if (isHacked)
        {
            dormantTimer += Time.deltaTime;

            dormantPercentage = dormantTimer / OreManager.Instance.dormantTimer * 100;

            for (int i = 0; i < 100; i++)
            {
                if (dormantPercentage >= i && percentageCheck < i)
                {
                    percentageCheck = i;
                    OreManager.Instance.ChangeOreInfo(isHacked, dormantTimer, oreIndex_x, oreIndex_y, percentageCheck, tempOreHealth, gameObject.transform.position);

                    break;
                }
            }

            if (dormantTimer >= OreManager.Instance.dormantTimer)
            {
                OreRespawn();
            }
        }
    }


    //--------------------


    public void OreInteraction(Items itemName)
    {
        if (isHacked) { return; }

        InteracteableType interactableType = InteracteableType.None;

        if (gameObject.GetComponent<InteractableObject>())
        {
            interactableType = gameObject.GetComponent<InteractableObject>().interactableType;
        }
        else
        {
            return;
        }

        //If Object is an Ore
        if ((interactableType == InteracteableType.Tungsten_Ore || interactableType == InteracteableType.Stone_Ore
                 || interactableType == InteracteableType.Cryonite_Ore || interactableType == InteracteableType.Magnetite_Ore
                 || interactableType == InteracteableType.Viridian_Ore || interactableType == InteracteableType.Gold_Ore
                 || interactableType == InteracteableType.AríditeCrystal_Ore)
                 && SelectionManager.Instance.onTarget)
        {
            //print("Interact with an Ore - " + itemName);

            //Play HackingSound
            #region
            float tempPitchCount = (float)(oreHealth / 5);

            if (itemName == Items.None || itemName == Items.Flashlight || itemName == Items.AríditeCrystal)
            {
                if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_PickaxeUsage_Hand_Clip(1.25f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_PickaxeUsage_Hand_Clip(1.2f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_PickaxeUsage_Hand_Clip(1.15f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_PickaxeUsage_Hand_Clip(1.1f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_PickaxeUsage_Hand_Clip(1.05f);
                }
                else
                {
                    SoundManager.Instance.Play_PickaxeUsage_Hand_Clip(1);
                }
            }
            else if (itemName == Items.WoodPickaxe)
            {
                if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(1.25f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(1.2f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(1.15f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(1.1f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(1.05f);
                }
                else
                {
                    SoundManager.Instance.Play_PickaxeUsage_WoodPickaxe_Clip(1);
                }
            }
            else if (itemName == Items.StonePickaxe)
            {
                if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(1.25f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(1.2f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(1.15f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(1.1f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(1.05f);
                }
                else
                {
                    SoundManager.Instance.Play_PickaxeUsage_StonePickaxe_Clip(1);
                }
            }
            else if (itemName == Items.CryonitePickaxe)
            {
                if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(1.25f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(1.2f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(1.15f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(1.1f);
                }
                else if ((tempOreHealth - OreManager.Instance.oreHealthReducer) >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(1.05f);
                }
                else
                {
                    SoundManager.Instance.Play_PickaxeUsage_CryonitePickaxe_Clip(1);
                }
            }
            #endregion

            //Reduce the Ore's health
            if (itemName == Items.None || itemName == Items.Flashlight || itemName == Items.AríditeCrystal)
            {
                tempOreHealth -= (0.5f + PerkManager.Instance.perkValues.oreVeinDurability_Decrease);
            }
            else
            {
                tempOreHealth -= (MainManager.Instance.GetItem(itemName).orePower + PerkManager.Instance.perkValues.oreVeinDurability_Decrease);
            }

            //Update Cracks
            SetOreCracks();

            //Check if the OreHealth is 0
            if ((tempOreHealth - OreManager.Instance.oreHealthReducer) <= 0)
            {
                //Play OreDestroy sound
                SoundManager.Instance.Play_PickaxeUsage_OreIsDestroyd_Clip();

                //Spawn Ores
                #region
                Vector2 spawnBuff = Vector2.zero;

                //Get buffs based on equippedItem
                if (HotbarManager.Instance.selectedItem == Items.WoodPickaxe)
                    spawnBuff = new Vector2(0, 1);
                else if (HotbarManager.Instance.selectedItem == Items.StonePickaxe)
                    spawnBuff = new Vector2(1, 2);
                else if (HotbarManager.Instance.selectedItem == Items.CryonitePickaxe)
                    spawnBuff = new Vector2(2, 3);

                //Calculate oreDrop
                int spawnCount = (int)Random.Range(dropRate.x + spawnBuff.x + PerkManager.Instance.perkValues.resource_DropRate_Increase.x, dropRate.y + spawnBuff.y + PerkManager.Instance.perkValues.resource_DropRate_Increase.y);

                //Spawn Ores
                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnOreItems(interactableType);
                }
                #endregion

                //Hide Ore Object for a time
                oreVein.SetActive(false);

                isHacked = true;
            }
        }

        OreManager.Instance.ChangeOreInfo(isHacked, dormantTimer, oreIndex_x, oreIndex_y, percentageCheck, tempOreHealth, gameObject.transform.position);
    }
    void SpawnOreItems(InteracteableType interactableType)
    {
        if (interactableType == InteracteableType.Tungsten_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Tungsten, gameObject, false, null, 0.2f);
        }
        else if (interactableType == InteracteableType.Stone_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Stone, gameObject, false, null, 0.2f);
        }
        else if (interactableType == InteracteableType.Cryonite_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Cryonite, gameObject, false, null, 0.2f);
        }
        else if (interactableType == InteracteableType.Magnetite_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Magnetite, gameObject, false, null, 0.2f);
        }
        else if (interactableType == InteracteableType.Viridian_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Viridian, gameObject, false, null, 0.2f);
        }
        else if (interactableType == InteracteableType.Gold_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Gold, gameObject, false, null, 0.2f);
        }
        else if (interactableType == InteracteableType.AríditeCrystal_Ore)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.AríditeCrystal, gameObject, false, null, 0.2f);
        }
    }


    //--------------------


    public void SetOreCracks()
    {
        for (int i = 0; i < rendererList.Count; i++)
        {
            if (propertyBlock != null)
            {
                propertyBlock.SetFloat("_Cracks", 1 - (tempOreHealth / oreHealth));

                rendererList[i].SetPropertyBlock(propertyBlock);
            }
        }
    }


    //--------------------


    void OreRespawn()
    {
        //Stop dormant process
        isHacked = false;

        //Set Precentage to 100%
        dormantPercentage = 100;

        percentageCheck = 0;
        dormantTimer = 0;

        tempOreHealth = oreHealth;

        OreManager.Instance.ChangeOreInfo(isHacked, dormantTimer, oreIndex_x, oreIndex_y, percentageCheck, tempOreHealth, gameObject.transform.position);

        SetOreCracks();

        //Show Mesh
        oreVein.SetActive(true);
    }

    public void LoadOre(bool _isHacked, float _dormantTimer, int _oreIndex_j, int _oreIndex_l, int _precentageCheck, float _oreHealth)
    {
        //Set Parameters
        isHacked = _isHacked;
        oreIndex_x = _oreIndex_j;
        oreIndex_y = _oreIndex_l;
        percentageCheck = _precentageCheck;
        dormantTimer = _dormantTimer;
        tempOreHealth = _oreHealth;

        //Check if Animation and pickablePart should be hidden
        if (isHacked)
        {
            //Hide Mesh
            oreVein.SetActive(false);
        }
    }
}
