using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [SerializeField] float transparencyValue = 1;
    float distance;

    [SerializeField] Collider objectCollider;
    [SerializeField] List<GameObject> objectPartsList_Base = new List<GameObject>();
    [SerializeField] List<GameObject> objectPartsList_Pickable = new List<GameObject>();
    public List<Material> materialList = new List<Material>();

    SphereCollider sphereCollider;

    public List<Renderer> rendererList = new List<Renderer>();

    string TransparencyName = "_Transparency";
    string InvisibleTriggerPointTagnName = "InvisibleTriggerPoint";

    MaterialPropertyBlock propertyBlock;

    public bool isVisible;


    //--------------------


    private void Start()
    {
        //Add a SphereCollider to this GameObject
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = 4f;
        sphereCollider.isTrigger = true;

        distance = 0;
        transparencyValue = 1;

        //Setup TempMaterials
        for (int i = 0; i < materialList.Count; i++)
        {
            materialList[i] = Instantiate(materialList[i]);
        }

        propertyBlock = new MaterialPropertyBlock();

        UpdateVisibility();
    }


    //--------------------


    public void UpdateVisibility()
    {
        if (HotbarManager.Instance.selectedItem == Items.Flashlight)
        {
            for (int i = 0; i < rendererList.Count; i++)
            {
                propertyBlock.SetFloat(TransparencyName, transparencyValue);

                rendererList[i].SetPropertyBlock(propertyBlock);
            }
        }
        else
        {
            transparencyValue = 1;
            
            for (int i = 0; i < rendererList.Count; i++)
            {
                propertyBlock.SetFloat(TransparencyName, transparencyValue);

                rendererList[i].SetPropertyBlock(propertyBlock);
            }
        }

        if (transparencyValue >= 1)
        {
            objectCollider.enabled = false;

            for (int i = 0; i < objectPartsList_Base.Count; i++)
            {
                objectPartsList_Base[i].SetActive(false);
            }

            for (int i = 0; i < objectPartsList_Pickable.Count; i++)
            {
                objectPartsList_Pickable[i].SetActive(false);
            }

            isVisible = false;
        }
        else
        {
            objectCollider.enabled = true;

            if (gameObject.GetComponent<Plant>())
            {
                if (gameObject.GetComponent<Plant>().isPicked)
                {
                    for (int i = 0; i < objectPartsList_Base.Count; i++)
                    {
                        objectPartsList_Base[i].SetActive(true);
                    }

                    for (int i = 0; i < objectPartsList_Base.Count; i++)
                    {
                        objectPartsList_Pickable[i].SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < objectPartsList_Base.Count; i++)
                    {
                        objectPartsList_Base[i].SetActive(true);
                    }

                    for (int i = 0; i < objectPartsList_Base.Count; i++)
                    {
                        objectPartsList_Pickable[i].SetActive(true);
                    }
                }
            }

            isVisible = true;
        }
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(InvisibleTriggerPointTagnName))
        {
            distance = Vector3.Distance(transform.position, other.gameObject.transform.position);

            transparencyValue = (distance / sphereCollider.radius);

            UpdateVisibility();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(InvisibleTriggerPointTagnName))
        {
            distance = Vector3.Distance(transform.position, other.gameObject.transform.position);

            transparencyValue = (distance / sphereCollider.radius);

            UpdateVisibility();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(InvisibleTriggerPointTagnName))
        {
            distance = 0;
            transparencyValue = 1;

            UpdateVisibility();
        }
    }
}