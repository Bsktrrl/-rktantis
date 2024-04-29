using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Entering Water");

            PlayerMovement.Instance.movementSpeedVarianceByWater = 0.5f;
            WeatherManager.Instance.waterValue = 20;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Exiting Water");

            PlayerMovement.Instance.movementSpeedVarianceByWater = 1f;
            WeatherManager.Instance.waterValue = 0;
        }
    }
}
