using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSubPanel : MonoBehaviour
{
    public ItemSubCategories panelName;

    public void SetDisplay()
    {
        gameObject.name = panelName.ToString();
    }
}
