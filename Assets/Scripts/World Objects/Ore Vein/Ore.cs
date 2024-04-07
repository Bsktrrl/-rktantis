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

    [Header("Dormant")]
    public bool isHacked;
    public float dormantTimer;
    public float dormantPercentage;

    public int oreIndex_x;
    public int oreIndex_y;
    public int percentageCheck = 0;


    //--------------------


    private void Start()
    {
        tempOreHealth = oreHealth;
    }
    private void Update()
    {
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
        if (interactableType == InteracteableType.Tungsten_Ore || interactableType == InteracteableType.Stone_Ore
                 || interactableType == InteracteableType.Cryonite_Ore || interactableType == InteracteableType.Magnetite_Ore
                 || interactableType == InteracteableType.Viridian_Ore || interactableType == InteracteableType.Gold_Ore
                 || interactableType == InteracteableType.AríditeCrystal_Ore)
        {
            print("Interact with an Ore - " + itemName);

            //Play HackingSound
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

            //Reduce the Ore's health
            if (itemName == Items.None || itemName == Items.Flashlight || itemName == Items.AríditeCrystal)
            {
                tempOreHealth -= 0.5f;
            }
            else
            {
                tempOreHealth -= MainManager.Instance.GetItem(itemName).orePower;
            }

            //Update Cracks
            SetOreCracks();

            //Check if the OreHealth is 0
            if ((tempOreHealth - OreManager.Instance.oreHealthReducer) <= 0)
            {
                //Play OreDestroy sound
                SoundManager.Instance.Play_PickaxeUsage_OreIsDestroyd_Clip();

                //Spawn at least 1 item into the World
                SpawnOreItems(interactableType);

                //Spawn additional items into the World based on the pickaxe used
                bool isSpawningItems = true;
                float modifier = 0;
                while (isSpawningItems)
                {
                    float rand = Random.Range(0, 100);

                    if ((itemName == Items.None || itemName == Items.Flashlight || itemName == Items.AríditeCrystal) && rand <= (OreManager.Instance.woodPickaxe_Droprate - modifier))
                    {
                        SpawnOreItems(interactableType);
                    }
                    else if (itemName == Items.WoodPickaxe && rand <= (OreManager.Instance.woodPickaxe_Droprate - modifier))
                    {
                        SpawnOreItems(interactableType);
                    }
                    else if (itemName == Items.StonePickaxe && rand <= (OreManager.Instance.stonePickaxe_Droprate - modifier))
                    {
                        SpawnOreItems(interactableType);
                    }
                    else if (itemName == Items.CryonitePickaxe && rand <= (OreManager.Instance.cryonitePickaxe_Droprate - modifier))
                    {
                        SpawnOreItems(interactableType);
                    }
                    else
                    {
                        isSpawningItems = false;
                    }

                    modifier += OreManager.Instance.oreDropRateReducer;
                }

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
        renderer.sharedMaterial.SetFloat("_Cracks", 1 - (tempOreHealth / oreHealth));
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
