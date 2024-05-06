using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    Animator anim;
    Vector3 fallDirection;

    [Header("Prefabs")]
    public GameObject mesh;
    public GameObject treeObject_LOD0;
    public GameObject treeObject_LOD1;
    public GameObject treeObject_LOD2;

    [Header("Stats")]
    public float treeHealth;
    [SerializeField] float tempTreeHealth;
    public Vector2 dropRate;

    [Header("Dormant")]
    public bool isCut;
    public float dormantTimer;
    public float dormantPercentage;

    public int treeIndex_x;
    public int treeIndex_y;
    public int percentageCheck = 0;

    Items itemName = Items.None;
    InteracteableType interactableType = InteracteableType.None;
    bool isFalling;

    Vector3 meshStartPos;
    Quaternion meshStartRot;


    //--------------------


    private void Start()
    {
        anim = GetComponent<Animator>();
        tempTreeHealth = treeHealth;

        meshStartPos = mesh.transform.position;
        meshStartRot = mesh.transform.rotation;
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


    public void TreeInteraction(Items _itemName)
    {
        if (isCut || isFalling) { return; }

        itemName = _itemName;

        if (gameObject.GetComponent<InteractableObject>())
        {
            interactableType = gameObject.GetComponent<InteractableObject>().interactableType;
        }
        else
        {
            return;
        }

        //If Object is a Tree
        if ((interactableType == InteracteableType.Palm_Tree || interactableType == InteracteableType.BloodTree || interactableType == InteracteableType.BloodTreeBush
            || interactableType == InteracteableType.Tree_4 || interactableType == InteracteableType.Tree_5 || interactableType == InteracteableType.Tree_6
            || interactableType == InteracteableType.Tree_7 || interactableType == InteracteableType.Tree_8 || interactableType == InteracteableType.Tree_9
            || interactableType == InteracteableType.Cactus)
            && SelectionManager.Instance.onTarget)
        {
            //print("Interact with a Tree - " + _itemName);

            //Play hit animation when tree is hit
            anim.SetTrigger("GotHit");

            //Play HackingSound
            #region
            float tempPitchCount = (float)(treeHealth / 5);

            if (_itemName == Items.None || _itemName == Items.Flashlight || _itemName == Items.AríditeCrystal)
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
            else if (_itemName == Items.WoodAxe)
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
            else if (_itemName == Items.StoneAxe)
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
            else if (_itemName == Items.CryoniteAxe)
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
            #endregion

            //Reduce the Tree's health
            if (_itemName == Items.None || _itemName == Items.Flashlight || _itemName == Items.AríditeCrystal)
            {
                tempTreeHealth -= (0.5f + PerkManager.Instance.perkValues.treeDurability_Decrease);
            }
            else
            {
                tempTreeHealth -= (MainManager.Instance.GetItem(_itemName).treePower + PerkManager.Instance.perkValues.treeDurability_Decrease);
            }

            //Check if the TreeHealth is 0
            if ((tempTreeHealth - TreeManager.Instance.treeHealthReducer) <= 0)
            {
                isFalling = true;

                //Play TreeDestroy sound
                SoundManager.Instance.Play_AxeUsage_TreeFalling_Clip();

                //Get direction of camera
                fallDirection = new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z);
                fallDirection = fallDirection.normalized;
                fallDirection = transform.InverseTransformDirection(fallDirection);

                //Player fall animation when tree is falling
                anim.SetFloat("FallDirectionX", fallDirection.x);
                anim.SetFloat("FallDirectionY", fallDirection.z);
                anim.SetTrigger("Fall");
            }
        }

        TreeManager.Instance.ChangeTreeInfo(isCut, dormantTimer, treeIndex_x, treeIndex_y, percentageCheck, tempTreeHealth, gameObject.transform.position);
    }


    //--------------------


    //Animation event at end of fall animation
    void Fallen(AnimationEvent evt)
    {
        SoundManager.Instance.Play_AxeUsage_TreeHitGround_Clip();

        if (evt.animatorClipInfo.weight >= 0.5f)
        {
            StartCoroutine(FallingTreeCouroutine(0.25f));
        }
    }

    IEnumerator FallingTreeCouroutine(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnItemsAfterFalling(interactableType, itemName);

        isFalling = false;
    }
    void SpawnItemsAfterFalling(InteracteableType interactableType, Items itemName)
    {
        //Spawn Wood
        #region
        Vector2 spawnBuff = Vector2.zero;

        //Get buffs based on equippedItem
        if (HotbarManager.Instance.selectedItem == Items.WoodAxe)
            spawnBuff = new Vector2(0, 1);
        else if (HotbarManager.Instance.selectedItem == Items.StoneAxe)
            spawnBuff = new Vector2(1, 2);
        else if (HotbarManager.Instance.selectedItem == Items.CryoniteAxe)
            spawnBuff = new Vector2(2, 3);

        //Calculate oreDrop
        int spawnCount = (int)Random.Range(dropRate.x + spawnBuff.x + PerkManager.Instance.perkValues.resource_DropRate_Increase.x, dropRate.y + spawnBuff.y + PerkManager.Instance.perkValues.resource_DropRate_Increase.y);

        //Spawn Wood
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnTreeItemsToWorld(interactableType);
        }
        #endregion

        //Hide Tree Object for a time
        mesh.SetActive(false);
        if (treeObject_LOD0)
            treeObject_LOD0.SetActive(false);
        if (treeObject_LOD1)
            treeObject_LOD1.SetActive(false);
        if (treeObject_LOD2)
            treeObject_LOD2.SetActive(false);

        //Reset the tree's rotation 
        mesh.transform.SetLocalPositionAndRotation(Vector3.zero, meshStartRot);
        if (treeObject_LOD0)
            treeObject_LOD0.transform.SetLocalPositionAndRotation(treeObject_LOD0.transform.position, treeObject_LOD0.transform.rotation);
        if (treeObject_LOD1)
            treeObject_LOD1.transform.SetLocalPositionAndRotation(treeObject_LOD1.transform.position, treeObject_LOD1.transform.rotation);
        if (treeObject_LOD2)
            treeObject_LOD2.transform.SetLocalPositionAndRotation(treeObject_LOD2.transform.position, treeObject_LOD2.transform.rotation);

        isCut = true;
    }
    void SpawnTreeItemsToWorld(InteracteableType interactableType)
    {
        //Spawn Wood
        if (interactableType == InteracteableType.Palm_Tree || interactableType == InteracteableType.BloodTree || interactableType == InteracteableType.BloodTreeBush
            || interactableType == InteracteableType.Tree_4 || interactableType == InteracteableType.Tree_5 || interactableType == InteracteableType.Tree_6
            || interactableType == InteracteableType.Tree_7 || interactableType == InteracteableType.Tree_8 || interactableType == InteracteableType.Tree_9)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Wood, gameObject, false, null, 1f);
        }

        //Spawn Cactus
        else if (interactableType == InteracteableType.Cactus)
        {
            InventoryManager.Instance.SpawnItemToWorld(Items.Cactus, gameObject, false, null, 0.5f);
        }
    }


    //--------------------


    void TreeRespawn()
    {
        //Stop dormant process
        isCut = false;

        //Set Percentage to 100%
        dormantPercentage = 100;

        percentageCheck = 0;
        dormantTimer = 0;

        tempTreeHealth = treeHealth;

        TreeManager.Instance.ChangeTreeInfo(isCut, dormantTimer, treeIndex_x, treeIndex_y, percentageCheck, tempTreeHealth, gameObject.transform.position);

        //Rotate Mesh to Start Rotation
        anim.SetTrigger("Reset");

        //Show Mesh
        mesh.transform.SetLocalPositionAndRotation(Vector3.zero, meshStartRot);
        mesh.SetActive(true);

        if (treeObject_LOD0)
        {
            treeObject_LOD0.transform.SetLocalPositionAndRotation(Vector3.zero, treeObject_LOD0.transform.rotation);
            treeObject_LOD0.SetActive(true);
        }
        if (treeObject_LOD1)
        {
            treeObject_LOD1.transform.SetLocalPositionAndRotation(Vector3.zero, treeObject_LOD1.transform.rotation);
            treeObject_LOD1.SetActive(true);
        }
        if (treeObject_LOD2)
        {
            treeObject_LOD2.transform.SetLocalPositionAndRotation(Vector3.zero, treeObject_LOD2.transform.rotation);
            treeObject_LOD2.SetActive(true);
        }
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
            if (treeObject_LOD0)
                treeObject_LOD0.SetActive(false);
            if (treeObject_LOD1)
                treeObject_LOD1.SetActive(false);
            if (treeObject_LOD2)
                treeObject_LOD2.SetActive(false);
        }
    }
}
