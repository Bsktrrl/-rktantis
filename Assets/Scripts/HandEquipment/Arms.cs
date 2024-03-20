using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : Singleton<Arms>
{
    public Animator anim;


    //--------------------


    void Start()
    {
        PlayerButtonManager.refillBottle_isPressed += FillWater;
        PlayerButtonManager.isPressed_EquipmentActivate += UseEquippedItem;

        anim = GetComponent<Animator>();
    }


    //--------------------


    void UseEquippedItem()
    {
        anim.SetTrigger("Click");
    }
    void FillWater()
    {
        if (EquippmentManager.Instance.toolHolderParent.transform.childCount > 1)
        {
            if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1))
            {
                if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>())
                {
                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().FillDrink();
                }
            }
        }
    }


    //--------------------


    //Animation event
    public void InteractionFrame()
    {
        //Check if the Pickaxe is good enough for the ore to mine
        if (EquippmentManager.Instance.toolHolderParent.transform.childCount > 1)
        {
            //If item is in the Hand
            if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1))
            {
                if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>())
                {
                    //Cryonite Pickaxe
                    if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Flashlight
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.AríditeCrystal)
                    {
                        if (SelectionManager.Instance.selecedObject)
                        {
                            if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
                            {
                                EquippedItem equippedItem = new EquippedItem();

                                if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Flashlight)
                                {
                                    equippedItem.itemName = Items.Flashlight;
                                }
                                else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.AríditeCrystal)
                                {
                                    equippedItem.itemName = Items.AríditeCrystal;
                                }

                                //Tungsten
                                if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                                {
                                    
                                    //Hit the Ore
                                    EquippmentManager.Instance.Hit(equippedItem);
                                }

                                //Gold
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    CannotHit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.Hit(equippedItem);
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    CannotHit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    CannotHit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    CannotHit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    CannotHit();
                                }
                            }
                        }
                    }

                    //Wood Pickaxe
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.WoodPickaxe)
                    {
                        if (SelectionManager.Instance.selecedObject)
                        {
                            if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
                            {
                                //Tungsten
                                if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    CannotHit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    CannotHit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    CannotHit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    CannotHit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    CannotHit();
                                }
                            }
                        }
                    }

                    //Stone Pickaxe
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.StonePickaxe)
                    {
                        if (SelectionManager.Instance.selecedObject)
                        {
                            if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
                            {
                                //Tungsten
                                if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    CannotHit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    CannotHit();
                                }
                            }
                        }
                    }

                    //Cryonite Pickaxe
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.CryonitePickaxe)
                    {
                        if (SelectionManager.Instance.selecedObject)
                        {
                            if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
                            {
                                //Tungsten
                                if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                }
                            }
                        }
                    }
                }
            }
        }

        //If nothing is in the Hand - Punch with the Hand
        else if (EquippmentManager.Instance.toolHolderParent.transform.childCount == 1)
        {
            print("1. Hand Hit");

            if (SelectionManager.Instance.selecedObject)
            {
                if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>())
                {
                    EquippedItem equippedItem = new EquippedItem();

                    equippedItem.itemName = Items.None;

                    //Tungsten
                    if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                    {

                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                    }

                    //Gold
                    else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                    {
                        CannotHit();
                    }

                    //Stone
                    else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                    }

                    //Cryonite
                    else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                    {
                        CannotHit();
                    }

                    //Viridian
                    else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                    {
                        CannotHit();
                    }

                    //Magnetite
                    else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                    {
                        CannotHit();
                    }

                    //Arídite Crystal
                    else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                    {
                        CannotHit();
                    }
                }
            }
        }
    }

    void CannotHit()
    {
        SoundManager.Instance.Play_PickaxeUsage_CannotHit_Clip();
    }
}
