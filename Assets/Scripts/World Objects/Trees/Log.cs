using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    [SerializeField] TreeParent treeparent;
    [SerializeField] float secondsWithGravity = 1;


    //--------------------


    public void ActivateLogGravity()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine("RemoveGravity");
    }

    IEnumerator RemoveGravity()
    {
        //Wait for x seconds
        yield return new WaitForSeconds(secondsWithGravity);

        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
