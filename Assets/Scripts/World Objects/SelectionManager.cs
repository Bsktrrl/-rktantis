using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : Singleton<SelectionManager>
{
    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text;

    public bool onTarget = false;

    public GameObject selecedObject;
    public GameObject selectedTree;

    public GameObject chopHolder;

    InteractableObject newInteractableObject;


    //--------------------


    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (Time.frameCount % MainManager.Instance.updateInterval == 0 && MainManager.Instance.menuStates == MenuStates.None)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, MainManager.Instance.InteractableDistance))
            {
                Transform selectionTransform = hit.transform;

                //When raycasting something that is interactable
                if (selectionTransform.GetComponent<InteractableObject>())
                {
                    newInteractableObject = selectionTransform.GetComponent<InteractableObject>();
                }
                else
                {
                    newInteractableObject = null;
                }


                //-----


                //If looking at an Interacteable Object, show its UI to the player
                if (newInteractableObject)
                {
                    //Show Inventory info
                    onTarget = true;
                    selecedObject = newInteractableObject.gameObject;

                    //Change Active text
                    if (selecedObject.GetComponent<InteractableObject>().interacteableType == InteracteableType.Pickup)
                    {
                        //Set correct UI-info for Pickup to be displayed
                        interaction_text.text = selecedObject.GetComponent<InteractableObject>().itemName.ToString();
                        interaction_Info_UI.SetActive(true);
                    }
                    else if (selecedObject.GetComponent<InteractableObject>().interacteableType == InteracteableType.Inventory)
                    {
                        //Set correct UI-info for Inventory/Chest to be displayed
                        interaction_text.text = selecedObject.GetComponent<InteractableObject>().itemName.ToString();
                    }
                    else if (selecedObject.GetComponent<InteractableObject>().interacteableType == InteracteableType.CraftingTable)
                    {
                        //Set correct UI-info for Machine to be displayed
                        interaction_text.text = selecedObject.GetComponent<InteractableObject>().itemName.ToString();
                        interaction_Info_UI.SetActive(true);
                    }

                    //Set UI screen Active
                    interaction_Info_UI.SetActive(true);
                }
                //If there is a Hit without an interacteable script
                else
                {
                    interaction_Info_UI.SetActive(false);
                    onTarget = false;
                }


                // || Trees || //
                #region
                //ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

                ////If looking at a tree
                //if (choppableTree && choppableTree.playerInRange)
                //{
                //    choppableTree.canBeChopped = true;
                //    selectedTree = choppableTree.gameObject;
                //    chopHolder.gameObject.SetActive(true);
                //}
                //else
                //{
                //    if (selectedTree != null)
                //    {
                //        selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                //        selectedTree = null;
                //    }

                //    chopHolder.gameObject.SetActive(false);
                //}
                #endregion

            }

            //If there is no script attached at all
            else
            {
                interaction_Info_UI.SetActive(false);
                onTarget = false;
            }
        }
    }
}