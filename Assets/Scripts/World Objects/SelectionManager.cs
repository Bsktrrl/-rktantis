using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SelectionManager : Singleton<SelectionManager>
{
    public bool onTarget = false;

    public GameObject selecedObject;

    public string tag;

    InteractableObject newInteractableObject;
    Plant newPlantObject;


    //--------------------


    void Update()
    {
        if (Time.frameCount % MainManager.Instance.updateInterval == 0 && MainManager.Instance.menuStates == MenuStates.None)
        {
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

                //When raycasting something that is interactable
                newInteractableObject = null;
                newPlantObject = null;

                if (selectionTransform.GetComponent<InteractableObject>())
                {
                    newInteractableObject = selectionTransform.GetComponent<InteractableObject>();
                }
                else if(selectionTransform.GetComponent<Plant>())
                {
                    newPlantObject = selectionTransform.GetComponent<Plant>();
                }


                //-----


                //If looking at an Interacteable Object, show its UI to the player
                if (newInteractableObject != null)
                {
                    //Show Inventory info
                    onTarget = true;
                    selecedObject = newInteractableObject.gameObject;

                    LookAtManager.Instance.typeLookingAt = newInteractableObject.GetComponent<InteractableObject>().interactableType;

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

                    LookAtManager.Instance.typeLookingAt = newPlantObject.pickablePart.GetComponent<InteractableObject>().interactableType;

                    if (newInteractableObject != null)
                    {
                        if (newInteractableObject.gameObject.activeInHierarchy)
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