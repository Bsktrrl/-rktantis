using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [SerializeField] MeshRenderer renderer;

    [SerializeField] float transparencyValue = new float();

    [SerializeField] Vector3 distancePos = new Vector3();
    [SerializeField] float distance;

    [SerializeField] float ratio = new float();


    //--------------------


    private void Start()
    {
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
        if (other.gameObject.CompareTag("InvisibleLight"))
        {
            distance = Vector3.Distance(transform.position, other.gameObject.transform.position);
            if (other.gameObject.GetComponent<CapsuleCollider>())
            {
                transparencyValue = (distance / other.gameObject.GetComponent<CapsuleCollider>().height) - 0.1f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleLight"))
        {
            distance = Vector3.Distance(transform.position, other.gameObject.transform.position);
            if (other.gameObject.GetComponent<CapsuleCollider>())
            {
                transparencyValue = (distance / other.gameObject.GetComponent<CapsuleCollider>().height) -0.1f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleLight"))
        {
            distance = 0;
            distancePos = Vector3.zero;
            transparencyValue = 1;
            ratio = 0;
        }
    }
}