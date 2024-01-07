using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] GameObject currentScreen;
    [SerializeField] GameObject newScreen;

    public void MoveToNewScreen()
    {
        currentScreen.SetActive(false);
        newScreen.SetActive(true);
    }
}
