using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    Animator anim;


    //--------------------


    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Vector3.Dot(Camera.main.transform.forward, transform.forward) >= 0)
                {
                    anim.SetBool("Direction", true);
                }
                else
                {
                    anim.SetBool("Direction", false);
                }
                anim.SetTrigger("Interact");
            }
        }
    }
}
