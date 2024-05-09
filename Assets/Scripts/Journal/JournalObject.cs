using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalObject : MonoBehaviour
{
    [Header("Save/Load Object")]
    public bool isPicked;

    public int journalPageIndex_x;
    public int journalPageIndex_y;

    [SerializeField] GameObject campFlag;


    //--------------------


    private void Start()
    {
        if (campFlag)
        {
            campFlag.SetActive(true);
        }
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        if (campFlag && isPicked)
        {
            campFlag.SetActive(false);
        }
    }


    //--------------------


    public void JournalPageInteraction()
    {
        if (isPicked) { return; }

        JournalManager.Instance.ChangeJournalPageInfo(true, journalPageIndex_x, journalPageIndex_y, gameObject.transform.position);
    }
    public void LoadJournalPage(bool _isPicked, int _journalPageIndex_j, int _journalPageIndex_l)
    {
        //print("Load JournalPage: [" + _journalPageIndex_j + "][" + _journalPageIndex_l + "]: " + _isPicked);

        //Set Parameters
        isPicked = _isPicked;
        journalPageIndex_x = _journalPageIndex_j;
        journalPageIndex_y = _journalPageIndex_l;

        //Check if Animation and pickablePart should be hidden
        if (isPicked)
        {
            gameObject.SetActive(false);
        }
    }
}
