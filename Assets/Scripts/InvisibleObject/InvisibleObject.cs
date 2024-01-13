using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [SerializeField] MeshRenderer renderer;

    [SerializeField] float transparencyValue = new float();

    [SerializeField] Vector3 distancePos = new Vector3();

    [SerializeField] float ratio = new float();


    //--------------------


    private void Start()
    {
        transparencyValue = 100;
    }
    private void Update()
    {
        renderer.sharedMaterial.SetFloat("_Transparency", transparencyValue);
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleLight"))
        {
            distancePos = transform.position - other.gameObject.GetComponent<InvisibleLight>().spherePos;

            ratio = (other.transform.position - other.gameObject.GetComponent<InvisibleLight>().spherePos).magnitude;

            transparencyValue = distancePos.magnitude / ratio;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleLight"))
        {
            distancePos = transform.position - other.gameObject.GetComponent<InvisibleLight>().spherePos;

            transparencyValue = distancePos.magnitude / ratio;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleLight"))
        {
            transparencyValue = 100;
        }
    }
}