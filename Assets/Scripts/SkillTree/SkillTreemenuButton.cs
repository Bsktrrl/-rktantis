using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreemenuButton : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();
    }
}
