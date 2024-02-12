using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundCollider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("10000. Entered");
        InventoryManager.Instance.DisplayInventoryItemInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("10000. Exited");
        InventoryManager.Instance.DisplayInventoryItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        print("10000. Moved");
        InventoryManager.Instance.DisplayInventoryItemInfo();
    }
}
