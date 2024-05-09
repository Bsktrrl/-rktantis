using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimations : MonoBehaviour
{
    Animator anim;
    ParticleSystem sneeze;

    Transform style1;
    Transform style2;
    Transform style3;
    Transform beard;
    int style;
    bool hasBeard;

    void Start()
    {
        //Get reference to animator and sneezing particle system
        anim = GetComponent<Animator>();
        sneeze = transform.Find("Armature_Ghost/Spine1_Jnt/Spine2_Jnt/Spine3_Jnt/Neck_Jnt/Head_Jnt").GetComponent<ParticleSystem>();

        //Get reference to different styles and beard
        style1 = transform.Find("WaterGhost1");
        style2 = transform.Find("WaterGhost2");
        style3 = transform.Find("WaterGhost3");
        beard = transform.Find("Beard");
        
        //Set style and beard
        style = Random.Range(1,4);
        hasBeard = Random.value > 0.5f;
        style1.gameObject.SetActive(false);
        style2.gameObject.SetActive(false);
        style3.gameObject.SetActive(false);
        beard.gameObject.SetActive(false);
        if (style == 1)
        {
            style1.gameObject.SetActive(true);
        }
        else if (style == 2)
        {
            style2.gameObject.SetActive(true);
        }
        else if (style == 3)
        {
            style3.gameObject.SetActive(true);
        }
        if (hasBeard == true)
        {
            beard.gameObject.SetActive(true);
        }

        StartCoroutine(IdleAnimations(5));
    }

    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        //Press G to randomize style
        if (Input.GetKeyDown(KeyCode.G))
        {
            style = Random.Range(1, 4);
            hasBeard = Random.value > 0.5f;
            style1.gameObject.SetActive(false);
            style2.gameObject.SetActive(false);
            style3.gameObject.SetActive(false);
            beard.gameObject.SetActive(false);
            if (style == 1)
            {
                style1.gameObject.SetActive(true);
            }
            else if (style == 2)
            {
                style2.gameObject.SetActive(true);
            }
            else if (style == 3)
            {
                style3.gameObject.SetActive(true);
            }
            if (hasBeard == true)
            {
                beard.gameObject.SetActive(true);
            }
        }
    }

    //Change idle animation int in animator every 5 seconds
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

    //Play sneeze particle system (called by animation events)
    public void Sneeze()
    {
        sneeze.Play();
    }
}
