using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArideaGateTest : MonoBehaviour
{
    Animator anim;

    [SerializeField] GameObject Ar�dianKey_1;


    //--------------------


    void Start()
    {
        anim = GetComponent<Animator>();

        Ar�dianKey_1.SetActive(false);
    }


    //--------------------


    public void ActivateAr�deaGate()
    {
        if (InventoryManager.Instance.GetAmountOfItemInInventory(0, Items.Ar�dianKey) > 0)
        {
            Ar�dianKey_1.SetActive(true);
            SoundManager.Instance.Play_Ar�deaGate_KeyPlacement_Clip();

            StartCoroutine(WaitForGateToTurn(1));
        }
    }

    IEnumerator WaitForGateToTurn(float time)
    {
        yield return new WaitForSeconds(time);

        SoundManager.Instance.Play_Ar�deaGate_Rotate_Clip();
        anim.SetTrigger("InsertKey");

        yield return new WaitForSeconds(5f);

        SoundManager.Instance.Play_Ar�deaGate_InPlace_Clip();

        yield return new WaitForSeconds(2.5f);

        MainManager.Instance.SetDemoEndingText();
    }
}
