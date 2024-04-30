using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : Singleton<JournalManager>
{
    #region Variables
    public JournalMenuState journalMenuState;
    public int activeJournalPageIndex;

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
    public List<GameObject> mentorJournalPageList = new List<GameObject>();
    public List<GameObject> playerJournalPageList = new List<GameObject>();
    public List<GameObject> personalJournalPageList = new List<GameObject>();
    [Space(10)]
    public List<JournalPageInfo> mentorStoryJournalPageList = new List<JournalPageInfo>();
    public List<JournalPageInfo> playerStoryJournalPageList = new List<JournalPageInfo>();
    public List<JournalPageInfo> personalStoryJournalPageList = new List<JournalPageInfo>();
    [Space(10)]
    [SerializeField] List<int> mentorStoryJournalPageIndexList = new List<int>();
    [SerializeField] List<int> playerStoryJournalPageIndexList = new List<int>();
    [SerializeField] List<int> personalStoryJournalPageIndexList = new List<int>();

    public bool journalPageIsSelected;


    [Header("Folder Structure")]
    GameObject journalPageWorldObject_Parent;
    [SerializeField] List<List<JournalPageToSave>> journalPageTypeObjectList = new List<List<JournalPageToSave>>();

    [Header("+ Sign")]
    public List<bool> journalPage_PlussSign_Mentor = new List<bool>();
    public List<bool> journalPage_PlussSign_Player = new List<bool>();
    public List<bool> journalPage_PlussSign_Personal = new List<bool>();
    public GameObject journalPageButton_PlussSign_Mentor;
    public GameObject journalPageButton_PlussSign_Player;
    public GameObject journalPageButton_PlussSign_Personal;

    [Header("Notification")]
    public GameObject notificationParent;
    public Image notificationImage;
    float fadingNotificationImageValue;
    bool fadingNotificationImageCheck;
    bool towardsVisible;

    #endregion


    //--------------------


    private void Awake()
    {
        journalPageWorldObject_Parent = GameObject.Find("JournalPage_Parent");
    }
    private void Start()
    {
        journalMenuState = JournalMenuState.MentorJournal;
        journalPageIsSelected = false;
    }
    private void Update()
    {
        //Set NotificationImage Visibility
        #region
        if (fadingNotificationImageCheck)
        {
            //Change Visible Value
            if (towardsVisible)
            {
                fadingNotificationImageValue += Time.deltaTime;
            }
            else
            {
                fadingNotificationImageValue -= Time.deltaTime;
            }

            //Set if Visibility is going Up or Down
            if (fadingNotificationImageValue >= 1 && towardsVisible)
            {
                towardsVisible = false;
            }
            else if (fadingNotificationImageValue <= 0 && !towardsVisible)
            {
                towardsVisible = true;
            }

            //Change Visibility
            notificationImage.color = new Color(1, 1, 1, fadingNotificationImageValue);
        }
        #endregion

        if (TabletManager.Instance.journal_Parent.activeInHierarchy)
        {
            SetButtonPlussIcons();
        }
    }


    //--------------------


    public void LoadData()
    {
        #region LoadJournals
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
        #endregion

        #region Save/Load Objects
        journalPageTypeObjectList.Clear();

        for (int i = 0; i < DataManager.Instance.journalPageTypeObjectList_Store.Count; i++)
        {
            List<JournalPageToSave> journalPageToSaveList = new List<JournalPageToSave>();

            journalPageTypeObjectList.Add(journalPageToSaveList);

            for (int j = 0; j < DataManager.Instance.journalPageTypeObjectList_Store[i].journalPageToSaveList.Count; j++)
            {
                journalPageTypeObjectList[journalPageTypeObjectList.Count - 1].Add(DataManager.Instance.journalPageTypeObjectList_Store[i].journalPageToSaveList[j]);
            }
        }

        SetupJournalPageList();
        #endregion

        #region JournalPage "+"
        journalPage_PlussSign_Mentor = DataManager.Instance.journalPage_PlussSign_Mentor_Store;
        journalPage_PlussSign_Player = DataManager.Instance.journalPage_PlussSign_Player_Store;
        journalPage_PlussSign_Personal = DataManager.Instance.journalPage_PlussSign_Personal_Store;

        //Mentor Journal
        if (journalPage_PlussSign_Mentor.Count <= 0)
            SetupJournalPage_Mentor_SignList();
        else
            UpdateJournalPage_Mentor_SignsObject();

        //Player Journal
        if (journalPage_PlussSign_Player.Count <= 0)
            SetupJournalPage_Player_SignList();
        else
            UpdateJournalPage_Player_SignsObject();

        //Personal Journal
        if (journalPage_PlussSign_Personal.Count <= 0)
            SetupJournalPage_Personal_SignList();
        else
            UpdateJournalPage_Personal_SignsObject();
        #endregion

        SaveData();
    }
    public void SaveData()
    {
        DataManager.Instance.mentorStoryJournalPageIndexList_Store = mentorStoryJournalPageIndexList;
        DataManager.Instance.playerStoryJournalPageIndexList_Store = playerStoryJournalPageIndexList;
        DataManager.Instance.personalStoryJournalPageIndexList_Store = personalStoryJournalPageIndexList;

        #region Save/Load Objects
        List<ListOfJournalPageToSave> journalPageToSaveList = new List<ListOfJournalPageToSave>();

        for (int i = 0; i < journalPageTypeObjectList.Count; i++)
        {
            ListOfJournalPageToSave journalPageToSave = new ListOfJournalPageToSave();

            journalPageToSaveList.Add(journalPageToSave);

            for (int j = 0; j < journalPageTypeObjectList[i].Count; j++)
            {
                journalPageToSaveList[journalPageToSaveList.Count - 1].journalPageToSaveList.Add(journalPageTypeObjectList[i][j]);
            }
        }

        DataManager.Instance.journalPageTypeObjectList_Store = journalPageToSaveList;
        #endregion

        #region JournalPage "+"
        DataManager.Instance.journalPage_PlussSign_Mentor_Store = journalPage_PlussSign_Mentor;
        DataManager.Instance.journalPage_PlussSign_Player_Store = journalPage_PlussSign_Player;
        DataManager.Instance.journalPage_PlussSign_Personal_Store = journalPage_PlussSign_Personal;
        #endregion
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

        //ResetInfoPage();

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

        //ResetInfoPage();

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

        //ResetInfoPage();

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


    //--------------------


    void SetupJournalPageList()
    {
        List<List<JournalPageToSave>> tempJournalPageTypeObjectList = new List<List<JournalPageToSave>>();
        List<List<bool>> checkedJournalPage = new List<List<bool>>();

        //Add elements to "checkedBlueprint" as children
        for (int i = 0; i < journalPageWorldObject_Parent.transform.childCount; i++)
        {
            List<JournalPageToSave> tempJournalPagetoSave = new List<JournalPageToSave>();
            tempJournalPageTypeObjectList.Add(tempJournalPagetoSave);

            List<bool> templist_Outer = new List<bool>();
            checkedJournalPage.Add(templist_Outer);

            for (int j = 0; j < journalPageWorldObject_Parent.transform.GetChild(i).transform.childCount; j++)
            {
                bool templist_Inner = false;
                checkedJournalPage[i].Add(templist_Inner);
            }
        }

        //Set Old JournalPage
        for (int i = 0; i < checkedJournalPage.Count; i++)
        {
            for (int k = 0; k < checkedJournalPage[i].Count; k++)
            {
                if (journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<JournalObject>())
                {
                    for (int j = 0; j < journalPageTypeObjectList.Count; j++)
                    {
                        for (int l = 0; l < journalPageTypeObjectList[j].Count; l++)
                        {
                            if (journalPageTypeObjectList[j][l].journalPagePos == journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<JournalObject>().transform.position)
                            {
                                //Setup Save_List
                                JournalPageToSave tempJournalPage = new JournalPageToSave();

                                tempJournalPage.isPicked = journalPageTypeObjectList[j][l].isPicked;
                                tempJournalPage.journalPagePos = journalPageTypeObjectList[j][l].journalPagePos;

                                tempJournalPageTypeObjectList[i].Add(tempJournalPage);

                                checkedJournalPage[i][k] = true;

                                //Set info in Child
                                journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<JournalObject>().LoadJournalPage(journalPageTypeObjectList[j][l].isPicked, j, l);

                                l = journalPageTypeObjectList[j].Count;
                                j = journalPageTypeObjectList.Count;

                                break;
                            }
                        }
                    }
                }
            }
        }

        //Set New JournalPage
        for (int i = 0; i < checkedJournalPage.Count; i++)
        {
            for (int j = 0; j < checkedJournalPage[i].Count; j++)
            {
                if (!checkedJournalPage[i][j])
                {
                    if (journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(j))
                    {
                        //print("New JournalPage: [" + i + "][" + j + "]");
                        //Give all Legal Objects an index
                        journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<JournalObject>().journalPageIndex_x = i;
                        journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<JournalObject>().journalPageIndex_y = j;

                        //Make a TreeTypeObjectList
                        JournalPageToSave tempJournalPage = new JournalPageToSave();

                        tempJournalPage.isPicked = journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<JournalObject>().isPicked;
                        tempJournalPage.journalPageIndex_x = journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<JournalObject>().journalPageIndex_x;
                        tempJournalPage.journalPageIndex_y = journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<JournalObject>().journalPageIndex_y;
                        tempJournalPage.journalPagePos = journalPageWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<JournalObject>().transform.position;

                        tempJournalPageTypeObjectList[i].Add(tempJournalPage);
                    }
                }
            }
        }

        //Set New BlueprintTypeObjectList
        journalPageTypeObjectList.Clear();
        journalPageTypeObjectList = tempJournalPageTypeObjectList;
    }
    public void ChangeJournalPageInfo(bool _isPicked, int _journalPageIndex_j, int _journalPageIndex_l, Vector3 _journalPagePos)
    {
        JournalPageToSave journalPageTree = new JournalPageToSave();

        journalPageTree.isPicked = _isPicked;
        journalPageTree.journalPageIndex_x = _journalPageIndex_j;
        journalPageTree.journalPageIndex_y = _journalPageIndex_l;
        journalPageTree.journalPagePos = _journalPagePos;

        journalPageTypeObjectList[_journalPageIndex_j][_journalPageIndex_l] = journalPageTree;

        SaveData();
    }


    //--------------------


    public void JournalNotification()
    {
        SoundManager.Instance.Play_JournalPage_GetNewJournalPage_Clip();

        fadingNotificationImageValue = 0;

        notificationImage.color = new Color(1, 1, 1, fadingNotificationImageValue);
        notificationParent.SetActive(true);

        towardsVisible = true;
        fadingNotificationImageCheck = true;

        StartCoroutine(NotificationDuration(6));
    }
    IEnumerator NotificationDuration(float time)
    {
        yield return new WaitForSeconds(time);

        notificationParent.SetActive(false);

        fadingNotificationImageCheck = false;
    }


    //--------------------


    //Journal "+"
    #region Mentor
    void SetupJournalPage_Mentor_SignList()
    {
        for (int i = 0; i < mentorJournalPageList.Count; i++)
        {
            journalPage_PlussSign_Mentor.Add(true);

            mentorJournalPageList[i].GetComponent<JournalPage>().SetupIfPlussIsActive(true);
        }

        SaveData();
    }
    public void UpdateJournalPage_Mentor_SignsObject()
    {
        CheckIfJournalPage_MentorListCountHasChanged();

        for (int i = 0; i < mentorJournalPageList.Count; i++)
        {
            mentorJournalPageList[i].GetComponent<JournalPage>().SetupIfPlussIsActive(journalPage_PlussSign_Mentor[i]);
        }

        SaveData();
    }
    public void UpdateMentorPlussSignsSave(GameObject obj)
    {
        CheckIfJournalPage_MentorListCountHasChanged();

        for (int i = 0; i < mentorJournalPageList.Count; i++)
        {
            if (mentorJournalPageList[i] == obj)
            {
                journalPage_PlussSign_Mentor[i] = false;

                break;
            }
        }

        SaveData();
    }
    public void CheckIfJournalPage_MentorListCountHasChanged()
    {
        while (mentorJournalPageList.Count > journalPage_PlussSign_Mentor.Count)
        {
            journalPage_PlussSign_Mentor.Insert(0, true);
        }
    }
    #endregion
    #region Player
    void SetupJournalPage_Player_SignList()
    {
        for (int i = 0; i < playerJournalPageList.Count; i++)
        {
            journalPage_PlussSign_Player.Add(true);

            playerJournalPageList[i].GetComponent<JournalPage>().SetupIfPlussIsActive(true);
        }

        SaveData();
    }
    public void UpdateJournalPage_Player_SignsObject()
    {
        CheckIfJournalPage_PlayerListCountHasChanged();

        for (int i = 0; i < playerJournalPageList.Count; i++)
        {
            playerJournalPageList[i].GetComponent<JournalPage>().SetupIfPlussIsActive(journalPage_PlussSign_Player[i]);
        }

        SaveData();
    }
    public void UpdatePlayerPlussSignsSave(GameObject obj)
    {
        CheckIfJournalPage_PlayerListCountHasChanged();

        for (int i = 0; i < playerJournalPageList.Count; i++)
        {
            if (playerJournalPageList[i] == obj)
            {
                journalPage_PlussSign_Player[i] = false;

                break;
            }
        }

        SaveData();
    }
    public void CheckIfJournalPage_PlayerListCountHasChanged()
    {
        while (playerJournalPageList.Count > journalPage_PlussSign_Player.Count)
        {
            journalPage_PlussSign_Player.Insert(0, true);
        }
    }
    #endregion
    #region Personal
    void SetupJournalPage_Personal_SignList()
    {
        for (int i = 0; i < personalJournalPageList.Count; i++)
        {
            journalPage_PlussSign_Personal.Add(true);

            personalJournalPageList[i].GetComponent<JournalPage>().SetupIfPlussIsActive(true);
        }

        SaveData();
    }
    public void UpdateJournalPage_Personal_SignsObject()
    {
        CheckIfJournalPage_PersonalListCountHasChanged();

        for (int i = 0; i < personalJournalPageList.Count; i++)
        {
            personalJournalPageList[i].GetComponent<JournalPage>().SetupIfPlussIsActive(journalPage_PlussSign_Personal[i]);
        }

        SaveData();
    }
    public void UpdatePersonalPlussSignsSave(GameObject obj)
    {
        CheckIfJournalPage_PersonalListCountHasChanged();

        for (int i = 0; i < personalJournalPageList.Count; i++)
        {
            if (personalJournalPageList[i] == obj)
            {
                journalPage_PlussSign_Personal[i] = false;

                break;
            }
        }

        SaveData();
    }
    public void CheckIfJournalPage_PersonalListCountHasChanged()
    {
        while (personalJournalPageList.Count > journalPage_PlussSign_Personal.Count)
        {
            journalPage_PlussSign_Personal.Insert(0, true);
        }
    }
    #endregion

    //Buttons with "+"
    #region
    void SetButtonPlussIcons()
    {
        bool plussIconCheck = false;

        if (mentorJournalPageList.Count > journalPage_PlussSign_Mentor.Count)
        {
            plussIconCheck = true;
        }
        else
        {
            for (int i = 0; i < journalPage_PlussSign_Mentor.Count; i++)
            {
                if (journalPage_PlussSign_Mentor[i])
                {
                    plussIconCheck = true;
                }
            }
        }

        if (plussIconCheck)
            journalPageButton_PlussSign_Mentor.SetActive(true);
        else
            journalPageButton_PlussSign_Mentor.SetActive(false);

        plussIconCheck = false;
        if (playerJournalPageList.Count > journalPage_PlussSign_Player.Count)
        {
            plussIconCheck = true;
        }
        else
        {
            for (int i = 0; i < journalPage_PlussSign_Player.Count; i++)
            {
                if (journalPage_PlussSign_Player[i])
                {
                    plussIconCheck = true;
                }
            }
        }

        if (plussIconCheck)
            journalPageButton_PlussSign_Player.SetActive(true);
        else
            journalPageButton_PlussSign_Player.SetActive(false);

        plussIconCheck = false;
        if (personalJournalPageList.Count > journalPage_PlussSign_Personal.Count)
        {
            plussIconCheck = true;
        }
        else
        {
            for (int i = 0; i < journalPage_PlussSign_Personal.Count; i++)
            {
                if (journalPage_PlussSign_Personal[i])
                {
                    plussIconCheck = true;
                }
            }
        }

        if (plussIconCheck)
            journalPageButton_PlussSign_Personal.SetActive(true);
        else
            journalPageButton_PlussSign_Personal.SetActive(false);
    }
    #endregion
}

public enum JournalMenuState
{
    None,

    MentorJournal,
    PlayerJournal,
    PersonalJournal
}


[Serializable]
public class JournalPageToSave
{
    public bool isPicked;

    public int journalPageIndex_x;
    public int journalPageIndex_y;

    public Vector3 journalPagePos = new Vector3();
}

[Serializable]
public class ListOfJournalPageToSave
{
    public List<JournalPageToSave> journalPageToSaveList = new List<JournalPageToSave>();
}