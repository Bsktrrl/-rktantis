using UnityEngine;

public class Animations_Objects : MonoBehaviour
{
    Animator anim;

    bool hasStarted = false;


    //--------------------


    void Start()
    {
        anim = GetComponent<Animator>();

        StopAnimation();

        hasStarted = true;
    }


    //--------------------


    public void StartAnimation()
    {
        anim.SetBool("isActive", true);

        //Play Start Sound
        if (gameObject.GetComponent<InteractableObject>() && hasStarted)
        {
            //If Chest
            if (gameObject.GetComponent<InteractableObject>().itemName == Items.SmallChest)
            {
                SoundManager.Instance.Play_Chests_OpenSmallChest_Clip();
            }
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.MediumChest)
            {
                SoundManager.Instance.Play_Chests_OpenMediumChest_Clip();
            }
            else if(gameObject.GetComponent<InteractableObject>().itemName == Items.BigChest)
            {
                SoundManager.Instance.Play_Chests_OpenBigChest_Clip();
            }

            //If Crafting Table
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.CraftingTable)
            {
                SoundManager.Instance.Play_InteractableObjects_OpenCraftingTable_Clip();
            }

            //If Skill Tree Table
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.SkillTable)
            {
                SoundManager.Instance.Play_InteractableObjects_OpenSkillTreeTable_Clip();
            }

            //If Skill Tree Table
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.SmallCropPlot
                     || gameObject.GetComponent<InteractableObject>().itemName == Items.MediumCropPlot
                     || gameObject.GetComponent<InteractableObject>().itemName == Items.LargeCropPlot)
            {
                SoundManager.Instance.Play_InteractableObjects_OpenCropPlot_Clip();
            }
        }
    }
    public void StopAnimation()
    {
        anim.SetBool("isActive", false);

        //Play Start Sound
        if (gameObject.GetComponent<InteractableObject>() && hasStarted)
        {
            //If Chest
            if (gameObject.GetComponent<InteractableObject>().itemName == Items.SmallChest)
            {
                SoundManager.Instance.Play_Chests_CloseSmallChest_Clip();
            }
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.MediumChest)
            {
                SoundManager.Instance.Play_Chests_CloseMediumChest_Clip();
            }
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.BigChest)
            {
                SoundManager.Instance.Play_Chests_CloseBigChest_Clip();
            }

            //If Crafting Table
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.CraftingTable)
            {
                SoundManager.Instance.Play_InteractableObjects_CloseCraftingTable_Clip();
            }

            //If Skill Tree Table
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.SkillTable)
            {
                SoundManager.Instance.Play_InteractableObjects_CloseSkillTreeTable_Clip();
            }

            //If Skill Tree Table
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.SmallCropPlot
                     || gameObject.GetComponent<InteractableObject>().itemName == Items.MediumCropPlot
                     || gameObject.GetComponent<InteractableObject>().itemName == Items.LargeCropPlot)
            {
                SoundManager.Instance.Play_InteractableObjects_CloseCropPlot_Clip();
            }
        }
    }
}
