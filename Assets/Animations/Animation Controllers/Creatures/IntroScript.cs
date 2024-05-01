using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    public GameObject Crystal;
    public GameObject Crystal1;
    public GameObject SpawnCrystal;
    public GameObject CrystalBone;


    //--------------------


    public void StartMovement()
    {
        Crystal.SetActive(true);
        Crystal1.SetActive(false);
    }

    void CrystalSwap ()
    {
        Crystal.SetActive(false);
        Crystal1.SetActive(true);
    }
    void CrystalSpawn()
    {
        Crystal1.SetActive(false);
        Instantiate(SpawnCrystal, CrystalBone.transform);
    }
}
