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
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

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
        if (subCategories == ItemSubCategories.Axe && SelectionManager.Instance.selectedTree != null)
        {
            if (SelectionManager.Instance.selectedTree.GetComponent<ChoppableTree>().treeParent != null)
            {
                SelectionManager.Instance.selectedTree.GetComponent<ChoppableTree>().treeParent.gameObject.GetComponent<TreeParent>().ObjectInteraction();
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
            if (gameObject.GetComponent<BuildingHammer>().tempObj_Selected)
            {
                if (gameObject.GetComponent<BuildingHammer>().tempObj_Selected.GetComponent<InteractableObject>())
                {
                    gameObject.GetComponent<BuildingHammer>().tempObj_Selected.GetComponent<InteractableObject>().DestroyThisObject();
                    gameObject.GetComponent<BuildingHammer>().DestroyThisObject();
                }
                else
                {
                    Destroy(gameObject.GetComponent<BuildingHammer>().tempObj_Selected);
                }
            }
            
            gameObject.GetComponent<BuildingHammer>().tempObj_Selected = null;
        }

        //If Equipped Object is an Axe
        else if (gameObject.GetComponent<Axe>() != null)
        {
            gameObject.GetComponent<Axe>().DestroyThisObject();
        }

        Destroy(gameObject);
    }
}
