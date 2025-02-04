using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    [Header("General")]
    public PlantType plantType;
    public GameObject pickablePart;
    public float thisGrowthTime = 300;

    [Header("Flower Color")]
    [SerializeField] List<GameObject> ColorMeshObject = new List<GameObject>();
    [SerializeField] List<Material> plantColors = new List<Material>();

    [Header("Other")]
    public bool isPicked;
    [HideInInspector] public float growthTimer;
    public float growthPrecentage;

    [HideInInspector] public int plantIndex_x;
    [HideInInspector] public int plantIndex_y;

    float growthAnimationTime = 0.29f;

    Animator animator;

    [HideInInspector] public int percentageCheck = 0;

    [Header("CropPlot")]
    public bool isInCropPlot;
    public int CropPlotSlotIndex;
    public bool plantIsReadyInCropPlot;


    //--------------------


    void Awake()
    {
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
    }
    private void Start()
    {
        PlayerButtonManager.objectInterraction_isPressedDown += ObjectInteraction;

        //Set Color of Flower
        #region
        Material plantmaterial = GetRandomPlantColorMaterial();

        if (plantmaterial != null)
        {
            for (int i = 0; i < ColorMeshObject.Count; i++)
            {
                if (ColorMeshObject[i].GetComponent<MeshRenderer>())
                {
                    ColorMeshObject[i].GetComponent<MeshRenderer>().material = plantmaterial;
                }
            }
        }
        #endregion
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        #region Precentage Timer
        if (isPicked)
        {
            growthTimer += Time.deltaTime;

            growthPrecentage = growthTimer / (thisGrowthTime * PlantManager.Instance.growthGlobalTimerMultiplier) * 100;

            for (int i = 0; i < 100; i++)
            {
                if (growthPrecentage >= i && percentageCheck < i)
                {
                    percentageCheck = i;
                    PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex_x, plantIndex_y, percentageCheck, gameObject.transform.position);

                    break;
                }
            }

            if (growthTimer >= (thisGrowthTime * PlantManager.Instance.growthGlobalTimerMultiplier))
            {
                PlantGrown();
            }
        }
        #endregion
    }


    //--------------------


    public void LoadPlant(bool _isPicked, float _growthTimer, int _plantIndex_j, int _plantIndex_l, int _precentageCheck)
    {
        //Set Parameters
        isPicked = _isPicked;
        plantIndex_x = _plantIndex_j;
        plantIndex_y = _plantIndex_l;
        percentageCheck = _precentageCheck;
        growthTimer = _growthTimer;

        //Check if Animation and pickablePart should be hidden
        if (isPicked)
        {
            //Hide Mesh
            pickablePart.SetActive(false);

            //Set active animation
            if (GetComponent<Animator>())
            {
                animator.SetBool("Picked", true);
            }
        }
    }


    //--------------------


    public void PickPlant()
    {
        //Set picked animation
        if (GetComponent<Animator>())
        {
            animator.SetBool("Picked", true);
        }

        //Hide Mesh
        pickablePart.SetActive(false);

        //Start growing process
        growthTimer = 0;
        isPicked = true;

        PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex_x, plantIndex_y, percentageCheck, gameObject.transform.position);
    }

    void PlantGrown()
    {
        //Stop growing process
        isPicked = false;

        //Set Precentage to 100%
        growthPrecentage = 100;

        //Set active animation
        if (GetComponent<Animator>())
        {
            animator.SetBool("Picked", false);
        }

        //Show Mesh after a set amount of time 
        StartCoroutine(GrowBackCoroutine(growthAnimationTime));

        percentageCheck = 0;
        growthTimer = 0;

        PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex_x, plantIndex_y, percentageCheck, gameObject.transform.position);
    }
    IEnumerator GrowBackCoroutine(float time)
    {
        //Wait for time seconds
        yield return new WaitForSeconds(time);

        //Show Mesh
        pickablePart.SetActive(true);
    }


    void ObjectInteraction()
    {
        if (isPicked) { return; }

        if (SelectionManager.Instance.selectedObject == gameObject)
        {
            if (SelectionManager.Instance.onTarget && MainManager.Instance.menuStates == MenuStates.None)
            {
                //If Object is a Plant
                InteractableObject tempObject = pickablePart.GetComponent<InteractableObject>();

                //Pick the PlantItem from the plant
                for (int i = 0; i < tempObject.amount; i++)
                {
                    //Check If item can be added
                    InventoryManager.Instance.AddItemToInventory(0, tempObject.itemName);

                    #region PickUpSound
                    float slotSize = 0;
                    float pickupItemSize = 0;
                    float inventorySize = 0;

                    //Calculate if there is room for the pickedUpItem
                    for (int j = 0; j < InventoryManager.Instance.inventories[0].itemsInInventory.Count; j++)
                    {
                        slotSize += InventoryManager.Instance.inventories[0].itemsInInventory[j].itemSize.x * InventoryManager.Instance.inventories[0].itemsInInventory[j].itemSize.y;
                    }
                    pickupItemSize = MainManager.Instance.GetItem(tempObject.itemName).itemSize.x * MainManager.Instance.GetItem(tempObject.itemName).itemSize.y;
                    inventorySize = InventoryManager.Instance.inventories[0].inventorySize.x * InventoryManager.Instance.inventories[0].inventorySize.y;

                    //Set SOund based on predicted itemSpace
                    if (slotSize + pickupItemSize <= inventorySize)
                    {
                        SoundManager.Instance.Play_Inventory_PickupItem_Clip();
                        print("Is room: " + slotSize + " + " + pickupItemSize + " <= " + inventorySize);
                    }
                    else
                    {
                        SoundManager.Instance.Play_Inventory_InventoryIsFull_Clip();
                        print("Is Full: " + slotSize + " + " + pickupItemSize + " <= " + inventorySize);
                    }
                    #endregion


                    //SoundManager.Instance.Play_Inventory_PickupItem_Clip();

                    PickPlant();
                }
            }
        }
    }


    //--------------------


    public Material GetRandomPlantColorMaterial()
    {
        int index = 0;

        if (plantColors.Count > 0)
        {
            index = UnityEngine.Random.Range(0, plantColors.Count);
            return plantColors[index];
        }

        return null;
    }


    //--------------------


    public void DestroyThisPlantObject()
    {
        print("Destroy PlantObject: " + gameObject.name);

        //Unsubscribe from Event
        PlayerButtonManager.objectInterraction_isPressedDown -= ObjectInteraction;

        Destroy(gameObject);
    }
}