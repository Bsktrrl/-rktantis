using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchItemSlot : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemName;


    //--------------------


    public void SetItemInfo(Sprite sprite, Items name)
    {
        itemImage.sprite = sprite;
        itemName.text = SpaceTextConverting.Instance.SetText(name.ToString());
    }
}
