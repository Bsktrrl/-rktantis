using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] List<GameObject> collidingObjectList;


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
    }

    private void Update()
    {
        GetAllObjectsInSphere();
    }


    //--------------------


    public void GetAllObjectsInSphere()
    {
        if (sphereCollider != null)
        {
            //Check if any invisibleObject is inside the Sphere
            if (Physics.CheckSphere(transform.position + sphereCollider.center, sphereCollider.radius, InvisibleObjectManager.Instance.invisibleObjectLayerMask))
            {
                //Find all colliders inside sphereCollider
                Collider[] collidersInsideSphere = Physics.OverlapSphere(transform.position + sphereCollider.center, sphereCollider.radius, InvisibleObjectManager.Instance.invisibleObjectLayerMask);

                //If there are missing objects from the list, remove them
                #region
                for (int i = collidingObjectList.Count - 1; i >= 0; i--)
                {
                    bool isColliderFound = false;

                    foreach (Collider collider in collidersInsideSphere)
                    {
                        if (collidingObjectList[i] == null)
                        {
                            isColliderFound = false;
                            break;
                        }
                        else if (collidingObjectList[i].GetComponent<Collider>() == collider)
                        {
                            isColliderFound = true;
                            break;
                        }
                    }

                    //If the collider is not found, remove the gameObject from the list
                    if (!isColliderFound)
                    {
                        collidingObjectList.RemoveAt(i);
                    }
                }
                #endregion


                //If object is not in the list, add it
                #region
                foreach (Collider collider in collidersInsideSphere)
                {
                    if (!collidingObjectList.Contains(collider.transform.gameObject))
                    {
                        collidingObjectList.Add(collider.transform.gameObject);
                    }
                }
                #endregion


                //Check if this gameObject is Hidden
                if (collidingObjectList.Count <= 0)
                {

                    distance = 0;
                    transparencyValue = 1;

                    UpdateVisibilityNew();
                }

                //Check if this gameObject is Visible
                else
                {
                    if (collidingObjectList.Count == 1)
                    {
                        distance = Vector3.Distance(transform.position, collidingObjectList[0].transform.position);
                        transparencyValue = (distance / sphereCollider.radius);
                    }
                    else
                    {
                        //Find the gameObject closes to this gameObject in distance
                        List<float> tempIndex = new List<float>();
                        int lowestElement = 0;

                        for (int i = 0; i < collidingObjectList.Count; i++)
                        {
                            tempIndex.Add(Vector3.Distance(gameObject.transform.position, collidingObjectList[i].transform.position));
                        }

                        for (int i = 0; i < collidingObjectList.Count; i++)
                        {
                            if (tempIndex[i] == tempIndex.Min())
                            {
                                lowestElement = i;

                                break;
                            }
                        }

                        distance = Vector3.Distance(transform.position, collidingObjectList[lowestElement].transform.position);
                        transparencyValue = (distance / sphereCollider.radius);
                    }

                    UpdateVisibilityNew();
                }
            }
            else
            {
                if (collidingObjectList.Count > 0)
                {
                    collidingObjectList.Clear();

                }

                transparencyValue = 1;

                UpdateVisibilityNew();
            }
        }
    }

    public void UpdateVisibilityNew()
    {
        //Hide Object
        if (transparencyValue >= 1)
        {
            //Update rendererList
            UpdateRenderList();

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

        //Show Object
        else
        {
            //Update rendererList
            UpdateRenderList();

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

            isVisible = true;
        }
    }
    void UpdateRenderList()
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
}