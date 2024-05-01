using UnityEngine;

public class MouseMovement : Singleton<MouseMovement>
{
    public float mouseSensitivity = 90f;

    float xRotation = 0f;
    float YRotation = 0f;

    public Vector2 clampInDegrees = new Vector2(360, 160);


    //--------------------


    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        clampInDegrees = new Vector2(360, 160);
    }

    void Update()
    {
        if (PauseGameManager.Instance.GetPause()) { return; }

        Movement();
    }


    //--------------------


    void Movement()
    {
        if (MainManager.Instance.menuStates == MenuStates.None)
        {
            //Get Mouse Axis
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.smoothDeltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.smoothDeltaTime;

            //Control rotation around x axis
            xRotation -= mouseY;

            //Clamp the rotation
            xRotation = Mathf.Clamp(xRotation, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

            //control rotation around y axis
            YRotation += mouseX;

            //applying both rotations
            MainManager.Instance.playerBody.transform.localRotation = Quaternion.Euler(0f, YRotation, 0f);
            MainManager.Instance.mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            MainManager.Instance.mainMainCamera.transform.localRotation = MainManager.Instance.mainCamera.transform.localRotation;
        }
    }
}