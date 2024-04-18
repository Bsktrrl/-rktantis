using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        PlayerButtonManager.isPressed_EquipmentDeactivate += StopUsingEquipments;

        anim = GetComponent<Animator>();
    }


    //--------------------


    void UseEquippedItem()
    {
        if (HotbarManager.Instance.selectedItem == Items.GhostCapturer)
        {
            if (HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>())
            {
                HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>().StartCapturing();
            }
        }
        else
        {
            anim.SetTrigger("Click");
        }
    }
    void StopUsingEquipments()
    {
        if (HotbarManager.Instance.selectedItem == Items.GhostCapturer)
        {
            if (HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>())
            {
                HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>().StopCapturing();
            }
        }
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
                    #region Pickaxe
                    //Hand
                    if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Flashlight
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.AríditeCrystal
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.None)
                    {
                        if (SelectionManager.Instance.selectedObject)
                        {
                            if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>())
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
                                else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.None)
                                {
                                    equippedItem.itemName = Items.None;
                                }

                                //Tungsten
                                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    CannotHit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    CannotHit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    CannotHit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    CannotHit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    CannotHit();
                                }
                            }
                        }
                    }

                    //Wood Pickaxe
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.WoodPickaxe)
                    {
                        if (SelectionManager.Instance.selectedObject)
                        {
                            if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>())
                            {
                                //Tungsten
                                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    CannotHit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    CannotHit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    CannotHit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    CannotHit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    CannotHit();
                                }
                            }
                        }
                    }

                    //Stone Pickaxe
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.StonePickaxe)
                    {
                        if (SelectionManager.Instance.selectedObject)
                        {
                            if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>())
                            {
                                //Tungsten
                                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    CannotHit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    CannotHit();
                                }
                            }
                        }
                    }

                    //Cryonite Pickaxe
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.CryonitePickaxe)
                    {
                        if (SelectionManager.Instance.selectedObject)
                        {
                            if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>())
                            {
                                //Tungsten
                                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Gold
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Stone
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Cryonite
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Viridian
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Magnetite
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Arídite Crystal
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                                {
                                    //Hit the Ore
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }
                            }
                        }
                    }
                    #endregion


                    #region Hammer
                    //Hammer
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.WoodBuildingHammer
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.StoneBuildingHammer
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.CryoniteBuildingHammer)
                    {
                        EquippedItem equippedItem = EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>();

                        //Hit the Tree
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    #endregion

                    #region Water Container
                    //WaterContainer
                    else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Cup
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Bottle
                        || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Bucket)
                    {
                        EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                        CanHit();
                    }
                    #endregion

                    #region Axe
                    if (SelectionManager.Instance.selectedMovableObjectToRemove)
                    {
                        //If a movableObject (BuildingObjects)
                        if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>())
                        {
                            EquippedItem equippedItem = EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>();

                            if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.WoodAxe)
                            {
                                equippedItem.itemName = Items.WoodAxe;
                            }
                            else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.StoneAxe)
                            {
                                equippedItem.itemName = Items.StoneAxe;
                            }
                            else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.CryoniteAxe)
                            {
                                equippedItem.itemName = Items.CryoniteAxe;
                            }

                            //Hit a movableObject
                            EquippmentManager.Instance.Hit(equippedItem);
                            CanHit();
                        }

                    }

                    else if (SelectionManager.Instance.selectedObject)
                    {
                        //If an InteractableObject (Trees)
                        if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>())
                        {
                            //Hand
                            if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.Flashlight
                                || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.AríditeCrystal
                                || EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.None)
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
                                else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.None)
                                {
                                    equippedItem.itemName = Items.None;
                                }

                                //Palm Tree
                                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Palm_Tree)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Tree 2
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTree)
                                {
                                    CannotHit();
                                }

                                //Tree 3
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTreeBush)
                                {
                                    CannotHit();
                                }

                                //Tree 4
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_4)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Tree 5
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_5)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Tree 6
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_6)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Tree 7
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_7)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Tree 8
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_8)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Tree 9
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_9)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.Hit(equippedItem);
                                    CanHit();
                                }

                                //Cactus
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cactus)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }
                            }

                            //Wood Axe
                            else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.WoodAxe)
                            {
                                //Palm Tree
                                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Palm_Tree)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 2
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTree)
                                {
                                    CannotHit();
                                }

                                //Tree 3
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTreeBush)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 4
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_4)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 5
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_5)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 6
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_6)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 7
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_7)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 8
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_8)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 9
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_9)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Cactus
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cactus)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }
                            }

                            //Stone Axe
                            else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.StoneAxe)
                            {
                                //Palm Tree
                                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Palm_Tree)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 2
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTree)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 3
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTreeBush)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 4
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_4)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 5
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_5)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 6
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_6)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 7
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_7)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 8
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_8)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 9
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_9)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Cactus
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cactus)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }
                            }

                            //Cryonite Axe
                            else if (EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().itemName == Items.CryoniteAxe)
                            {
                                //Palm Tree
                                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Palm_Tree)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 2
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTree)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 3
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTreeBush)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 4
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_4)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 5
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_5)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 6
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_6)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 7
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_7)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 8
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_8)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Tree 9
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_9)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }

                                //Cactus
                                else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cactus)
                                {
                                    //Hit the Tree
                                    EquippmentManager.Instance.toolHolderParent.transform.GetChild(1).gameObject.GetComponent<EquippedItem>().Hit();
                                    CanHit();
                                }
                            }

                        }
                    }
                    #endregion
                }
            }
        }

        //If nothing is in the Hand - Punch with the Hand
        else if (EquippmentManager.Instance.toolHolderParent.transform.childCount == 1)
        {
            if (SelectionManager.Instance.selectedObject)
            {
                if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>())
                {
                    EquippedItem equippedItem = new EquippedItem();

                    equippedItem.itemName = Items.None;

                    #region OreVeins
                    //Tungsten
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tungsten_Ore)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }

                    //Gold
                    else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Gold_Ore)
                    {
                        CannotHit();
                    }

                    //Stone
                    else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Stone_Ore)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }

                    //Cryonite
                    else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cryonite_Ore)
                    {
                        CannotHit();
                    }

                    //Viridian
                    else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Viridian_Ore)
                    {
                        CannotHit();
                    }

                    //Magnetite
                    else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Magnetite_Ore)
                    {
                        CannotHit();
                    }

                    //Arídite Crystal
                    else if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.AríditeCrystal_Ore)
                    {
                        CannotHit();
                    }
                    #endregion

                    #region Trees
                    //Palm Tree
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Palm_Tree)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    //Tree_2
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTree)
                    {
                        CannotHit();
                    }
                    //Tree_3
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.BloodTreeBush)
                    {
                        CannotHit();
                    }
                    //Tree_4
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_4)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    //Tree_5
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_5)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    //Tree_6
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_6)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    //Tree_7
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_7)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    //Tree_8
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_8)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    //Tree_9
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Tree_9)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    //Cactus
                    if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cactus)
                    {
                        //Hit the Ore
                        EquippmentManager.Instance.Hit(equippedItem);
                        CanHit();
                    }
                    #endregion
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
        print("44444. Cannot Hit");

        SoundManager.Instance.Play_PickaxeUsage_CannotHit_Clip();

        cannotHit = true;
    }
}
