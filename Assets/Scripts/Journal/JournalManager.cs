using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : Singleton<JournalManager>
{
    public JournalMenuState journalMenuState;
    int activeJournalPageIndex;

    [Header("Scriptable Objects")]
    public JournalPage_SO mentorJournalPage_SO;
    public JournalPage_SO playerJournalPage_SO;
    public JournalPage_SO personalJournalPage_SO;

    [Header("Journal Menus")]
    public GameObject journal_Content_Menu;
    public GameObject journal_MentorStory_Menu;
    public GameObject journal_PlayerStory_Menu;
    public GameObject journal_PersonalStory_Menu;


    [Header("Journal Prefab")]
    [SerializeField] GameObject journal_Prefab;

    [Header("Info Page")]
    [SerializeField] GameObject InfoPage_Parent;
    [SerializeField] GameObject InfoPage_Content;
    [SerializeField] TextMeshProUGUI title_Text;
    [SerializeField] TextMeshProUGUI autorName_Text;
    [SerializeField] TextMeshProUGUI destination_Text;
    [SerializeField] TextMeshProUGUI message_Text;
    [SerializeField] TextMeshProUGUI date_Text;
    [SerializeField] TextMeshProUGUI time_Text;
    [SerializeField] GameObject message_Clip_Button;
    [SerializeField] AudioClip message_Clip;

    [Header("lists")]
    [SerializeField] List<GameObject> mentorJournalPageList = new List<GameObject>();
    [SerializeField] List<GameObject> playerJournalPageList = new List<GameObject>();
    [SerializeField] List<GameObject> personalJournalPageList = new List<GameObject>();
    [Space(10)]
    public List<JournalPageInfo> mentorStoryJournalPageList = new List<JournalPageInfo>();
    public List<JournalPageInfo> playerStoryJournalPageList = new List<JournalPageInfo>();
    public List<JournalPageInfo> personalStoryJournalPageList = new List<JournalPageInfo>();
    [Space(10)]
    [SerializeField] List<int> mentorStoryJournalPageIndexList = new List<int>();
    [SerializeField] List<int> playerStoryJournalPageIndexList = new List<int>();
    [SerializeField] List<int> personalStoryJournalPageIndexList = new List<int>();

    public bool journalPageIsSelected;


    //--------------------


    private void Start()
    {
        journalMenuState = JournalMenuState.MentorJournal;
        journalPageIsSelected = false;
    }


    //--------------------


    public void LoadData()
    {
        //Load Mentor Journal
        mentorStoryJournalPageIndexList = DataManager.Instance.mentorStoryJournalPageIndexList_Store;
        for (int i = 0; i < mentorStoryJournalPageIndexList.Count; i++)
        {
            SetupJournalPageList_Start(JournalMenuState.MentorJournal, mentorStoryJournalPageIndexList[i]);
        }

        //Load Player Journal
        playerStoryJournalPageIndexList = DataManager.Instance.playerStoryJournalPageIndexList_Store;
        for (int i = 0; i < playerStoryJournalPageIndexList.Count; i++)
        {
            SetupJournalPageList_Start(JournalMenuState.PlayerJournal, playerStoryJournalPageIndexList[i]);
        }

        //Load Personal Journal
        personalStoryJournalPageIndexList = DataManager.Instance.personalStoryJournalPageIndexList_Store;
        for (int i = 0; i < personalStoryJournalPageIndexList.Count; i++)
        {
            SetupJournalPageList_Start(JournalMenuState.PersonalJournal, personalStoryJournalPageIndexList[i]);
        }
    }
    public void SaveData()
    {
        DataManager.Instance.mentorStoryJournalPageIndexList_Store = mentorStoryJournalPageIndexList;
        DataManager.Instance.playerStoryJournalPageIndexList_Store = playerStoryJournalPageIndexList;
        DataManager.Instance.personalStoryJournalPageIndexList_Store = personalStoryJournalPageIndexList;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_Journals");
    }

    void PrintError(int index)
    {
        print(index + "----- ObjectList: Mentor: " + mentorJournalPageList.Count + " | Player: " + playerJournalPageList.Count + " | Personal: " + personalJournalPageList.Count);
        print(index + "----- InfoListMentor: " + mentorStoryJournalPageList.Count + " | Player: " + playerStoryJournalPageList.Count + " | Personal: " + personalStoryJournalPageList.Count);
        print(index + "----- IntListMentor: " + mentorStoryJournalPageIndexList.Count + " | Player: " + playerStoryJournalPageIndexList.Count + " | Personal: " + personalStoryJournalPageIndexList.Count);
    }


    //--------------------


    public void SetupInfoPage(JournalPage journalPage)
    {
        JournalPage_SO temp_SO = GetJournalPage_SO();

        for (int i = 0; i < temp_SO.journalPageList.Count; i++)
        {
            if (temp_SO.journalPageList[i].title == journalPage.title_Text.text)
            {
                InfoPage_Content.GetComponent<RectTransform>().sizeDelta = new Vector2(630, temp_SO.journalPageList[i].pageHeight);
                break;
            }
        }
        
        activeJournalPageIndex = journalPage.pageIndex;

        title_Text.text = journalPage.title_Text.text;
        autorName_Text.text = journalPage.autorName_Text.text;
        destination_Text.text = journalPage.destination_Text.text;

        //Date
        date_Text.text = journalPage.date_Text.text;

        //Time
        time_Text.text = journalPage.date_Text.text;

        //Message
        #region
        message_Text.text = journalPage.message.text;

        if (journalPage.message_Clip)
        {
            message_Clip_Button.SetActive(true);
        }
        else
        {
            message_Clip_Button.SetActive(false);
        }

        message_Clip = journalPage.message_Clip;
        #endregion
    }
    public void ResetInfoPage()
    {
        activeJournalPageIndex = -1;

        title_Text.text = "";
        autorName_Text.text = "";
        destination_Text.text = "";
        message_Text.text = "";

        date_Text.text = "";
        time_Text.text = "";

        message_Clip_Button.SetActive(false);

        message_Clip = null;
    }


    //--------------------

    public void SetupJournalPageList_Start(JournalMenuState journalMenuState, int index)
    {
        //Add page info
        JournalPageInfo tempJournalPageInfo = new JournalPageInfo();

        //Add Journal Prefab to the list
        if (journalMenuState == JournalMenuState.MentorJournal)
        {
            tempJournalPageInfo.journalMenuState = JournalMenuState.MentorJournal;

            tempJournalPageInfo.title = mentorJournalPage_SO.journalPageList[index].title;
            tempJournalPageInfo.autor = mentorJournalPage_SO.journalPageList[index].autor;
            tempJournalPageInfo.destination = mentorJournalPage_SO.journalPageList[index].destination;

            tempJournalPageInfo.date = mentorJournalPage_SO.journalPageList[index].date;
            tempJournalPageInfo.time = mentorJournalPage_SO.journalPageList[index].time;

            tempJournalPageInfo.message = mentorJournalPage_SO.journalPageList[index].message;
            tempJournalPageInfo.message_Clip = mentorJournalPage_SO.journalPageList[index].message_Clip;

            mentorStoryJournalPageList.Add(tempJournalPageInfo);

            mentorJournalPageList.Add(Instantiate(journal_Prefab) as GameObject);
            mentorJournalPageList[mentorJournalPageList.Count - 1].GetComponent<JournalPage>().SetupPageInfo(mentorStoryJournalPageList[mentorJournalPageList.Count - 1], index);
        }
        else if (journalMenuState == JournalMenuState.PlayerJournal)
        {
            tempJournalPageInfo.journalMenuState = JournalMenuState.PlayerJournal;

            tempJournalPageInfo.title = playerJournalPage_SO.journalPageList[index].title;
            tempJournalPageInfo.autor = playerJournalPage_SO.journalPageList[index].autor;
            tempJournalPageInfo.destination = playerJournalPage_SO.journalPageList[index].destination;

            tempJournalPageInfo.date = playerJournalPage_SO.journalPageList[index].date;
            tempJournalPageInfo.time = playerJournalPage_SO.journalPageList[index].time;

            tempJournalPageInfo.message = playerJournalPage_SO.journalPageList[index].message;
            tempJournalPageInfo.message_Clip = playerJournalPage_SO.journalPageList[index].message_Clip;

            playerStoryJournalPageList.Add(tempJournalPageInfo);

            playerJournalPageList.Add(Instantiate(journal_Prefab) as GameObject);
            playerJournalPageList[playerJournalPageList.Count - 1].GetComponent<JournalPage>().SetupPageInfo(playerStoryJournalPageList[playerStoryJournalPageList.Count - 1], index);
        }
        else if (journalMenuState == JournalMenuState.PersonalJournal)
        {
            tempJournalPageInfo.journalMenuState = JournalMenuState.PersonalJournal;

            tempJournalPageInfo.title = personalJournalPage_SO.journalPageList[index].title;
            tempJournalPageInfo.autor = personalJournalPage_SO.journalPageList[index].autor;
            tempJournalPageInfo.destination = personalJournalPage_SO.journalPageList[index].destination;

            tempJournalPageInfo.date = personalJournalPage_SO.journalPageList[index].date;
            tempJournalPageInfo.time = personalJournalPage_SO.journalPageList[index].time;

            tempJournalPageInfo.message = personalJournalPage_SO.journalPageList[index].message;
            tempJournalPageInfo.message_Clip = personalJournalPage_SO.journalPageList[index].message_Clip;

            personalStoryJournalPageList.Add(tempJournalPageInfo);

            personalJournalPageList.Add(Instantiate(journal_Prefab) as GameObject);
            personalJournalPageList[personalJournalPageList.Count - 1].GetComponent<JournalPage>().SetupPageInfo(personalStoryJournalPageList[personalStoryJournalPageList.Count - 1], index);
        }

        //Give correct Parent, Position, Rotation and Scale
        for (int i = 0; i < mentorJournalPageList.Count; i++)
        {
            mentorJournalPageList[i].transform.SetParent(journal_MentorStory_Menu.transform);
            mentorJournalPageList[i].transform.localScale = new Vector3(1, 1, 1);
            mentorJournalPageList[i].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }
        for (int i = 0; i < playerJournalPageList.Count; i++)
        {
            playerJournalPageList[i].transform.SetParent(journal_PlayerStory_Menu.transform);
            playerJournalPageList[i].transform.localScale = new Vector3(1, 1, 1);
            playerJournalPageList[i].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }
        for (int i = 0; i < personalJournalPageList.Count; i++)
        {
            personalJournalPageList[i].transform.SetParent(journal_PersonalStory_Menu.transform);
            personalJournalPageList[i].transform.localScale = new Vector3(1, 1, 1);
            personalJournalPageList[i].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
    public void SetupJournalPageList_WhenAdding(JournalMenuState journalMenuState, int index)
    {
        SoundManager.Instance.Play_JournalPage_GetNewJournalPage_Clip();

        //Add page info
        JournalPageInfo tempJournalPageInfo = new JournalPageInfo();

        //Add Journal Prefab to the list
        if (journalMenuState == JournalMenuState.MentorJournal)
        {
            tempJournalPageInfo.journalMenuState = JournalMenuState.MentorJournal;

            tempJournalPageInfo.title = mentorJournalPage_SO.journalPageList[index].title;
            tempJournalPageInfo.autor = mentorJournalPage_SO.journalPageList[index].autor;
            tempJournalPageInfo.destination = mentorJournalPage_SO.journalPageList[index].destination;

            tempJournalPageInfo.date = mentorJournalPage_SO.journalPageList[index].date;
            tempJournalPageInfo.time = mentorJournalPage_SO.journalPageList[index].time;

            tempJournalPageInfo.message = mentorJournalPage_SO.journalPageList[index].message;
            tempJournalPageInfo.message_Clip = mentorJournalPage_SO.journalPageList[index].message_Clip;

            mentorStoryJournalPageList.Insert(0, tempJournalPageInfo);

            mentorJournalPageList.Insert(0, Instantiate(journal_Prefab) as GameObject);
            mentorJournalPageList[0].GetComponent<JournalPage>().SetupPageInfo(mentorStoryJournalPageList[0], index);
        }
        else if (journalMenuState == JournalMenuState.PlayerJournal)
        {
            tempJournalPageInfo.journalMenuState = JournalMenuState.PlayerJournal;

            tempJournalPageInfo.title = playerJournalPage_SO.journalPageList[index].title;
            tempJournalPageInfo.autor = playerJournalPage_SO.journalPageList[index].autor;
            tempJournalPageInfo.destination = playerJournalPage_SO.journalPageList[index].destination;

            tempJournalPageInfo.date = playerJournalPage_SO.journalPageList[index].date;
            tempJournalPageInfo.time = playerJournalPage_SO.journalPageList[index].time;

            tempJournalPageInfo.message = playerJournalPage_SO.journalPageList[index].message;
            tempJournalPageInfo.message_Clip = playerJournalPage_SO.journalPageList[index].message_Clip;

            playerStoryJournalPageList.Insert(0, tempJournalPageInfo);

            playerJournalPageList.Insert(0, Instantiate(journal_Prefab) as GameObject);
            playerJournalPageList[0].GetComponent<JournalPage>().SetupPageInfo(playerStoryJournalPageList[0], index);
        }
        else if (journalMenuState == JournalMenuState.PersonalJournal)
        {
            tempJournalPageInfo.journalMenuState = JournalMenuState.PersonalJournal;

            tempJournalPageInfo.title = personalJournalPage_SO.journalPageList[index].title;
            tempJournalPageInfo.autor = personalJournalPage_SO.journalPageList[index].autor;
            tempJournalPageInfo.destination = personalJournalPage_SO.journalPageList[index].destination;

            tempJournalPageInfo.date = personalJournalPage_SO.journalPageList[index].date;
            tempJournalPageInfo.time = personalJournalPage_SO.journalPageList[index].time;

            tempJournalPageInfo.message = personalJournalPage_SO.journalPageList[index].message;
            tempJournalPageInfo.message_Clip = personalJournalPage_SO.journalPageList[index].message_Clip;

            personalStoryJournalPageList.Insert(0, tempJournalPageInfo);

            personalJournalPageList.Insert(0, Instantiate(journal_Prefab) as GameObject);
            personalJournalPageList[0].GetComponent<JournalPage>().SetupPageInfo(personalStoryJournalPageList[0], index);
        }

        //Give correct Parent, Position, Rotation and Scale
        for (int i = 0; i < mentorJournalPageList.Count; i++)
        {
            mentorJournalPageList[i].transform.SetParent(InfoPage_Content.transform);
        }
        for (int i = 0; i < mentorJournalPageList.Count; i++)
        {
            mentorJournalPageList[i].transform.SetParent(journal_MentorStory_Menu.transform);
            mentorJournalPageList[i].transform.localScale = new Vector3(1, 1, 1);
            mentorJournalPageList[i].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }

        for (int i = 0; i < playerJournalPageList.Count; i++)
        {
            playerJournalPageList[i].transform.SetParent(InfoPage_Content.transform);
        }
        for (int i = 0; i < playerJournalPageList.Count; i++)
        {
            playerJournalPageList[i].transform.SetParent(journal_PlayerStory_Menu.transform);
            playerJournalPageList[i].transform.localScale = new Vector3(1, 1, 1);
            playerJournalPageList[i].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }

        for (int i = 0; i < personalJournalPageList.Count; i++)
        {
            personalJournalPageList[i].transform.SetParent(InfoPage_Content.transform);
        }
        for (int i = 0; i < personalJournalPageList.Count; i++)
        {
            personalJournalPageList[i].transform.SetParent(journal_PersonalStory_Menu.transform);
            personalJournalPageList[i].transform.localScale = new Vector3(1, 1, 1);
            personalJournalPageList[i].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }
    }


    //--------------------


    public void AddJournalPageToList(JournalMenuState journalMenuState, int index)
    {
        SetupJournalPageList_WhenAdding(journalMenuState, index);

        //Add Index info to the indexLists
        switch (journalMenuState)
        {
            case JournalMenuState.None:
                break;

            case JournalMenuState.MentorJournal:
                mentorStoryJournalPageIndexList.Insert(0, index);
                break;
            case JournalMenuState.PlayerJournal:
                playerStoryJournalPageIndexList.Insert(0, index);
                break;
            case JournalMenuState.PersonalJournal:
                personalStoryJournalPageIndexList.Insert(0, index);
                break;

            default:
                break;
        }

        SaveData();
    }


    //--------------------


    public void MessageClipButton_isPressed()
    {
        if (message_Clip && SoundManager.Instance.audioSource_Journal_VoiceMessage != null)
        {
            SoundManager.Instance.Play_JournalPage_VoiceMessage_Clip(message_Clip);

            print("1. Play Message");
        }
    }


    //--------------------


    public void MentorJournalButton_isPressed()
    {
        journalMenuState = JournalMenuState.MentorJournal;
        journalPageIsSelected = false;

        ResetInfoPage();

        //Set the "Content" Size of the visible Journal List
        journal_Content_Menu.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 0);
        for (int i = 0; i < mentorJournalPageList.Count; i++)
        {
            journal_Content_Menu.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 110);
        }
        journal_Content_Menu.GetComponent<RectTransform>().sizeDelta += new Vector2(0, -10);

        journal_MentorStory_Menu.SetActive(true);
        journal_PlayerStory_Menu.SetActive(false);
        journal_PersonalStory_Menu.SetActive(false);
    }
    public void PlayerJournalButton_isPressed()
    {
        journalMenuState = JournalMenuState.PlayerJournal;
        journalPageIsSelected = false;

        ResetInfoPage();

        //Set the "Content" Size of the visible Journal List
        journal_Content_Menu.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 0);
        for (int i = 0; i < playerJournalPageList.Count; i++)
        {
            journal_Content_Menu.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 110);
        }
        journal_Content_Menu.GetComponent<RectTransform>().sizeDelta += new Vector2(0, -10);

        journal_MentorStory_Menu.SetActive(false);
        journal_PlayerStory_Menu.SetActive(true);
        journal_PersonalStory_Menu.SetActive(false);
    }
    public void PersonalJournalButton_isPressed()
    {
        journalMenuState = JournalMenuState.PersonalJournal;
        journalPageIsSelected = false;

        ResetInfoPage();

        //Set the "Content" Size of the visible Journal List
        journal_Content_Menu.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 0);
        for (int i = 0; i < personalJournalPageList.Count; i++)
        {
            journal_Content_Menu.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 110);
        }
        journal_Content_Menu.GetComponent<RectTransform>().sizeDelta += new Vector2(0, -10);

        journal_MentorStory_Menu.SetActive(false);
        journal_PlayerStory_Menu.SetActive(false);
        journal_PersonalStory_Menu.SetActive(true);
    }


    //--------------------


    public JournalPage_SO GetJournalPage_SO()
    {
        switch (journalMenuState)
        {
            case JournalMenuState.None:
                return mentorJournalPage_SO;

            case JournalMenuState.MentorJournal:
                return mentorJournalPage_SO;
            case JournalMenuState.PlayerJournal:
                return playerJournalPage_SO;
            case JournalMenuState.PersonalJournal:
                return personalJournalPage_SO;

            default:
                return mentorJournalPage_SO;
        }
    }
}

public enum JournalMenuState
{
    None,

    MentorJournal,
    PlayerJournal,
    PersonalJournal
}
