using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
