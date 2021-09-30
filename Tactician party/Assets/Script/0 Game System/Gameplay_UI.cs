using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameplay_UI : MonoBehaviour
{
    // - - - Game Data Usage - - -
    private MainSystem Main_data;

    // - - - IU Objects - - -
    [Header("User Interface")]
    public GameObject[] playerHeart;

    public TMPro.TextMeshProUGUI currencyText;

    // - - - Recuit Variable - - -
    [Header("Requied game object")]
    public GameObject[] inventoryTile;
    public GameObject[] playerUnitsPrefad;

    private Vector3 spawnLocation;
    private int adventurer_Number;
    private int swordman_Number;
    private int lancer_Number;
    private int rogue_Number;
    private int archer_Number;
    private int cleric_Number;
    private int mage_Number;

    // - - - Unit's Class Selection - - -
    [Header("Class Selection")]
    public GameObject classSelect_UI;
    public GameObject selected_Unit;

    public bool selected = false;

    // - - - Game Speed - - 
    [Header("X2 Button")]
    [SerializeField] private Image ImageBotton;
    private bool speedUp;

    void Start()
    {
        Main_data = gameObject.GetComponent<MainSystem>();
    }

    void Update()
    {
        PlayerLife();
        classSelect_Interface();
        GameSpeed();
        CurrencyUpdate();
    }

    private void PlayerLife()
    {
        if(Main_data.playerLifePoint == 3)
        {
            playerHeart[0].SetActive(true);
            playerHeart[1].SetActive(true);
            playerHeart[2].SetActive(true);
        }
        else if (Main_data.playerLifePoint == 2)
        {
            playerHeart[0].GetComponent<Image>().color = new Vector4(1, 0.785f, 0.785f, 1);
            playerHeart[1].GetComponent<Image>().color = new Vector4(1, 0.785f, 0.785f, 1);
            playerHeart[2].GetComponent<Image>().color = new Vector4(0.15f, 0.15f, 0.15f, 1);
        }
        else if(Main_data.playerLifePoint == 1)
        {
            playerHeart[0].GetComponent<Image>().color = new Vector4(1, 0.785f, 0.785f, 1);
            playerHeart[1].GetComponent<Image>().color = new Vector4(0.15f, 0.15f, 0.15f, 1);
            playerHeart[2].GetComponent<Image>().color = new Vector4(0.15f, 0.15f, 0.15f, 1);
        }
        else if(Main_data.playerLifePoint == 0)
        {
            playerHeart[0].GetComponent<Image>().color = new Vector4(0.15f, 0.15f, 0.15f, 1);
            playerHeart[1].GetComponent<Image>().color = new Vector4(0.15f, 0.15f, 0.15f, 1);
            playerHeart[2].GetComponent<Image>().color = new Vector4(0.15f, 0.15f, 0.15f, 1);
        }
    }

    private void CurrencyUpdate()
    {
        currencyText.SetText(Main_data.playerCurrency.ToString());
    }

    public void Enter_Battling()
    {
        MainSystem.BattlePhase = true;
        MainSystem.Queuing = true;
    }

    public void Recuit_function()
    {
        if(Main_data.playerCurrency > 0)
        {
            for (int index = 0; index < inventoryTile.Length; index++)
            {
                if (inventoryTile[index].GetComponent<TileScript>().unit == null)
                {
                    spawnLocation.x = inventoryTile[index].transform.position.x;
                    spawnLocation.y = 0.75f;
                    spawnLocation.z = inventoryTile[index].transform.position.z;

                    GameObject newAdventurer = Instantiate(playerUnitsPrefad[0], spawnLocation, Quaternion.identity);
                    BaseUnitData newAdventurerScript = newAdventurer.GetComponent<BaseUnitData>();
                    newAdventurerScript.unit_Tile = inventoryTile[index];
                    newAdventurer.name = "Adventurer : " + adventurer_Number;
                    adventurer_Number++;

                    Main_data.playerCurrency -= 15;
                    inventoryTile[index].GetComponent<TileScript>().unit = newAdventurer;
                    break;
                }
            }
        }
        
    }

    public void GameSpeed()
    {
        if (speedUp)
        {
            Time.timeScale = 2.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        MainSystem.MonsterIn_Wave = 0;
        MainSystem.BattlePhase = false;
        MainSystem.Queuing = false;
        MonsterSpawner.Spawned = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitgameButton()
    {
        Application.Quit();
    }

    public void SpeedUpButton()
    {
        if(speedUp)
        {
            speedUp = false;
            ImageBotton.color = Color.white;

        }
        else
        {
            speedUp = true;
            ImageBotton.color = Color.green;
        }
    }

    // - - - Unit's Class Selection - - -
    private void classSelect_Interface()
    {
        if(selected_Unit == null)
        {
            classSelect_UI.SetActive(false);
        }
        else
        {
            classSelect_UI.SetActive(true);
        }

        if (!selected)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastRightMouseHit))
                {
                    if (raycastRightMouseHit.transform.gameObject.tag == "PlayerUnit")
                    {
                        selected_Unit = raycastRightMouseHit.transform.gameObject;
                    }
                }
                selected = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                selected_Unit = null;
                selected = false;
            }
        }
    }

    public void SwordmanButton()
    {
        if(Main_data.playerCurrency >= 25)
        {
            GameObject newSwordman = Instantiate(playerUnitsPrefad[1], selected_Unit.transform.position, Quaternion.identity);
            BaseUnitData newSwordmanScript = newSwordman.GetComponent<BaseUnitData>();
            newSwordmanScript.unit_Tile = selected_Unit.GetComponent<BaseUnitData>().unit_Tile;
            newSwordman.name = "Swordman : " + swordman_Number;
            swordman_Number++;

            Main_data.playerCurrency -= 25;
            Destroy(selected_Unit);
        }
    }

    public void LancerButton()
    {
        if (Main_data.playerCurrency >= 25)
        {
            GameObject newLancer = Instantiate(playerUnitsPrefad[2], selected_Unit.transform.position, Quaternion.identity);
            BaseUnitData newLancerScript = newLancer.GetComponent<BaseUnitData>();
            newLancerScript.unit_Tile = selected_Unit.GetComponent<BaseUnitData>().unit_Tile;
            newLancer.name = "Lancer : " + lancer_Number;
            lancer_Number++;

            Main_data.playerCurrency -= 25;
            Destroy(selected_Unit);
        }
    }

    public void RogueButton()
    {
        if (Main_data.playerCurrency >= 35)
        {
            GameObject newRogue = Instantiate(playerUnitsPrefad[3], selected_Unit.transform.position, Quaternion.identity);
            BaseUnitData newRogueScript = newRogue.GetComponent<BaseUnitData>();
            newRogueScript.unit_Tile = selected_Unit.GetComponent<BaseUnitData>().unit_Tile;
            newRogue.name = "Rogue : " + rogue_Number;
            rogue_Number++;

            Main_data.playerCurrency -= 35;
            Destroy(selected_Unit);
        }
    }

    public void ArcherButton()
    {
        if (Main_data.playerCurrency >= 35)
        {
            GameObject newArcher = Instantiate(playerUnitsPrefad[4], selected_Unit.transform.position, Quaternion.identity);
            BaseUnitData newArcherScript = newArcher.GetComponent<BaseUnitData>();
            newArcherScript.unit_Tile = selected_Unit.GetComponent<BaseUnitData>().unit_Tile;
            newArcher.name = "Archer : " + archer_Number;
            archer_Number++;

            Main_data.playerCurrency -= 35;
            Destroy(selected_Unit);
        }
    }

    public void ClericButton()
    {
        if (Main_data.playerCurrency >= 45)
        {
            GameObject newCleric = Instantiate(playerUnitsPrefad[5], selected_Unit.transform.position, Quaternion.identity);
            BaseUnitData newClericScript = newCleric.GetComponent<BaseUnitData>();
            newClericScript.unit_Tile = selected_Unit.GetComponent<BaseUnitData>().unit_Tile;
            newCleric.name = "Cleric : " + cleric_Number;
            cleric_Number++;

            Main_data.playerCurrency -= 45;
            Destroy(selected_Unit);
        }
    }

    public void MageButton()
    {
        if (Main_data.playerCurrency >= 40)
        {
            GameObject newMage = Instantiate(playerUnitsPrefad[6], selected_Unit.transform.position, Quaternion.identity);
            BaseUnitData newMageScript = newMage.GetComponent<BaseUnitData>();
            newMageScript.unit_Tile = selected_Unit.GetComponent<BaseUnitData>().unit_Tile;
            newMage.name = "Mage : " + mage_Number;
            mage_Number++;

            Main_data.playerCurrency -= 40;
            Destroy(selected_Unit);
        }
    }
}