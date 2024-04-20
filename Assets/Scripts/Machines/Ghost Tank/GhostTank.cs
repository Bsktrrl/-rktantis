using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GhostTank : MonoBehaviour
{
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


    //--------------------


    private void Start()
    {
        Display_Parent.SetActive(false);
        ghostObject_Parent.SetActive(false);

        anim.SetBool("isActive", false);

        StartCoroutine(WaitForNextTankAnimation(UnityEngine.Random.Range(10, 120)));
    }
    private void Update()
    {
        UpdateTankDisplay();
        ReduceFuel(Time.deltaTime * 3);
    }


    //--------------------


    public void SetupGhostTank(GhostElement element, float fuelAmount)
    {
        ghostTankContent.interactableType = InteracteableType.GhostTank;

        ghostTankContent.machinePos = transform.position;
        ghostTankContent.machineRot = transform.rotation;

        ghostTankContent.GhostElement = element;
        ghostTankContent.elementalFuelAmount = fuelAmount;

        MachineManager.Instance.SaveData();
    }


    //--------------------


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
        else
        {
            RemoveGhost();
        }

        MachineManager.Instance.SaveData();
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
                ghostTankContent.elementalFuelAmount = 100;

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

        GhostManager.Instance.RemoveGhostFromCapturer();

        Display_Parent.SetActive(true);
        ghostObject_Parent.SetActive(true);

        anim.SetBool("isActive", true);

        MachineManager.Instance.SaveData();
    }
    void RemoveGhost()
    {
        print("333. Remove Ghost");

        SoundManager.Instance.Play_GhostTank_RemovedFromGhostTank_Clip();

        ghostObject_Parent.SetActive(false);
        Display_Parent.SetActive(false);

        ghostTankContent.GhostElement = GhostElement.None;
        ghostTankContent.elementalFuelAmount = 0;

        List<Material> materials = new List<Material>();

        materials.Add(skinnedMeshRenderer1.materials[0]);
        materials.Add(skinnedMeshRenderer1.materials[1]);
        materials.Add(skinnedMeshRenderer1.materials[2]);

        materials[2] = ghostTankBase;

        skinnedMeshRenderer1.SetMaterials(materials);
        skinnedMeshRenderer2.SetMaterials(materials);

        anim.SetBool("isActive", false);

        MachineManager.Instance.SaveData();
    }


    //--------------------


    void UpdateTankDisplay()
    {
        if (ghostObject_Parent.activeInHierarchy)
        {
            //Update percentage text
            fuelPercentage.text = ghostTankContent.elementalFuelAmount.ToString("F0") + "%";

            //Update percentageText color
            if (ghostTankContent.elementalFuelAmount >= 100)
            {
                fuelPercentage.color = blue;
            }
            else
            {
                fuelPercentage.color = orange;
            }

            //Update Image Fill
            if (ghostTankContent.elementalFuelAmount >= 100)
            {
                fuel_Image.fillAmount = 1f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 90)
            {
                fuel_Image.fillAmount = 0.9f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 80)
            {
                fuel_Image.fillAmount = 0.8f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 70)
            {
                fuel_Image.fillAmount = 0.7f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 60)
            {
                fuel_Image.fillAmount = 0.6f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 50)
            {
                fuel_Image.fillAmount = 0.5f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 40)
            {
                fuel_Image.fillAmount = 0.4f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 30)
            {
                fuel_Image.fillAmount = 0.3f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 20)
            {
                fuel_Image.fillAmount = 0.2f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 10)
            {
                fuel_Image.fillAmount = 0.1f;
            }
            else if (ghostTankContent.elementalFuelAmount >= 0)
            {
                fuel_Image.fillAmount = 0f;
            }

            //Change Ghost Visibility
            float check = ghostTankContent.elementalFuelAmount / 100;
            float tempValue = 1 - check;
            ghostObject_Parent.GetComponent<GhostInTank>().transparencyValue = tempValue;
        }
    }

    public void ReduceFuel(float value) //Temporary just draining //Change to drain based on other Machines (CropPlot)
    {
        if (ghostObject_Parent.activeInHierarchy)
        {
            ghostTankContent.elementalFuelAmount -= value;

            if (ghostTankContent.elementalFuelAmount > 100)
            {
                ghostTankContent.elementalFuelAmount = 100;
            }
            else if (ghostTankContent.elementalFuelAmount <= 0)
            {
                RemoveGhost();
            }

            GhostManager.Instance.SaveData();
        }
    }


    //--------------------


    IEnumerator WaitForNextTankAnimation(float time)
    {
        if (ghostObject_Parent.GetComponent<GhostInTank>())
        {
            ghostObject_Parent.GetComponent<GhostInTank>().SetGhostAnimation();
        }
        
        yield return new WaitForSeconds(time);

        //tankAnimationTimer = UnityEngine.Random.Range(10, 120);
        StartCoroutine(WaitForNextTankAnimation(UnityEngine.Random.Range(10, 120)));
    }
}

[Serializable]
public class GhostTankContent
{
    public InteracteableType interactableType;

    public Vector3 machinePos;
    public Quaternion machineRot;

    public GhostElement GhostElement;
    public float elementalFuelAmount;
}