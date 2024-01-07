using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ChoppableTree : MonoBehaviour
{
    public GameObject treeParent;

    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeCurrentHealth;

    //SphereCollider accessCollider = new SphereCollider();


    //--------------------


    private void Start()
    {
        //Add SphereCollider for interacting with the tree
        //accessCollider = gameObject.AddComponent<SphereCollider>();
        //accessCollider.radius = WorldObjectManager.instance.objectColliderRadius;
        //accessCollider.isTrigger = true;

        treeCurrentHealth = treeMaxHealth;
    }


    //--------------------


    public void GetHit()
    {
        treeCurrentHealth -= 1;
    }


    //--------------------


    private void OnTriggerEnter(Collider collision)
    {
        //If a player is entering the area
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        //If a player is exiting the area
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
