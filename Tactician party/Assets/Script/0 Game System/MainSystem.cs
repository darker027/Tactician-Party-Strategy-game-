using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    // - - - Data Containing - - -
    [Header("Unit tiles")]
    [SerializeField] private GameObject[] PlayerTiles;
    [HideInInspector] public GameObject[] Data_PlayerTiles => PlayerTiles;
    [SerializeField] private GameObject[] MonsterTiles;
    [HideInInspector] public GameObject[] Data_MonsterTiles => MonsterTiles;

    // - - - Game Phase - - -
    public static bool BattlePhase = false;
    private bool Gameover;

    // - - - Player Life Point - - -
    public float playerLifePoint;
    private float maxLifePoint = 3;

    // - - - Queue of attacking - - -
    public static bool Queuing = false;
    private List<GameObject> battling_Units = new List<GameObject>();
    private List<BaseUnitData> battling_UnitSC = new List<BaseUnitData>();

    private bool AllIdle = false;
    public bool AttackCommand;
    public bool NextNumber;

    private int QueuingNumber = 0;
    private float AttackDelay = 1;

    // - - - Player Party - - -
    [SerializeField] private List<GameObject> player_Party = new List<GameObject>();
    private bool all_playerUnitDead;

    // - - - Monster Wave - - -
    public static int MonsterIn_Wave;
    private int WaveComplete;

    // - - - Player Currency - - -
    public int playerCurrency;

    // - - - UI controller - - -
    [SerializeField] private GameObject BattleButton;
    [SerializeField] private GameObject GameoverText;
    [SerializeField] private GameObject WavescompleteText;
    [SerializeField] private GameObject RestartButton;
    [SerializeField] private GameObject QuitgameButton;

    // - - - - - - - - - - - - - - - - - - - - - - - - -
    void Start()
    {
        playerLifePoint = maxLifePoint;
        playerCurrency = 50;
    }

    void Update()
    {
        // - Battle phase controlling -
        MonsterWave_Checker();
        PlayerParty_Checker();
        Attacking_Queue();
        Attacking_Command();
        Debug.Log(BattlePhase);

        // - UI controlling -
        GamePhase_UI();

        // - Cheat code -
        if (Input.GetKeyDown(KeyCode.G))
        {
            playerCurrency = 999;
        }
    }

    // - - - Wave combat checking function - - -
    private void Attacking_Queue()
    {
        if (BattlePhase && Queuing)
        {
            // Reset order
            battling_Units = new List<GameObject>();
            battling_UnitSC = new List<BaseUnitData>();
            QueuingNumber = 0;

            // Player units queuing
            for (int index = 0; index < PlayerTiles.Length; index++)
            {
                if (PlayerTiles[index].GetComponent<TileScript>().unit_On == true)
                {
                    if (battling_Units.Contains(PlayerTiles[index].GetComponent<TileScript>().unit))
                    {

                    }
                    else
                    {
                        battling_Units.Add(PlayerTiles[index].GetComponent<TileScript>().unit);
                        battling_UnitSC.Add(PlayerTiles[index].GetComponent<TileScript>().unit.GetComponent<BaseUnitData>());
                    }
                }
            }
            // Monster units queuing
            for (int index = 0; index < MonsterTiles.Length; index++)
            {
                if (MonsterTiles[index].GetComponent<TileScript>().unit_On == true)
                {
                    if (battling_Units.Contains(MonsterTiles[index].GetComponent<TileScript>().unit))
                    {

                    }
                    else
                    {
                        battling_Units.Add(MonsterTiles[index].GetComponent<TileScript>().unit);
                        battling_UnitSC.Add(MonsterTiles[index].GetComponent<TileScript>().unit.GetComponent<BaseUnitData>());
                    }
                }
            }

            for (int index = 0; index < battling_Units.Count; index++)
            {
                if(battling_Units[index] == null)
                {
                    battling_Units.RemoveAt(index);
                }
            }

            for (int index = 0; index < battling_UnitSC.Count; index++)
            {
                if (battling_UnitSC[index] == null)
                {
                    battling_UnitSC.RemoveAt(index);
                }
            }

            // Order all the queue
            battling_UnitSC.Sort(SortingQueue_System);
            Queuing = false;
        }
    }

    private void Attacking_Command()
    {
        if (BattlePhase && !Queuing)
        {
            for (int number = 0; number < battling_UnitSC.Count; number++)
            {
                if(battling_UnitSC[number] == null)
                {
                    battling_UnitSC.RemoveAt(number);
                }
                else
                {
                    if (battling_UnitSC[number].unitStatus != BaseUnitData.state.Idle)
                    {
                        AllIdle = false;
                        break;
                    }
                    else
                    {
                        AllIdle = true;
                    }
                }
            }

            if(AllIdle && AttackCommand)
            {
                AttackDelay -= 1 * Time.deltaTime;
                if(AttackDelay <= 0)
                {
                    if(QueuingNumber >= battling_UnitSC.Count)
                    {
                        QueuingNumber = 0;
                    }
                    AttackCommand = false;
                    AttackDelay = 1;
                }
            }

            if (AllIdle && !AttackCommand)
            {
                battling_UnitSC[QueuingNumber].AttackTarget = true;
                Debug.Log(battling_UnitSC[QueuingNumber].name + ": attack");
                AttackCommand = true;
                NextNumber = true;
            }

            if (AllIdle && NextNumber)
            {
                if (QueuingNumber < battling_UnitSC.Count)
                {
                    QueuingNumber = QueuingNumber + 1;
                    NextNumber = false;
                }
            }
        }
    }

    private void MonsterWave_Checker()
    {
        if (BattlePhase && MonsterIn_Wave == 0)
        {
            WaveComplete += 1;
            gameObject.GetComponent<MonsterSpawner>().difficultyLevel += 1;
            MonsterSpawner.Spawned = false;
            BattlePhase = false;
            Queuing = false;
        }
    }

    private void PlayerParty_Checker()
    {
        for(int index = 0; index < PlayerTiles.Length; index++)
        {
            if(PlayerTiles[index].GetComponent<TileScript>().unit_On == true)
            {
                if (player_Party.Contains(PlayerTiles[index].GetComponent<TileScript>().unit))
                {
                    
                }
                else
                {
                    player_Party.Add(PlayerTiles[index].GetComponent<TileScript>().unit);
                }
            }
        }

        for (int index = 0; index < player_Party.Count; index++)
        {
            if (player_Party[index] == null)
            {
                player_Party.RemoveAt(index);
            }
        }

        if (BattlePhase && player_Party.Count != 0 && playerLifePoint > 0)
        {
            for (int index = 0; index < player_Party.Count; index++)
            {
                if (player_Party[index] != null)
                {
                    all_playerUnitDead = false;
                    break;
                }
                else
                {
                    all_playerUnitDead = true;
                }
            }
            if (all_playerUnitDead)
            {
                playerLifePoint -= 1f;
                GetComponent<MonsterSpawner>().difficultyLevel = 0;
            }
        }

        if (BattlePhase && player_Party.Count == 0 && playerLifePoint > 0)
        {
            playerLifePoint -= 1f;

            // - Reset round -
            GameObject[] Monster = GameObject.FindGameObjectsWithTag("MonsterUnit");
            for (int index = 0; index < Monster.Length; index++)
            {
                Destroy(Monster[index]);
            }
            MonsterSpawner.Spawned = false;
            BattlePhase = false;
            Queuing = false;
        }

        if (playerLifePoint <= 0)
        {
            Gameover = true;
        }
    }

    private int SortingQueue_System(BaseUnitData a, BaseUnitData b)
    {
        if (a.Mod_AttackSpeed > b.Mod_AttackSpeed)
        { return -1; }
        else if(a.Mod_AttackSpeed < b.Mod_AttackSpeed)
        { return 1; }
        else
        { return 0; }
    }

    // - - - UI controller function - - -
    private void GamePhase_UI()
    {
        if (!BattlePhase)
        {
            if (!Gameover)
            {
                BattleButton.SetActive(true);
            }
            else
            {
                BattleButton.SetActive(false);
            }
        }
        else
        {
            BattleButton.SetActive(false);
        }

        if (Gameover)
        {
            WavescompleteText.GetComponent<TMPro.TextMeshProUGUI>().SetText("Waves complete : " + WaveComplete);
            GameoverText.SetActive(true);
            WavescompleteText.SetActive(true);
            RestartButton.SetActive(true);
            QuitgameButton.SetActive(true);
        }
    }
}
