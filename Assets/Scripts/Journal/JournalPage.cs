using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class JournalPage : MonoBehaviour, IPointerEnterHandler
{
    public JournalMenuState journalMenuState;
    public int pageIndex;

    public TextMeshProUGUI title_Text;
    public TextMeshProUGUI autorName_Text;
    public TextMeshProUGUI destination_Text;
    public TextMeshProUGUI date_Text;
    public TextMeshProUGUI time_Text;

    public TextMeshProUGUI message;
    public GameObject message_Clip_Button;
    public AudioClip message_Clip;


    //--------------------


    public void SetupPageInfo(JournalPageInfo journalPageInfo, int index)
    {
        journalMenuState = journalPageInfo.journalMenuState;

        title_Text.text = journalPageInfo.title;
        autorName_Text.text = journalPageInfo.autor;
        destination_Text.text = journalPageInfo.destination;

        //Date
        #region
        string tempDay = journalPageInfo.date.x.ToString();
        string tempMonth = journalPageInfo.date.y.ToString();
        string tempYear = journalPageInfo.date.z.ToString();

        if (journalPageInfo.date.x < 10)
            tempDay = "0" + journalPageInfo.date.x;
        if (journalPageInfo.date.y < 10)
            tempMonth = "0" + journalPageInfo.date.y;
        if (journalPageInfo.date.z < 10)
            tempYear = "0" + journalPageInfo.date.z;

        date_Text.text = tempDay + "/" + tempMonth + "/" + tempYear;
        #endregion

        //Time
        #region
        string tempHour = journalPageInfo.time.x.ToString();
        string tempMinute = journalPageInfo.time.y.ToString();
        string tempSecond = journalPageInfo.time.z.ToString();

        if (journalPageInfo.time.x < 10)
            tempHour = "0" + journalPageInfo.time.x;
        if (journalPageInfo.time.y < 10)
            tempMinute = "0" + journalPageInfo.time.y;
        if (journalPageInfo.time.z < 10)
            tempSecond = "0" + journalPageInfo.time.z;

        time_Text.text = tempHour + ":" + tempMinute + ":" + tempSecond;
        #endregion

        //Message
        #region
        message.text = journalPageInfo.message;

        if (journalPageInfo.message_Clip)
        {
            message_Clip = journalPageInfo.message_Clip;
        }

        if (message_Clip)
        {
            message_Clip_Button.SetActive(true);
        }
        else
        {
            message_Clip_Button.SetActive(false);
        }
        #endregion

        pageIndex = index;
    }


    //--------------------


    public void JournalPageButton_isClicked()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        JournalManager.Instance.journalPageIsSelected = true;
        JournalManager.Instance.SetupInfoPage(this);
    }
    public void MessageClipButton_isClicked()
    {
        if (message_Clip && SoundManager.Instance.audioSource_Journal_VoiceMessage != null)
        {
            SoundManager.Instance.Play_JournalPage_VoiceMessage_Clip(message_Clip);

            print("2. Play Message");
        }
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();

        if (!JournalManager.Instance.journalPageIsSelected)
        {
            JournalManager.Instance.SetupInfoPage(this);
        }
    }
}
