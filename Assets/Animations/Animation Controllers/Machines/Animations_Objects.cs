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
            if (gameObject.GetComponent<InteractableObject>().itemName == Items.SmallChest)
            {
                SoundManager.Instance.PlayOpenSmallChest_Clip();
            }
            else if(gameObject.GetComponent<InteractableObject>().itemName == Items.BigChest)
            {
                SoundManager.Instance.PlayOpenBigChest_Clip();
            }
        }
    }
    public void StopAnimation()
    {
        anim.SetBool("isActive", false);

        //Play Start Sound
        if (gameObject.GetComponent<InteractableObject>() && hasStarted)
        {
            if (gameObject.GetComponent<InteractableObject>().itemName == Items.SmallChest)
            {
                SoundManager.Instance.PlayCloseSmallChest_Clip();
            }
            else if (gameObject.GetComponent<InteractableObject>().itemName == Items.BigChest)
            {
                SoundManager.Instance.PlayCloseBigChest_Clip();
            }
        }
    }
}
