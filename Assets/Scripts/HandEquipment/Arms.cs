using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : Singleton<Arms>
{
    public Animator anim;


    //--------------------


    void Start()
    {

        PlayerButtonManager.drink_isPressed += FillWater;

        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        //Left Click input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Click");
        }
    }


    //--------------------


    //Animation event
    public void InteractionFrame()
    {
        if (EquippmentManager.Instance.toolHolderParent.transform.childCount > 1)
        {
            if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1))
            {
                if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>())
                {
                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                }
            }
        }
    }


    //--------------------


    void FillWater()
    {
        if (EquippmentManager.Instance.toolHolderParent.transform.childCount > 1)
        {
            if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1))
            {
                if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>())
                {
                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().FillDrink();
                }
            }
        }
    }
}
