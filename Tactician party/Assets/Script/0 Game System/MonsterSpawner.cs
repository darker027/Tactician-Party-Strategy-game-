using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // - - - Monster data - - -
    [SerializeField] private List<GameObject> MonsterTile;
    [SerializeField] private List<GameObject> MonsterUnit;

    // - - - Spawn pattern - - -
    [SerializeField] private List<SpawnPattern> Patterns;

    // - - - Spawner Variable - - -
    public int difficultyLevel = 0;
    private Vector3 spawnLocation;
    private int patternNumber = -1;
    private int MonsterType; // futurn use

    [HideInInspector] public static bool Spawned;

    // - - - - - - - - - - - - - - - - - - - - - - - - -
    void Start()
    {

    }

    void Update()
    {
        if (!MainSystem.BattlePhase)
        {
            if (!Spawned)
            {
                Random_Pattern();
            }
        }
    }

    // - - - Spawner function - - -
    public void Random_Pattern()
    {
        if (patternNumber == -1)
        {
            if(difficultyLevel >= 0 && difficultyLevel <= 2)
            {
                patternNumber = Random.Range(0, 2);
            }
            else if(difficultyLevel > 2 && difficultyLevel <= 4)
            {
                patternNumber = Random.Range(2, 4);
            }
            else if(difficultyLevel > 4 && difficultyLevel <= 6)
            {
                patternNumber = Random.Range(4, 6);
            }
            else if (difficultyLevel > 6 && difficultyLevel <= 8)
            {
                patternNumber = Random.Range(6, 8);
            }
            else if (difficultyLevel > 8 && difficultyLevel <= 10)
            {
                patternNumber = Random.Range(8, 10);
            }
        }
        else
        {
            Spawning_Monster();
        }
    }

    void Spawning_Monster()
    {
        for(int index = 0; index < Patterns[patternNumber].SpawnOn_Tile.Count; index++)
        {
            spawnLocation.x = Patterns[patternNumber].SpawnOn_Tile[index].transform.position.x;
            spawnLocation.y = 0.75f;
            spawnLocation.z = Patterns[patternNumber].SpawnOn_Tile[index].transform.position.z;

            GameObject Monster = Instantiate(MonsterUnit[0], spawnLocation, Quaternion.identity);
            BaseUnitData MonsterScript = Monster.GetComponent<BaseUnitData>();
            MonsterScript.unit_Tile = Patterns[patternNumber].SpawnOn_Tile[index];

            Patterns[patternNumber].SpawnOn_Tile[index].GetComponent<TileScript>().unit = Monster;
        }
        MainSystem.MonsterIn_Wave = Patterns[patternNumber].SpawnOn_Tile.Count;
        Spawned = true;

        patternNumber = -1;
    }
}

[System.Serializable]
public class SpawnPattern
{
    public List<GameObject> SpawnOn_Tile;
}
