using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemImage;

    [SerializeField] Vector2 posOffset;

    Vector2 pos;
    Vector2 movePos;


    //--------------------


    public void Start()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            MainManager.Instance.mainCanvas.transform as RectTransform, Input.mousePosition,
            MainManager.Instance.mainCanvas.worldCamera,
            out pos);

        MoveObject();
    }
    private void Update()
    {
        MoveObject();
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveObject();
    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }


    //--------------------


    public void SetInfo_StaticItem()
    {

    }
    public void SetInfo_ConsumableItem()
    {

    }
    public void SetInfo_EquipableHandItem()
    {

    }
    public void SetInfo_EquipableClothesItem()
    {

    }


    //--------------------


    void MoveObject()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            MainManager.Instance.mainCanvas.transform as RectTransform,
            Input.mousePosition,
            MainManager.Instance.mainCanvas.worldCamera,
            out movePos);

        transform.position = MainManager.Instance.mainCanvas.transform.TransformPoint(movePos + posOffset);
    }
}
