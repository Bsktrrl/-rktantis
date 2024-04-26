using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    Animator anim;

    public GameObject parent;

    int check = 0;

    bool isInTransition;


    //--------------------


    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isInTransition)
            {
                isInTransition = true;

                if (anim)
                {
                    GetComponent<DoorCounter>().doorCounter++;

                    if (Vector3.Dot(Camera.main.transform.forward, transform.forward) >= 0)
                    {
                        anim.SetBool("Direction", true);
                    }
                    else
                    {
                        anim.SetBool("Direction", false);
                    }

                    anim.SetTrigger("Interact");


                    if (GetComponent<DoorCounter>().doorCounter % 2 == 0)
                    {
                        PlayDoorCloseSound(GetComponent<AudioSource>(), anim);

                        GetComponent<MeshCollider>().isTrigger = false;
                    }
                    else
                    {
                        PlayDoorOpenSound(GetComponent<AudioSource>(), anim);

                        GetComponent<MeshCollider>().isTrigger = true;
                    }
                }
                else
                {
                    parent.GetComponent<DoorCounter>().doorCounter++;

                    if (Vector3.Dot(Camera.main.transform.forward, transform.forward) >= 0)
                    {
                        parent.GetComponent<Animator>().SetBool("Direction", true);
                    }
                    else
                    {
                        parent.GetComponent<Animator>().SetBool("Direction", false);
                    }

                    parent.GetComponent<Animator>().SetTrigger("Interact");

                    if (parent.GetComponent<DoorCounter>().doorCounter % 2 == 0)
                    {
                        PlayDoorCloseSound(parent.GetComponent<AudioSource>(), null);

                        GetComponent<MeshCollider>().isTrigger = false;
                    }
                    else
                    {
                        PlayDoorOpenSound(parent.GetComponent<AudioSource>(), null);

                        GetComponent<MeshCollider>().isTrigger = true;
                    }
                }

                StartCoroutine(Transitiontime(0.35f));
            }
        }
    }

    void PlayDoorOpenSound(AudioSource source, Animator anim)
    {
        source.volume = SoundManager.Instance.sound_World;
        
        if (anim == null)
        {
            if (parent.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Wood)
            {
                SoundManager.Instance.Play_Building_WoodDoor_Open_Clip(source);
            }
            else if (parent.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Stone)
            {
                SoundManager.Instance.Play_Building_StoneDoor_Open_Clip(source);
            }
            else if (parent.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Cryonite)
            {
                SoundManager.Instance.Play_Building_CryoniteDoor_Open_Clip(source);
            }
        }
        else
        {
            if (GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Wood)
            {
                SoundManager.Instance.Play_Building_WoodDoor_Open_Clip(source);
            }
            else if (GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Stone)
            {
                SoundManager.Instance.Play_Building_StoneDoor_Open_Clip(source);
            }
            else if (GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Cryonite)
            {
                SoundManager.Instance.Play_Building_CryoniteDoor_Open_Clip(source);
            }
        }
    }
    void PlayDoorCloseSound(AudioSource source, Animator anim)
    {
        source.volume = SoundManager.Instance.sound_World;

        if (anim == null)
        {
            if (parent.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Wood)
            {
                SoundManager.Instance.Play_Building_WoodDoor_Close_Clip(source);
            }
            else if (parent.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Stone)
            {
                SoundManager.Instance.Play_Building_StoneDoor_Close_Clip(source);
            }
            else if (parent.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Cryonite)
            {
                SoundManager.Instance.Play_Building_CryoniteDoor_Close_Clip(source);
            }
        }
        else
        {
            if (GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Wood)
            {
                SoundManager.Instance.Play_Building_WoodDoor_Close_Clip(source);
            }
            else if (GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Stone)
            {
                SoundManager.Instance.Play_Building_StoneDoor_Close_Clip(source);
            }
            else if (GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Cryonite)
            {
                SoundManager.Instance.Play_Building_CryoniteDoor_Close_Clip(source);
            }
        }
    }

    IEnumerator Transitiontime(float time)
    {
        yield return new WaitForSeconds(time);

        isInTransition = false;
    }
}
