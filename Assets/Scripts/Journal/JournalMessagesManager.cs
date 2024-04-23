using System;

public class JournalMessagesManager : Singleton<JournalMessagesManager>
{
    MessagesConditionChecks messagesConditionChecks = new MessagesConditionChecks();


    //--------------------


    private void Update()
    {
        if (DataManager.Instance.hasLoaded)
        {
            //The Beginning
            StartingAdventure();

            //Interractions
            OpeningTablet();
            TableInteraction();
            HoldingAr�dianCrystal();
            HoldingGhostCapturer();
            HoldingAr�dianKey();

            //Events
            CapturingGhost();
        }
    }


    //--------------------


    public void LoadData()
    {
        messagesConditionChecks = DataManager.Instance.messagesConditionChecks_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.messagesConditionChecks_Store = messagesConditionChecks;
    }


    //--------------------


    void ActivateJournalPage(int index, bool notification)
    {
        if (notification)
        {
            JournalManager.Instance.JournalNotification();
        }

        JournalManager.Instance.AddJournalPageToList(JournalMenuState.PlayerJournal, index);

        SaveData();
    }
    void ActivateJournalPage(int index)
    {
        JournalManager.Instance.JournalNotification();

        JournalManager.Instance.AddJournalPageToList(JournalMenuState.PlayerJournal, index);

        SaveData();
    }


    //--------------------


    void StartingAdventure()
    {
        if (!messagesConditionChecks.start_GeneralInfo)
        {
            messagesConditionChecks.start_GeneralInfo = true;

            ActivateJournalPage(0, false);
        }
    }
    void OpeningTablet()
    {
        if (!messagesConditionChecks.openingTablet
            && TabletManager.Instance.tabletMenuState != TabletMenuState.None)
        {
            messagesConditionChecks.openingTablet = true;

            ActivateJournalPage(1);
        }
    }
    void TableInteraction()
    {
        if (!messagesConditionChecks.tableInteraction
            && (TabletManager.Instance.tabletMenuState == TabletMenuState.CraftingTable
            || TabletManager.Instance.tabletMenuState == TabletMenuState.ResearchTable
            || TabletManager.Instance.tabletMenuState == TabletMenuState.SkillTree))
        {
            messagesConditionChecks.tableInteraction = true;

            ActivateJournalPage(2);
        }
    }

    void HoldingAr�dianCrystal()
    {
        if (!messagesConditionChecks.holdingAr�dianCrystal
            && HotbarManager.Instance.selectedItem == Items.Ar�diteCrystal)
        {
            messagesConditionChecks.holdingAr�dianCrystal = true;

            ActivateJournalPage(3);
        }
    }
    void HoldingGhostCapturer()
    {
        if (!messagesConditionChecks.holdingGhostCapturer
            && HotbarManager.Instance.selectedItem == Items.GhostCapturer)
        {
            messagesConditionChecks.holdingGhostCapturer = true;

            ActivateJournalPage(4);
        }
    }
    void CapturingGhost()
    {
        if (!messagesConditionChecks.capturingGhost
            && GhostManager.Instance.ghostCapturerStats.ghostCapturedStats[0].isTaken)
        {
            messagesConditionChecks.capturingGhost = true;

            ActivateJournalPage(5);
        }
    }
    void HoldingAr�dianKey()
    {
        if (!messagesConditionChecks.holdingAr�dianKey
            && InventoryManager.Instance.GetAmountOfItemInInventory(0, Items.Ar�dianKey) > 0)
        {
            messagesConditionChecks.holdingAr�dianKey = true;

            ActivateJournalPage(6);
        }
    }
}

[Serializable]
public class MessagesConditionChecks
{
    public bool start_GeneralInfo; //Entrance and the Key

    public bool openingTablet;
    public bool tableInteraction;

    public bool holdingAr�dianCrystal;
    public bool holdingGhostCapturer;
    public bool capturingGhost;
    public bool holdingAr�dianKey;
}