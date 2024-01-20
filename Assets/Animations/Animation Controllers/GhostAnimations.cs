using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimations : MonoBehaviour
{
    Animator anim;
    ParticleSystem sneeze;

    void Start()
    {
        anim = GetComponent<Animator>();
        sneeze = transform.Find("Armature_Ghost/Spine1_Jnt/Spine2_Jnt/Spine3_Jnt/Neck_Jnt/Head_Jnt").GetComponent<ParticleSystem>();
    }

    
    void Update()
    {
        if (anim.GetInteger("IdleAnimation") != 0)
        {
            anim.SetInteger("IdleAnimation", 0);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetInteger("IdleAnimation", Random.Range(0, 8));
        }
    }

    public void Sneeze()
    {
        sneeze.Play();
    }
}
