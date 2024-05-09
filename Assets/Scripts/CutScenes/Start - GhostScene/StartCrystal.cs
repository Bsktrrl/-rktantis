using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCrystal : MonoBehaviour
{
    bool isTurnedOn;

    [SerializeField] MeshRenderer mesh_LOD0;
    [SerializeField] MeshRenderer mesh_LOD1;
    [SerializeField] Light light;
    [SerializeField] SphereCollider collissionPoint;


    private void Start()
    {
        mesh_LOD0.enabled = false;
        mesh_LOD1.enabled = false;

        light.enabled = false;
        collissionPoint.enabled = false;
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        if (CutSceneManager.Instance.cutScenes.start_GhostCrystal_Scene && !isTurnedOn)
        {
            if (gameObject)
            {
                mesh_LOD0.enabled = true;
                mesh_LOD1.enabled = true;

                light.enabled = true;
                collissionPoint.enabled = true;

                isTurnedOn = true;
            }
        }
    }
}
