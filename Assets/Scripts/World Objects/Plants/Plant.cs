using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject pickablePart;

    [SerializeField] PlantType plantType;

    public bool isPicked;
    [HideInInspector] public float growthTimer;
    [SerializeField] float growthPrecentage;

    [HideInInspector] public int plantIndex;

    float growthAnimationTime = 0.29f;

    Animator animator;

    [HideInInspector] public int precentageCheck = 0;


    //--------------------


    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isPicked)
        {
            growthTimer += Time.deltaTime;

            growthPrecentage = growthTimer / PlantManager.Instance.growthTimer * 100;

            #region Precentage Timer
            if (growthPrecentage >= 95 && precentageCheck < 19)
            {
                precentageCheck = 19;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 90 && precentageCheck < 18)
            {
                precentageCheck = 18;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 85 && precentageCheck < 17)
            {
                precentageCheck = 17;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 80 && precentageCheck < 16)
            {
                precentageCheck = 16;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 75 && precentageCheck < 15)
            {
                precentageCheck = 15;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 70 && precentageCheck < 14)
            {
                precentageCheck = 14;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 65 && precentageCheck < 13)
            {
                precentageCheck = 13;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 60 && precentageCheck < 12)
            {
                precentageCheck = 12;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 55 && precentageCheck < 11)
            {
                precentageCheck = 11;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 50 && precentageCheck < 10)
            {
                precentageCheck = 10;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 45 && precentageCheck < 9)
            {
                precentageCheck = 9;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 40 && precentageCheck < 8)
            {
                precentageCheck = 8;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 35 && precentageCheck < 7)
            {
                precentageCheck = 7;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 30 && precentageCheck < 6)
            {
                precentageCheck = 6;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 25 && precentageCheck < 5)
            {
                precentageCheck = 5;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 20 && precentageCheck < 4)
            {
                precentageCheck = 4;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 15 && precentageCheck < 3)
            {
                precentageCheck = 3;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 10 && precentageCheck < 2)
            {
                precentageCheck = 2;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            else if (growthPrecentage >= 5 && precentageCheck < 1)
            {
                precentageCheck = 1;
                PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
            }
            #endregion

            if (growthTimer >= PlantManager.Instance.growthTimer)
            {
                PlantGrown();
            }
        }
    }


    //--------------------


    public void LoadPlant(bool _isPicked, float _growthTimer, int _plantIndex, int _precentageCheck)
    {
        //Set Parameters
        isPicked = _isPicked;
        plantIndex = _plantIndex;
        precentageCheck = _precentageCheck;
        growthTimer = _growthTimer;

        //Check if Animation and pickablePart should be hidden
        if (isPicked)
        {
            //Hide Mesh
            pickablePart.SetActive(false);

            //Set active animation
            if (animator)
            {
                animator.SetBool("Picked", true);
            }
        }
    }


    //--------------------


    public void PickPlant()
    {
        //Set picked animation
        animator.SetBool("Picked", true);

        //Hide Mesh
        pickablePart.SetActive(false);

        //Start growing process
        growthTimer = 0;
        isPicked = true;

        PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
    }

    void PlantGrown()
    {
        //Stop growing process
        isPicked = false;

        //Set Precentage to 100%
        growthPrecentage = 100;

        //Set active animation
        animator.SetBool("Picked", false);

        //Show Mesh after a set amount of time 
        StartCoroutine(GrowBackCoroutine(growthAnimationTime));

        precentageCheck = 0;
        growthTimer = 0;

        PlantManager.Instance.ChangePlantInfo(isPicked, growthTimer, plantIndex, precentageCheck, gameObject.transform.position);
    }
    IEnumerator GrowBackCoroutine(float time)
    {
        //Wait for time seconds
        yield return new WaitForSeconds(time);

        //Show Mesh
        pickablePart.SetActive(true);
    }
}