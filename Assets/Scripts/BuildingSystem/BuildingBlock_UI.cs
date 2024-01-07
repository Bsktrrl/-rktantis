using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingBlock_UI : MonoBehaviour, IPointerEnterHandler
{
    public BuildingType BuildingType;
    public BuildingMaterial BuildingMaterial;

    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildingManager.instance.buildingType_Selected = BuildingType;
        BuildingManager.instance.buildingMaterial_Selected = BuildingMaterial;

        BuildingSystemMenu.instance.SetSelectedImage(gameObject.GetComponent<Image>().sprite);

        BuildingBlock_Parent tempParent = BuildingManager.instance.GetBuildingBlock(BuildingType, BuildingMaterial);
        if (tempParent != null)
        {
            //Set requirements for both BuildingMenu and on main screen
            BuildingManager.instance.SetBuildingRequirements(tempParent, BuildingSystemMenu.instance.buildingRequirement_Parent);
            BuildingManager.instance.SetBuildingRequirements(BuildingManager.instance.GetBuildingBlock(BuildingManager.instance.buildingType_Selected, BuildingManager.instance.buildingMaterial_Selected), BuildingManager.instance.buildingRequirement_Parent);
        }

        //Update "Free Block" if Hammer is selected
        if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
        {
            EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

            print("200. New Selected Block Set: Type: " + BuildingType + " | Material: " + BuildingMaterial);
        }

        BuildingManager.instance.SaveData();
    }
}
