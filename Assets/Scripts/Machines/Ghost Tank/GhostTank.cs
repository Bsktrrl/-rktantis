using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GhostTank : MonoBehaviour
{
    #region Variables
    public Animator anim;
    public GameObject ghostObject_Parent;

    public GhostTankContent ghostTankContent;

    [Header("Display")]
    [SerializeField] GameObject Display_Parent;
    [SerializeField] Image fuel_Image;
    [SerializeField] TextMeshProUGUI fuelPercentage;
    [SerializeField] Color blue;
    [SerializeField] Color orange;


    [Header("Materials")]
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer1;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer2;

    [SerializeField] Material ghostTankBase;
    [SerializeField] Material ghostTankWater;

    bool setupGhostTank;
    #endregion


    //--------------------


    private void Start()
    {
        if (!setupGhostTank)
        {
            Display_Parent.SetActive(false);
            ghostObject_Parent.SetActive(false);

            anim.SetBool("isActive", false);
        }

        StartCoroutine(WaitForNextTankAnimation(UnityEngine.Random.Range(10, 60)));
    }
    private void Update()
    {
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        UpdateTankDisplay();

        int growingPlantsTemp = GetAmountOfGrowingPlants();

        ReduceFuel(0.075f * growingPlantsTemp * Time.deltaTime);

        if (growingPlantsTemp > 0 && ghostTankContent.currentFuelAmount > 0)
        {
            anim.SetBool("isActive", true);
        }
        else
        {
            anim.SetBool("isActive", false);
        }

        if (ghostTankContent.currentFuelAmount > 0 && ghostTankContent.GhostElement != GhostElement.None)
        {
            Display_Parent.SetActive(true);
            ghostObject_Parent.SetActive(true);
        }
        else
        {
            Display_Parent.SetActive(false);
            ghostObject_Parent.SetActive(false);
        }
    }


    //--------------------


    #region Setup Ghost Tank
    public void SetupGhostTank()
    {
        setupGhostTank = true;

        ghostTankContent.interactableType = InteracteableType.GhostTank;

        ghostTankContent.GhostElement = GhostElement.None;
        ghostTankContent.currentFuelAmount = 0;

        //Insert Ghost
        if (ghostTankContent.GhostElement != GhostElement.None)
        {
            InsertGhost();
        }

        //Save Object Setup
        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());

        if (ghostTankContent.currentFuelAmount > 0 && ghostTankContent.GhostElement != GhostElement.None)
        {
            Display_Parent.SetActive(true);
            ghostObject_Parent.SetActive(true);
        }

        anim.SetBool("isActive", true);

        setupGhostTank = false;
    }
    public void SetupGhostTank(GhostTankContent _ghostTankContent)
    {
        setupGhostTank = true;

        ghostTankContent.interactableType = InteracteableType.GhostTank;

        ghostTankContent.GhostElement = _ghostTankContent.GhostElement;
        ghostTankContent.currentFuelAmount = _ghostTankContent.currentFuelAmount;

        //Insert Ghost
        if (ghostTankContent.GhostElement != GhostElement.None)
        {
            InsertGhost();
        }

        //Save Object Setup
        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());

        if (ghostTankContent.currentFuelAmount > 0 && ghostTankContent.GhostElement != GhostElement.None)
        {
            Display_Parent.SetActive(true);
            ghostObject_Parent.SetActive(true);
        }

        anim.SetBool("isActive", true);

        setupGhostTank = false;
    }
    #endregion


    //--------------------


    #region Interact with Ghost Tank
    public void InteractWithGhostTank()
    {
        if (ghostTankContent.GhostElement == GhostElement.None)
        {
            if (HotbarManager.Instance.selectedItem == Items.GhostCapturer)
            {
                if (GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[0].isTaken)
                {
                    InsertGhost();
                }
            }
        }

        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());
    }
    void InsertGhost()
    {
        print("333. Insert Ghost");

        SoundManager.Instance.Play_GhostTank_AddedToGhostTank_Clip();

        for (int i = GhostManager.Instance.ghostCapturerStats.ghostCapturedStats.Count - 1; i >= 0; i--)
        {
            if (GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[i].isTaken)
            {
                ghostTankContent.GhostElement = GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[i].ghostElement;
                
                if (!setupGhostTank)
                {
                    ghostTankContent.currentFuelAmount = 100;
                }

                ghostObject_Parent.GetComponent<GhostInTank>().ghostStats.ghostState = GhostStates.Tank;
                ghostObject_Parent.GetComponent<GhostInTank>().ghostStats.isBeard = GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[i].isBeard;
                ghostObject_Parent.GetComponent<GhostInTank>().ghostStats.ghostAppearance = GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[i].ghostAppearance;
                
                ghostObject_Parent.GetComponent<GhostInTank>().transparencyValue = 0;

                break;
            }
        }

        ghostObject_Parent.GetComponent<GhostInTank>().SetGhostAppearance();
        ghostObject_Parent.GetComponent<GhostInTank>().UpdateRenderList();

        List<Material> materials = new List<Material>();

        materials.Add(skinnedMeshRenderer1.materials[0]);
        materials.Add(skinnedMeshRenderer1.materials[1]);
        materials.Add(skinnedMeshRenderer1.materials[2]);

        materials[2] = ghostTankWater;

        skinnedMeshRenderer1.SetMaterials(materials);
        skinnedMeshRenderer2.SetMaterials(materials);

        if (!setupGhostTank)
        {
            GhostManager.Instance.RemoveGhostFromCapturer();
        }
       
        Display_Parent.SetActive(true);
        ghostObject_Parent.SetActive(true);

        anim.SetBool("isActive", true);
    }
    public void RemoveGhost()
    {
        print("333. Remove Ghost");

        SoundManager.Instance.Play_GhostTank_RemovedFromGhostTank_Clip();

        ghostObject_Parent.SetActive(false);
        Display_Parent.SetActive(false);

        ghostTankContent.GhostElement = GhostElement.None;
        ghostTankContent.currentFuelAmount = 0;

        List<Material> materials = new List<Material>();

        materials.Add(skinnedMeshRenderer1.materials[0]);
        materials.Add(skinnedMeshRenderer1.materials[1]);
        materials.Add(skinnedMeshRenderer1.materials[2]);

        materials[2] = ghostTankBase;

        skinnedMeshRenderer1.SetMaterials(materials);
        skinnedMeshRenderer2.SetMaterials(materials);

        anim.SetBool("isActive", false);

        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());
    }
    #endregion


    //--------------------


    #region Display
    void UpdateTankDisplay()
    {
        if (ghostObject_Parent.activeInHierarchy)
        {
            //Update percentage text
            fuelPercentage.text = ghostTankContent.currentFuelAmount.ToString("F0") + "%";

            //Update percentageText color
            if (ghostTankContent.currentFuelAmount >= 100)
            {
                fuelPercentage.color = blue;
            }
            else
            {
                fuelPercentage.color = orange;
            }

            //Update Image Fill
            if (ghostTankContent.currentFuelAmount >= 100)
            {
                fuel_Image.fillAmount = 1f;
            }
            else if (ghostTankContent.currentFuelAmount >= 90)
            {
                fuel_Image.fillAmount = 0.9f;
            }
            else if (ghostTankContent.currentFuelAmount >= 80)
            {
                fuel_Image.fillAmount = 0.8f;
            }
            else if (ghostTankContent.currentFuelAmount >= 70)
            {
                fuel_Image.fillAmount = 0.7f;
            }
            else if (ghostTankContent.currentFuelAmount >= 60)
            {
                fuel_Image.fillAmount = 0.6f;
            }
            else if (ghostTankContent.currentFuelAmount >= 50)
            {
                fuel_Image.fillAmount = 0.5f;
            }
            else if (ghostTankContent.currentFuelAmount >= 40)
            {
                fuel_Image.fillAmount = 0.4f;
            }
            else if (ghostTankContent.currentFuelAmount >= 30)
            {
                fuel_Image.fillAmount = 0.3f;
            }
            else if (ghostTankContent.currentFuelAmount >= 20)
            {
                fuel_Image.fillAmount = 0.2f;
            }
            else if (ghostTankContent.currentFuelAmount >= 10)
            {
                fuel_Image.fillAmount = 0.1f;
            }
            else if (ghostTankContent.currentFuelAmount >= 0)
            {
                fuel_Image.fillAmount = 0f;
            }

            //Change Ghost Visibility
            float check = ghostTankContent.currentFuelAmount / 100;
            float tempValue = 1 - check;
            ghostObject_Parent.GetComponent<GhostInTank>().transparencyValue = tempValue;
        }
    }
    #endregion

    #region Fuel
    public void ReduceFuel(float value) //Temporary just draining //Change to drain based on other Machines (CropPlot)
    {
        if (ghostObject_Parent.activeInHierarchy)
        {
            ghostTankContent.currentFuelAmount -= value;

            if (ghostTankContent.currentFuelAmount > 100)
            {
                ghostTankContent.currentFuelAmount = 100;
            }
            else if (ghostTankContent.currentFuelAmount <= 0)
            {
                RemoveGhost();
            }

            BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());

            GhostManager.Instance.SaveData();
        }
    }
    #endregion


    //--------------------


    int GetAmountOfGrowingPlants()
    {
        int counter = 0;

        if (gameObject.GetComponent<MoveableObject>().connectionPointObject)
        {
            if (gameObject.GetComponent<MoveableObject>().connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith >= 0)
            {
                //Get Connected BuildingObject
                GameObject obj = BuildingSystemManager.Instance.worldBuildingObjectListSpawned[gameObject.GetComponent<MoveableObject>().connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith];

                //If getting a CropPlot
                if (obj.GetComponent<CropPlot>())
                {
                    for (int i = 0; i < obj.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList.Count; i++)
                    {
                        if (obj.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[i].cropState == CropState.Growing)
                        {
                            counter++;
                        }
                    }
                }
            }
        }
        
        return counter;
    }


    //--------------------


    IEnumerator WaitForNextTankAnimation(float time)
    {
        if (ghostObject_Parent.GetComponent<GhostInTank>())
        {
            ghostObject_Parent.GetComponent<GhostInTank>().SetGhostAnimation();
        }
        
        yield return new WaitForSeconds(time);

        int tankAnimationTimer = UnityEngine.Random.Range(10, 60);
        StartCoroutine(WaitForNextTankAnimation(tankAnimationTimer));
    }
}

[Serializable]
public class GhostTankContent
{
    public InteracteableType interactableType;

    //public Vector3 machinePos;
    //public Quaternion machineRot;

    public GhostElement GhostElement;
    public float currentFuelAmount;
}