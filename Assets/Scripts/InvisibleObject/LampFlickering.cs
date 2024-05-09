using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFlickering : MonoBehaviour
{
    [SerializeField] GameObject collisionPoint;
    [SerializeField] GameObject lamp;
    [SerializeField] Material lampMaterial;
    [SerializeField] Color lampColor_On;
    [SerializeField] Color lampColor_Off;

    public MaterialPropertyBlock propertyBlock;
    public List<Renderer> rendererList = new List<Renderer>();

    float turnOff_Timer;
    float turnOn_Timer;


    //------------------------



    private void Start()
    {
        turnOff_Timer = Random.Range(0.1f, 3f);
        turnOn_Timer = Random.Range(0.01f, 1f);

        propertyBlock = new MaterialPropertyBlock();
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        turnOff_Timer -= Time.deltaTime;
        turnOn_Timer -= Time.deltaTime;

        if (collisionPoint.activeInHierarchy && lamp.activeInHierarchy)
        {
            if (turnOff_Timer <= 0)
            {
                TurnOff();
            }
        }
        else
        {
            if (turnOn_Timer <= 0)
            {
                TurnOn();
            }
        }
    }

    void TurnOff()
    {
        collisionPoint.SetActive(false);
        //lamp.SetActive(false);

        lamp.GetComponent<Light>().intensity = 1f /*new Color(lampColor_On.r, lampColor_On.g, lampColor_On.b, 50)*/;

        //lampMaterial.SetColor("_AlbedoColor", lampColor_Off);

        propertyBlock.SetColor("_AlbedoColor", lampColor_Off);

        for (int i = 0; i < rendererList.Count; i++)
        {
            rendererList[i].SetPropertyBlock(propertyBlock);
        }

        turnOn_Timer = Random.Range(0.01f, 0.2f);
    }
    void TurnOn()
    {
        collisionPoint.SetActive(true);
        //lamp.SetActive(true);

        lamp.GetComponent<Light>().intensity = 5 /*new Color(lampColor_On.r, lampColor_On.g, lampColor_On.b, 255)*/;

        //lampMaterial.SetColor("_AlbedoColor", lampColor_On);

        propertyBlock.SetColor("_AlbedoColor", lampColor_On);

        for (int i = 0; i < rendererList.Count; i++)
        {
            rendererList[i].SetPropertyBlock(propertyBlock);
        }

        turnOff_Timer = Random.Range(0.1f, 3f);
    }
}
