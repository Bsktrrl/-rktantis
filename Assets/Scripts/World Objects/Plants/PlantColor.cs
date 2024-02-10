using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantColor : MonoBehaviour
{
    [Header("Plant Color")]
    [SerializeField] List<GameObject> ColorMeshObject = new List<GameObject>();
    [SerializeField] List<Material> plantColors = new List<Material>();


    //--------------------


    private void Start()
    {
        //Set Color of Flower
        #region
        for (int i = 0; i < ColorMeshObject.Count; i++)
        {
            if (ColorMeshObject[i].GetComponent<MeshRenderer>())
            {
                ColorMeshObject[i].GetComponent<MeshRenderer>().material = GetRandomPlantColorMaterial();
            }
        }
        #endregion
    }


    //--------------------


    public Material GetRandomPlantColorMaterial()
    {
        int index = Random.Range(0, plantColors.Count);

        return plantColors[index];
    }
}
