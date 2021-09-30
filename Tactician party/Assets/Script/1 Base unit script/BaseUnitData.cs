using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitData : MonoBehaviour
{
    // - - - - - - - - -
    public GameObject floatTextDMG;
    // - - - - - - - - -

    // - - - Game Data Usage - - -
    [HideInInspector] public GameObject Main_Data;
    [HideInInspector] public GameObject[] PlayerTiles;
    [HideInInspector] public GameObject[] MonsterTiles;

    // - - - Unit Class - - -
    [Header("Unit class debug!")]
    public ClassBaseStat unit_ClassStat;
    public string unit_ClassName;

    // - - - Unit Status - - -
    public enum state { Idle, Moving, Attacking, }
    [Header("Unit status debug!")]
    public state unitStatus = state.Idle;

    // - - - Unit Modified Stat - - -
    [HideInInspector] public float Mod_Attack;
    [HideInInspector] public float Mod_AttackSpeed;

    [HideInInspector] public float Mod_Health;
    [HideInInspector] public float Mod_Defense;

    [HideInInspector] public float Mod_Mana;
    [HideInInspector] public float Mod_EnergyRechange;
    [HideInInspector] public bool skillReady;

    [HideInInspector] public float Mod_Critical;
    [HideInInspector] public float Mod_Penetration;
    [HideInInspector] public bool critHit = false;   // Critical attack
    [HideInInspector] public bool peneHit = false;   // Penetrated attack

    [HideInInspector] public float Mod_Dodge;
    [HideInInspector] public float Mod_Block;
    [HideInInspector] public bool dodging = false;    // Dodge attack
    [HideInInspector] public bool blocking = false;   // Block attack

    [Header("Unit stat debug!")]
    public float currentHealth = 0;     // Use as current health of an unit (Mod_Health is the maximun number of health).
    public float currentEnergy = 0;
    public float characterMovespeed = 20f;

    // - - - Unit In-Game Data - - - 
    [Header("Unit in-game data!")]
    public GameObject unit_Tile;
    public GameObject target_Tile;

    public string unit_TilePosition;
    public string target_TilePosition;

    public Vector3 unit_Location;
    public Vector3 target_Location;
    public Vector3 current_Location;

    public GameObject target_Unit;

    public bool AttackTarget;

    // - - - Gameplay system - - -
    [Header("Engine Setting!")]
    public Camera mainCamera;
    public LayerMask layerMask;
    public bool dragging;
}