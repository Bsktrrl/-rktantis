using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger involves the second object
        if (other.gameObject.CompareTag("Water"))
        {
            print("Is In Water");
        }
        else
        {
            print("Is NOT In Water");
        }
    }

    //public List<GameObject> waterObjects; // Reference to the second object

    //bool isCollidingWithWater;


    //--------------------


    //void Update()
    //{
    //    // Check for collision between the first object and the second object
    //    if (CheckCollision())
    //    {
    //        print("Is touching Water");

    //        WeatherManager.Instance.waterValue = 20;

    //        PlayerMovement.Instance.movementSpeedVarianceByWater = 0.2f;
    //    }
    //    else
    //    {
    //        print("Is NOT touching Water");
    //        WeatherManager.Instance.waterValue = 0;

    //        PlayerMovement.Instance.movementSpeedVarianceByWater = 1f;
    //    }
    //}


    //--------------------


    //bool CheckCollision()
    //{
    //    // Get the position, height, and radius of the first object (assuming it has a CapsuleCollider)
    //    CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
    //    Vector3 capsulePosition = transform.TransformPoint(capsuleCollider.center);
    //    float capsuleHeight = capsuleCollider.height;
    //    float capsuleRadius = capsuleCollider.radius;

    //    isCollidingWithWater = false;

    //    for (int i = 0; i < waterObjects.Count; i++)
    //    {
    //        // Get the size of the second object (assuming it has a BoxCollider)
    //        Vector3 boxSize = waterObjects[i].GetComponent<BoxCollider>().size;

    //        // Check for collision using Physics.CheckCapsule and Physics.CheckBox
    //        isCollidingWithWater = Physics.CheckCapsule(capsulePosition - Vector3.up * capsuleHeight / 2, capsulePosition + Vector3.up * capsuleHeight / 2, capsuleRadius) &&
    //               Physics.CheckBox(waterObjects[i].transform.position, boxSize / 2);

    //        //// Get the size of the second object (assuming it has a BoxCollider)
    //        //Vector3 boxSize = waterObjects[i].GetComponent<BoxCollider>().size;

    //        //isCollidingWithWater = Physics.CheckCapsule(capsulePosition - Vector3.up * capsuleHeight / 2, capsulePosition + Vector3.up * capsuleHeight / 2, capsuleRadius)
    //        //       && Physics.CheckBox(waterObjects[i].transform.position, boxSize / 2);

    //        if (isCollidingWithWater)
    //        {
    //            break;
    //        }
    //    }

    //    return isCollidingWithWater;


    //    //// Get the position and radius of the first object (assuming it has a SphereCollider)
    //    //Vector3 spherePosition = transform.position;
    //    //float sphereRadius = GetComponent<CapsuleCollider>().radius;

    //    //// Get the size of the second object (assuming it has a BoxCollider)
    //    //for (int i = 0; i < waterObjects.Count; i++)
    //    //{
    //    //    Vector3 boxSize = waterObjects[i].GetComponent<BoxCollider>().size;

    //    //    // Check for collision using Physics.CheckSphere and Physics.CheckBox
    //    //    return Physics.CheckCapsule(spherePosition, sphereRadius) && Physics.CheckBox(waterObjects[i].transform.position, boxSize / 2);
    //    //}

    //    //return false;
    //}
}
