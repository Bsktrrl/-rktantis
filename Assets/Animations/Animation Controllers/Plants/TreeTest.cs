using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTest : MonoBehaviour
{
    Animator anim;
    Vector3 fallDirection;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Get direction of camera
        fallDirection = new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z);
        fallDirection = fallDirection.normalized;
        fallDirection = transform.InverseTransformDirection(fallDirection);
        
        //Play hit animation on button press T
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("GotHit");
        }

        //Player fall animation on button press Y
        if (Input.GetKeyDown(KeyCode.Y))
        {
            anim.SetFloat("FallDirectionX", fallDirection.x);
            anim.SetFloat("FallDirectionY", fallDirection.z);
            anim.SetTrigger("Fall");
        }
    }

    //Animation event at end of fall animation
    void Fallen(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight >= 0.5f)
        {
            print("Tree has fallen");
        }
    }
}
