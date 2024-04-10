using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCapturer : MonoBehaviour
{
    Animator anim;

    [Header("General")]
    public bool isUsed;

    [Header("Slots")]
    public List<GameObject> slotObjectList = new List<GameObject>();


    //--------------------


    void Start()
    {
        anim = GetComponent<Animator>();

        UpdateGhostCapturer();
    }


    //--------------------


    void UpdateGhostCapturer()
    {
        //Reset ActiveSlots
        for (int i = 0; i < slotObjectList.Count; i++)
        {
            slotObjectList[i].SetActive(false);
        }

        //Set Active Slots and fill with capturedInfo
        #region
        SetCapturerInfo(0, "Slot1");
        SetCapturerInfo(1, "Slot2");
        SetCapturerInfo(2, "Slot3");
        SetCapturerInfo(3, "Slot4");
        SetCapturerInfo(4, "Slot5");
        SetCapturerInfo(5, "Slot6");
        SetCapturerInfo(6, "Slot7");
        SetCapturerInfo(7, "Slot8");
        #endregion
    }
    void SetCapturerInfo(int index, string slotName)
    {
        if (GhostManager.Instance.ghostCapturerStats.activeGhostCapturerSlotList[index] == true)
        {
            //Set if filled
            if (GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[index].ghostElement != GhostElement.None)
            {
                anim.SetBool("slotName", true);
            }
            else
            {
                anim.SetBool("slotName", false);
            }

            //Set Slot active
            anim.SetInteger("SlotsAmount", index + 1);
            slotObjectList[index].SetActive(true);
        }
    }
}
