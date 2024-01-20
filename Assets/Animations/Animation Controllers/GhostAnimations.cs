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
        StartCoroutine(IdleAnimations(5));
    }

    
    IEnumerator IdleAnimations(float waitTime)
    {
        if (anim.GetInteger("IdleAnimation") != 0)
        {
            anim.SetInteger("IdleAnimation", 0);
        }
        else
        {
            anim.SetInteger("IdleAnimation", Random.Range(0,8));
        }
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(IdleAnimations(5));
    }

    public void Sneeze()
    {
        sneeze.Play();
    }
}
