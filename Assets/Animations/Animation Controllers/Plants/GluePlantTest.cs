using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluePlantTest : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (anim.GetBool("Picked") == true)
            {
                anim.SetBool("Picked", false);
            }
            else
            {
                anim.SetBool("Picked", true);
            }
        }
    }
}
