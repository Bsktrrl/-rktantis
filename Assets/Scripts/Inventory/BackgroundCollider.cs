using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundCollider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.DisplayInventoryItemInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.DisplayInventoryItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        InventoryManager.Instance.DisplayInventoryItemInfo();
    }
}
