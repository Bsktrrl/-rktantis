using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public bool onTarget = false;

    public GameObject selecedObject;

    public string tag;

    InteractableObject newInteractableObject;
    Plant newPlantObject;
    Ghost newGhostObject;


    //--------------------


    void Update()
    {
        if (Time.frameCount % MainManager.Instance.updateInterval == 0 && MainManager.Instance.menuStates == MenuStates.None)
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            //// Calculate the middle of the screen
            //Vector3 middleOfScreen = new Vector3(screenWidth / 2f, screenHeight / 2f, 0f);
            //// Convert the middle of the screen to a ray
            //Ray ray = Camera.main.ScreenPointToRay(middleOfScreen);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition - new Vector3(0, -175f, 0));
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

                //When raycasting something that is interactable
                newInteractableObject = null;
                newPlantObject = null;
                newGhostObject = null;

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


                //If looking at an Interacteable Object, show its UI to the player
                if (newInteractableObject != null)
                {
                    //Show Inventory info
                    onTarget = true;
                    selecedObject = newInteractableObject.gameObject;

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
                    selecedObject = newPlantObject.gameObject;

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
                    selecedObject = newGhostObject.gameObject;

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
                    onTarget = false;

                    LookAtManager.Instance.typeLookingAt = InteracteableType.None;

                    LookAtManager.Instance.TurnOffScreens();
                }
            }

            //If there is no script attached at all
            else
            {
                LookAtManager.Instance.TurnOffScreens();

                onTarget = false;

                //Get the layer looking at
                tag = "";

                LookAtManager.Instance.typeLookingAt = InteracteableType.None;
            }
        }
    }
}