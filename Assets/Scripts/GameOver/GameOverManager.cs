using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : Singleton<GameOverManager>
{
    public bool GameOverResetButton;

    public GameObject gameOver_Panel;
    public Image gameOver_BG;
    public Image arídianCrystal_Icon;
    public TextMeshProUGUI gameOver_Text;
    public TextMeshProUGUI reset_Text;

    [SerializeField] float transparencyValue = 0;

    //-------------------------


    private void Start()
    {
        PlayerButtonManager.objectInterraction_isPressedDown += ResetAfterGameOver;
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        //For testing
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SoundManager.Instance.Play_GameOver_Clip();
            PlayerManager.Instance.TransferPlayerToDyingPos();

            RemoveAllWorldItems();
        }

        //Set "GameOver" Screen
        if (MainManager.Instance.gameStates == GameStates.GameOver)
        {
            if (transparencyValue < 1)
            {
                transparencyValue += Time.deltaTime / 2;

                if (transparencyValue > 1)
                {
                    transparencyValue = 1;
                }
            }

            gameOver_BG.color = new Color(0, 0, 0, transparencyValue);
            arídianCrystal_Icon.color = new Color(1, 1, 1, transparencyValue);
            gameOver_Text.color = new Color(MainManager.Instance.mainColor_Blue.r, MainManager.Instance.mainColor_Blue.g, MainManager.Instance.mainColor_Blue.b, transparencyValue);
            reset_Text.color = new Color(MainManager.Instance.mainColor_Blue.r, MainManager.Instance.mainColor_Blue.g, MainManager.Instance.mainColor_Blue.b, transparencyValue);
        }
    }


    //-------------------------


    public void TriggerGameOver()
    {
        transparencyValue = 0;
        gameOver_Panel.SetActive(true);
        MainManager.Instance.gameStates = GameStates.GameOver;
        TabletManager.Instance.CloseTablet();

        SoundManager.Instance.Play_GameOver_Clip();

        StartCoroutine(GameOverTimer(2));
    }
    IEnumerator GameOverTimer(float time)
    {
        yield return new WaitForSeconds(time);

        GameOverResetButton = true;
    }
    public void ResetAfterGameOver()
    {
        if (GameOverResetButton)
        {
            PlayerManager.Instance.TransferPlayerToDyingPos();

            RemoveAllWorldItems();
            RemoveAllInventoryItems();

            HealthManager.Instance.ResetPlayerHealthValues();

            transparencyValue = 0;
            gameOver_Panel.SetActive(false);

            //End GameOver
            if (GameOverResetButton)
            {
                GameOverResetButton = false;

                MainManager.Instance.gameStates = GameStates.None;
            }

            SoundManager.Instance.Stop_GameOver_Clip();
        }
    }


    //-------------------------


    void RemoveAllWorldItems()
    {
        //Remove all items from the world
        for (int i = WorldObjectManager.Instance.worldObjectList.Count - 1; i >= 0; i--)
        {
            if (WorldObjectManager.Instance.worldObjectList[i].GetComponent<InteractableObject>()
                && WorldObjectManager.Instance.worldObjectList[i].GetComponent<InteractableObject>().itemName != Items.ArídianKey)
            {
                WorldObjectManager.Instance.worldObjectList[i].GetComponent<InteractableObject>().DestroyThisObject();
            }
        }

        //Clear the lists
        WorldObjectManager.Instance.worldObjectList.Clear();
        WorldObjectManager.Instance.worldObjectList_ToSave.Clear();
    }

    void RemoveAllInventoryItems()
    {
        for (int i = InventoryManager.Instance.inventories[0].itemsInInventory.Count - 1; i >= 0; i--)
        {
            if (InventoryManager.Instance.inventories[0].itemsInInventory[i].itemName != Items.ArídianKey)
            {
                InventoryManager.Instance.RemoveItemFromInventory(0, InventoryManager.Instance.inventories[0].itemsInInventory[i].itemName, InventoryManager.Instance.inventories[0].itemsInInventory[i].itemID, true);
            }
        }
    }
}
