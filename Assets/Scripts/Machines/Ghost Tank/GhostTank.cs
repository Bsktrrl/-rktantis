using System;
using UnityEngine;

public class GhostTank : MonoBehaviour
{
    public GameObject ghostObject_Parent;

    public GhostTankContent ghostTankContent;


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
        if (HotbarManager.Instance.selectedItem == Items.GhostCapturer)
        {
            if (GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[0].isTaken)
            {
                InsertGhost();
            }
        }
    }
    void InsertGhost()
    {
        print("InsertGhost");

        GhostStats tempStats = GhostManager.Instance.RemoveGhostFromCapturer();

        ghostTankContent.GhostElement = tempStats.ghostElement;
        ghostTankContent.elementalFuelAmount = 100;

        ghostObject_Parent.GetComponent<Ghost>().ghostStats.isBeard = tempStats.isBeard;
        ghostObject_Parent.GetComponent<Ghost>().ghostStats.ghostAppearance = tempStats.ghostAppearance;

        UpdateGhostTank();

        ghostObject_Parent.SetActive(true);
    }
    void RemoveGhost()
    {
        ghostObject_Parent.SetActive(false);

        ghostTankContent.GhostElement = GhostElement.None;
        ghostTankContent.elementalFuelAmount = 0;
    }


    //--------------------


    void UpdateGhostTank()
    {
        ghostObject_Parent.GetComponent<Ghost>().SetGhostAppearance();
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