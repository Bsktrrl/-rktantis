using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArideaGateTest : MonoBehaviour
{
    Animator anim;

    [SerializeField] GameObject ArídianKey_1;


    //--------------------


    void Start()
    {
        anim = GetComponent<Animator>();

        ArídianKey_1.SetActive(false);
    }


    //--------------------


    public void ActivateArídeaGate()
    {
        if (InventoryManager.Instance.GetAmountOfItemInInventory(0, Items.ArídianKey) > 0)
        {
            ArídianKey_1.SetActive(true);
            SoundManager.Instance.Play_ArídeaGate_KeyPlacement_Clip();

            StartCoroutine(WaitForGateToTurn(1));
        }
    }

    IEnumerator WaitForGateToTurn(float time)
    {
        yield return new WaitForSeconds(time);

        SoundManager.Instance.Play_ArídeaGate_Rotate_Clip();
        anim.SetTrigger("InsertKey");

        yield return new WaitForSeconds(5f);

        SoundManager.Instance.Play_ArídeaGate_InPlace_Clip();

        yield return new WaitForSeconds(2.5f);

        MainManager.Instance.SetDemoEndingText();
    }
}
