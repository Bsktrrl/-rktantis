using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [SerializeField] float transparencyValue = 1;
    [SerializeField] float distance;

    [SerializeField] Collider objectCollider;
    [SerializeField] List<GameObject> objectPartsList = new List<GameObject>();
    [SerializeField] List<Material> materialList = new List<Material>();

    SphereCollider sphereCollider;
    GameObject tempCollider;


    //--------------------


    private void Start()
    {
        //Add a SphereCollider to this GameObject
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = 5f;
        sphereCollider.isTrigger = true;

        distance = 0;
        transparencyValue = 1;
    }
    private void Update()
    {
        if (HotbarManager.Instance.selectedItem == Items.Flashlight)
        {
            for (int i = 0; i < materialList.Count; i++)
            {
                materialList[i].SetFloat("_Transparency", transparencyValue);
            }
        }
        else
        {
            transparencyValue = 1;

            for (int i = 0; i < materialList.Count; i++)
            {
                materialList[i].SetFloat("_Transparency", transparencyValue);
            }
        }

        if (transparencyValue >= 1)
        {
            objectCollider.enabled = false;

            for (int i = 0; i < objectPartsList.Count; i++)
            {
                objectPartsList[i].SetActive(false);
            }
        }
        else
        {
            objectCollider.enabled = true;

            for (int i = 0; i < objectPartsList.Count; i++)
            {
                objectPartsList[i].SetActive(true);
            }
        }
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleTriggerPoint"))
        {
            tempCollider = other.gameObject;

            distance = Vector3.Distance(transform.position, other.gameObject.transform.position);

            transparencyValue = (distance / sphereCollider.radius);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleTriggerPoint"))
        {
            tempCollider = other.gameObject;

            distance = Vector3.Distance(transform.position, other.gameObject.transform.position);

            transparencyValue = (distance / sphereCollider.radius);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleTriggerPoint"))
        {
            tempCollider = null;

            distance = 0;
            transparencyValue = 1;
        }
    }
}