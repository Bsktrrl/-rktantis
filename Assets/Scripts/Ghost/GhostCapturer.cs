using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCapturer : MonoBehaviour
{
    public Animator anim;

    [Header("General")]
    public bool isActive;

    [Header("Slots")]
    public List<GameObject> slotObjectList = new List<GameObject>();
    public List<Material> materialSlotList = new List<Material>();

    [Header("invisibleObjectCollider")]
    [SerializeField] GameObject invisibleObjectCollider;
    string defaultLayer = "Default";
    string invisibleLightLayer = "InvisibleLight";

    [Header("Lefs")]
    [SerializeField] GameObject leaf1;
    [SerializeField] GameObject leaf2;

    [Header("Snapping_Raycast")]
    public LayerMask ghostLayerMask;
    Ray ray;
    RaycastHit hit;


    //--------------------


    void Start()
    {
        //anim = GetComponent<Animator>();

        anim.SetBool("Capturing", false);

        StopCapturing();

        UpdateGhostCapturer();
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        if (isActive)
        {
            RotateLeafBlades();

            RaycastGhosts();
        }
    }


    //--------------------


    public void UpdateGhostCapturer()
    {
        //Reset ActiveSlots
        for (int i = 0; i < slotObjectList.Count; i++)
        {
            slotObjectList[i].SetActive(false);
        }

        //Set Active Slots and fill with capturedInfo
        #region
        if (GhostManager.Instance.ghostCapturerStats.slotsActivated > 0)
            SetCapturerInfo(0, "Slot1");
        if (GhostManager.Instance.ghostCapturerStats.slotsActivated > 1)
            SetCapturerInfo(1, "Slot2");
        if (GhostManager.Instance.ghostCapturerStats.slotsActivated > 2)
            SetCapturerInfo(2, "Slot3");
        if (GhostManager.Instance.ghostCapturerStats.slotsActivated > 3)
            SetCapturerInfo(3, "Slot4");
        if (GhostManager.Instance.ghostCapturerStats.slotsActivated > 4)
            SetCapturerInfo(4, "Slot5");
        if (GhostManager.Instance.ghostCapturerStats.slotsActivated > 5)
            SetCapturerInfo(5, "Slot6");
        if (GhostManager.Instance.ghostCapturerStats.slotsActivated > 6)
            SetCapturerInfo(6, "Slot7");
        if (GhostManager.Instance.ghostCapturerStats.slotsActivated > 7)
            SetCapturerInfo(7, "Slot8");
        #endregion
    }
    void SetCapturerInfo(int index, string slotName)
    {
        //Set if filled
        if (GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[index].isTaken)
        {
            anim.SetBool(slotName, true);
        }
        else
        {
            anim.SetBool(slotName, false);
        }

        //Set Filled Material
        if (gameObject.GetComponentInChildren<SkinnedMeshRenderer>())
        {
            if (anim.GetBool(slotName))
            {
                switch (GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[index].ghostElement)
                {
                    case GhostElement.None:
                        materialSlotList[index] = GhostManager.Instance.material_Empty;
                        break;

                    case GhostElement.Water:
                        materialSlotList[index] = GhostManager.Instance.material_Water;
                        break;
                    case GhostElement.Fire:
                        break;
                    case GhostElement.Stone:
                        break;
                    case GhostElement.Wind:
                        break;
                    case GhostElement.Poison:
                        break;
                    case GhostElement.Power:
                        break;

                    default:
                        materialSlotList[index] = GhostManager.Instance.material_Empty;
                        break;
                }
            }
        }
        
        //Set Slot active
        anim.SetInteger("SlotsAmount", index + 1);
        slotObjectList[index].SetActive(true);
    }


    //--------------------


    public void StartCapturing()
    {
        print("1. StartCapturing");
        for (int i = 0; i < GhostManager.Instance.ghostCapturerStats.slotsActivated; i++)
        {
            print("2. StartCapturing");
            if (!GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[i].isTaken)
            {
                print("3. StartCapturing");
                //invisibleObjectCollider.layer = LayerMask.NameToLayer(invisibleLightLayer);
                isActive = true;

                SoundManager.Instance.Play_Ghost_TargetGhost_Clip();

                return;
            }
        }
    }
    public void StopCapturing()
    {
        //invisibleObjectCollider.layer = LayerMask.NameToLayer(defaultLayer);

        SoundManager.Instance.Stop_Ghost_TargetGhost_Clip();

        GhostManager.Instance.hasTarget = false;
        isActive = false;
    }

    //--------------------


    void RaycastGhosts()
    {
        if (isActive)
        {
            ray = MainManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, PlayerManager.Instance.InteractableDistance * (1 + (PerkManager.Instance.perkValues.playerRange_Increase_Percentage / 100)), ghostLayerMask))
            {
                if (hit.collider.gameObject.GetComponent<Ghost>() && hit.collider.gameObject.GetComponent<InvisibleObject>())
                {
                    if (hit.collider.gameObject.GetComponent<InvisibleObject>().transparencyValue < 1)
                    {
                        GhostManager.Instance.targetGhostObject = hit.collider.gameObject;

                        GhostManager.Instance.hasTarget = true;
                    }
                    else
                    {
                        StopRaycastingGhost();
                    }
                }
                else
                {
                    StopRaycastingGhost();
                }
            }
            else
            {
                StopRaycastingGhost();
            }
        }
        else
        {
            if (GhostManager.Instance.targetGhostObject)
            {
                StopRaycastingGhost();
            }
        }
    }
    void StopRaycastingGhost()
    {
        if (GhostManager.Instance.targetGhostObject)
        {
            if (GhostManager.Instance.targetGhostObject.GetComponent<Ghost>())
            {
                GhostManager.Instance.hasTarget = false;

                if (GhostManager.Instance.targetGhostObject.GetComponent<Ghost>().capturedRate <= 0)
                {
                    GhostManager.Instance.targetGhostObject = null;
                }
            }
        }
    }


    //--------------------


    void RotateLeafBlades()
    {
        if (isActive)
        {
            // Calculate the rotation for the clockwise cube
            Quaternion clockwiseRotation = Quaternion.Euler(GhostManager.Instance.leafRotationSpeed * Time.deltaTime, 0f, 0f);

            // Apply rotation to the clockwise cube
            leaf1.transform.SetLocalPositionAndRotation(leaf1.transform.localPosition, leaf1.transform.localRotation * clockwiseRotation);

            // Calculate the rotation for the counter-clockwise cube
            Quaternion counterClockwiseRotation = Quaternion.Euler(-GhostManager.Instance.leafRotationSpeed * Time.deltaTime, 0f, 0f);

            // Apply rotation to the counter-clockwise cube
            leaf2.transform.SetLocalPositionAndRotation(leaf2.transform.localPosition, leaf2.transform.localRotation * counterClockwiseRotation);
        }
    }
}
