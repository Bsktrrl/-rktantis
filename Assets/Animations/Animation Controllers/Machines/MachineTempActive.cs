using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTempActive : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isActive", true);
    }
}
