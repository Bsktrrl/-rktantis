using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundCollider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("10000. Entered");
        InventoryManager.Instance.HideInventoryItemInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("10000. Exited");
        InventoryManager.Instance.HideInventoryItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        print("10000. Moved");
        InventoryManager.Instance.HideInventoryItemInfo();
    }
}
