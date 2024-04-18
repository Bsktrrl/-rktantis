using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public bool onTarget = false;
    bool onTargetTemp = false;

    public GameObject selectedMovableObjectToRemove; //Object for removal with Axe
    public GameObject selectedObject;

    public string tag;

    InteractableObject newInteractableObject;
    Plant newPlantObject;
    Ghost newGhostObject;

    bool lookAway;


    //--------------------


    void Update()
    {
        if (Time.frameCount % MainManager.Instance.updateInterval == 0 && MainManager.Instance.menuStates == MenuStates.None)
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, PlayerManager.Instance.InteractableDistance))
            {
                //If Hitting the SphereCollider to an InvisibleObject, ignore the hit
                if (hit.transform.gameObject.GetComponent<InvisibleObject>())
                {
                    if (hit.transform.gameObject.GetComponent<SphereCollider>())
                    {
                        return;
                    }
                }

                Transform selectionTransform = hit.transform;

                //Get the layer looking at
                tag = selectionTransform.gameObject.tag;

                //selectedMovableObjectToRemove = selectionTransform.gameObject;

                //When raycasting something that is interactable
                newInteractableObject = null;
                newPlantObject = null;
                newGhostObject = null;


                //-----


                //Check for a MovableObject that can be removed
                if (selectionTransform.gameObject
                    && (HotbarManager.Instance.selectedItem == Items.WoodAxe || HotbarManager.Instance.selectedItem == Items.StoneAxe || HotbarManager.Instance.selectedItem == Items.CryoniteAxe))
                {
                    GameObject tempSelectedMovableObjectToRemove = selectedMovableObjectToRemove;

                    //Object
                    if (selectionTransform.gameObject.GetComponent<MoveableObject>())
                    {
                        selectedMovableObjectToRemove = selectionTransform.gameObject;

                        if (selectedMovableObjectToRemove && tempSelectedMovableObjectToRemove != selectedMovableObjectToRemove)
                        {
                            BuildingDisplayManager.Instance.UpdateScreenBuildingRewardDisplayInfo();
                        }

                        onTarget = true;
                        onTargetTemp = true;
                        lookAway = true;
                    }
                    //ParentObject
                    else if (selectionTransform.gameObject.GetComponentInParent<MoveableObject>())
                    {
                        selectedMovableObjectToRemove = selectionTransform.gameObject.GetComponentInParent<MoveableObject>().gameObject;

                        if (selectedMovableObjectToRemove && tempSelectedMovableObjectToRemove != selectedMovableObjectToRemove)
                        {
                            BuildingDisplayManager.Instance.UpdateScreenBuildingRewardDisplayInfo();
                        }

                        onTarget = true;
                        onTargetTemp = true;
                        lookAway = true;
                    }
                    //ParentParentObject
                    else if (selectionTransform.parent)
                    {
                        if (selectionTransform.parent.GetComponentInParent<MoveableObject>())
                        {
                            selectedMovableObjectToRemove = selectionTransform.parent.GetComponentInParent<MoveableObject>().gameObject;

                            if (selectedMovableObjectToRemove && tempSelectedMovableObjectToRemove != selectedMovableObjectToRemove)
                            {
                                BuildingDisplayManager.Instance.UpdateScreenBuildingRewardDisplayInfo();
                            }

                            onTarget = true;
                            onTargetTemp = true;
                            lookAway = true;
                        }
                    }
                    //Nothing
                    else
                    {
                        if (selectedMovableObjectToRemove)
                        {
                            BuildingDisplayManager.Instance.ResetRewardScreenDisplay();

                            lookAway = false;
                        }

                        selectedMovableObjectToRemove = null;
                        onTargetTemp = false;
                    }
                }
                else
                {
                    if (selectedMovableObjectToRemove)
                    {
                        BuildingDisplayManager.Instance.ResetRewardScreenDisplay();

                        lookAway = false;
                    }

                    selectedMovableObjectToRemove = null;
                    onTargetTemp = false;
                }
                
                //Check for looking at other blocks
                if (selectionTransform.GetComponent<InteractableObject>())
                {
                    newInteractableObject = selectionTransform.GetComponent<InteractableObject>();
                }
                else if(selectionTransform.GetComponent<Plant>())
                {
                    newPlantObject = selectionTransform.GetComponent<Plant>();
                }
                else if (selectionTransform.GetComponent<Ghost>())
                {
                    newGhostObject = selectionTransform.GetComponent<Ghost>();
                }


                //-----


                //If looking at an Interactable Object, show its UI to the player
                if (newInteractableObject != null)
                {
                    //Show Inventory info
                    onTarget = true;
                    selectedObject = newInteractableObject.gameObject;

                    if (newInteractableObject.GetComponent<InteractableObject>())
                    {
                        LookAtManager.Instance.typeLookingAt = newInteractableObject.GetComponent<InteractableObject>().interactableType;
                    }
                    
                    if (newInteractableObject != null)
                    {
                        if (newInteractableObject.gameObject.activeInHierarchy)
                        {
                            LookAtManager.Instance.LookAt();
                        }
                    }
                }

                //If looking at a Plant, show its UI to the player
                else if (newPlantObject != null)
                {
                    //Show Inventory info
                    onTarget = true;
                    selectedObject = newPlantObject.gameObject;

                    if (newPlantObject.pickablePart.GetComponent<InteractableObject>())
                    {
                        LookAtManager.Instance.typeLookingAt = newPlantObject.pickablePart.GetComponent<InteractableObject>().interactableType;
                    }

                    if (newPlantObject != null)
                    {
                        if (newPlantObject.gameObject.activeInHierarchy)
                        {
                            LookAtManager.Instance.LookAt();
                        }
                    }
                }

                //If looking at a Ghost, show its UI to the player
                else if (newGhostObject != null)
                {
                    //Show Inventory info
                    onTarget = true;
                    selectedObject = newGhostObject.gameObject;

                    if (newGhostObject.GetComponent<Ghost>())
                    {
                        LookAtManager.Instance.typeLookingAt = newGhostObject.GetComponent<Ghost>().interactableType;
                    }
                    
                    if (newGhostObject != null)
                    {
                        if (newGhostObject.gameObject.activeInHierarchy)
                        {
                            LookAtManager.Instance.LookAt();
                        }
                    }
                }

                //If there is a Hit without an interacteable script
                else
                {
                    if (!onTargetTemp)
                    {
                        onTarget = false;

                        if (lookAway)
                        {
                            BuildingDisplayManager.Instance.ResetRewardScreenDisplay();

                            lookAway = false;
                        }
                    }

                    LookAtManager.Instance.typeLookingAt = InteracteableType.None;

                    LookAtManager.Instance.TurnOffScreens();
                }
            }

            //If there is no script attached at all
            else
            {
                LookAtManager.Instance.TurnOffScreens();

                if (!onTargetTemp)
                {
                    onTarget = false;
                }
                
                //Get the layer looking at
                tag = "";


                selectedMovableObjectToRemove = null;

                onTargetTemp = false;
                onTarget = false;

                if (lookAway)
                {
                    BuildingDisplayManager.Instance.ResetRewardScreenDisplay();

                    lookAway = false;
                }

                LookAtManager.Instance.typeLookingAt = InteracteableType.None;
            }
        }
    }
}