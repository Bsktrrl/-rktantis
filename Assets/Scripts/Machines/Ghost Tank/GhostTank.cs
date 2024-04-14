using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GhostTank : MonoBehaviour
{
    public Animator anim;
    public GameObject ghostObject_Parent;

    public GhostTankContent ghostTankContent;

    float tankAnimationTimer = 1;

    [Header("Materials")]
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer1;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer2;

    [SerializeField] Material ghostTankBase;
    [SerializeField] Material ghostTankWater;


    //--------------------


    private void Start()
    {
        ghostObject_Parent.SetActive(false);

        StartCoroutine(WaitForNextTankAnimation(tankAnimationTimer));
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
        print("InsertGhost");

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

        List<Material> materials = new List<Material>();

        materials.Add(skinnedMeshRenderer1.materials[0]);
        materials.Add(skinnedMeshRenderer1.materials[1]);
        materials.Add(skinnedMeshRenderer1.materials[2]);

        materials[2] = ghostTankWater;

        skinnedMeshRenderer1.SetMaterials(materials);
        skinnedMeshRenderer2.SetMaterials(materials);

        GhostManager.Instance.RemoveGhostFromCapturer();

        ghostObject_Parent.SetActive(true);

        anim.SetBool("isActive", true);
    }
    void RemoveGhost()
    {
        SoundManager.Instance.Play_GhostTank_RemovedFromGhostTank_Clip();

        ghostObject_Parent.SetActive(false);

        ghostTankContent.GhostElement = GhostElement.None;
        ghostTankContent.elementalFuelAmount = 0;

        List<Material> materials = new List<Material>();

        materials.Add(skinnedMeshRenderer1.materials[0]);
        materials.Add(skinnedMeshRenderer1.materials[1]);
        materials.Add(skinnedMeshRenderer1.materials[2]);

        materials[2] = ghostTankBase;

        skinnedMeshRenderer1.SetMaterials(materials);
        skinnedMeshRenderer2.SetMaterials(materials);

        //skinnedMeshRenderer1.materials[2] = ghostTankBase;
        //skinnedMeshRenderer2.materials[2] = ghostTankBase;

        anim.SetBool("isActive", true);
    }

    IEnumerator WaitForNextTankAnimation(float time)
    {
        if (ghostObject_Parent.GetComponent<GhostInTank>())
        {
            ghostObject_Parent.GetComponent<GhostInTank>().SetGhostAnimation();
        }
        
        yield return new WaitForSeconds(time);

        print("11111. WaitForNextTankAnimation: " + gameObject.name);

        tankAnimationTimer = UnityEngine.Random.Range(15, 40);
        StartCoroutine(WaitForNextTankAnimation(tankAnimationTimer));
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