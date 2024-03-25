using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : Singleton<Arms>
{
    public Animator anim;

    public bool cannotHit;


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
                                    CanHit();
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
                                    CanHit();
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
                                    CanHit();
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
                                    CanHit();
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
                                    CanHit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
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
                                    CanHit();
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
                                    CanHit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selecedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }
                            }
                        }
                    }

                    //WaterContainer
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Cup
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Bottle
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Bucket)
                    {
                        EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                        CanHit();
                    }
                }
            }
        }

        //If nothing is in the Hand - Punch with the Hand
        else if (EquippmentManager.Instance.toolHolderParent.transform.childCount == 1)
        {
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
                        CanHit();
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
                        CanHit();
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


    //--------------------


    public void OpenTabletAnimation()
    {
        anim.SetBool("Tablet", true);
    }
    public void CloseTabletAnimation()
    {
        anim.SetBool("Tablet", false);
    }


    //--------------------


    void CanHit()
    {
        cannotHit = false;
    }
    void CannotHit()
    {
        SoundManager.Instance.Play_PickaxeUsage_CannotHit_Clip();

        cannotHit = true;
    }
}
