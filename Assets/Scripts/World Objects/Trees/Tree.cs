using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject treeObject;

    [Header("Stats")]
    public float treeHealth;
    [SerializeField] float tempTreeHealth;

    [Header("Dormant")]
    public bool isCut;
    public float dormantTimer;
    public float dormantPercentage;

    public int treeIndex_x;
    public int treeIndex_y;
    public int percentageCheck = 0;


    //--------------------


    private void Start()
    {
        tempTreeHealth = treeHealth;
    }
    private void Update()
    {
        if (isCut)
        {
            dormantTimer += Time.deltaTime;

            dormantPercentage = dormantTimer / TreeManager.Instance.dormantTimer * 100;

            for (int i = 0; i < 100; i++)
            {
                if (dormantPercentage >= i && percentageCheck < i)
                {
                    percentageCheck = i;
                    TreeManager.Instance.ChangeTreeInfo(isCut, dormantTimer, treeIndex_x, treeIndex_y, percentageCheck, tempTreeHealth, gameObject.transform.position);

                    break;
                }
            }

            if (dormantTimer >= TreeManager.Instance.dormantTimer)
            {
                TreeRespawn();
            }
        }
    }


    //--------------------


    public void TreeInteraction(Items itemName)
    {
        if (isCut) { return; }

        InteracteableType interactableType = InteracteableType.None;

        if (gameObject.GetComponent<InteractableObject>())
        {
            interactableType = gameObject.GetComponent<InteractableObject>().interactableType;
        }
        else
        {
            return;
        }

        //If Object is a Tree
        if (interactableType == InteracteableType.Palm_Tree || interactableType == InteracteableType.Tree_2 || interactableType == InteracteableType.Tree_3
            || interactableType == InteracteableType.Tree_4 || interactableType == InteracteableType.Tree_5 || interactableType == InteracteableType.Tree_6
            || interactableType == InteracteableType.Tree_7 || interactableType == InteracteableType.Tree_8 || interactableType == InteracteableType.Tree_9
            || interactableType == InteracteableType.Cactus)
        {
            print("Interact with an Tree - " + itemName);

            //Play HackingSound
            float tempPitchCount = (float)(treeHealth / 5);

            if (itemName == Items.None || itemName == Items.Flashlight || itemName == Items.AríditeCrystal)
            {
                if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_AxeUsage_Hand_Clip(1.25f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_AxeUsage_Hand_Clip(1.2f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_AxeUsage_Hand_Clip(1.15f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_AxeUsage_Hand_Clip(1.1f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_AxeUsage_Hand_Clip(1.05f);
                }
                else
                {
                    SoundManager.Instance.Play_AxeUsage_Hand_Clip(1);
                }
            }
            else if (itemName == Items.WoodAxe)
            {
                if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_AxeUsage_WoodAxe_Clip(1.25f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_AxeUsage_WoodAxe_Clip(1.2f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_AxeUsage_WoodAxe_Clip(1.15f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_AxeUsage_WoodAxe_Clip(1.1f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_AxeUsage_WoodAxe_Clip(1.05f);
                }
                else
                {
                    SoundManager.Instance.Play_AxeUsage_WoodAxe_Clip(1);
                }
            }
            else if (itemName == Items.StoneAxe)
            {
                if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_AxeUsage_StoneAxe_Clip(1.25f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_AxeUsage_StoneAxe_Clip(1.2f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_AxeUsage_StoneAxe_Clip(1.15f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_AxeUsage_StoneAxe_Clip(1.1f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_AxeUsage_StoneAxe_Clip(1.05f);
                }
                else
                {
                    SoundManager.Instance.Play_AxeUsage_StoneAxe_Clip(1);
                }
            }
            else if (itemName == Items.CryoniteAxe)
            {
                if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 5)
                {
                    SoundManager.Instance.Play_AxeUsage_CryoniteAxe_Clip(1.25f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 4)
                {
                    SoundManager.Instance.Play_AxeUsage_CryoniteAxe_Clip(1.2f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 3)
                {
                    SoundManager.Instance.Play_AxeUsage_CryoniteAxe_Clip(1.15f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 2)
                {
                    SoundManager.Instance.Play_AxeUsage_CryoniteAxe_Clip(1.1f);
                }
                else if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) >= tempPitchCount * 1)
                {
                    SoundManager.Instance.Play_AxeUsage_CryoniteAxe_Clip(1.05f);
                }
                else
                {
                    SoundManager.Instance.Play_AxeUsage_CryoniteAxe_Clip(1);
                }
            }

            //Reduce the Tree's health
            if (itemName == Items.None || itemName == Items.Flashlight || itemName == Items.AríditeCrystal)
            {
                tempTreeHealth -= 0.5f;
            }
            else
            {
                tempTreeHealth -= MainManager.Instance.GetItem(itemName).treePower;
            }

            //Check if the TreeHealth is 0
            if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) <= 0)
            {
                //Play TreeDestroy sound
                SoundManager.Instance.Play_AxeUsage_TreeFalling_Clip();

                //Spawn at least 1 item into the World
                SpawnTreeItems(interactableType);

                //Spawn additional items into the World based on the Axe used
                bool isSpawningItems = true;
                float modifier = 0;
                while (isSpawningItems)
                {
                    float rand = Random.Range(0, 100);

                    if (itemName == Items.WoodAxe && rand <= (TreeManager.Instance.woodAaxe_Droprate - modifier))
                    {
                        SpawnTreeItems(interactableType);
                    }
                    else if (itemName == Items.StonePickaxe && rand <= (TreeManager.Instance.stoneAxe_Droprate - modifier))
                    {
                        SpawnTreeItems(interactableType);
                    }
                    else if (itemName == Items.CryonitePickaxe && rand <= (TreeManager.Instance.cryoniteAxe_Droprate - modifier))
                    {
                        SpawnTreeItems(interactableType);
                    }
                    else
                    {
                        isSpawningItems = false;
                    }

                    modifier += TreeManager.Instance.treeDropRateReducer;
                }

                //Hide Tree Object for a time
                treeObject.SetActive(false);

                isCut = true;
            }
        }

        TreeManager.Instance.ChangeTreeInfo(isCut, dormantTimer, treeIndex_x, treeIndex_y, percentageCheck, tempTreeHealth, gameObject.transform.position);
    }
    void SpawnTreeItems(InteracteableType interactableType)
    {
        //Spawn Wood
        if (interactableType == InteracteableType.Palm_Tree || interactableType == InteracteableType.Tree_2 || interactableType == InteracteableType.Tree_3
            || interactableType == InteracteableType.Tree_4 || interactableType == InteracteableType.Tree_5 || interactableType == InteracteableType.Tree_6
            || interactableType == InteracteableType.Tree_7 || interactableType == InteracteableType.Tree_8 || interactableType == InteracteableType.Tree_9)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Wood, gameObject);
        }

        //Spawn Cactus
        else if (interactableType == InteracteableType.Cactus)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Cactus, gameObject);
        }
    }


    //--------------------


    void TreeRespawn()
    {
        //Stop dormant process
        isCut = false;

        //Set Precentage to 100%
        dormantPercentage = 100;

        percentageCheck = 0;
        dormantTimer = 0;

        tempTreeHealth = treeHealth;

        TreeManager.Instance.ChangeTreeInfo(isCut, dormantTimer, treeIndex_x, treeIndex_y, percentageCheck, tempTreeHealth, gameObject.transform.position);

        //Show Mesh
        treeObject.SetActive(true);
    }

    public void LoadTree(bool _isCut, float _dormantTimer, int _treeIndex_j, int _treeIndex_l, int _precentageCheck, float _treeHealth)
    {
        //Set Parameters
        isCut = _isCut;
        treeIndex_x = _treeIndex_j;
        treeIndex_y = _treeIndex_l;
        percentageCheck = _precentageCheck;
        dormantTimer = _dormantTimer;
        tempTreeHealth = _treeHealth;

        //Check if Animation and pickablePart should be hidden
        if (isCut)
        {
            //Hide Mesh
            treeObject.SetActive(false);
        }
    }
}
