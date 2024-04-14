using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    public float transparencyValue = 1;

    [SerializeField] Collider objectCollider;

    [SerializeField] List<GameObject> objectPartsList_Base = new List<GameObject>();
    [SerializeField] List<GameObject> objectPartsList_Pickable = new List<GameObject>();

    public List<Material> materialList = new List<Material>();
    public List<Renderer> rendererList = new List<Renderer>();

    [SerializeField] List<GameObject> collidingObjectList;

    //SphereCollider sphereCollider;
    GameObject sphereCollider;
    public MaterialPropertyBlock propertyBlock;

    string TransparencyName = "_Transparency";
    public bool isVisible;
    float distance;

    [SerializeField] bool isPicture;
    [SerializeField] Texture pictureSprite;

    public bool isInTank;



    //--------------------


    private void Start()
    {
        //Add a SphereCollider to this GameObject
        sphereCollider = Instantiate(InvisibleObjectManager.Instance.sphereObject_Prefab);

        //sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.GetComponent<SphereCollider>().radius = 4f;
        sphereCollider.GetComponent<SphereCollider>().isTrigger = true;

        propertyBlock = new MaterialPropertyBlock();

        distance = 0;
        transparencyValue = 1;
        UpdateRenderList();

        //Setup TempMaterials
        for (int i = 0; i < materialList.Count; i++)
        {
            materialList[i] = Instantiate(materialList[i]);
        }

        collidingObjectList.Clear();

        //If Object is a Picture, change the Texture
        if (isPicture)
        {
            propertyBlock.SetTexture("_AlbedoMap", pictureSprite);
            propertyBlock.SetTexture("_NormalMap", pictureSprite);
            propertyBlock.SetTexture("_RoughnessMap", pictureSprite);

            rendererList[0].SetPropertyBlock(propertyBlock);
        }
    }

    private void Update()
    {
        if (!isInTank)
        {
            GetAllObjectsInSphere();

            SetSphereRange();
        }
    }


    //--------------------


    public void GetAllObjectsInSphere()
    {
        if (sphereCollider != null)
        {
            //Check if any invisibleObject is inside the Sphere
            if (Physics.CheckSphere(transform.position + sphereCollider.GetComponent<SphereCollider>().center, sphereCollider.GetComponent<SphereCollider>().radius, InvisibleObjectManager.Instance.invisibleObjectLayerMask))
            {
                //Find all colliders inside sphereCollider
                Collider[] collidersInsideSphere = Physics.OverlapSphere(transform.position + sphereCollider.GetComponent<SphereCollider>().center, sphereCollider.GetComponent<SphereCollider>().radius, InvisibleObjectManager.Instance.invisibleObjectLayerMask);

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


                //Check if this gameObject will be invisible
                if (collidingObjectList.Count <= 0)
                {
                    distance = 0;
                    transparencyValue = 1;

                    UpdateVisibilityNew();
                }

                //Check if this gameObject will be Visible
                else
                {
                    if (collidingObjectList.Count == 1)
                    {
                        distance = Vector3.Distance(transform.position, collidingObjectList[0].transform.position);
                        transparencyValue = (distance / sphereCollider.GetComponent<SphereCollider>().radius);
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
                        transparencyValue = (distance / sphereCollider.GetComponent<SphereCollider>().radius);
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

            //If a Plant
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

                    for (int i = 0; i < objectPartsList_Pickable.Count; i++)
                    {
                        objectPartsList_Pickable[i].SetActive(true);
                    }
                }
            }

            //If a Ghost
            else if (gameObject.GetComponent<Ghost>())
            {
                if (gameObject.GetComponent<Ghost>().ghostStats.isBeard)
                {
                    gameObject.GetComponent<Ghost>().beard.SetActive(true);
                }

                switch (gameObject.GetComponent<Ghost>().ghostStats.ghostAppearance)
                {
                    case GhostAppearance.Type1:
                        for (int i = 0; i < gameObject.GetComponent<Ghost>().style1.Count; i++)
                            gameObject.GetComponent<Ghost>().style1[i].SetActive(true);
                        break;
                    case GhostAppearance.Type2:
                        for (int i = 0; i < gameObject.GetComponent<Ghost>().style2.Count; i++)
                            gameObject.GetComponent<Ghost>().style2[i].SetActive(true);
                        break;
                    case GhostAppearance.Type3:
                        for (int i = 0; i < gameObject.GetComponent<Ghost>().style3.Count; i++)
                            gameObject.GetComponent<Ghost>().style3[i].SetActive(true);
                        break;

                    default:
                        break;
                }
            }

            //If something else
            else
            {
                for (int i = 0; i < objectPartsList_Base.Count; i++)
                {
                    objectPartsList_Base[i].SetActive(true);
                }

                for (int i = 0; i < objectPartsList_Pickable.Count; i++)
                {
                    objectPartsList_Pickable[i].SetActive(true);
                }
            }

            isVisible = true;
        }
    }
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


    //--------------------


    public void SetSphereRange()
    {
        if (HotbarManager.Instance.selectedItem == Items.Flashlight)
        {
            sphereCollider.GetComponent<SphereCollider>().radius = InvisibleObjectManager.Instance.flashlight_Distance;
        }
        else if (HotbarManager.Instance.selectedItem == Items.AríditeCrystal)
        {
            sphereCollider.GetComponent<SphereCollider>().radius = InvisibleObjectManager.Instance.aríditeCrystal_Distance;
        }
        else if (HotbarManager.Instance.selectedItem == Items.GhostCapturer)
        {
            sphereCollider.GetComponent<SphereCollider>().radius = InvisibleObjectManager.Instance.ghostCapture_Distance;
        }
    }
}