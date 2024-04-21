using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInTank : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSource_Ghost_GhostSounds;

    [Header("General")]
    public GhostStats ghostStats = new GhostStats();

    [Header("Styles")]
    public GameObject beard;
    public List<GameObject> style1 = new List<GameObject>();
    public List<GameObject> style2 = new List<GameObject>();
    public List<GameObject> style3 = new List<GameObject>();

    [Header("Invisibility")]
    public float transparencyValue = 0;
    string TransparencyName = "_Transparency";
    public MaterialPropertyBlock propertyBlock;
    public List<Material> materialList = new List<Material>();
    public List<Renderer> rendererList = new List<Renderer>();


    //--------------------


    private void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
    }
    private void Update()
    {
        UpdateRenderList();
    }


    //--------------------


    public void UpdateRenderList()
    {
        for (int i = 0; i < rendererList.Count; i++)
        {
            if (propertyBlock != null)
            {
                propertyBlock.SetFloat(TransparencyName, transparencyValue);

                rendererList[i].SetPropertyBlock(propertyBlock);
            }
        }
    }

    public void SetGhostAppearance()
    {
        //Reset Styles
        for (int i = 0; i < style1.Count; i++)
            style1[i].SetActive(false);
        for (int i = 0; i < style2.Count; i++)
            style2[i].SetActive(false);
        for (int i = 0; i < style3.Count; i++)
            style3[i].SetActive(false);

        //Set Beard
        if (ghostStats.isBeard)
        {
            beard.SetActive(true);
        }
        else
        {
            beard.SetActive(false);
        }

        //Set Style
        switch (ghostStats.ghostAppearance)
        {
            case GhostAppearance.Type1:
                for (int i = 0; i < style1.Count; i++)
                    style1[i].SetActive(true);
                break;
            case GhostAppearance.Type2:
                for (int i = 0; i < style2.Count; i++)
                    style2[i].SetActive(true);
                break;
            case GhostAppearance.Type3:
                for (int i = 0; i < style3.Count; i++)
                    style3[i].SetActive(true);
                break;

            default:
                break;
        }
    }


    //--------------------


    public void SetGhostAnimation()
    {
        if (anim.GetInteger("IdleAnimation") != 0)
        {
            anim.SetInteger("IdleAnimation", 0);
        }
        else
        {
            //Set Animation
            //anim.SetInteger("IdleAnimation", Random.Range(0, 8));
            anim.SetInteger("IdleAnimation", 1);

            //Set Animation Sound
            if (anim.GetInteger("IdleAnimation") == 1)
            {
                SoundManager.Instance.Play_GhostAnimation_Spin1_Clip(audioSource_Ghost_GhostSounds);
            }
            else if (anim.GetInteger("IdleAnimation") == 2)
            {
                SoundManager.Instance.Play_GhostAnimation_Spin2_Clip(audioSource_Ghost_GhostSounds);
            }
            else if (anim.GetInteger("IdleAnimation") == 3)
            {
                SoundManager.Instance.Play_GhostAnimation_Spin3_Clip(audioSource_Ghost_GhostSounds);
            }
            else if (anim.GetInteger("IdleAnimation") == 4)
            {
                SoundManager.Instance.Play_GhostAnimation_FakeDeath_Clip(audioSource_Ghost_GhostSounds);
            }
            else if (anim.GetInteger("IdleAnimation") == 5)
            {
                SoundManager.Instance.Play_GhostAnimation_Sneeze1_Clip(audioSource_Ghost_GhostSounds);
            }
            else if (anim.GetInteger("IdleAnimation") == 6)
            {
                SoundManager.Instance.Play_GhostAnimation_Sneeze2_Clip(audioSource_Ghost_GhostSounds);
            }
            else if (anim.GetInteger("IdleAnimation") == 7)
            {
                SoundManager.Instance.Play_GhostAnimation_Wave_Clip(audioSource_Ghost_GhostSounds);
            }
        }
    }
}
