using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRequirementSlot : MonoBehaviour
{
    public Image requirement_image;
    public Image requirement_BGimage;
    public TextMeshProUGUI requirement_ItemName;
    public TextMeshProUGUI requirement_amount;

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
