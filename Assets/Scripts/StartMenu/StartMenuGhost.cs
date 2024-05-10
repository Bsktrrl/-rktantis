using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuGhost : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSource_Ghost_GhostSounds;

    [SerializeField] GameObject ghostObject_Parent;
    [SerializeField] float ghostAnimationWaitingTime;

    [SerializeField] List<bool> animationCheck = new List<bool>();


    //--------------------


    private void Start()
    {
        ghostAnimationWaitingTime = Random.Range(4f, 7f);

        for (int i = 0; i < 7; i++)
        {
            animationCheck.Add(false);
        }
    }
    private void Update()
    {
        AnimationRunCheck();
    }


    //--------------------


    void AnimationRunCheck()
    {
        ghostAnimationWaitingTime -= Time.deltaTime;

        if (ghostAnimationWaitingTime < 0)
        {
            SetGhostAnimation();

            ghostAnimationWaitingTime = Random.Range(15f, 20f);

            StartCoroutine(ResetGhostAnimation(1f));
        }
    }
    IEnumerator ResetGhostAnimation(float time)
    {
        yield return new WaitForSeconds(time);

        ResetGhostAnimation();
    }


    //--------------------

    public void SetGhostAnimation()
    {
        if (anim.GetInteger("IdleAnimation") != 0)
        {
            anim.SetInteger("IdleAnimation", 0);

            print("111. Idle");
        }
        else
        {
            //Reset List if all is full
            #region
            bool saftyCheck = true;
            for (int i = 0; i < animationCheck.Count; i++)
            {
                if (!animationCheck[i])
                {
                    saftyCheck = false;

                    break;
                }
            }
            if (saftyCheck)
            {
                for (int i = 0; i < animationCheck.Count; i++)
                {
                    animationCheck[i] = false;
                }
            }
            #endregion

            int randAnim = Random.Range(1, 8);

            while (animationCheck[randAnim - 1])
            {
                randAnim = Random.Range(1, 8);
            }

            if (animationCheck[randAnim - 1])
            {
                SetGhostAnimation();
            }
            else
            {
                //Set Animation
                anim.SetInteger("IdleAnimation", randAnim);

                //Set Animation Sound
                if (anim.GetInteger("IdleAnimation") == 1)
                {
                    StartMenuSounds.Instance.Play_GhostAnimation_Spin1_Clip(audioSource_Ghost_GhostSounds);

                    print("1. Spin 1: " + randAnim);

                    animationCheck[0] = true;
                }
                else if (anim.GetInteger("IdleAnimation") == 2)
                {
                    StartMenuSounds.Instance.Play_GhostAnimation_Spin2_Clip(audioSource_Ghost_GhostSounds);

                    print("2. Spin 2: " + randAnim);

                    animationCheck[1] = true;
                }
                else if (anim.GetInteger("IdleAnimation") == 3)
                {
                    StartMenuSounds.Instance.Play_GhostAnimation_Spin3_Clip(audioSource_Ghost_GhostSounds);

                    print("3. Spin 3: " + randAnim);

                    animationCheck[2] = true;
                }
                else if (anim.GetInteger("IdleAnimation") == 4)
                {
                    StartMenuSounds.Instance.Play_GhostAnimation_Sneeze1_Clip(audioSource_Ghost_GhostSounds);

                    print("4. Sneeze 1: " + randAnim);

                    animationCheck[3] = true;
                }
                else if (anim.GetInteger("IdleAnimation") == 5)
                {
                    StartMenuSounds.Instance.Play_GhostAnimation_Sneeze2_Clip(audioSource_Ghost_GhostSounds);

                    print("5. Sneeze 2: " + randAnim);

                    animationCheck[4] = true;
                }
                else if (anim.GetInteger("IdleAnimation") == 6)
                {
                    StartMenuSounds.Instance.Play_GhostAnimation_FakeDeath_Clip(audioSource_Ghost_GhostSounds);

                    print("6. FakeDeath: " + randAnim);

                    animationCheck[5] = true;
                }
                else if (anim.GetInteger("IdleAnimation") == 7)
                {
                    StartMenuSounds.Instance.Play_GhostAnimation_Wave_Clip(audioSource_Ghost_GhostSounds);

                    print("7. Wave: " + randAnim);

                    animationCheck[6] = true;
                }
            }
        }
    }
    public void ResetGhostAnimation()
    {
        anim.SetInteger("IdleAnimation", 0);
    }
}
