using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCapturerTest : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Activate the capturing animation with left click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("Capturing", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.SetBool("Capturing", false);
        }

        ////Set the number of active slots with the number keys
        //if (Input.GetKey(KeyCode.LeftShift) == false)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha1))
        //    {
        //        anim.SetInteger("SlotsAmount", 1);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha2))
        //    {
        //        anim.SetInteger("SlotsAmount", 2);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha3))
        //    {
        //        anim.SetInteger("SlotsAmount", 3);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha4))
        //    {
        //        anim.SetInteger("SlotsAmount", 4);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha4))
        //    {
        //        anim.SetInteger("SlotsAmount", 4);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha5))
        //    {
        //        anim.SetInteger("SlotsAmount", 5);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha6))
        //    {
        //        anim.SetInteger("SlotsAmount", 6);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha7))
        //    {
        //        anim.SetInteger("SlotsAmount", 7);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha8))
        //    {
        //        anim.SetInteger("SlotsAmount", 8);
        //    }
        //}


        ////Set the slots to filled or unfilled with shift + number keys
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha1))
        //    {
        //        if (anim.GetBool("Slot1") == true)
        //        {
        //            anim.SetBool("Slot1", false);
        //        }
        //        else
        //        {
        //            anim.SetBool("Slot1", true);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha2))
        //    {
        //        if (anim.GetBool("Slot2") == true)
        //        {
        //            anim.SetBool("Slot2", false);
        //        }
        //        else
        //        {
        //            anim.SetBool("Slot2", true);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha3))
        //    {
        //        if (anim.GetBool("Slot3") == true)
        //        {
        //            anim.SetBool("Slot3", false);
        //        }
        //        else
        //        {
        //            anim.SetBool("Slot3", true);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha4))
        //    {
        //        if (anim.GetBool("Slot4") == true)
        //        {
        //            anim.SetBool("Slot4", false);
        //        }
        //        else
        //        {
        //            anim.SetBool("Slot4", true);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha5))
        //    {
        //        if (anim.GetBool("Slot5") == true)
        //        {
        //            anim.SetBool("Slot5", false);
        //        }
        //        else
        //        {
        //            anim.SetBool("Slot5", true);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha6))
        //    {
        //        if (anim.GetBool("Slot6") == true)
        //        {
        //            anim.SetBool("Slot6", false);
        //        }
        //        else
        //        {
        //            anim.SetBool("Slot6", true);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha7))
        //    {
        //        if (anim.GetBool("Slot7") == true)
        //        {
        //            anim.SetBool("Slot7", false);
        //        }
        //        else
        //        {
        //            anim.SetBool("Slot7", true);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha8))
        //    {
        //        if (anim.GetBool("Slot8") == true)
        //        {
        //            anim.SetBool("Slot8", false);
        //        }
        //        else
        //        {
        //            anim.SetBool("Slot8", true);
        //        }
        //    }
        //}
    }
}
