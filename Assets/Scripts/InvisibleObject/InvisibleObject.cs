using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [SerializeField] MeshRenderer renderer;
    //[SerializeField] SphereCollider sphereCollider;

    [SerializeField] float transparencyValue = new float();
    [SerializeField] float distance;
    SphereCollider sphereCollider;


    //--------------------


    private void Start()
    {
        //Add a SphereCollider to thismeObject
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = 5f;
        sphereCollider.isTrigger = true;

        distance = 0;
        transparencyValue = 1;
    }
    private void Update()
    {
        renderer.material.SetFloat("_Transparency", transparencyValue);
    }

    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleTriggerPoint"))
        {
            print("OnTriggerEnter");

            distance = Vector3.Distance(transform.position, other.gameObject.transform.position);

            transparencyValue = (distance / sphereCollider.radius);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleTriggerPoint"))
        {
            print("OnTriggerStay");
            distance = Vector3.Distance(transform.position, other.gameObject.transform.position);

            transparencyValue = (distance / sphereCollider.radius);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleTriggerPoint"))
        {
            print("OnTriggerExit");
            distance = 0;
            transparencyValue = 1;
        }
    }
}