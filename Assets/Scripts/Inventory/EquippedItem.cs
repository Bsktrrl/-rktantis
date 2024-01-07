using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquippedItem : MonoBehaviour
{
    public Animator animator;
    public ItemSubCategories subCategories;


    //--------------------


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    //--------------------


    public void HitAnimation()
    {
        GameObject selectedTree = SelectionManager.instance.selectedTree;

        if (selectedTree != null)
        {
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }

        animator.SetTrigger("hit");
    }

    public void GetHit()
    {
        //The point in the animation where equipped item hits

        //If Axe is equipped
        if (subCategories == ItemSubCategories.Axe && SelectionManager.instance.selectedTree != null)
        {
            if (SelectionManager.instance.selectedTree.GetComponent<ChoppableTree>().treeParent != null)
            {
                SelectionManager.instance.selectedTree.GetComponent<ChoppableTree>().treeParent.gameObject.GetComponent<TreeParent>().ObjectInteraction();
            }
        }
    }

    public void RemoveDurability()
    {
        
    }


    //--------------------


    public void DestroyObject()
    {
        //If Equipped Object is a BuildingHammer
        if (gameObject.GetComponent<BuildingHammer>() != null)
        {
            Destroy(gameObject.GetComponent<BuildingHammer>().tempObj_Selected);
            gameObject.GetComponent<BuildingHammer>().tempObj_Selected = null;
        }

        Destroy(gameObject);
    }
}
