using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsTest : Singleton<ArmsTest>
{
    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Set the item category with the number keys (0=Nothing, 1=Tools, 2=Flashlight, 3=Crystal, 4=Cup/Bottle, 5=Bucket)
        //if(Input.GetKey(KeyCode.LeftShift) == false && Input.GetKey(KeyCode.LeftAlt) == false)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha0))   //Nothing
        //    {
        //        anim.SetInteger("ItemCategory", 0);
        //        anim.SetTrigger("ItemUpdate");
        //    }

        //    if (Input.GetKeyDown(KeyCode.Alpha1))   //Tools
        //    {
        //        anim.SetInteger("ItemCategory", 1);
        //        anim.SetTrigger("ItemUpdate");
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha2))   //FlashLight
        //    {
        //        anim.SetInteger("ItemCategory", 2);
        //        anim.SetTrigger("ItemUpdate");
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha3))   //Crystal
        //    {
        //        anim.SetInteger("ItemCategory", 3);
        //        anim.SetTrigger("ItemUpdate");
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha4))   //Cup/Bottle
        //    {
        //        anim.SetInteger("ItemCategory", 4);
        //        anim.SetTrigger("ItemUpdate");
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha5))   //Bucket
        //    {
        //        anim.SetInteger("ItemCategory", 5);
        //        anim.SetTrigger("ItemUpdate");
        //    }
        //}

        //Tablet
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(anim.GetBool("Tablet") == false)
            {
                anim.SetBool("Tablet", true);
                print(anim.GetBool("Tablet"));
            }
            else if (anim.GetBool("Tablet") == true)
            {
                anim.SetBool("Tablet", false);
                print(anim.GetBool("Tablet"));
            }
        }

        //Set the tool category with shift + number keys (1=Axe, 2=Pickaxe, 3=Hammer, 4=Sword, 0=GhostCapturer)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetInteger("ToolCategory", 1);
                anim.SetTrigger("ItemUpdate");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetInteger("ToolCategory", 2);
                anim.SetTrigger("ItemUpdate");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                anim.SetInteger("ToolCategory", 3);
                anim.SetTrigger("ItemUpdate");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                anim.SetInteger("ToolCategory", 4);
                anim.SetTrigger("ItemUpdate");
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                anim.SetInteger("ToolCategory", 0);
                anim.SetTrigger("ItemUpdate");
            }
        }

        //Set the tool rank with alt + number keys (1=Wood, 2=Stone, 3=Cryonite)
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetInteger("ToolRank", 1);
                anim.SetTrigger("ItemUpdate");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetInteger("ToolRank", 2);
                anim.SetTrigger("ItemUpdate");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                anim.SetInteger("ToolRank", 3);
                anim.SetTrigger("ItemUpdate");
            }
        }

        //Click input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Click");
        }
    }

    //Animation event
    void InteractionFrame()
    {
        print("Interact");
    }
}
