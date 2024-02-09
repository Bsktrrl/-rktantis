using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Outliner : MonoBehaviour
{
    [SerializeField] MeshRenderer object_MeshRenderer;

    Material selectedOutlinerMaterial;


    //--------------------


    private void Start()
    {
        for (int i = 0; i < object_MeshRenderer.materials.Length; i++)
        {
            if (object_MeshRenderer.materials[i].name == OutlinerManager.Instance.outliner_Material.name + " (Instance)")
            {
                selectedOutlinerMaterial = object_MeshRenderer.materials[i];
                selectedOutlinerMaterial.color = OutlinerManager.Instance.outlinerColor;

                break;
            }
        }

        HideOutliner();
    }


    //--------------------


    public void ShowOutliner()
    {
        if (selectedOutlinerMaterial)
        {
            if (selectedOutlinerMaterial.GetFloat("_Alpha") == 1)
            {
                return;
            }

            selectedOutlinerMaterial.SetFloat("_Alpha", 1);
        }
    }
    public void HideOutliner()
    {
        if (selectedOutlinerMaterial)
        {
            if (selectedOutlinerMaterial.GetFloat("_Alpha") == 0)
            {
                return;
            }

            selectedOutlinerMaterial.SetFloat("_Alpha", 0);
        }
    }
}
