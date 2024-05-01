using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StartGhostScene : MonoBehaviour
{
    [SerializeField] GameObject ghost;

    bool startCutscene;
    bool endCutscene;

    GameObject ghostObject;
    GameObject ghostMovingObject;
    GameObject ghostCrystalObject;


    //--------------------


    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        if (startCutscene && !endCutscene)
        {
            // Calculate the direction to the target
            Vector3 directionToTarget = ghostMovingObject.transform.position - MainManager.Instance.mainCamera.transform.position;

            // Create a rotation towards the target
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Smoothly rotate towards the target
            //MainManager.Instance.mainCamera.transform.rotation = targetRotation;
            MainManager.Instance.mainCamera.transform.rotation = Quaternion.Slerp(MainManager.Instance.mainCamera.transform.rotation, targetRotation, 2 * Time.deltaTime);
            MainManager.Instance.mainMainCamera.transform.rotation = MainManager.Instance.mainCamera.transform.rotation;
        }
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        if (other.gameObject.CompareTag("Player") && !CutSceneManager.Instance.cutScenes.start_GhostCrystal_Scene)
        {
            PauseGameManager.Instance.PauseGame();

            ghostObject = Instantiate(ghost);
            ghostObject.transform.SetPositionAndRotation(new Vector3(-58.21f, 30.92f, 52.38f), Quaternion.Euler(0, 58, 0));
            ghostObject.SetActive(true);

            ghostMovingObject = FindChild(ghostObject.transform, "Spine1_Jnt").gameObject;

            startCutscene = true;

            StartCoroutine(EndCutscene(7.45f));
        }
    }
    IEnumerator EndCutscene(float time)
    {
        yield return new WaitForSeconds(time);

        ghostCrystalObject = FindChild(ghostObject.transform, "AriditeCrystal(Clone)").gameObject;

        if (ghostCrystalObject.GetComponent<InteractableObject>())
        {
            ghostCrystalObject.GetComponent<InteractableObject>().DestroyThisInteractableObject();
        }

        endCutscene = true;

        PauseGameManager.Instance.UnpauseGame();

        Destroy(ghostObject);

        CutSceneManager.Instance.StartGhostCrystal_Scene_isOver();
    }

    private Transform FindChild(Transform parent, string name)
    {
        //Check if the current parent matches the desired name
        if (parent.name == name)
        {
            return parent;
        }

        //Traverse through all child objects recursively
        foreach (Transform child in parent)
        {
            Transform result = FindChild(child, name);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
