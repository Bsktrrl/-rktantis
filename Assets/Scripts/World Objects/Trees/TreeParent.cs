using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeParent : MonoBehaviour
{
    [SerializeField] GameObject Healthytree;
    [SerializeField] GameObject choppedTree;

    public Animator animator;

    int HP = 5;


    //--------------------

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    //--------------------


    public void ObjectInteraction()
    {
        if (Healthytree.GetComponent<ChoppableTree>().playerInRange && Healthytree.GetComponent<ChoppableTree>().canBeChopped
            && SelectionManager.Instance.selectedTree == gameObject.GetComponentInChildren<ChoppableTree>().gameObject
            && MainManager.Instance.menuStates == MenuStates.None)
        {
            print("ObjectInteraction");
            HP -= 1;

            if (HP <= 0)
            {
                animator.SetTrigger("TreeCut");
            }
            else
            {
                animator.SetTrigger("TreeShake");

                //if (EquipmentManager.instance.toolHolderParent != null)
                //{
                //    EquipmentManager.instance.toolHolderParent.GetComponentInChildren<EquipableItem>().RemoveDurability();
                //}
            }
        }
    }

    public void TreeHitGround()
    {
        int temp = choppedTree.transform.childCount;

        choppedTree.SetActive(true);
        ActivateGravity();

        for (int i = 0; i < temp; i++)
        {
            choppedTree.transform.GetChild(0).parent = MainManager.Instance.treeParent.transform;
        }
        
        Destroy(gameObject);
    }

    void ActivateGravity()
    {
        for (int i = 0; i < choppedTree.transform.childCount; i++)
        {
            choppedTree.transform.GetChild(i).GetComponent<Log>().ActivateLogGravity();
        }
    }

    public int GetHealth()
    {
        return HP;
    }

    public void SetHealth(int _hp)
    {
        HP = _hp;
    }
}
