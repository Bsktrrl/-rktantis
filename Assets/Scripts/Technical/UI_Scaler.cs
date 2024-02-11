using UnityEngine;

public class UI_Scaler : MonoBehaviour
{
    private void Start()
    {
        if (gameObject.GetComponent<RectTransform>())
        {
            gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }
}

