using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    // - - - Tile data - - -
    private string tile_Name;
    [HideInInspector] public string tileName => tile_Name;

    private string tile_Position;
    [HideInInspector] public string tilePosition => tile_Position;

    private Transform tile_Transform;
    [HideInInspector] public Transform tileTransform => tile_Transform;

    // - - - Unit data - - -
    [SerializeField] public GameObject unit;
    private bool unit_OnTile;
    [HideInInspector] public bool unit_On => unit_OnTile;

    // - - - Visual Variable - - -
    [SerializeField] Material defaultTile;
    [SerializeField] Material selectedTile;

    // - - - - - - - - - - - - - - - - - - - - - - - - -
    private void Awake()
    {
        tile_Name = gameObject.name;
        tile_Position = tileName.Substring(tileName.Length - 2);
        tile_Transform = gameObject.transform;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (unit == null)
        {
            unit_OnTile = false;
        }
        else
        {
            unit_OnTile = true;
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - -
    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material = selectedTile;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = defaultTile;
    }
}
