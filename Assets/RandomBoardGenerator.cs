using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RandomBoardGenerator : NetworkBehaviour
{
    public GameObject hexagonPrefab;
    public Tile brick;
    public Tile ore;
    public Tile wood;
    public Tile wheat;
    public Tile wool;
    public Tile desert;

    private enum TileType
    {
        Bricks,
        Ore,
        Wheat,
        Wool,
        Wood,
        Desert,
    }

    private readonly Vector3[] _tileOffsets = new[]
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(2, 0, 2),
        new Vector3(-2, 0, -2),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),
        new Vector3(-2, 0, 0),
        new Vector3(2, 0, 0),
        new Vector3(0, 0, 2),
        new Vector3(0, 0, -2),
        new Vector3(-3, 0, -1),
        new Vector3(3, 0, 1),
        new Vector3(-2, 0, 2),
        new Vector3(2, 0, -2),
        new Vector3(4, 0, 0),
        new Vector3(-4, 0, 0),
        new Vector3(-3, 0, 1),
        new Vector3(3, 0, -1)
    };

    private readonly Dictionary<TileType, Tile> _tiles = new Dictionary<TileType, Tile>();

    [SerializeField] private Vector2 tileOffset = new Vector2(0.9f, 1.6f);

    private void Start()
    {
        _tiles[TileType.Bricks] = brick;
        _tiles[TileType.Ore] = ore;
        _tiles[TileType.Wheat] = wheat;
        _tiles[TileType.Wool] = wool;
        _tiles[TileType.Wood] = wood;
        _tiles[TileType.Desert] = desert;
    }

    public void CreateRandomBoard()
    {
        GenerateTilesServerRPC();
    }

    private GameObject CreateHexagon(Vector3 position, TileType tileType)
    {
        var hexagon = Instantiate(hexagonPrefab);
        hexagon.transform.position = new Vector3(position.x * tileOffset.x, 0, position.z * tileOffset.y);
        hexagon.GetComponent<Renderer>().material.mainTexture = _tiles[tileType].texture;
        return hexagon;
    }

    [ServerRpc]
    private void GenerateTilesServerRPC()
    {
        var types = new List<TileType>();
        foreach (var type in (TileType[]) Enum.GetValues(typeof(TileType)))
        {
            for (var i = 0; i < _tiles[type].quantity; i++)
            {
                types.Add(type);
            }
        }
        Utilities.ShuffleList(types);
        
        
        SpawnTilesClientRpc(types.ToArray());
    }

    [ClientRpc]
    private void SpawnTilesClientRpc(TileType[] tileTypes)
    {
        var hexagonGOs = new List<GameObject>();
        CreateHexagons(hexagonGOs, tileTypes);
    }

    private void CreateHexagons(List<GameObject> hexagons, TileType[] tileTypes)
    {
        for (var i = 0; i < _tileOffsets.Length; i++)
        {
            hexagons.Add(CreateHexagon(_tileOffsets[i], tileTypes[i]));
        }
    }
}