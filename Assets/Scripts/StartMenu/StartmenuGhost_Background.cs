using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartmenuGhost_Background : MonoBehaviour
{
    bool movingRight;
    [SerializeField] float speed = 5;
    [SerializeField] float amplitude = 1;
    [SerializeField] float frequency = 1;

    Vector3 initialPosition;
    Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.position;

        initialRotation = Quaternion.Euler(0, 90, 0);
    }
    private void Update()
    {
        float newY = initialPosition.y + amplitude * Mathf.Sin(Time.time * frequency);

        #region Movememnt
        if (movingRight)
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x + (Time.deltaTime * speed), newY, transform.position.z), initialRotation);
        }
        else
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x - (Time.deltaTime * speed), newY, transform.position.z), initialRotation);
        }

        if (transform.position.x > -26.3f && movingRight)
        {
            movingRight = false;
            initialRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (transform.position.x < -41f && !movingRight)
        {
            movingRight = true;
            initialRotation = Quaternion.Euler(0, 90, 0);
        }
        #endregion
    }
}
