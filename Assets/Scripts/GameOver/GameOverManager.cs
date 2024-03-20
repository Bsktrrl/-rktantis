using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{



    //-------------------------


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveWorldItems();
            RemoveInventoryItems();
        }
    }


    //-------------------------


    void RemoveWorldItems()
    {
        //Remove all items from the world
        for (int i = WorldObjectManager.Instance.worldObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(WorldObjectManager.Instance.worldObjectList[i]);
        }

        //Clear the lists
        WorldObjectManager.Instance.worldObjectList.Clear();
        WorldObjectManager.Instance.worldObjectList_ToSave.Clear();
    }

    void RemoveInventoryItems()
    {

    }
}
