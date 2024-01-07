using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum ItemCategories
{
    [Description("")] None,

    [Description("Raw")] Raw,

    [Description("Food/Water")] Food,
    [Description("Other")] Other,
    [Description("Tools")] Tools,
    [Description("Weapons")] Weapons,
    [Description("Equipments")] Equipments,
    [Description("Resources")] Resources,
    [Description("Navigation")] Navigation,
    [Description("Decoration")] Decoration
}

public enum ItemSubCategories
{
    //None
    [Description("")] None,

    //Food
    [Description("F_A")][InspectorName("Food/F_A")] F_A,
    [Description("F_B")][InspectorName("Food/F_B")] F_B,
    [Description("F_C")][InspectorName("Food/F_C")] F_C,
    [Description("F_D")][InspectorName("Food/F_D")] F_D,

    //Tools
    [Description("Axe")][InspectorName("Tools/Axe")] Axe,
    [Description("Building Hammer")][InspectorName("Tools/Building Hammer")] BuildingHammer,
    [Description("T_C")][InspectorName("Tools/T_C")] T_C,

    //Weapons
    [Description("Weapons")][InspectorName("Weapons/Weapons")] Weapons,
    [Description("W_B")][InspectorName("Weapons/W_B")] W_B,

    //Other
    [Description("Chests")][InspectorName("Chests/Chests")] Chests

}
