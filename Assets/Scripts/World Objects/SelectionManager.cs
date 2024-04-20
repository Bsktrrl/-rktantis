using System.Drawing;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    [Header("OnTarget")]
    public bool onTarget = false;
    bool onTargetTemp = false;

    [Header("Selected Objects")]
    public GameObject selectedObject; //Object Looking at
    public GameObject selectedMovableObjectToRemove; //Object for removal with Axe

    [Header("Tags")]
    public string tag;

    [Header("Object Types")]
    InteractableObject newInteractableObject;
    Plant newPlantObject;
    Ghost newGhostObject;

    [Header("Raycast")]
    Ray ray;
    RaycastHit hit;


    //--------------------


    void Update()
    {
        if (Time.frameCount % MainManager.Instance.updateInterval == 0
            && MainManager.Instance.menuStates == MenuStates.None)
        {
            Raycast_SelectedObject();
            Raycast_ObjectToRemove();
        }
    }

    void Raycast_SelectedObject()
    {
        selectedObject = null;

        ray = MainManager.Instance.mainMainCamera.ScreenPointToRay(Input.mousePosition);

        Vector3 startPoint = ray.origin /*+ MainManager.Instance.mainMainCamera.transform.forward*/;

        //Make Debug Line
        Debug.DrawRay(startPoint, MainManager.Instance.mainMainCamera.transform.forward * (PlayerManager.Instance.InteractableDistance), UnityEngine.Color.white);

        if (Physics.Raycast(ray, out hit, PlayerManager.Instance.InteractableDistance, ~BuildingSystemManager.Instance.layerMask_BuildingBlock))
        {
            Transform selectionTransform = hit.transform;
            tag = selectionTransform.gameObject.tag;

            //Reset ObjectReferences
            newInteractableObject = null;
            newPlantObject = null;
            newGhostObject = null;

            //Check if hitting an InteractableObject
            if (selectionTransform.GetComponent<InteractableObject>())
            {
                newInteractableObject = selectionTransform.GetComponent<InteractableObject>();
                onTarget = true;
                selectedObject = selectionTransform.gameObject;

                LookAtCheck();
            }

            //Check if hitting a Plant
            else if (selectionTransform.GetComponent<Plant>())
            {
                newPlantObject = selectionTransform.GetComponent<Plant>();
                onTarget = true;
                selectedObject = selectionTransform.gameObject;

                LookAtCheck();
            }
            else if (selectionTransform.GetComponent<Ghost>())
            {
                newGhostObject = selectionTransform.GetComponent<Ghost>();
                onTarget = true;
                selectedObject = selectionTransform.gameObject;

                LookAtCheck();
            }

            //If Hitting the SphereCollider to an InvisibleObject, ignore the hit
            else if (hit.transform.gameObject.GetComponent<InvisibleObject>())
            {
                if (hit.transform.gameObject.GetComponent<SphereCollider>())
                {
                    tag = "";
                    BuildingDisplayManager.Instance.ResetRewardScreenDisplay();
                    LookAtManager.Instance.typeLookingAt = InteracteableType.None;
                    onTarget = false;

                    return;
                }
            }
            else
            {
                tag = "";
                BuildingDisplayManager.Instance.ResetRewardScreenDisplay();
                LookAtManager.Instance.typeLookingAt = InteracteableType.None;
                onTarget = false;
            }
        }
        else
        {
            tag = "";
            BuildingDisplayManager.Instance.ResetRewardScreenDisplay();
            LookAtManager.Instance.typeLookingAt = InteracteableType.None;
        }
    }
    void LookAtCheck()
    {
        //If looking at an Interactable Object, show its UI to the player
        if (newInteractableObject != null)
        {
            print("newInteractableObject != null");
            //Show Inventory info
            onTarget = true;
            selectedObject = newInteractableObject.gameObject;

            if (newInteractableObject.GetComponent<InteractableObject>())
            {
                LookAtManager.Instance.typeLookingAt = newInteractableObject.GetComponent<InteractableObject>().interactableType;
            }

            if (newInteractableObject.gameObject.activeInHierarchy)
            {
                LookAtManager.Instance.LookAt();
            }
        }

        //If looking at a Plant, show its UI to the player
        else if (newPlantObject != null)
        {
            print("newPlantObject != null");
            //Show Inventory info
            onTarget = true;
            selectedObject = newPlantObject.gameObject;

            if (newPlantObject.pickablePart.GetComponent<InteractableObject>())
            {
                LookAtManager.Instance.typeLookingAt = newPlantObject.pickablePart.GetComponent<InteractableObject>().interactableType;
            }

            if (newPlantObject.gameObject.activeInHierarchy)
            {
                LookAtManager.Instance.LookAt();
            }
        }

        //If looking at a Ghost, show its UI to the player
        else if (newGhostObject != null)
        {
            print("newGhostObject != null");
            //Show Inventory info
            onTarget = true;
            selectedObject = newGhostObject.gameObject;

            if (newGhostObject.GetComponent<Ghost>())
            {
                LookAtManager.Instance.typeLookingAt = newGhostObject.GetComponent<Ghost>().interactableType;
            }

            if (newGhostObject.gameObject.activeInHierarchy)
            {
                LookAtManager.Instance.LookAt();
            }
        }

        //If there is a Hit without an "Interactable"-script
        else
        {
            print("If there is a Hit without an \"Interactable\"-script");

            selectedObject = null;
            onTarget = false;

            //BuildingDisplayManager.Instance.ResetRewardScreenDisplay();

            LookAtManager.Instance.typeLookingAt = InteracteableType.None;
            LookAtManager.Instance.TurnOffScreens();
        }
    }
    
    void Raycast_ObjectToRemove()
    {
        if (HotbarManager.Instance.selectedItem == Items.WoodAxe
            || HotbarManager.Instance.selectedItem == Items.StoneAxe
            || HotbarManager.Instance.selectedItem == Items.CryoniteAxe)
        {
            ray = MainManager.Instance.mainMainCamera.ScreenPointToRay(Input.mousePosition);

            Vector3 startPoint = ray.origin /*+ MainManager.Instance.mainMainCamera.transform.forward*/;

            //Make Debug Line
            Debug.DrawRay(startPoint, MainManager.Instance.mainMainCamera.transform.forward * (PlayerManager.Instance.InteractableDistance + 1), UnityEngine.Color.blue);

            //Check if hitting an Object with a Layer
            #region
            //Furniture
            if (Physics.Raycast(startPoint, MainManager.Instance.mainMainCamera.transform.forward * (PlayerManager.Instance.InteractableDistance + 1), out hit, PlayerManager.Instance.InteractableDistance + 1, BuildingSystemManager.Instance.layerMask_Furniture))
            {
                if (hit.transform.gameObject.GetComponent<MoveableObject>())
                {
                    print("Select a Furniture: " + hit.transform.gameObject.name);

                    selectedMovableObjectToRemove = hit.transform.gameObject;
                }
            }

            //Machine
            else if (Physics.Raycast(startPoint, MainManager.Instance.mainMainCamera.transform.forward * (PlayerManager.Instance.InteractableDistance + 1), out hit, PlayerManager.Instance.InteractableDistance + 1, BuildingSystemManager.Instance.layerMask_Machine))
            {
                if (hit.transform.gameObject.GetComponent<MoveableObject>())
                {
                    print("Select a Machine: " + hit.transform.gameObject.name);

                    selectedMovableObjectToRemove = hit.transform.gameObject;
                }
            }

            //BuildingBlock
            else if (Physics.Raycast(startPoint, MainManager.Instance.mainMainCamera.transform.forward * (PlayerManager.Instance.InteractableDistance + 1), out hit, PlayerManager.Instance.InteractableDistance + 1, BuildingSystemManager.Instance.layerMask_BuildingBlockModel))
            {
                if (hit.transform.gameObject.GetComponent<Model>())
                {
                    print("Select a BuildingBlock: " + hit.transform.gameObject.name);

                    selectedMovableObjectToRemove = hit.transform.gameObject;
                }
            }

            //Nothing
            else
            {
                selectedMovableObjectToRemove = null;
            }
            #endregion

            BuildingDisplayManager.Instance.UpdateScreenBuildingRewardDisplayInfo();
        }
        else
        {
            selectedMovableObjectToRemove = null;

            BuildingDisplayManager.Instance.UpdateScreenBuildingRewardDisplayInfo();
        }
    }
}